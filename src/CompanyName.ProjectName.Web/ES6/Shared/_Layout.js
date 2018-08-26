abp.event.on('abp.notifications.received', function ({ id, notification }) {
    switch (notification.notificationName) {
        case "Zero.NewUserRegistered":
            abp.notify.success('', "有新用户注册!");
            break;
        default:
            break;
    }
});