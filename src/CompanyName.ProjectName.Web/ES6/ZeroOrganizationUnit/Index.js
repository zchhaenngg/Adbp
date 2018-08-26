(function () {

    //init tree
    let tree = new abp.jstree('#organization-jstree')
        .useDefaultContextmenuItems();
    tree.data = function () {
        return abp.ajax({
            url: '/api/services/app/organizationUnit/GetOrganizationUnits'
        });
    }
    tree.createNode = function (obj, parentId, newName) {
        return abp.services.app.organizationUnit.createOrganizationUnit({
            DisplayName: newName,
            ParentId: parentId
        });
    };
    tree.deleteNode = function (obj, id) {
        return abp.services.app.organizationUnit.deleteOrganizationUnit(id);
    };
    tree.renameNode = function (obj, id, newName) {
        return abp.services.app.organizationUnit.updateOrganizationUnit({
            DisplayName: newName,
            Id: id
        });
    };
    tree.changeNode = function () {
        if (this.isSingleSelected()) {
            $(".organization-deselect").addClass("d-none");
            $(".organization-select").removeClass("d-none");
            $("#organization-display").html(this.singleSelected().displayName);
            if (this.singleSelected().isStatic) {
                $("#btn-organization_addUser").attr("disabled", "disabled");
            }
            else {
                $("#btn-organization_addUser").removeAttr("disabled");
            }
            showMembers();
        }
        else {
            $(".organization-deselect").removeClass("d-none");
            $(".organization-select").addClass("d-none");
        }
    }
    tree.show();
    //init table
    let table_members = new abp.table.server("#table-members", {
        "order": [[0, "desc"]],
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
                    if ($('#organization-jstree').data("adbp_jstree").singleSelected().isStatic) {
                        return '';
                    }
                    return `<button class="btn btn-default btn-xs btn-member_remove" title="删除">
                             <i class="fa fa-times" aria-hidden="true"></i>
                            </button>`;
                }
            },
            { data: 'UserName' },
            { data: 'Name' },
            { data: 'Surname' },
            {
                data: 'CreationTime', render: function (data, type, full, meta) {
                    return abp.timing.datetimeStr(data);
                }
            },
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
            { data: 'Surname' },
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
    }
});