﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.Authorization
{
    public static class PermissionNames
    {
        public const string Permissions_Tenant = "Permissions.Tenant";

        //Permissions_Users CRUD
        public const string Permissions_User = "Permissions.User";
        public const string Permissions_User_Create = "Permissions.User.Create";
        public const string Permissions_User_Retrieve = "Permissions.User.Retrieve";
        public const string Permissions_User_Update = "Permissions.User.Update";
        public const string Permissions_User_Delete = "Permissions.User.Delete";

        //Permissions_Roles CRUD
        public const string Permissions_Role = "Permissions.Role";
        public const string Permissions_Role_Create = "Permissions.Role.Create";
        public const string Permissions_Role_Retrieve = "Permissions.Role.Retrieve";
        public const string Permissions_Role_Update = "Permissions.Role.Update";
        public const string Permissions_Role_Delete = "Permissions.Role.Delete";

        //Permissions_OrganizationUnits CRUD
        public const string Permissions_OrganizationUnit = "Permissions.OrganizationUnit";
        public const string Permissions_OrganizationUnit_Create = "Permissions.OrganizationUnit.Create";
        public const string Permissions_OrganizationUnit_Retrieve = "Permissions.OrganizationUnit.Retrieve";
        public const string Permissions_OrganizationUnit_Update = "Permissions.OrganizationUnit.Update";
        public const string Permissions_OrganizationUnit_Delete = "Permissions.OrganizationUnit.Delete";
        
        //只有查看
        public const string Permissions_AuditLog = "Permissions.AuditLog";
        public const string Permissions_LoginAttemptLog = "Permissions.LoginAttemptLog";
        public const string Permissions_Home = "Permissions.Home";
        

        public const string Permissions_SystemSetting = "Permissions.SystemSetting";
    }
}
