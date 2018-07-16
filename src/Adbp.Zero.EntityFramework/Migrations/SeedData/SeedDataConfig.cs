using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Adbp.Zero.Authorization;

namespace Adbp.Zero.Migrations.SeedData
{
    public class SeedDataConfig
    {
        public static List<AuthorizationProvider> AuthorizationProviders;
        static SeedDataConfig()
        {
            AuthorizationProviders = new List<AuthorizationProvider> {
                new ZeroAuthorizationProvider()
            };
        }
    }
}
