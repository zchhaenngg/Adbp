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
    }
}
