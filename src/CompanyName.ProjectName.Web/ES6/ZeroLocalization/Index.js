(function () {

    window.table = new abp.table.client("#table-localizations", {
        "order": [[1, "asc"], [3, "asc"]],
        columns: [
            {
                render: function (data, type, full, meta) {
                    return '';
                }
            },
            { data: 'source' },
            { data: 'name' },
            { data: 'enValue' },
            { data: 'zhValue' },
        ]
    }).contact(["draw.dt", "select.dt", "deselect.dt"], "#btn-localization_edit", function (e, dt, type, indexes) {
        if (dt.isSingleSelected()) {
            $(this).removeAttr("disabled");
        }
        else {
            $(this).attr("disabled", true);
        }
    });
    table.data = function () {
        return abp.ajax({
            url: abp.appPath + 'ZeroLocalization/GetAllLocalizedStrings'
        });
    }
    table.show();

    $('#table-localizations_search').on('keyup', function () {
        table.search(this.value);
    });

    function initEditModal({ source, name, enValue, zhValue }) {
        let $form = $("#modal-localization_edit").find("form");
        $form.resetForm();
        $form.find("[name=Source]").val(source);
        $form.find("[name=Name]").val(name);
        $form.find("[name=EnValue]").val(enValue);
        $form.find("[name=ZhValue]").val(zhValue);
    }

    $("#modal-localization_edit").on("show.bs.modal", function () {
        var row = window.table.singleSelected();
        initEditModal(row);
    });

})();