using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Adbp.Domain.Entities;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.Emails
{
    public class Email: FullAuditedTOEntity<long, User>
    {
        /// <summary>
        /// 收件人
        /// </summary>
        public virtual string To { get; set; }
        /// <summary>
        /// Carbon Copy(抄送)
        /// </summary>
        public virtual string CC { get; set; }
        /// <summary>
        /// Blind Carbon Copy(暗抄送)
        /// </summary>
        public virtual string Bcc { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual bool IsBodyHtml { get; set; } = true;
        public virtual MailPriority Priority { get; set; }
        public virtual EmailStatus Status { get; set; }
    }

    public enum EmailStatus
    {
        /// <summary>
        /// 等待发送
        /// </summary>
        Pending = 1,
        /// <summary>
        /// 发送成功
        /// </summary>
        Succeed = 2,
        /// <summary>
        /// 发送失败
        /// </summary>
        Failed = 3
    }
}
