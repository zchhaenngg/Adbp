"use strict";

(function () {

    window.table = new abp.table.server("#userIndex-table", {
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
                return data;
            }
        }, { data: 'Surname' }, { data: 'Name' }, {
            data: 'IsActive', render: function render(data, type, full, meta) {
                return data ? "是" : "否";
            }
        }, {
            data: 'CreationTime', render: function render(data, type, full, meta) {
                return abp.timing.datetimeStr(data);
            }
        }]
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "#btn-userIndex_edit, #btn-userIndex_delete", function (e, dt, type, indexes) {
        if (dt.isSingleSelected()) {
            if (!dt.singleSelected().IsStatic) {
                $(this).removeAttr("disabled");
            } else {
                $(this).attr("disabled", true);
            }
        } else {
            $(this).attr("disabled", true);
        }
    });
    table.show();

    $('#table-search').on('change', function () {
        table.show();
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
        $form.resetForm();

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
        if (RoleIds != null) {
            for (var i in RoleIds) {
                $form.find("[name=RoleIds][value='" + RoleIds[i] + "']").prop("checked", true);
            }
        }
    }

    $("#modal-user_edit").on("show.bs.modal", function () {
        var row = window.table.singleSelected();
        initEditModal(row);
    });

    $("#btn-userIndex_delete").on("click", function () {
        abp.message.confirm('', "确认删除用户！").done(function (value) {
            if (value) {
                var row = window.table.singleSelected();
                abp.services.app.user.delete(row.Id).done(function () {
                    abp.notify.success("操作成功！");
                    table.show();
                });
            }
        });
    });
})();