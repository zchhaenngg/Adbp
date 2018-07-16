using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.WebApi.Authorization;
using Abp.WebApi.Controllers;
using Adbp.Zero.Authorization;

namespace Adbp.Sample.Api.Controllers
{
    [AbpApiAuthorize(ZeroPermissionNames.Permissions_User)]
    public class TestApiController: AbpApiController
    {
        public string Post([FromBody]string msg)
        {
            return msg;
        }
    }
}
