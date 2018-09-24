(function () {

    let table = new abp.table.server("#userIndex-table", {
        "order": [[3, "desc"]],
        'ajax': {
            url: '/zerousers/getUsers',
            data: function (params) {
                params.search.value = $("#table-search").val();
                return params;
            }
        },
        columns: [
            {
                render: function (data, type, full, meta) {
                    return '';
                }
            },
            {
                data: 'UserName', render: function (data, type, full, meta) {
                    if (full.IsStatic) {
                        data += `<span class="badge badge-danger ml-2">Static</span>`;
                    }
                    return data;
                }
            },
            { data: 'Surname' },
            { data: 'Name' },
            {
                data: 'IsActive', render: function (data, type, full, meta) {
                    return data ? L("Yes") : L("No");
                }
            },
            {
                data: 'CreationTime', render: function (data, type, full, meta) {
                    return abp.timing.datetimeStr(data);
                }
            }
        ]
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "", function (e, dt, type, indexes) {
        controllers.edit(e, dt, type, indexes);
        controllers.delete(e, dt, type, indexes);
    });

    function initEditModal({ Id, UserName, Name, Surname, EmailAddress, IsActive, RoleIds }) {
        let $form = $("#modal-user_edit").find("form");

        $form.find("[name=Id]").val(Id);
        $form.find("[name=UserName]").val(UserName);
        $form.find("[name=Name]").val(Name);
        $form.find("[name=Surname]").val(Surname);
        $form.find("[name=EmailAddress]").val(EmailAddress);
        if (IsActive) {
            $form.find("[name=IsActive]").attr("checked", "checked");
        }
        else {
            $form.find("[name=IsActive]").removeAttr("checked");
        }
        if (RoleIds !== null) {
            for (var i in RoleIds) {
                $form.find(`[name=RoleIds][value='${RoleIds[i]}']`).prop("checked", true);
            }
        }
    }

    let controllers = {
        doInit: function () {
            table.show();

            $("#modal-user_create").on("show.bs.modal", function () {
                $(this).resetForm();
            });

            $("#modal-user_edit").on("show.bs.modal", function () {
                $(this).resetForm();

                let dt = $("#userIndex-table").data("adbp_dt");
                let row = dt.singleSelected();
                initEditModal(row);
            });

            $('#table-search').on('change', function () {
                table.show();
            });

            $("#btn-userIndex_delete").on("click", function () {
                abp.message.confirm('', L("AreYouSureToDelete")).done((value) => {
                    if (value) {
                        let dt = $("#userIndex-table").data("adbp_dt");
                        let { Id } = dt.singleSelected();
                        abp.services.app.user.delete(Id).done(function () {
                            abp.notify.success(L("SavedSuccessfully"));
                            table.show();
                        });
                    }
                });
            });
        },
        disable: function (dt, selector, permission) {
            if (abp.auth.isGranted(permission) && dt.isSingleSelected() && !dt.singleSelected().IsStatic) {
                $(selector).removeAttr("disabled");
            }
            else {
                $(selector).attr("disabled", true);
            }
        },
        edit: function (e, dt, type, indexes) {
            controllers.disable(dt, "#btn-userIndex_edit", "Permissions.User.Update");
        },
        delete: function (e, dt, type, indexes) {
            controllers.disable(dt, "#btn-userIndex_delete", "Permissions.User.Delete");
        }
    };
    controllers.doInit();
})();