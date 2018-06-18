"use strict";

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

abp.jstree = function ($) {
    if (!$.fn.jstree) {
        throw "require jquery.jstree!";
    }
    function JsTree(selector, options) {
        this.selector = selector;
        this.options = options || {};
        this.isUseDefaultContextmenuItems = false;

        $(this.selector).data("adbp_jstree", this);
    }

    JsTree.prototype.useDefaultContextmenuItems = function (menuOptions) {
        this.isUseDefaultContextmenuItems = true;
        return this;
    };

    JsTree.prototype.getDefaultContextmenuItems = function () {

        return {
            "create": {
                "separator_before": false,
                "separator_after": true,
                "_disabled": false,
                "label": "Create",
                "action": function action(data) {
                    var inst = $.jstree.reference(data.reference),
                        obj = inst.get_node(data.reference);
                    inst.create_node(obj, {}, "last", function (new_node) {
                        try {
                            new_node.isNew = true;
                            inst.edit(new_node);
                        } catch (ex) {
                            setTimeout(function () {
                                inst.edit(new_node);
                            }, 0);
                        }
                    });
                }
            },
            "rename": {
                "separator_before": false,
                "separator_after": false,
                "_disabled": false,
                "label": "Rename",
                "action": function action(data) {
                    var inst = $.jstree.reference(data.reference),
                        obj = inst.get_node(data.reference);
                    obj.isNew = false;
                    inst.edit(obj);
                }
            },
            "remove": {
                "separator_before": false,
                "icon": false,
                "separator_after": false,
                "_disabled": false,
                "label": "Delete",
                "action": function action(data) {
                    var inst = $.jstree.reference(data.reference),
                        obj = inst.get_node(data.reference);
                    if (inst.is_selected(obj)) {
                        inst.delete_node(inst.get_selected());
                    } else {
                        inst.delete_node(obj);
                    }
                }
            }
        };
    };

    JsTree.prototype.show = function () {
        if (this.isInitialised()) {
            this.destroy(); //未测试
        }
        var _this = this;
        this.getData().done(function (data) {
            $.extend(_this.options, {
                core: {
                    'data': data
                }
            });
            if (_this.isUseDefaultContextmenuItems) {
                $.extend(_this.options, {
                    'contextmenu': {
                        items: _this.getDefaultContextmenuItems()
                    }
                });
                $.extend(_this.options.core, {
                    'check_callback': true
                });
                if (_this.options.plugins === undefined) {
                    _this.options.plugins = [];
                }
                if (!_this.options.plugins.includes("contextmenu")) {
                    _this.options.plugins.push("contextmenu");
                }
            }

            $(_this.selector).jstree(_this.options).on('rename_node.jstree', function (event, obj) {
                _this.renameNode(obj, obj.node.id, obj.text).fail(function () {
                    obj.instance.refresh();
                });
            }).on('delete_node.jstree', function (event, obj) {
                $.Deferred(function ($dfd) {
                    abp.message.confirm("\u60A8\u786E\u5B9A\u8981\u5220\u9664 " + obj.node.text + " \u5417\uFF1F", function (ok) {
                        if (ok) {
                            $dfd.resolve(obj.node.id);
                        } else {
                            $dfd.reject.apply(this);
                        }
                    });
                }).done(function (id) {
                    _this.deleteNode(obj, id).fail(function () {
                        obj.instance.refresh();
                    });
                }).fail(function () {
                    obj.instance.refresh();
                });
            }).on('create_node.jstree', function (event, obj) {
                _this.createNode(obj, obj.parent, obj.node.text).done(function (response) {
                    obj.instance.set_id(obj.node, response.id);
                }).fail(function () {
                    obj.instance.refresh();
                });
            }).on("changed.jstree", function (e, data) {
                _this.changeNode(e, data);
            });
        });
    };

    JsTree.prototype.createNode = function (obj, parentId, newName) {
        //todo return defered;
    };

    JsTree.prototype.deleteNode = function (obj, id) {};
    JsTree.prototype.renameNode = function (obj, id, newName) {};

    JsTree.prototype.destroy = function () {
        $(this.selector).jstree('destroy');
    };

    JsTree.prototype.changeNode = function (e, data) {};

    JsTree.prototype.checkSingleSelected = function () {
        var arr = $("#organization-jstree").jstree(true).get_selected();
        if (arr.length == 0) {
            throw "No selected";
        }
        if (arr.length > 1) {
            throw "Multi selected";
        }
    };

    JsTree.prototype.isSingleSelected = function () {
        return $(this.selector).jstree(true).get_selected().length === 1;
    };

    JsTree.prototype.singleSelected = function () {
        this.checkSingleSelected();

        return this.allSelected()[0];
    };

    JsTree.prototype.singleSelectedId = function () {
        this.checkSingleSelected();
        var id = $("#organization-jstree").jstree(true).get_selected()[0];
        return eval(id);
    };

    JsTree.prototype.allSelected = function () {
        var arr = $("#organization-jstree").jstree(true).get_selected(true);
        return arr.map(function (x) {
            return x.original;
        });
    };

    JsTree.prototype.data = function () {};

    JsTree.prototype.getData = function () {
        var defered = this.data();
        if (defered === undefined || !defered.done) {
            throw "data must return defered obj";
        }

        var _this = this;
        return defered.then(function (result) {
            return result.map(function (x) {
                return _this.getTreeItem(x);
            });
        });
    };
    //如果返回值不是parentId,displayName则重写此方法即可
    JsTree.prototype.getTreeItem = function (dto) {
        return _extends({}, dto, {
            id: dto.id,
            parent: dto.parentId === null ? '#' : dto.parentId,
            text: dto.displayName
        });
    };

    JsTree.prototype.jstree = function () {
        //// get an existing instance (will not create new instance)
        return $(this.selector).jstree(true);
    };

    JsTree.prototype.isInitialised = function () {
        return $(this.selector).jstree(true) !== false;
    };
    return JsTree;
}(jQuery);