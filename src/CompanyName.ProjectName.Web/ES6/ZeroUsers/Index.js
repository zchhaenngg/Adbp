(function () {

    window.table = new abp.table.server("#userIndex-table", {
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
            { data: 'UserName' },
            { data: 'Surname' },
            { data: 'Name' },
            {
                data: 'IsActive', render: function (data, type, full, meta) {
                    return data ? "是" : "否";
                }
            },
            {
                data: 'CreationTime', render: function (data, type, full, meta) {
                    return abp.timing.datetimeStr(data);
                }
            },
        ]
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "#btn-userIndex_edit", function (e, dt, type, indexes) {
        if (dt.isSingleSelected()) {
            $(this).removeAttr("disabled");
        }
        else {
            $(this).attr("disabled", true);
        }
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "#btn-userIndex_delete", function (e, dt, type, indexes) {
        if (dt.isSingleSelected()) {
            if (!dt.singleSelected().isStatic) {
                $(this).removeAttr("disabled");
            }
            else {
                $(this).attr("disabled", true);
            }
        }
        else {
            $(this).attr("disabled", true);
        }
    });
    table.show();

    $('#table-search').on('change', function () {
        table.show();
    });

    function initEditModal({ Id, UserName, Name, Surname, EmailAddress, IsActive, RoleIds }) {
        let $form = $("#modal-user_edit").find("form");
        $form.resetForm();

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
        if (RoleIds != null) {
            for (var i in RoleIds) {
                $form.find(`[name=RoleIds][value='${RoleIds[i]}']`).prop("checked", true);
            }
        }
    }

    $("#modal-user_edit").on("show.bs.modal", function () {
        var row = window.table.singleSelected();
        initEditModal(row);
    });

    $("#btn-userIndex_delete").on("click", function () {
        abp.message.confirm('', "确认删除用户！").done((value) => {
            if (value) {
                var row = window.table.singleSelected();
                abp.services.app.user.delete(row.Id).done(function () {
                    abp.notify.success("操作成功！");
                    table.show();
                });
            }
        })
    });
})();