扩展 **OrganizationUnit**, **UserOrganizationUnit**
===================

ZeroOrganizationUnit 增加15个保留字段以及, 
-------------------
| 字段 | 必须 | 备注 |
|----|----|----|
| GroupCode| 否 | 组代码, 常用于根节点, 示意特定组织。 |
| Comments | 否 | 备注 |
| **IsStatic** | 必须 | 不可修改或删除Static记录 |

ZeroUserOrganizationUnit 增加15个保留字段以及,
----------------------
| 字段 | 必须 | 备注 |
|----|----|----|
| **IsStatic** | 必须 | 不可修改或删除Static记录 |


- 静态组织是不可以修改以及删除的

| Setting配置项 | 备注 | 默认 |
|----|----|----|
| 启用组织管理功能 | 关闭此功能后, 组织机构以及组织用户都只能读了 | 默认开启 |
| 是否允许添加根组织 | 允许用户在组织机构管理页面添加根组织 | 默认允许 |
| 是否允许添加Static组织的子组织 | ...添加Static组织的子组织 | 默认允许 |
| 允许添加的组织的最大层级数 | Depth Control | 默认16 |
| 允许向Static组织中添加用户 | ...向Static组织中添加用户 | 默认允许 |

