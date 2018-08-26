"use strict";

(function () {

    window.table = new abp.table.client("#table-notifications", {
        "order": [[1, "desc"]],
        columns: [{
            render: function render(data, type, full, meta) {
                return '';
            }
        }, { data: 'description' }, {
            data: 'isSubscribed', render: function render(data, type, full, meta) {
                if (data == null) {
                    return "";
                } else if (data) {
                    return abp.localization.localize("BOOL_ISSUBSCRIBED_TRUE", "AdbpZero");
                } else {
                    return abp.localization.localize("BOOL_ISSUBSCRIBED_FALSE", "AdbpZero");
                }
            }
        }]
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "#btn-notification_edit", function (e, dt, type, indexes) {
        if (dt.isSingleSelected()) {
            $(this).removeAttr("disabled");

            var row = dt.singleSelected();
            if (row.isSubscribed !== true) {
                $(this).attr("title", "订阅");
                $(this).find("i.fas").removeClass("text-danger").addClass("text-muted");
            } else {
                $(this).attr("title", "取消订阅");
                $(this).find("i.fas").removeClass("text-muted").addClass("text-danger");
            }
        } else {
            $(this).attr("disabled", true);
        }
    });
    table.data = function () {
        return abp.services.app.notification.getAllAvailable();
    };
    table.show();

    $('#table-notifications_search').on('keyup', function () {
        table.search(this.value);
    });

    $("#btn-notification_edit").on("click", function () {
        var row = window.table.singleSelected();
        if (row.isSubscribed) {
            abp.message.confirm('', "确认取消订阅！").done(function (value) {
                if (value) {

                    abp.services.app.notification.unsubscribe(row.name).done(function () {
                        abp.notify.success("操作成功！");
                        table.show();
                    });
                }
            });
        } else {
            abp.message.confirm('', "确认订阅！").done(function (value) {
                if (value) {

                    abp.services.app.notification.subscribe(row.name).done(function () {
                        abp.notify.success("操作成功！");
                        table.show();
                    });
                }
            });
        }
    });
})();
(function () {

    var tableRead = new abp.table.client("#table-notifications_read", {
        "order": [[1, "desc"]],
        "select": 'multi', //multi
        columns: [{
            render: function render(data, type, full, meta) {
                return '';
            }
        }, { data: 'description' }, {
            data: 'severityStr', render: function render(data, type, full, meta) {
                return abp.localization.localize("ENUM_NotificationSeverity_" + data, "AdbpZero");
            }
        }, {
            data: 'stateStr', render: function render(data, type, full, meta) {
                return abp.localization.localize("ENUM_UserNotificationState_" + data, "AdbpZero");
            }
        }, {
            data: 'creationTime', render: function render(data, type, full, meta) {
                return abp.timing.datetimeStr(data);
            }
        }]
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "#btn-notification_read, #btn-notification_unread", function (e, dt, type, indexes) {
        if (dt.isSelected()) {
            $(this).removeAttr("disabled");
        } else {
            $(this).attr("disabled", true);
        }
    });
    tableRead.data = function () {
        return abp.services.app.notification.getUserNotifications();
    };
    tableRead.show();

    $('#table-notifications_read-search').on('keyup', function () {
        tableRead.search(this.value);
    });

    $("#btn-notification_read").on("click", function () {
        var ids = tableRead.allSelected().map(function (x) {
            return x.id;
        });
        abp.services.app.notification.readNotifications(ids).done(function () {
            tableRead.show();
        });
    });

    $("#btn-notification_unread").on("click", function () {
        var ids = tableRead.allSelected().map(function (x) {
            return x.id;
        });
        abp.services.app.notification.unreadNotifications(ids).done(function () {
            tableRead.show();
        });
    });
})();