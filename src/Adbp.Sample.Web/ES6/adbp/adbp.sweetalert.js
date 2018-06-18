var abp = abp || {};
(function ($) {
	if (!sweetAlert || !$) {
		throw "require jQuery, sweetAlert!";
	}
    abp.message = abp.message || {};

    abp.libs = abp.libs || {};
    abp.libs.swal = {
        config: {
            'default': {

            },
            info: {
                icon: 'info',
                button: "OK",
            },
            success: {
                icon: 'success',
                button: "OK",
            },
            warn: {
                icon: 'warning',
                button: "OK",
            },
            error: {
                icon: 'error',
                button: "OK",
            },
            confirm: {
                icon: 'warning',
                title: 'Are you sure?',
                buttons: ['Cancel', 'Yes']
            }
        }
    };

    function showMessage(options) {
        return $.Deferred(function ($dfd) {
            swal(options).then((value) => {
                if (value === true) {
                    $dfd.resolve(value);
                }
                else {
                    $dfd.reject(value);
                }
            });
        });
    }

    abp.message.info = function (message, title) {
        var opts = $.extend({}, abp.libs.swal.config.default, abp.libs.swal.config.info, {
            text: message,
            title: title
        });
        return showMessage(opts);
    };

    abp.message.success = function (message, title) {
        var opts = $.extend({}, abp.libs.swal.config.default, abp.libs.swal.config.success, {
            text: message,
            title: title
        });
        return showMessage(opts);
    };

    abp.message.warn = function (message, title) {
        var opts = $.extend({}, abp.libs.swal.config.default, abp.libs.swal.config.warn, {
            text: message,
            title: title
        });
        return showMessage(opts);
    };

    abp.message.error = function (message, title) {

        var opts = $.extend({}, abp.libs.swal.config.default, abp.libs.swal.config.error, {
            text: message,
            title: title
        });
        return showMessage(opts);
    };

    abp.message.confirm = function (message, title) {

        var opts = $.extend({}, abp.libs.swal.config.default, abp.libs.swal.config.confirm, {
            text: message,
            title: title
        });
        return showMessage(opts);
    };

    //abp.message.content = function (dom) {
        //当成form，获取所有输入信息以键值对的形式保存
        //不太好用，不能校验等等。综合考虑，暂停扩展
    //}

    abp.event.on('abp.dynamicScriptsInitialized', function () {
        abp.libs.swal.config.info.button = abp.localization.abpWeb('Yes');
        abp.libs.swal.config.success.button = abp.localization.abpWeb('Yes');
        abp.libs.swal.config.warn.button = abp.localization.abpWeb('Yes');
        abp.libs.swal.config.error.button = abp.localization.abpWeb('Yes');

		abp.libs.swal.config.confirm.title = abp.localization.abpWeb('AreYouSure');
		abp.libs.swal.config.confirm.buttons = [abp.localization.abpWeb('Cancel'), abp.localization.abpWeb('Yes')];
	});
})(jQuery);