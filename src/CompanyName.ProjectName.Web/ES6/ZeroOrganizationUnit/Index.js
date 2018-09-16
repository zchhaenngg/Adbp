﻿var _configs = {
    canAddRootOrganizationUnit: abp.setting.getBoolean("Adbp.Zero.OrganizationSettings.CanAddRootOrganizationUnit"),
    canAddUserInStaticOrganizationUnit: abp.setting.getBoolean("Adbp.Zero.OrganizationSettings.CanAddUserInStaticOrganizationUnit"),
    canAddChildOrganizationUnitInStaticOrganizationUnit: abp.setting.getBoolean("Adbp.Zero.OrganizationSettings.CanAddChildOrganizationUnitInStaticOrganizationUnit"),
    enableOrganizationUnitManagement: abp.setting.getBoolean("Adbp.Zero.OrganizationSettings.EnableOrganizationUnitManagement")
};

var _controller = (function () {
    let { canAddRootOrganizationUnit, canAddUserInStaticOrganizationUnit, enableOrganizationUnitManagement } = _configs;
    return {
        doInit: function () {
            let enableButton = enableOrganizationUnitManagement ?
                canAddRootOrganizationUnit ? true : false
                : false;
            if (enableButton) {
                $("#btn-root_add").removeAttr("disabled");
            }
            else {
                $("#btn-root_add").attr("disabled", "disabled");
            }
        },
        toggleOrganizationPanel: function () {
            var tree = $('#organization-jstree').data("adbp_jstree");
            if (tree.isSingleSelected()) {
                $(".organization-deselect").addClass("d-none");
                $(".organization-select").removeClass("d-none");
            }
            else {
                $(".organization-deselect").removeClass("d-none");
                $(".organization-select").addClass("d-none");
            }
        },
        toggle_addUserButton: function () {
            let enableButton = true;
            if (!enableOrganizationUnitManagement) {
                enableButton = false; 
            }
            else {
                //不允许添加Static组织的用户时,有且仅有ou是Static时, 才禁用添加用户功能
                if (!canAddUserInStaticOrganizationUnit) {
                    var tree = $('#organization-jstree').data("adbp_jstree");
                    if (tree.isSingleSelected() && tree.singleSelected().isStatic === true) {
                        enableButton = false;
                    }
                }
            }
            if (enableButton) {
                $("#btn-organization_addUser").removeAttr("disabled");
            }
            else {
                $("#btn-organization_addUser").attr("disabled", "disabled");
            }
        }
    };
})();
_controller.doInit();

