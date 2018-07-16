using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Localization;

namespace Adbp.Zero.Localization.Dto
{
    public class UpdateLocalizedStringInput
    {
        [Required]
        public string Source { get; set; }

        /// <summary>
        /// key
        /// </summary>
        [Required]
        public string Name { get; set; }

        [Required]
        public string EnValue { get; set; }

        public string ZhValue { get; set; }
    }
}
