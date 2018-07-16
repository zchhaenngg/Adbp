多语言支持
===============

权限
---------------
```
<text name="Permissions_Project" value="Projects" />
<text name="Permissions_Project_Create" value="Projects.Create" />
<text name="Permissions_Project_Update" value="Projects.Retrieve" />
<text name="Permissions_Project_Delete" value="Projects.Update" />
<text name="Permissions_Project_Retrieve" value="Projects.Delete" />
```

导航菜单
---------------
```
<text name="MENU_Projects" value="Projects"/>
<text name="MENU_Reports" value="Reports" />
```

枚举
---------------
```
yield return new PageQueryItem(nameof(Report.ConfirmedResult),
    EnumLikeStr<ConfirmedResult>(query.Search.Value), ExpressionOperate.Contains);
```

```
{
    data: 'ConfirmedResult', render: function (data, type, full, meta) {
        return abp.localization.localize("ENUM_ConfirmedResult_" + data);
    }
},
```

Bool
---------------
```
<text name="BOOL_ISAUTOCONFIRMED_TRUE" value="Sysem Auto Confirmed" />
<text name="BOOL_ISAUTOCONFIRMED_FALSE" value="Manual Confirmed" />
```    

```
yield return new PageQueryItem(nameof(Report.IsAutoConfirmed), 
    BoolLikeStr(nameof(Report.IsAutoConfirmed), query.Search.Value), ExpressionOperate.Equal);
```

```
{
    data: 'IsAutoConfirmed', render: function (data, type, full, meta) {
        if (data == null) {
            return "";
        }
        else if (data) {
            return abp.localization.localize("BOOL_ISAUTOCONFIRMED_TRUE");
        }
        else {
            return abp.localization.localize("BOOL_ISAUTOCONFIRMED_FALSE");
        }
    }
}
```

UserStrLike
---------------
```
yield return new PageQueryItem(nameof(Report.OwnerId),
    UserLikeStr(_userRepository, query.Search.Value), ExpressionOperate.Contains);
```