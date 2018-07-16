Application Services
=================

Create Application Dtos
----------------
+ CreateGuestDto
```
[AutoMapTo(typeof(Guest))]
public class CreateGuestDto
{
}
```

+ UpdateGuestDto
```
[AutoMapTo(typeof(Guest))]
public class UpdateGuestDto : EntityDto<long>
{

}
```

+ GuestDto
```
[AutoMapFrom(typeof(Guest))]
public class GuestDto : EntityDto<long>
{
}
```

Create Application Services
-------------------

CRUD is Required.

+ Create an interface, implement the `IApplicationService` interface and `IAdbpCrudAppService` interface.

    let's define an interface for an application service:

    ```
    public interface IGuestAppService: IAdbpCrudAppService<long, GuestDto, CreateGuestDto, UpdateGuestDto>, IApplicationService
    {

    }
    ```

+ Create an ApplicationService Class that implement the `IApplicationService` interface as declared above. 
    
    Now we can implement the IGuestAppService:
    
    ```
    [AbpAuthorize(SamplePermissionNames.Permissions_Guest)]
    public class GuestAppService : ZeroCrudAppServiceBase<Guest,long, GuestDto, CreateGuestDto, UpdateGuestDto>, IGuestAppService
    {
        public GuestAppService(
            IRepository<Guest, long> guestRepository,
            SysObjectSettingManager sysObjectSettingManager
            ):base(guestRepository, sysObjectSettingManager)
        {

        }
    }
    ```

CRUD is not Required.

+ Create an interface, implement the `IApplicationService` interface

    let's define an interface for an application service:
    
    ```
    public interface IContactAppService: IApplicationService
    {
    }
    ```

+ Create an ApplicationService Class that implement the `IApplicationService` interface as declared above. 

    Now we can implement the IContactAppService:

    ```
    [AbpAuthorize(SamplePermissionNames.Permissions_Contact)]
    public class ContactAppService : ZeroAppServiceBase, IContactAppService
    {
    }
    ```

CRUD Permissions
=====================

+ 权限常量
```
public static class SamplePermissionNames
{
    public const string Permissions_Guest = "Permissions.Guest";
    public const string Permissions_Guest_Create = "Permissions.Guest.Create";
    public const string Permissions_Guest_Retrieve = "Permissions.Guest.Retrieve";
    public const string Permissions_Guest_Update = "Permissions.Guest.Update";
    public const string Permissions_Guest_Delete = "Permissions.Guest.Delete";
}
```

+ 初始化权限
```
public class SampleAuthorizationProvider: AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
        context.CreatePermission(SamplePermissionNames.Permissions_Guest, L("Permissions_Guest"));
        context.CreatePermission(SamplePermissionNames.Permissions_Guest_Create, L("Permissions_Guest_Create"));
        context.CreatePermission(SamplePermissionNames.Permissions_Guest_Update, L("Permissions_Guest_Update"));
        context.CreatePermission(SamplePermissionNames.Permissions_Guest_Delete, L("Permissions_Guest_Delete"));
        context.CreatePermission(SamplePermissionNames.Permissions_Guest_Retrieve, L("Permissions_Guest_Retrieve"));
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, SampleConsts.LocalizationSourceName);
    }
}
```

+ 权限多语言
```
<!--角色赋予权限时，权限的displayName-->
<text name="Permissions_Guest" value="Guests" />
<text name="Permissions_Guest_Create" value="Guests.Create" />
<text name="Permissions_Guest_Retrieve" value="Guests.Retrieve" />
<text name="Permissions_Guest_Update" value="Guests.Update" />
<text name="Permissions_Guest_Delete" value="Guests.Delete" />
```