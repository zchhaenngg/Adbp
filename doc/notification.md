`[Abp_Notifications]`  是队列表，当订阅事件发生后，将通知插入到这张表中，执行完Job，删除该记录

`Abp_NotificationSubscriptions` 订阅表，用户订阅了通知，某条记录的通知...  基础表

`[Abp_TenantNotifications]`, `[Abp_UserNotifications]` 发生了一次事件则添加一条`[Abp_TenantNotifications]`记录，其中`[Abp_UserNotifications]`，描述了该通知对应的各个用户的是否已读情况。



### 通知订阅了`UpdateUser`通知的所有用户
```
await _notiticationPublisher.PublishAsync("UpdateUser", data: CreateNotificationData(user.Id), severity: NotificationSeverity.Success);
```

#### 通知订阅了`Id为5的这条记录的UpdateUser`通知的所有用户 
```
var entityIdentifier = new Abp.Domain.Entities.EntityIdentifier(typeof(User), 5);
await _notiticationPublisher.PublishAsync("UpdateUser", severity: NotificationSeverity.Success, entityIdentifier: entityIdentifier); 
```

以上实际为2个不同的通知，设计意图分别为只要用户信息发生变化就通知特定用户A，与只有某个用户的信息发生变化才通知特定用户B。