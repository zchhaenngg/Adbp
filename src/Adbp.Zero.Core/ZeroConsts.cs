using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero
{
    public static class ZeroConsts
    {
        public const string LocalizationSourceName = "AdbpZero";

        public const bool MultiTenancyEnabled = true;

        public const int DefaultTenantId = 1;
        
        public static class SearchCmds
        {
            public const string SymbolException = "@exception";
        }

        public static class SettingGroups
        {
            public const string SettingGroups_LDAP = "SettingGroups.LDAP";
            public const string SettingGroups_Mail = "SettingGroups.Mail";
            public const string SettingGroups_UserManagement = "SettingGroups.UserManagement";
            public const string SettingGroups_BackgroundWorkers = "SettingGroups.BackgroundWorkers";
            public const string SettingGroups_LanguageTimeZone = "SettingGroups.LanguageTimeZone";
            public const string SettingGroups_Others = "SettingGroups.Others";
            public const string SettingGroups_DATETIME = "SettingGroups.DATETIME";
        }
    }
}
