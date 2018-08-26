(function ($) {
    if (!$) {
        throw "dependency jQuery!";
    }
    if (!abp.ajax) {
        throw "dependency adbp.jQuery!";
    }
    if (!$.fn.ajaxForm) {
        throw "dependency jquery.form!";
    }
    if (!abp.validate) {
        throw "dependency adbp.validate!";
    }

    $.fn.abpAjaxForm = function (userOptions) {

        if (typeof userOptions === 'function') {
            userOptions = { success: userOptions };
        }
        else {
            userOptions = userOptions || {};
        }
        var options = $.extend({}, $.fn.abpAjaxForm.defaults, userOptions);

        options.beforeSubmit = function (params, $form) {
            abp.ajax.blockUI(options);

            if (abp.isDispatched($form[0], "click")) {
                return false;
            }

            var valid = userOptions.enableValidate !== false
                && abp.validate($form);
            if (valid === false) {
                return false;
            }
            return userOptions.beforeSubmit
                && userOptions.beforeSubmit.apply(this, arguments);
        };

        options.success = function (data) {
            abp.ajax.handleResponse(data, userOptions);
        };

        options.error = function (jqXHR) {
            if (jqXHR.responseJSON && jqXHR.responseJSON.__abp) {
                abp.ajax.handleResponse(jqXHR.responseJSON, userOptions, null, jqXHR);
            } else {
                abp.httpStatusNotOkResponse(jqXHR.status);
            }
        };

        options.complete = function () {
            abp.ajax.unblockUI(options);
            userOptions.complete && userOptions.complete.apply(this, arguments);
        };

        return this.ajaxForm(options);
    };

    $.fn.abpAjaxForm.defaults = {
        method: 'POST'
    };

    /*从当前对象中反序列化出对象*/
    $.fn.deserialize = function () {
        var data = $(this).formToArray();
        var obj = {};
        data.map(function (x) {
            if (x.type == "checkbox") {
                if (!obj[x.name]) {
                    obj[x.name] = [];
                }
                obj[x.name].push(x.value);
            }
            else {
                obj[x.name] = x.value;
            }
        });
        return obj;
    }
})(jQuery);