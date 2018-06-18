﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;

namespace Adbp.Zero.Analasy.Dto
{
    [AutoMapFrom(typeof(AuditLog))]
    public class AuditLogDto : EntityDto<long>
    {
        public long? ImpersonatorUserId { get; set; }
        public long? UserId { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public string CustomData { get; set; }
        public DateTime ExecutionTime { get; set; }
        public int ExecutionDuration { get; set; }
        public string Exception { get; set; }
        public string BrowserInfo { get; set; }
        public string ClientName { get; set; }
        public string ClientIpAddress { get; set; }
        public string UserStr { get; set; }
    }
}
