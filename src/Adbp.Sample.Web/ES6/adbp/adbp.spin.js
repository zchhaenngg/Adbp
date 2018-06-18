var abp = abp || {};
(function ($) {
    if (!$) {
        throw "require jQuery!";
    }

    if (!abp.ui || !abp.ui.block) {
        throw "require abp.ui.block!";
    }
    
    abp.libs = abp.libs || {};

    abp.libs.spinjs = {
        defaultType: "medium",
        config: {
            'default': {
                lines: 11,
                length: 0,
                width: 10,
                radius: 20,
                corners: 1.0,
                trail: 60,
                speed: 1.2
            },
            small: {
                width: 4,
                radius: 7,
            },
            medium: {//default
                width: 10,
                radius: 20,
            }
            //large, larger, x-small, xx-small,...
        }
    };

    /*
     * type can be 'default', 'small', 'medium'
     */
    abp.ui.spin = function (elm, type, options) {
        elm = elm ? elm : document.body;
        type = type || abp.libs.spinjs.defaultType;
        options = options || {}
        var opts = $.extend({}, abp.libs.spinjs.config["default"], abp.libs.spinjs.config[type], options);
        $(elm).spin(opts);
    }

    abp.ui.clearSpin = function (elm) {
        elm = elm ? elm : document.body;
        $(elm).spin(false);
    }

    function parseElmOrConfig(elmOrConfig) {
        if (!elmOrConfig) {
            return {
                blockElement: document.body,
                spinElement: document.body,
                spinType: abp.libs.spinjs.defaultType
            }
        }
        else if (elmOrConfig.blockElement || elmOrConfig.spinElement || elmOrConfig.spinType) {
            //elmOrConfig is config
            return {
                blockElement: elmOrConfig.blockElement || document.body,
                spinElement: elmOrConfig.spinElement || document.body,
                spinType: elmOrConfig.spinType || abp.libs.spinjs.defaultType
            }
        }
        else {
            //elmOrConfig is elm
            return {
                blockElement: elmOrConfig,
                spinElement: elmOrConfig,
                spinType: abp.libs.spinjs.defaultType
            }
        }
    }

    //if not block please use abp.ui.spin directly.
    // example 1，abp.ui.setBusy()
    // example 2，abp.ui.setBusy("#spin-container")
    // example 3，
    //  abp.ui.setBusy({
    //      blockElement: "#spin-example",
    //      spinElement: "spin-container",
    //      spinType: "small"
    //  })
    abp.ui.setBusy = function (elmOrConfig, promise) {
        var obj = parseElmOrConfig(elmOrConfig);

        abp.ui.spin(obj.spinElement, obj.spinType);
        abp.ui.block(obj.blockElement);
        
        if (promise) {
            //promise
            if (promise.always) {
                promise.always(function () {
                    abp.ui.clearBusy(elmOrConfig);
                });
            }
            else if (promise['finally']) {
                promise['finally'](function () {
                    abp.ui.clearBusy(elmOrConfig);
                });
            }
        }
    };
    
    abp.ui.clearBusy = function (elmOrConfig) {
        var obj = parseElmOrConfig(elmOrConfig);

        abp.ui.unblock(obj.blockElement);
        abp.ui.clearSpin(obj.spinElement);
    };
})(jQuery);