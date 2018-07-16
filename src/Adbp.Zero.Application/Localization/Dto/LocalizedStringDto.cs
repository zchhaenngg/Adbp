using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Localization;

namespace Adbp.Zero.Localization.Dto
{
    [AutoMapFrom(typeof(ApplicationLanguageText))]
    public class LocalizedStringDto
    {
        public string Source { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Name { get; set; }

        public string EnValue { get; set; }

        public string ZhValue { get; set; }
    }
}
