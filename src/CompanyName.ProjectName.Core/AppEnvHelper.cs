using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.ProjectName
{
    public static class AppEnvHelper
    {
        public static bool IsJobExecutionEnabled()
        {
            return System.Configuration.ConfigurationManager.AppSettings["IsJobExecutionEnabled"]?.Trim().ToUpper() == "TRUE";
        }

        public static bool IsEmailWorkerEnabled()
        {
            return System.Configuration.ConfigurationManager.AppSettings["IsEmailWorkerEnabled"]?.Trim().ToUpper() == "TRUE";
        }
    }
}
