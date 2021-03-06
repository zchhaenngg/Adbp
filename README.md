# ASP.NET Dilrba Boilerplate
This is an ASP.NET Boilerplate module integrated with [ABP Module Zero](https://github.com/aspnetboilerplate/aspnetboilerplate)

## Features
- [Organization Unit](./doc/OrganizationUnit.md)
- [Ldap](./doc/Ldap.md)
- [Configuration](./doc/Configuration.md)
- [Localization](./doc/Localization.md)
- [Background Workers](./doc/Background-Wokers.md)
- [Permission](./doc/Permission.md)
- [Cron Expression](./doc/Cron.md)
- [Init data](./doc/Init-data.md)
- [Application Services](./doc/Application-Services.md)
- [Notification](./doc/notification.md)
- [js](./doc/adbp-js.md)

#### API 
#### $("#roleEditForm").resetForm() //清空表单

#### abp.httpStatusNotOkResponse //提取出来的方法

##### $.fn.deserialize
> var role = $("#roleEditForm").deserialize();  //获取参数对象

##### abp.validate
> abp.validate("#roleEditForm") === false //校验容器

##### abp.formSubmit
> 自动提交form。  自动校验，自动提交form, 自动关闭modal，提示操作成功，刷新关联的table。
> 1. form增加属性data-dt，如果有则提交成功后刷新该table
> 1. checkbox 实际为radioButton即只有一个选项，选中后不返回数组时。 data-checkbox="false"

##### abp.isDispatched //只有第1次指派有效，默认1500ms后重置为未指派.
> abp.isDispatched(this, "click", { milliseconds: 1500 }) //最后一个参数可以不传
> if (abp.isDispatched(this, "click")) return; //如果form已提交且距离上一次执行提交操作不到1500ms, 则忽略本次点击（防止人为不小心导致的重复提交

#### abp.formSubmit.get(formSelector)//获取同一个formSelector对象
> abp.formSubmit.get($form);

#### abp.escapeHtml 对特殊字符的处理，比如将字符串赋值给某个标签或标签中的某个属性。如果出现双引号、单引号、<、>、空格等会导致标签解析不正确
#### abp.maxDisplay(data, 30, "...")  显示最大的字符数

#### IMayHaveOwner OwnerId
一般实体都继承FullAuditedTOEntity<long, User>。//有租户有最终负责人

#### 设计原则
1. 用户输入的业务数据, 只能逻辑删除不能物理删除
2. 非业务数据, 如SysObjectSetting,管理员配置用户的数据访问条件, 应当物理删除, 反正有审计功能! 
3. 搜索框支持个性化的命令操作  尚未实现！
   取值函数命令：
	*>5* 
	*<7* 
	*>2017-05-01* 
	*<2018-06-17*

# 自定义审批流程
1. 组织结构的设计已完成
2. 汇报关系的设定  0%


## 总结创建一个新的页面需要的操作步骤，以管理联系人为例。
1. **...Core** Create Entity
  （实现统一接口FullAuditedTOEntity<long, User> 有租户有最终负责人）
1. **...EntityFramework** 
   Authorization，添加Permission以及多语言
   Update DbContext.cs/Init Data/Create Role/update-database -verbose
1. **...Application** Write ApplicationService
   新建DTO, 实现接口IApplicationService, 注意权限以及必须是虚方法(Virtual)
1. **...Web** NavigationProvider 添加菜单以及 多语言
1. **...Web** Controller -> Index And SearchAPI
1. **...Web** Index View Create/edit/delete/Search 


#### todo
1. 对日历控件的使用以及封装  
1. 对时间控件的使用以及封装
1. **...Web** Index View Create/edit/delete/Search  利用T4模板自动生成大部分代码，T4文件在/Models/T4
1. 企业微信推送消息