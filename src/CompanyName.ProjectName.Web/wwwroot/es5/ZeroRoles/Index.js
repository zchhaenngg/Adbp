"use strict";

(function () {

    function initEditModal(_ref) {
        var id = _ref.id,
            name = _ref.name,
            displayName = _ref.displayName,
            description = _ref.description,
            isStatic = _ref.isStatic;

        $("#roleEditForm").resetForm();

        $("#roleEditForm [name=Id]").val(id);
        $("#roleEditForm [name=IsStatic]").val(isStatic);

        $("#roleEditForm [name=Name]").val(name);
        $("#roleEditForm [name=DisplayName]").val(displayName);
        $("#roleEditForm [name=Description]").val(description);

        if (isStatic) {
            $("#roleEditForm [name=Name]").prop("disabled", true);
        } else {
            $("#roleEditForm [name=Name]").removeAttr("disabled");
        }
    }

    var table = new abp.table.client("#rolesIndex-table", {
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
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "", function (e, dt, type, indexes) {
        controllers.edit(e, dt, type, indexes);
        controllers.delete(e, dt, type, indexes);
        controllers.toggle(e, dt, type, indexes);
        controllers.permissions(e, dt, type, indexes);
    });
    table.data = function () {
        return abp.ajax({
            url: '/zeroRoles/GetRoles'
        });
    };

    var controllers = {
        doInit: function doInit() {
            //render table
            table.show();

            $("#RoleCreateModal").on("show.bs.modal", function () {
                $(this).resetForm();
            });

            $("#RoleEditModal").on("show.bs.modal", function () {
                $(this).resetForm();

                var dt = $("#rolesIndex-table").data("adbp_dt");
                var row = dt.singleSelected();
                initEditModal(row);
            });

            $('#table-search').on('keyup', function () {
                table.search(this.value);
            });

            $("#btn-roleIndex_delete").on("click", function () {
                abp.message.confirm('', L("AreYouSureToDelete")).done(function (value) {
                    if (value) {
                        var dt = $("#rolesIndex-table").data("adbp_dt");
                        var row = dt.singleSelected();
                        abp.services.app.role.delete(row.id).done(function () {
                            abp.notify.success(L("SavedSuccessfully"));
                            table.show();
                        });
                    }
                });
            });
        },
        disable: function disable(dt, selector, permission) {
            if (abp.auth.isGranted(permission) && dt.isSingleSelected()) {
                $(selector).removeAttr("disabled");
            } else {
                $(selector).attr("disabled", true);
            }
        },
        edit: function edit(e, dt, type, indexes) {
            controllers.disable(dt, "#btn-roleIndex_edit", "Permissions.Role.Update");
        },
        delete: function _delete(e, dt, type, indexes) {
            controllers.disable(dt, "#btn-roleIndex_delete", "Permissions.Role.Delete");
        },
        toggle: function toggle(e, dt, type, indexes) {
            if (dt.isSingleSelected()) {
                $(".role-select").removeClass("d-none");
                $(".role-deselect").addClass("d-none");
            } else {
                $(".role-select").addClass("d-none");
                $(".role-deselect").removeClass("d-none");
            }
        },
        permissions: function permissions(e, dt, type, indexes) {
            var $container = $(".accordionPermissions");
            $container.resetForm();
            $container.find("[name=Permissions]").removeAttr("disabled");
            $container.find("span.badge").html("");

            if (dt.isSingleSelected()) {
                var _dt$singleSelected = dt.singleSelected(),
                    id = _dt$singleSelected.id;

                $container.find("[name=Id]").val(id);

                abp.services.app.role.getRolePermissionDtos(id).done(function (items) {
                    for (var i in items) {
                        var _items$i = items[i],
                            isGranted = _items$i.isGranted,
                            isStatic = _items$i.isStatic,
                            permissionName = _items$i.permissionName;

                        var $permission = $container.find("[name=Permissions][value='" + permissionName + "']");
                        if (isGranted) {
                            $permission.prop("checked", true);
                        }
                        if (isStatic) {
                            $permission.prop("disabled", true);
                            $permission.next().find("span.badge").html("Static");
                        }
                    }
                });
            }
        }
    };

    controllers.doInit();
})();