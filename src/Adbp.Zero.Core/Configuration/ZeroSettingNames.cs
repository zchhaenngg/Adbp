using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.Configuration
{
    public static class ZeroSettingNames
    {
        public const string DateFormatting = "Adbp.Zero.LanguageTimeZone.DateFormatting";
        public const string TimeFormatting = "Adbp.Zero.LanguageTimeZone.TimeFormatting";
        public const string DateTimeFormatting = "Adbp.Zero.LanguageTimeZone.DateTimeFormatting";
        
        public static class BackgroundWorkers
        {
            public const string EmailWorkerTimerPeriodSeconds = "Adbp.BackgroundWorkers.EmailWorker.TimerPeriodSeconds";
        }

        public static class OrganizationSettings
        {
            /// <summary>
            /// 启用组织管理功能, 开启此功能后, 页面才可以添加组织, 删除组织, 添加组织用户, 删除组织用户 等等
            /// default is true
            /// </summary>
            public const string EnableOrganizationUnitManagement = "Adbp.Zero.OrganizationSettings.EnableOrganizationUnitManagement";

            /// <summary>
            /// 允许添加根组织
            /// </summary>
            public const string CanAddRootOrganizationUnit = "Adbp.Zero.OrganizationSettings.CanAddRootOrganizationUnit";

            /// <summary>
            /// 允许添加Static组织的子组织
            /// </summary>
            public const string CanAddChildOrganizationUnitInStaticOrganizationUnit = "Adbp.Zero.OrganizationSettings.CanAddChildOrganizationUnitInStaticOrganizationUnit";

            /// <summary>
            /// 允许添加的组织的最大层级数, 默认16级
            /// </summary>
            public const string MaxOrganizationUnitDepth = "Adbp.Zero.OrganizationSettings.MaxOrganizationUnitDepth";

            /// <summary>
            /// 允许向Static组织中添加用户
            /// default is false
            /// </summary>
            public const string CanAddUserInStaticOrganizationUnit = "Adbp.Zero.OrganizationSettings.CanAddUserInStaticOrganizationUnit";

        }
    }
}