(function () {
    let { canAddChildOrganizationUnitInStaticOrganizationUnit, enableOrganizationUnitManagement } = _configs;

    //init tree
    let tree = new abp.jstree('#organization-jstree');
    tree.data = function () {
        return abp.ajax({
            url: '/api/services/app/organizationUnit/GetOrganizationUnits'
        });
    };
    tree.getTreeItem = function (dto) {
        let text = dto.displayName;
        if (dto.isStatic === true) {
            text += `<span class="badge badge-danger ml-2">Static</span>`;
        }
        return {
            ...dto,
            id: dto.id,
            parent: dto.parentId === null ? '#' : dto.parentId,
            text: text

        };
    };
    tree.createNode = function (obj, parentId, newName) {
        if (obj.instance.get_node(obj.parent).original.isStatic && !canAddChildOrganizationUnitInStaticOrganizationUnit) {
            abp.notify.error("You can not add child OrganizationUnit In Static OrganizationUnit!");
            return $.Deferred(function ($dfd) {
                $dfd.reject();
            });
        }
        return abp.services.app.organizationUnit.createOrganizationUnit({
            DisplayName: newName,
            ParentId: parentId
        });
    };
    tree.deleteNode = function (obj, id) {
        return abp.services.app.organizationUnit.deleteOrganizationUnit(id);
    };
    tree.renameNode = function (obj, id, newName) {
        if (isNaN(id)) {
            return;//不是数字, 则忽略
        }
        if (obj.node.original.isStatic) {
            abp.notify.error("You can not rename Static OrganizationUnit Display Name!");
            return $.Deferred(function ($dfd) {
                $dfd.reject();
            });
        }
        return abp.services.app.organizationUnit.updateOrganizationUnit({
            DisplayName: newName,
            Id: id
        });
    };
    tree.changeNode = function () {
        _controller.toggle_addUserButton();
        _controller.toggleOrganizationPanel();

        if (this.isSingleSelected()) {
            $("#organization-display").html(this.singleSelected().displayName);
            showMembers();
        }
    };
    tree.show();
    //init table
    let table_members = new abp.table.server("#table-members", {
        "order": [],
        "columnDefs": null,
        'ajax': {
            url: '/zeroorganizationUnit/jsonGetOrganizationUserOuputs',
            data: function (params) {
                params.search.value = $("#table-search").val();
                params.organizationUnitId = tree.singleSelectedId();
                return params;
            }
        },
        columns: [
            {
                render: function (data, type, full, meta) {
                    if (!enableOrganizationUnitManagement || full.IsStatic === true) {
                        return '';
                    }
                    return `<button class="btn btn-default btn-xs btn-member_remove" title="删除">
                             <i class="fa fa-times" aria-hidden="true"></i>
                            </button>`;
                }
            },
            {
                data: 'UserName', render: function (data, type, full, meta) {
                    if (full.IsStatic === true) {
                        data += `<span class="badge badge-danger ml-2">Static</span>`;
                    }
                    return data;
                }
            },
            { data: 'Name' },
            { data: 'Surname' },
            {
                data: 'CreationTime', render: function (data, type, full, meta) {
                    return abp.timing.datetimeStr(data);
                }
            }
        ]
    }).setStyle("rtip");

    function showMembers() {
        table_members.show().done(function (result) {
            $("#table-members .btn-member_remove").off("click").on("click", function () {
                let data = table_members.find($(this).closest("tr"));
                console.log(data);
                abp.message.confirm(`您确定要将用户 ${data.Surname}${data.Name}(${data.UserName}) 从组织 ${data.OrganizationUnitName} 中移除吗？`).done(function (ok) {
                    if (ok) {
                        abp.services.app.organizationUnit.deleteOrganizationUnitUser(data.OrganizationUnitId, data.Id).done(function (rslt) {
                            abp.notify.success("操作成功");
                            showMembers();
                        });
                    }
                });
            });
        });
    }

    let table_toSelect = new abp.table.server("#table-toselect", {
        "order": [[0, "desc"]],
        "select": 'multi',
        'ajax': {
            url: '/zeroorganizationUnit/jsonGetUsersNotInOrganization',
            data: function (params) {
                params.search.value = $("#table-search_toselect").val();
                params.organizationUnitId = tree.singleSelectedId();
                return params;
            }
        },
        columns: [
            {
                render: function (data, type, full, meta) {
                    return '';
                }
            },
            { data: 'UserName' },
            { data: 'Name' },
            { data: 'Surname' }
        ]
    }).setStyle("rtip");

    $("#organization-modal_addUser-submit").on("click", function () {
        if (abp.isDispatched(this, "click")) {
            return;
        }
        var data = $("#table-toselect").data("adbp_dt").allSelected().map(function (x) {
            return {
                OrganizationUnitId: tree.singleSelectedId(),
                UserId: x.UserId
            };
        });
        abp.services.app.organizationUnit.addOrganizationUnitUsers(data).done(function (rslt) {
            abp.notify.success("操作成功");
            $("#organization-modal_addUser").modal("hide");
            showMembers();
        });
    });

    $("#organization-modal_addUser").on('shown.bs.modal', function (e) {
        table_toSelect.show();
    });

    $("#table-search").on("change", function () {
        if (abp.isDispatched(this, "change")) {
            return;
        }
        showMembers();
    });

    $("#table-search_toselect").on("change", function () {
        if (abp.isDispatched(this, "change")) {
            return;
        }
        table_toSelect.show();
    });
})();

abp.event.on('adbp.formsubmitInitialized', function () {
    let s = $("#index-form_createOrganizationUnit").data("adbp_formSubmit");
    s.afterSubmit = function () {
        let tree = $("#organization-jstree").data("adbp_jstree");
        tree.show();
    };
});