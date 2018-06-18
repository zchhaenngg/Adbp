"use strict";

/*
* FormSubmit aMomentExcuteOneTime
*/
(function ($) {

    abp.formSubmit = function () {
        function FormSubmit(formSelector, triggerElement, dtSelector) {
            this._formSelector = formSelector;
            this._triggerElement = triggerElement;
            this._dtSelector = dtSelector;

            this._showBsModal();
            $(this._formSelector).data("adbp_formSubmit", this);
        }

        /**
         * 返回值为false, 则不提交
         */
        FormSubmit.prototype.beforeSubmit = function () {
            return abp.validate(this._formSelector);
        };

        /**
         * form提交的各项参数，可以修改
         * @param {any} params
         */
        FormSubmit.prototype.data = function (params) {};

        FormSubmit.prototype.afterSubmit = function () {};

        FormSubmit.prototype.submit = function (action) {
            if (this.beforeSubmit() === false) {
                return;
            }
            var params = $(this._formSelector).deserialize();
            this.data(params);
            var _this = this;
            return abp.ajax({
                url: abp.appPath + action,
                type: 'POST',
                data: JSON.stringify(params)
            }).done(function () {
                abp.notify.success("操作成功！");
                $(_this._formSelector).resetForm();
                _this.hideModal();
                _this.refreshTable();
            }).done(function () {
                _this.afterSubmit();
            });
        };

        FormSubmit.prototype.hideModal = function () {
            $(this._formSelector).closest(".modal").modal("hide");
        };

        FormSubmit.prototype._showBsModal = function () {
            //let _this = this;
            //$(this._formSelector).closest(".modal").on("show.bs.modal", function () {
            //    $(_this._formSelector).resetForm();
            //});
        };

        FormSubmit.prototype.refreshTable = function () {
            if (!this._dtSelector) {
                return;
            }
            var dt = abp.table.datatable(this._dtSelector); //根据selector获取dt
            dt.show();
        };

        return FormSubmit;
    }();
    abp.formSubmit.get = function (formSelector) {
        return $(formSelector).data("adbp_formSubmit");
    };
    //只有第1次指派有效，interval后重置为未指派。
    abp.isDispatched = function () {

        function execute_key(name) {
            return "ameot_" + $.trim(name).toLowerCase();
        }

        function execute_times(element, name) {
            var times = $.data(element, execute_key(name));
            if (times === undefined) {
                return 0;
            }
            return times;
        }

        function set_execute_times(element, name, times) {
            $.data(element, execute_key(name), times);
        }

        function dispatched_options(options) {
            return $.extend({ milliseconds: 1500 }, options);
        }

        //即有效期间只有第一次有效，超过有效期间重置回未指派
        function dispatched_interval(options) {
            return dispatched_options(options).milliseconds;
        }

        function IsDispatched(element, name, options) {

            var times = execute_times(element, name);
            set_execute_times(element, name, ++times);

            var isDispatched = times !== 1;
            if (!isDispatched) {
                setTimeout(function () {
                    //等待重置回未指派
                    set_execute_times(element, name, 0);
                }, dispatched_interval(options));
            }
            if (isDispatched) {
                console.log("prevent");
            } else {
                console.log("dispatch ok!");
            }
            return isDispatched; //即将发生的指派如果是不是第1次则已指派过了
        }

        return IsDispatched;
    }();

    abp.escapeHtml = function (str) {
        var entityMap = {
            "&": "&amp;",
            "<": "&lt;",
            ">": "&gt;",
            '"': '&quot;',
            "'": '&#39;',
            "/": '&#x2F;',
            " ": '&nbsp;'
        };
        return String(str).replace(/[<>"'\/ &]/g, function (s) {
            return entityMap[s];
        });
    };

    abp.maxDisplay = function (str, maxLength, suffix) {
        if (!str) {
            return "";
        }
        suffix = suffix || "";
        return str.length <= maxLength ? str : str.substring(0, maxLength - suffix.length) + suffix;
    };
})(jQuery);
/*
 * menu form
 */
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({ 'trigger': 'hover' });
    //menu
    $(".adbp-menu-arrow").closest(".adbp-menu-item-content").on("click", function () {
        var $group = $(this).closest(".adbp-menu-item-group");
        var isOpen = $group.hasClass("adbp-menu-item-open");
        if (isOpen) {
            //close
            $group.removeClass("adbp-menu-item-open");
        } else {
            //open
            $group.addClass("adbp-menu-item-open");
        }
    });

    $(".adbp-ajaxForm").abpAjaxForm(function () {
        location.reload();
    });

    $(".adbp-formSubmit").each(function () {
        var $form = $(this).closest("form");
        var dtSelector = $form.data("dt");

        var formsubmit = new abp.formSubmit($form, this, dtSelector);
        formsubmit.data = function (params) {
            var $cheks = $form.find("[data-checkbox]");
            $cheks.each(function () {
                var name = $(this).attr("name");
                if (params[name] !== undefined) {
                    params[name] = params[name][0];
                }
            });
        };
    });

    $(".adbp-formSubmit").on("click", function () {
        if (abp.isDispatched(this, "click")) {
            return;
        }
        var $form = $(this).closest("form");
        var action = $.trim($form.attr("action"));
        var formsubmit = abp.formSubmit.get($form);
        formsubmit.submit(action);
    });

    abp.event.trigger('adbp.formsubmitInitialized');
});