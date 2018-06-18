using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;

namespace Adbp.Zero.Analasy.Dto
{
    [AutoMapFrom(typeof(UserLoginAttempt))]
    public class UserLoginAttemptDto : EntityDto<long>
    {
        public long? UserId { get; set; }
        public string UserNameOrEmailAddress { get; set; }
        public string ClientIpAddress { get; set; }
        public string ClientName { get; set; }
        public AbpLoginResultType Result { get; set; }
        public DateTime CreationTime { get; set; }
        public string ResultStr => Result.ToString();
    }
}
