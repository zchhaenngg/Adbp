'use strict';

abp.event.on('abp.notifications.received', function (_ref) {
    var id = _ref.id,
        notification = _ref.notification;

    switch (notification.notificationName) {
        case "Zero.NewUserRegistered":
            abp.notify.success('', "有新用户注册!");
            break;
        default:
            break;
    }
});