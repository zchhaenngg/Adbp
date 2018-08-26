"use strict";

var table = new abp.table.server("#selfLogins-table", {
    "columnDefs": null,
    "order": [[4, "desc"]],
    'ajax': {
        url: '/zeroanalasy/GetLoginAttempts',
        data: function data(params) {
            params.search.value = $("#table-search").val();
            return params;
        }
    },
    columns: [{ data: 'UserNameOrEmailAddress' }, { data: 'ClientIpAddress' }, { data: 'ClientName' }, {
        data: 'ResultStr', render: function render(data, type, full, meta) {
            return abp.localization.localize("ENUM_AbpLoginResultType_" + data, "AdbpZero");
        }
    }, {
        data: 'CreationTime', render: function render(data, type, full, meta) {
            return abp.timing.datetimeStr(data);
        }
    }]
}).setStyle("rtip");

table.show();

$("#table-search").on("change", function () {
    table.show();
});