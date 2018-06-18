using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Localization;

namespace Adbp.Zero.MVC.Models.Layout
{
    public class LanguageSelectionViewModel
    {
        public LanguageInfo CurrentLanguage { get; set; }

        public IReadOnlyList<LanguageInfo> Languages { get; set; }

        public string CurrentUrl { get; set; }
    }
}