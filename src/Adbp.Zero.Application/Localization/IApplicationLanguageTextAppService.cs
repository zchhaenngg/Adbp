using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Adbp.Paging.Dto;
using Adbp.Zero.Localization.Dto;

namespace Adbp.Zero.Localization
{
    public interface IApplicationLanguageTextAppService : IApplicationService
    {
        Task UpdateAsync(UpdateLocalizedStringInput input);
        List<LocalizedStringDto> GetAllLocalizedStringDtos();
    }
}
