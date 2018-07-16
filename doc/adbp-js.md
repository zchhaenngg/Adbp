abp.formSubmit
================
提交form的plugin.

Usage Example
----------------

- `<form>` markup  and `adbp-formSubmit` class 
```
<form id="sysObjects_form" action="api/services/app/SysObjectSetting/upsertRoleSysObjectSetting" data-dt="#table-details_sysObjects">
    <input type="hidden" name="RoleId" value="@Model.Id" />
    <div class="form-group row ml-2">
        <label for="SysObject_Name" class="col-43 col-form-label">访问对象:</label>
        <div class="col-9">
            <input type="text" name="SysObjectName" class="form-control-plaintext" id="SysObject_Name" value="">
        </div>
    </div>
    <button type="button" class="ml-2 btn btn-primary adbp-formSubmit">@L("Save")</button>
</form>
```


如果需要在提交前对参数进行调整, 如下, (`adbp.formsubmitInitialized` 事件发生在`abp.formSubmit`完成初始化之后) 
```
abp.event.on('adbp.formsubmitInitialized', function () {
    let table = $("#sysObjects_form").data("adbp_formSubmit");
    table.data = function (params) {
        // todo parmas.SysObjectName = ...
    }
});
```

adbp-ajaxForm
===============
点击 **submit** 按钮后,提交所在form.成功后刷新页面

```
<form class="form-inline card-text adbp-ajaxForm" action="/api/services/app/configuration/changeSettingForTenant">
    <input type="hidden" name="Name" value="Adbp.Zero.LanguageTimeZone.DateFormatting">
    <div class="form-group mb-2">
        <input type="text" class="form-control-plaintext" value="日期格式" readonly="">
    </div>
    <div class="form-group mx-sm-3 mb-2">
            <input type="text" name="Value" class="form-control" value="yyyy-MM-d">
    </div>
        <button type="submit" class="btn btn-primary mb-2"><i class="fas fa-check"></i></button>
</form>
```

typeahead
===============
输入并提示参考项

Dependencies
----------------
jQuery
typeahead.js


Usage Example
----------------
- Create typeahead from `<input>` markup.
```
 <input class="adbp-typeahead" type="text" data-suggestions="aaa, bbb" placeholder="xxx">
```

- Create typeahead using javascript.
```
 new abp.typeahead(selector, ['aaa', 'bbb']).initialise();
```