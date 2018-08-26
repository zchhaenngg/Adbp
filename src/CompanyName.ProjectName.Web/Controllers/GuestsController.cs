using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Adbp.Linq.Expressions;
using Adbp.Paging;
using Adbp.Paging.Dto;
using CompanyName.ProjectName.Authorization;
using CompanyName.ProjectName.Guests;
using CompanyName.ProjectName.Guests.Dto;

namespace CompanyName.ProjectName.Web.Controllers
{
    [AbpMvcAuthorize(ProjectNamePermissionNames.Permissions_Guest)]
    public class GuestsController : ProjectNameControllerBase
    {
        private readonly IGuestAppService _guestAppService;

        public GuestsController(IGuestAppService guestAppService)
        {
            _guestAppService = guestAppService;
        }

        // GET: Guests
        public ActionResult Index()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<ActionResult> GetGuests(DataTableQuery query)
        {
            IEnumerable<PageQueryItem> getPageQueryItems()
            {
                yield return new PageQueryItem(nameof(Guest.Name), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Guest.PinYin), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Guest.IsFemale), BoolLikeStr(nameof(Guest.IsFemale), query.Search.Value), ExpressionOperate.Equal);
                yield return new PageQueryItem(nameof(Guest.Title), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Guest.Company), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Guest.Description), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Guest.Telephone), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Guest.Guid), query.Search.Value, ExpressionOperate.Equal);
            }

            var input = new GenericPagingInput(
                query.Start,
                query.Length,
                list: getPageQueryItems().ToList());
            //input.Sorting = "CreationTime asc";
            var page = await _guestAppService.GetAllAsync(input);
            return Json(new DataTableResult<GuestDto>(page));
        }
    }
}