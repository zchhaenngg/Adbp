var adbp = adbp || {};

//生成唯一Id
function getComponentId(name) {
    for (let i = 1; i < 10000; i++) {
        let id = `Component__${name}_${i}`;
        if ($(`#${id}`).length > 0) {
            continue;//已存在
        }
        return id;
    }
}

//约束,每个template都要被一个div或其他标签包裹, 不允许为多个标签.
adbp.templates = {
    text: (function () {
        
        function Component() {
            
            this._options = {
                component: {
                    type: 'text',
                    id: getComponentId('text')//获取最大的 text
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
                    name, //校验时, 如果失败提示哪个字段报错的, 一般等于text
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
            let _this = this;
            let vd = (function () {
                let { name, r, minL, maxL, bReg, reg, minV, maxV, errorMsg } = _this._options.validation;
                return `{ name:'${name}', r:${r}, maxL:${maxL}, minL:${minL}, bReg:${bReg}, reg:${reg},minV:${minV}, maxV:${maxV},errorMsg:${errorMsg} }`;
            })();
            
            return `<div id="${this._options.component.id }">
                        <div class="form-group row">
                            <label class="${ this._options.label.classes }" title="${ this._options.label.title }">${ this._options.label.display }</label>
                            <div class="col">
                                <input id="${ this._options.input.id}" class="${this._options.input.classes}" type="${this._options.input.type}" name="${this._options.input.name}"  value="${this._options.input.value}"  placeholder="${this._options.input.placeholder}" data-vd="${vd}"/>
                            </div>
                        </div>
                    </div>`;
        };

        Component.prototype.replace = function (regionElement) {
            let htmlStr = `<div class="card mb-1 component">
                        <div class="card-header">
                            <button type="button" class="close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="card-body">
                            ${this.getTemplate()}
                        </div>
                    </div>`;

            $(htmlStr).insertAfter(regionElement);
            $(regionElement).remove();

            adbp.templates.utilities.setSelect(this._options.component.id);

            adbp.templates.utilities.bindClick();

            this.$component().data("adbp_templates", this);
        };

        Component.prototype.$component = function () {
            return $(`#${this._options.component.id}`).closest(".component");
        };

        //根据options 修改属性栏
        Component.prototype.render = function () {
            let $current = $(`#${this._options.component.id}`);
            let newHtml = this.getTemplate();

            $(newHtml).insertAfter($current);
            $current.remove();
        };
        
        return Component;
    })(),
    utilities: {
        getComponentId: function (container) {
            return $(container).find("[id^=Component__]").attr("id");
        },
        setSelect: function (componentId) {
            $("#design").find(".border-primary").removeClass("border-primary").removeClass("selected");
            $(`#${componentId}`).closest(".component").addClass("border-primary").addClass("selected");
        },
        bindClick: function () {
            $("#design .component").off("click").on("click", function () {
                let componentId = adbp.templates.utilities.getComponentId(this);
                adbp.templates.utilities.setSelect(componentId);
            });
        }
    }
};

//events  每个都要单独处理,并更新 对应的修改项
$("#area_properties").find("input, select").on("change", function () {
    
    let component = $("#design .component.selected").data("adbp_templates");
    component._options.label.display = this.value;
    component.render();
});
