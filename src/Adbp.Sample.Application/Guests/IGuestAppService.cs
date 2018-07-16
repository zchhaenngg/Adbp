using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Adbp.Paging.Dto;
using Adbp.Sample.Guests.Dto;
using Adbp.Zero;
using Adbp.Application.Services;

namespace Adbp.Sample.Guests
{
    public interface IGuestAppService: IAdbpCrudAppService<long, GuestDto, CreateGuestDto, UpdateGuestDto>, IApplicationService
    {
        void SendEmail(string body);
    }
}
