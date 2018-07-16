扩展OrganizationUnit
===================

增加字段
-------------------

| 字段 | 必须 | 备注 |
|----|----|----|
| GroupCode| 否 | 组代码, 常用于根节点, 示意特定组织。 |
| Comments | 否 | 备注 |
| IsStatic | 必须 | 类似于[Role](https://aspnetboilerplate.com/Pages/Documents/Zero/Role-Management)的设计, 一个静态的组织，是不可以被修改或删除的。 |

- 如何判断是否为静态的组织？
 
  找根节点。如果根节点是静态的就是静态的，子节点不作用途！