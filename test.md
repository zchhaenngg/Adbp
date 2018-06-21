概述
============

开始开发
============

接口
============

生成解锁码
------------

生成license
------------

生成Service
------------

登录
============

附录
=============

访问频率限制
------------------------

HttpStatusCode
401                 Unauthorized         Unauthorized indicates that the requested resource requires authentication
403                 Forbidden            indicates that the server refuses to fulfill the request.
404                 NotFound             1. No HTTP resource was found that matches the request URI      2. There is no such an entity. Entity type: {entityType.FullName}, id: {id}
500                 InternalServerError  indicates that a generic error has occurred on the server.
400                 BadRequest           1. the request could not be understood by the server 2.the validation fails


错误类型
------------------------
**全局错误码，前两位**
0x01  输入格式不正确
0xFF  应用程序内部错误  

全局错误码(HttpStatusCode:400)
------------------------
 **注意：开发者的程序应根据errcode来判断出错的情况，而不应该依赖errmsg来匹配，因为errmsg可能会调整。**
 错误码       错误说明                          备注
0x01000001    DongleId格式不正确                /^[0-9][-][0-9]{7}$/
0x01000002    非空校验失败                      The {0} value should not be null.
0x01000003                                      The ProductCode value should not be 0

常见问题-FAQ
------------------------
