var abp = abp || {};
(function ($) {
    if (!$) {
        throw "dependency jQuery!";
    }
    if (!$.fn.ajaxForm) {
        throw "dependency jquery.form!";
    }

    function isTextArea(selector) {
        let $selector = $(selector);
        if ($selector.length === 0) {
            throw "no element";
        }
        else if ($selector.length > 1) {
            return false;//checkbox
        }
        if ($selector.attr("type") === undefined) {
            return false;
        }
        return $selector[0].nodeName.toUpperCase() === "TEXTAREA"
            || $selector.attr("type").toUpperCase() === "TEXTAREA";
    }

    function isCheckBox(selector) {
        let $selector = $(selector);
        if ($selector.length === 0) {
            throw "no element";
        }
        if ($selector.attr("type") === undefined) {
            return false;
        }
        return $selector.attr("type").toUpperCase() === "CHECKBOX";
    }

    function isSelect(selector) {
        var $selector = $(selector);
        if ($selector.length === 0) {
            throw "no element";
        }
        return $selector[0].nodeName.toUpperCase() === "SELECT";
    }

    function validating(value, { name, r: required, minL, maxL, bReg, reg, minV, maxV }) {
        //name 提示的字段名称相当于field  校验失败返回false
        function _required(field, value) {
            if (typeof value !== "string") {
                throw new "argument type exception";
            }
            if (!value) {
                abp.notify.error(`${field} can not be empty`);
                return false;
            }
            return true;
        }
        function _minL(field, value, minL) {
            if (typeof value !== "string") {
                throw new "argument type exception";
            }
            if (value.length < minL) {
                abp.notify.error(`${field} at least ${minL} characters`);
                return false;
            }
            return true;
        }
        function _maxL(field, value, maxL) {
            if (value.length > maxL) {
                abp.notify.error(`the field ${field} cannot exceed ${maxL} characters`);
                return false;
            }
            return true;
        }

        if (required) {
            if (_required(name, value) === false) {
                return false;
            }
        }
        if (minL > 0) {
            if (_minL(name, value, minL) === false) {
                return false;
            }
        }
        if (maxL > 0) {
            if (_maxL(name, value, maxL) === false) {
                return false;
            }
        }
        if (bReg) {
            let r = abp.validate.regexps[bReg];
            if (r === undefined) {
                throw "bReg is not undefined";
            }
            if (!r.value.test(value)) {
                abp.notify.error(`field ${name}：${r.message}`);
                return false;
            }
        }
        return true;
    }

    abp.validate = function (container) {
        var params = $(container).formToArray();
        for (var i = 0; i < params.length; i++) {
            let { name, value } = params[i];
            let $e = $(container).find(`[name=${name}]`);
            if ($e.attr("type") !== undefined && $e.attr("type").toLowerCase() === "hidden") {
                continue;
            }
            if ($e.attr("readonly") !== undefined) {
                continue;
            }
            let opts = abp.validate.getOptions($e);
            if (validating(value, opts) === false) {
                return false;
            }
        }
        return true;
    };

    /*满足selector的第一个元素*/
    abp.validate.getOptions = function (selector) {

        function maxL() {
            if (isTextArea(selector)) {
                return 500;
            }
            if (isCheckBox(selector)) {
                return 100000;
            }
            if (isSelect(selector)) {
                return 100000;
            }
            return 20;
        }

        let opts = {
            name: $(selector).attr("name"),
            r: false,
            minL: 0,
            maxL: maxL(),
            bReg: false,
            reg: false,
            minV: false,
            maxV: false
        };
        let optionStr = $(selector).data("vd");
        if (optionStr) {
            let opts2 = eval("(" + optionStr + ")");
            return $.extend(opts, opts2);
        }
        return opts;
    };

    abp.validate.regexps = {
        n4: {
            value: /^(0|[1-9][0-9]{0,3})$/,//abp.localization.getSource("AbpZero")("CanNotDeleteStaticRole","hr")
            message: '只能输入0到9999'
        },
        n4f2: {
            value: /^(0|[1-9][0-9]{0,3})(\.[0-9]{1,2})?$/,
            message: "只能输入0到9999.99"
        },
        n9: {
            value: /^(0|[1-9][0-9]{0,8})$/,
            message: "最多输入9位的正整数或0"
        },
        telephone: {
            value: /^(1[0-9]{10})?$/,
            message: '请输入正确的手机号码'
        }
    };
})(jQuery);
