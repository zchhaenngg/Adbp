'use strict';

var adbp = adbp || {};

//生成唯一Id
function getComponentId(name) {
    for (var i = 1; i < 10000; i++) {
        var id = 'Component__' + name + '_' + i;
        if ($('#' + id).length > 0) {
            continue; //已存在
        }
        return id;
    }
}

//约束,每个template都要被一个div或其他标签包裹, 不允许为多个标签.
adbp.templates = {
    text: function () {

        function Component() {

            this._options = {
                component: {
                    type: 'text',
                    id: getComponentId('text') //获取最大的 text
                },
                label: {
                    classes: 'col-sm-2 col-form-label',
                    title: 'label title',
                    display: 'Display'
                },
                input: {
                    id: "",
                    type: "text", //text, number  只支持部分的,默认 text
                    classes: "form-control",
                    name: "",
                    value: "",
                    placeholder: ""
                },
                validation: {
                    name: name, //校验时, 如果失败提示哪个字段报错的, 一般等于text
                    r: false,
                    minL: -1,
                    maxL: 64,
                    bReg: '',
                    reg: '',
                    minV: '',
                    maxV: '',
                    errorMsg: ''
                }
            };
        }

        Component.prototype.getTemplate = function () {
            var _this = this;
            var vd = function () {
                var _this$_options$valida = _this._options.validation,
                    name = _this$_options$valida.name,
                    r = _this$_options$valida.r,
                    minL = _this$_options$valida.minL,
                    maxL = _this$_options$valida.maxL,
                    bReg = _this$_options$valida.bReg,
                    reg = _this$_options$valida.reg,
                    minV = _this$_options$valida.minV,
                    maxV = _this$_options$valida.maxV,
                    errorMsg = _this$_options$valida.errorMsg;

                return '{ name:\'' + name + '\', r:' + r + ', maxL:' + maxL + ', minL:' + minL + ', bReg:' + bReg + ', reg:' + reg + ',minV:' + minV + ', maxV:' + maxV + ',errorMsg:' + errorMsg + ' }';
            }();

            return '<div id="' + this._options.component.id + '">\n                        <div class="form-group row">\n                            <label class="' + this._options.label.classes + '" title="' + this._options.label.title + '">' + this._options.label.display + '</label>\n                            <div class="col">\n                                <input id="' + this._options.input.id + '" class="' + this._options.input.classes + '" type="' + this._options.input.type + '" name="' + this._options.input.name + '"  value="' + this._options.input.value + '"  placeholder="' + this._options.input.placeholder + '" data-vd="' + vd + '"/>\n                            </div>\n                        </div>\n                    </div>';
        };

        Component.prototype.replace = function (regionElement) {
            var htmlStr = '<div class="card mb-1 component">\n                        <div class="card-header">\n                            <button type="button" class="close">\n                                <span aria-hidden="true">&times;</span>\n                            </button>\n                        </div>\n                        <div class="card-body">\n                            ' + this.getTemplate() + '\n                        </div>\n                    </div>';

            $(htmlStr).insertAfter(regionElement);
            $(regionElement).remove();

            adbp.templates.utilities.setSelect(this._options.component.id);

            adbp.templates.utilities.bindClick();

            this.$component().data("adbp_templates", this);
        };

        Component.prototype.$component = function () {
            return $('#' + this._options.component.id).closest(".component");
        };

        //根据options 修改属性栏
        Component.prototype.render = function () {
            var $current = $('#' + this._options.component.id);
            var newHtml = this.getTemplate();

            $(newHtml).insertAfter($current);
            $current.remove();
        };

        return Component;
    }(),
    utilities: {
        getComponentId: function getComponentId(container) {
            return $(container).find("[id^=Component__]").attr("id");
        },
        setSelect: function setSelect(componentId) {
            $("#design").find(".border-primary").removeClass("border-primary").removeClass("selected");
            $('#' + componentId).closest(".component").addClass("border-primary").addClass("selected");
        },
        bindClick: function bindClick() {
            $("#design .component").off("click").on("click", function () {
                var componentId = adbp.templates.utilities.getComponentId(this);
                adbp.templates.utilities.setSelect(componentId);
            });
        }
    }
};

//events  每个都要单独处理,并更新 对应的修改项
$("#area_properties").find("input, select").on("change", function () {

    var component = $("#design .component.selected").data("adbp_templates");
    component._options.label.display = this.value;
    component.render();
});