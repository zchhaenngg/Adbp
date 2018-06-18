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
using Adbp.Sample.Authorization;
using Adbp.Sample.Contacts;
using Adbp.Sample.Contacts.Dto;

namespace Adbp.Sample.Web.Controllers
{
    [AbpMvcAuthorize(SamplePermissionNames.Permissions_Contact)]
    public class ContactsController : SampleControllerBase
    {
        private readonly IContactAppService _contactAppService;

        public ContactsController(IContactAppService contactAppService)
        {
            _contactAppService = contactAppService;
        }

        // GET: Contacts
        public ActionResult Index()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<ActionResult> GetContacts(DataTableQuery query)
        {
            IEnumerable<PageQueryItem> getPageQueryItems()
            {
                yield return new PageQueryItem(nameof(Contact.Name), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Contact.IsFemale), BoolLikeStr(nameof(Contact.IsFemale), query.Search.Value), ExpressionOperate.Equal);
                yield return new PageQueryItem(nameof(Contact.Telephone), query.Search.Value, ExpressionOperate.Like);
                yield return new PageQueryItem(nameof(Contact.Email), query.Search.Value, ExpressionOperate.Like);
            }

            var input = new GenericPagingInput(
                query.Start,
                query.Length,
                list: getPageQueryItems().ToList());
            //input.Sorting = "CreationTime asc";
            var page = await _contactAppService.GetContactDtosAsync(input);
            return Json(new DataTableResult<ContactDto>(page));
        }
    }
}