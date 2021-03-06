﻿var table = new abp.table.server("#logAttemptLog-table", {
    "columnDefs": null,
    "order": [[4, "desc"]],
    'ajax': {
        url: '/zeroanalasy/getUserLoginAttempts',
        data: function (params) {
            params.search.value = $("#table-search").val();
            return params;
        }
    },
    columns: [
        { data: 'UserNameOrEmailAddress' },
        { data: 'ClientIpAddress' },
        { data: 'ClientName' },
        {
            data: 'ResultStr', render: function (data, type, full, meta) {
                return abp.localization.localize("ENUM_AbpLoginResultType_" + data, "AdbpZero");
            }
        },
        {
            data: 'CreationTime', render: function (data, type, full, meta) {
                return abp.timing.datetimeStr(data);
            }
        },
    ]
}).setStyle("rtip");

table.show();

$("#table-search").on("change", function () {
    table.show();
});