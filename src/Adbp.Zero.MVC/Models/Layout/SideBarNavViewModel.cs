using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Application.Navigation;

namespace Adbp.Zero.MVC.Models.Layout
{
    public class SideBarNavViewModel
    {
        public UserMenu MainMenu { get; set; }

        public string ActiveMenuItemName { get; set; }
    }
}