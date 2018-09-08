"use strict";

(function () {
    //index页面
    var selector = {
        roleTable: "#rolesIndex-table",
        createModal: "#RoleCreateModal",
        createForm: "#roleCreateForm"
    };

    window.table = new abp.table.client(selector.roleTable, {
        "order": [[3, "desc"]],
        columns: [{
            render: function render(data, type, full, meta) {
                return '';
            }
        }, {
            data: 'name', render: function render(data, type, full, meta) {
                if (full.isStatic) {
                    data += "<span class=\"badge badge-danger ml-2\">Static</span>";
                }
                return "<a href=\"/zeroroles/details?roleId=" + full.id + "\">" + data + "</a>";
            }
        }, { data: 'displayName' }, {
            data: 'lastModificationTime', render: function render(data, type, full, meta) {
                return abp.timing.datetimeStr(data);
            }
        }]
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "#btn-roleIndex_edit", function (e, dt, type, indexes) {
        if (dt.isSingleSelected()) {
            $(this).removeAttr("disabled");
        } else {
            $(this).attr("disabled", true);
        }
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "#btn-roleIndex_delete", function (e, dt, type, indexes) {
        if (dt.isSingleSelected()) {
            if (!dt.singleSelected().isStatic) {
                $(this).removeAttr("disabled");
            } else {
                $(this).attr("disabled", true);
            }
        } else {
            $(this).attr("disabled", true);
        }
    });
    table.data = function () {
        return abp.ajax({
            url: '/zeroRoles/GetRoles'
        });
    };
    table.show();

    $(selector.createModal).on("show.bs.modal", function () {
        $(selector.createForm).resetForm();
    });

    $('#table-search').on('keyup', function () {
        table.search(this.value);
    });

    function initEditModal(_ref) {
        var id = _ref.id,
            name = _ref.name,
            displayName = _ref.displayName,
            description = _ref.description,
            permissions = _ref.permissions,
            isStatic = _ref.isStatic;

        $("#roleEditForm").resetForm();

        $("#roleEditForm [name=Id]").val(id);
        $("#roleEditForm [name=Name]").val(name);
        $("#roleEditForm [name=DisplayName]").val(displayName);
        $("#roleEditForm [name=Description]").val(description);
        if (permissions != null) {
            for (var i in permissions) {
                $("#roleEditForm [name=Permissions][value='" + permissions[i] + "']").prop("checked", true);
            }
        }
        if (isStatic) {
            $("#roleEditForm [name=Permissions]").attr("disabled", true);
        } else {
            $("#roleEditForm [name=Permissions]").removeAttr("disabled");
        }
    }

    $("#btn-roleIndex_edit").on("click", function () {
        var row = window.table.singleSelected();
        initEditModal(row);
        $("#RoleEditModal").modal("show");
    });

    $("#btn-roleIndex_delete").on("click", function () {
        abp.message.confirm('', "确认删除角色！").done(function (value) {
            if (value) {
                var row = window.table.singleSelected();
                abp.services.app.role.delete(row.id).done(function () {
                    abp.notify.success("操作成功！");
                    table.show();
                });
            }
        });
    });

    $(selector.createForm).abpAjaxForm(function () {
        $(selector.createForm).resetForm();
        $(selector.createModal).modal("hide");

        abp.notify.success("操作成功！");
        table.show();
    });
    $("#btn-editForm_submit").on("click", function () {
        if (abp.isDispatched(this, "click")) {
            return;
        }
        if (abp.validate("#roleEditForm") === false) {
            return false;
        }
        var role = $("#roleEditForm").deserialize();
        abp.services.app.role.updateRole(role).done(function () {
            $("#RoleEditModal").modal("hide");
            abp.notify.success("操作成功！");
            table.show();
        });
    });
})();