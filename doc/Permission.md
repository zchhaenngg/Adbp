权限
================

概述
----------------
用户发起请求，系统在执行业务逻辑之前，判断用户是否拥有该请求的行使权。

常见格式有 `Permissions.{Resource}` 和 `Permissions.{Resource}.{Action}`

数据源设置
==================
在表`SysObjectSetting`中设置各角色或组织对各数据源的访问级别，可设置 `Reject`，`Create`，`Retrieve`，`Update`，`Delete`。

默认用户只能看到`Owner`或`Creator`是自己的数据，其他所有数据都是被拒绝的。

附录
=================

常见权限
-----------------

+ 以`User`为例，

| 名称 | 使用场景 |  说明  |
|---|---|---|---|
| `Permissions.User` | 导航菜单 | 最基础的权限 |
| `Permissions.User.Retrieve` | 分页，详情 |  |
| `Permissions.User.Create` | 创建 | |
| `Permissions.User.Update` | 更新 | |
| `Permissions.User.Delete` | 删除 | |

