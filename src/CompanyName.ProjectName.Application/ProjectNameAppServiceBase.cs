using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Adbp.Linq.Expressions;
using Abp.Linq.Extensions;
using Adbp.Zero.Authorization.Users;
using Adbp.Paging.Dto;
using Adbp.Linq;
using Adbp.Domain.Entities;
using Abp.Runtime.Session;
using Abp.Domain.Entities.Auditing;
using Abp.UI;
using Adbp.Zero;
using Abp.Authorization;
using Adbp.Zero.SysObjectSettings;

namespace CompanyName.ProjectName
{
    public abstract class ProjectNameAppServiceBase : ZeroAppServiceBase
    {
        protected ProjectNameAppServiceBase(SysObjectSettingManager sysObjectSettingManager)
            :base(sysObjectSettingManager)
        {
            LocalizationSourceName = ProjectNameConsts.LocalizationSourceName;
        }
    }
}
