using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Net.Mail.Smtp;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Adbp.Zero.Configuration;
using Adbp.Zero.Emails;

namespace Adbp.Zero.BackgroundWorkers
{
    public class EmailWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<Email, long> _emailRepository;
        private readonly ISmtpEmailSender _smtpEmailSender;
        private readonly ISettingManager _settingManager;

        public EmailWorker(
            AbpTimer timer, 
            IRepository<Email, long> emailRepository,
            ISmtpEmailSender smtpEmailSender,
            ISettingManager settingManager)
            :base(timer)
        {
            _emailRepository = emailRepository;
            _smtpEmailSender = smtpEmailSender;
            _settingManager = settingManager;
            Timer.Period = 5 * 1000;//5 seconds (good for tests, but normally will be more)
        }

        [UnitOfWork(IsDisabled = true)]
        protected override void DoWork()
        {
            if (int.TryParse(SettingManager.GetSettingValue(ZeroSettingNames.BackgroundWorkers.EmailWorkerTimerPeriodSeconds), out int tmpPeriod))
            {
                Timer.Period = tmpPeriod * 1000;
            }
            Logger.Info("*************EmailWorker begin ...*************");
            AsyncHelper.RunSync(SendEmails);
            Logger.Info("*************EmailWorker end *************");
        }

        async Task SendEmails()
        {
            var emails = await _emailRepository.GetAllListAsync(x => x.Status == EmailStatus.Pending);
            foreach (var item in emails)
            {
                try
                {
                    var mail = new MailMessage
                    {
                        To = { item.To },
                        Subject = item.Subject,
                        Body = item.Body,
                        IsBodyHtml = item.IsBodyHtml,
                        Priority = item.Priority,
                    };
                    if (!string.IsNullOrWhiteSpace(item.CC))
                    {
                        mail.CC.Add(item.CC);
                    }
                    if (!string.IsNullOrWhiteSpace(item.Bcc))
                    {
                        mail.Bcc.Add(item.Bcc);
                    }
                    await _smtpEmailSender.SendAsync(mail);
                    item.Status = EmailStatus.Succeed;
                }
                catch (Exception ex)
                {
                    item.Status = EmailStatus.Failed;
                    Logger.Error(ex.Message, ex);
                }
                await _emailRepository.UpdateAsync(item);
            }
        }
    }
}
