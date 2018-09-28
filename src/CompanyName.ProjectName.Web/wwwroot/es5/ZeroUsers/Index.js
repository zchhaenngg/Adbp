"use strict";

(function () {

    var table = new abp.table.server("#userIndex-table", {
        "order": [[3, "desc"]],
        'ajax': {
            url: '/zerousers/getUsers',
            data: function data(params) {
                params.search.value = $("#table-search").val();
                return params;
            }
        },
        columns: [{
            render: function render(data, type, full, meta) {
                return '';
            }
        }, {
            data: 'UserName', render: function render(data, type, full, meta) {
                if (full.IsStatic) {
                    data += "<span class=\"badge badge-danger ml-2\">Static</span>";
                }
                return "<a href=\"/zerousers/details/" + full.Id + "\">" + data + "</a>";
            }
        }, { data: 'Surname' }, { data: 'Name' }, {
            data: 'IsActive', render: function render(data, type, full, meta) {
                return data ? L("Yes") : L("No");
            }
        }, {
            data: 'CreationTime', render: function render(data, type, full, meta) {
                return abp.timing.datetimeStr(data);
            }
        }]
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "", function (e, dt, type, indexes) {
        controllers.edit(e, dt, type, indexes);
        controllers.delete(e, dt, type, indexes);
    });

    function initEditModal(_ref) {
        var Id = _ref.Id,
            UserName = _ref.UserName,
            Name = _ref.Name,
            Surname = _ref.Surname,
            EmailAddress = _ref.EmailAddress,
            IsActive = _ref.IsActive,
            RoleIds = _ref.RoleIds;

        var $form = $("#modal-user_edit").find("form");

        $form.find("[name=Id]").val(Id);
        $form.find("[name=UserName]").val(UserName);
        $form.find("[name=Name]").val(Name);
        $form.find("[name=Surname]").val(Surname);
        $form.find("[name=EmailAddress]").val(EmailAddress);
        if (IsActive) {
            $form.find("[name=IsActive]").attr("checked", "checked");
        } else {
            $form.find("[name=IsActive]").removeAttr("checked");
        }
        if (RoleIds !== null) {
            for (var i in RoleIds) {
                $form.find("[name=RoleIds][value='" + RoleIds[i] + "']").prop("checked", true);
            }
        }
    }

    var controllers = {
        doInit: function doInit() {
            table.show();

            $("#modal-user_create").on("show.bs.modal", function () {
                $(this).resetForm();
            });

            $("#modal-user_edit").on("show.bs.modal", function () {
                $(this).resetForm();

                var dt = $("#userIndex-table").data("adbp_dt");
                var row = dt.singleSelected();
                initEditModal(row);
            });

            $('#table-search').on('change', function () {
                table.show();
            });

            $("#btn-userIndex_delete").on("click", function () {
                abp.message.confirm('', L("AreYouSureToDelete")).done(function (value) {
                    if (value) {
                        var dt = $("#userIndex-table").data("adbp_dt");

                        var _dt$singleSelected = dt.singleSelected(),
                            Id = _dt$singleSelected.Id;

                        abp.services.app.user.delete(Id).done(function () {
                            abp.notify.success(L("SavedSuccessfully"));
                            table.show();
                        });
                    }
                });
            });
        },
        disable: function disable(dt, selector, permission) {
            if (abp.auth.isGranted(permission) && dt.isSingleSelected() && !dt.singleSelected().IsStatic) {
                $(selector).removeAttr("disabled");
            } else {
                $(selector).attr("disabled", true);
            }
        },
        edit: function edit(e, dt, type, indexes) {
            controllers.disable(dt, "#btn-userIndex_edit", "Permissions.User.Update");
        },
        delete: function _delete(e, dt, type, indexes) {
            controllers.disable(dt, "#btn-userIndex_delete", "Permissions.User.Delete");
        }
    };
    controllers.doInit();
})();