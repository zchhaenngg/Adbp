using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using Adbp.Zero.Authorization.Users;
using Microsoft.AspNet.Identity;
using Adbp.Extensions;
using Adbp.Zero;
using Abp.IdentityFramework;

namespace Adbp.Zero.MVC.Controllers
{
    public abstract class ZeroControllerBase : AbpController
    {
        protected ZeroControllerBase()
        {
            LocalizationSourceName = ZeroConsts.LocalizationSourceName;
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// 对UserName, Surname, Name进行模糊查询，返回所有匹配的User. 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="nameLike"></param>
        /// <returns></returns>
        protected virtual List<User> UserLike(IRepository<User, long> repository, string nameLike)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if (string.IsNullOrWhiteSpace(nameLike))
            {
                return null;
            }
            return repository.GetAll().Where(
                x => x.UserName.Contains(nameLike) || x.Surname.Contains(nameLike) || x.Name.Contains(nameLike)
                ).ToList();
        }

        /// <summary>
        /// 对User进行模糊查询，返回匹配的UserId
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="nameLike"></param>
        /// <returns></returns>
        protected virtual string UserLikeStr(IRepository<User, long> repository, string nameLike)
        {
            var users = UserLike(repository, nameLike);
            if (users == null)
            {
                return null;
            }
            return string.Join(";", users.Select(x => x.Id).ToArray());
        }
        
        protected virtual string EnumLikeStr<TEnum>(string str)
            where TEnum : Enum
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            
            var items = EnumExtension.GetEnums<TEnum>().Select(x => 
                    (
                        item: x, 
                        text: L(x.SouceName()))
                    )
                .ToList();

            var likeItems = items.Where(
                        x => x.text.ToUpper().Contains(str.ToUpper())
                    ).Select(x => x.item);

            return string.Join(";", likeItems.Select(x => x.ToString().ToArray()));
        }

        protected virtual string BoolLikeStr(string name, string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            var trueLocal = L(true.SourceName(name));
            if (trueLocal.Contains(str))
            {
                return true.ToString();
            }
            var falseLocal = L(false.SourceName(name));
            if (falseLocal.Contains(str))
            {
                return false.ToString();
            }
            return null;
        }
    }
}
