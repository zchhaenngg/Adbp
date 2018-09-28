using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Runtime.Session;
using Adbp.Paging.Dto;
using Adbp.Zero.Authorization;
using Adbp.Zero.Localization.Dto;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Zero.Localization
{
    [AbpAuthorize(ZeroPermissionNames.Permissions_ApplicationLanguageText)]
    public class ApplicationLanguageTextAppService : ZeroAppServiceBase, IApplicationLanguageTextAppService
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly ILanguageManager _languageManager;
        private readonly IApplicationLanguageTextManager _applicationLanguageTextManager;
        private readonly IRepository<ApplicationLanguageText, long> _repository;
        
        public ApplicationLanguageTextAppService(
            ILocalizationManager localizationManager,
            ILanguageManager languageManager,
            IApplicationLanguageTextManager applicationLanguageTextManager,

            IRepository<ApplicationLanguageText, long> repository,
            SysObjectSettingManager sysObjectSettingManager) 
            : base(sysObjectSettingManager)
        {
            _localizationManager = localizationManager;
            _languageManager = languageManager;
            _applicationLanguageTextManager = applicationLanguageTextManager;
            _repository = repository;
        }

        public virtual List<LocalizedStringDto> GetAllLocalizedStringDtos()
        {
            var list = new List<LocalizedStringDto>();
            
            var sources = _localizationManager.GetAllSources().OrderBy(s => s.Name).ToArray();
            foreach (var source in sources)
            {
                IReadOnlyList<LocalizedString> ens = source.GetAllStrings(new CultureInfo("en"), includeDefaults: true);
                IReadOnlyList<LocalizedString> zhs = source.GetAllStrings(new CultureInfo("zh-Hans"), includeDefaults: true);
                foreach (var item in ens)
                {
                    list.Add(new LocalizedStringDto
                    {
                        Source = source.Name,
                        Name = item.Name,
                        EnValue = item.Value
                    });
                }
                foreach (var item in zhs)
                {
                    var elem = list.FirstOrDefault(x => x.Source == source.Name && x.Name == item.Name);
                    if (elem != null)
                    {
                        elem.ZhValue = item.Value;
                    }
                    else
                    {
                        list.Add(new LocalizedStringDto
                        {
                            Source = source.Name,
                            Name = item.Name,
                            EnValue = item.Value
                        });
                    }
                }
            }
            return list;
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_ApplicationLanguageText_Upsert)]
        public virtual async Task UpdateAsync(UpdateLocalizedStringInput input)
        {
            await _applicationLanguageTextManager.UpdateStringAsync(AbpSession.GetTenantId(), input.Source, new CultureInfo("en"), input.Name,input.EnValue);
            await _applicationLanguageTextManager.UpdateStringAsync(AbpSession.GetTenantId(), input.Source, new CultureInfo("zh-Hans"), input.Name, input.ZhValue);
        }
    }
}
