﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Abp.Zero.Ldap.Configuration;
using Adbp.Zero.Authorization;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.Configuration;
using Adbp.Zero.MultiTenancy;
using Adbp.Zero.Notifications;

namespace Adbp.Zero
{
    [DependsOn(
        typeof(AbpZeroLdapModule),
        typeof(AbpZeroCoreModule))]
    public class ZeroCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<ZeroCoreModuleConfig>();
            

            Configuration.Auditing.IsEnabledForAnonymousUsers = true;
            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            //Remove the following line to disable multi-tenancy.
            //Configuration.MultiTenancy.IsEnabled = AdbpZeroConsts.MultiTenancyEnabled;

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Adbp.Zero.Localization.Source"
                        )
                    )
                );

            ZeroRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Authorization.Providers.Add<ZeroAuthorizationProvider>();

            Configuration.Settings.Providers.Add<ZeroSettingProvider>();

            Configuration.Notifications.Providers.Add<ZeroNotificationProvider>();
        }

        public override void Initialize()
        {
            if (Configuration.Get<ZeroCoreModuleConfig>().EnableZeroLdapAuthenticationSource)
            {
                Configuration.Modules.ZeroLdap().Enable(typeof(ZeroLdapAuthenticationSource));
            }
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
