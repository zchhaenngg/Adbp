'use strict';

var abp = abp || {};
(function () {
    if (!toastr) {
        throw "require toastr.js!";
    }
    toastr.options.positionClass = 'toast-bottom-right';

    abp.notify = abp.notify || {};

    var showNotification = function showNotification(type, message, title, options) {
        toastr[type](message, title, options);
    };

    abp.notify.success = function (message, title, options) {
        showNotification('success', message, title, options);
    };

    abp.notify.info = function (message, title, options) {
        showNotification('info', message, title, options);
    };

    abp.notify.warn = function (message, title, options) {
        showNotification('warning', message, title, options);
    };

    abp.notify.error = function (message, title, options) {
        showNotification('error', message, title, options);
    };
})();