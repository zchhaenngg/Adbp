using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using Adbp.Paging.Dto;
using Adbp.Sample.Authorization;
using Adbp.Sample.Guests.Dto;
using Adbp.Zero;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Sample.Guests
{
    [AbpAuthorize(SamplePermissionNames.Permissions_Guest)]
    public class GuestAppService : ZeroCrudAppServiceBase<Guest,long, GuestDto, CreateGuestDto, UpdateGuestDto>, IGuestAppService
    {
        private readonly IEmailSender _emailSender;

        public GuestAppService(
            IRepository<Guest, long> guestRepository,
            SysObjectSettingManager sysObjectSettingManager,
            IEmailSender emailSender
            ) :base(guestRepository, sysObjectSettingManager)
        {
            _emailSender = emailSender;
        }

        public void SendEmail(string body)
        {
            _emailSender.Send(
             to: "xxxg@cccc.com",
             subject: "You have a new task!",
             body: body,
             isBodyHtml: true
             );
        }
    }
}
