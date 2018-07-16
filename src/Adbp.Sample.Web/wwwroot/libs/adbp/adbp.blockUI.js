'use strict';

var abp = abp || {};
(function ($) {
    if (!$ || !$.blockUI) {
        throw "require jQuery, jQuery.blockUI!";
    }

    $.extend($.blockUI.defaults, {
        message: ' ',
        css: {},
        overlayCSS: {
            backgroundColor: '#AAA',
            opacity: 0.3,
            cursor: 'wait'
        }
    });

    abp.ui = abp.ui || {};

    abp.ui.block = function (elm, opts) {
        if (!elm) {
            $.blockUI(opts);
        } else {
            $(elm).block(opts);
        }
    };

    abp.ui.unblock = function (elm, opts) {
        if (!elm) {
            $.unblockUI(opts);
        } else {
            $(elm).unblock(opts);
        }
    };
})(jQuery);