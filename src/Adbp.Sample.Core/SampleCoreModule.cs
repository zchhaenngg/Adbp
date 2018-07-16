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
using Adbp.Sample.Authorization;
using Adbp.Sample.Configuration;
using Adbp.Zero;

namespace Adbp.Sample
{
    [DependsOn(typeof(ZeroCoreModule))]
    public class SampleCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    SampleConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Adbp.Sample.Localization.Source"
                        )
                    )
                );

            Configuration.Modules.ZeroCoreModule().EnableZeroLdapAuthenticationSource = false;
            Configuration.Authorization.Providers.Add<SampleAuthorizationProvider>();
            Configuration.Settings.Providers.Add<SampleSettingProvider>();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
