[域账号集成](https://www.aspnetboilerplate.com/Pages/Documents/Zero/User-Management#ldap-active-directory)
==================
默认启用域账号登录，还需要步骤如下，

+ 配置域账号的信息
```
internal class SettingCreateor : SampleDbContextCreatorBase
{
    public SettingCreateor(SampleDbContext context)
            : base(context)
    {

    }
    internal void Create()
    {
        // LDAP
        AddSettingIfNotExists("Abp.Zero.Ldap.IsEnabled", true.ToString(), SampleConsts.DefaultTenantId);
    }
}
```

禁用域账号登录
----------------------

+ append code on method PreInitialize in SampleCoreModule
```
Configuration.Modules.ZeroCoreModule().EnableZeroLdapAuthenticationSource = false;
```

[Settings](https://www.aspnetboilerplate.com/Pages/Documents/Zero/User-Management#settings)
---------------

The LdapSettingNames class defines constants for setting names. You can use these constant names while changing settings (or getting settings). LDAP settings are per-tenant (for multi-tenant applications), so different tenants have different settings (see the setting definitions on github). 

As you can see in the MyLdapAuthenticationSource constructor, LdapAuthenticationSource expects ILdapSettings as a constructor argument. This interface is used to get the LDAP settings like domain, user name and password to connect to Active Directory. The default implementation (LdapSettings class) gets these settings from the setting manager.

If you work with Setting manager, then there's no problem. You can change the LDAP settings using the setting manager API. If you want, you can add some initial seed data to the database to enable LDAP auth by default.

Note: If you don't define a domain, username and password, LDAP authentication works for the current domain if your application runs in a domain with appropriate privileges.