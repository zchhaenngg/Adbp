using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.Ldap.Configuration;
using CompanyName.ProjectName.Authorization;
using CompanyName.ProjectName.Configuration;
using Adbp.Zero;

namespace CompanyName.ProjectName
{
    [DependsOn(typeof(ZeroCoreModule))]
    public class ProjectNameCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ProjectNameConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "CompanyName.ProjectName.Localization.Source"
                        )
                    )
                );

            Configuration.Modules.ZeroCoreModule().EnableZeroLdapAuthenticationSource = false;
            Configuration.Authorization.Providers.Add<ProjectNameAuthorizationProvider>();
            Configuration.Settings.Providers.Add<ProjectNameSettingProvider>();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
