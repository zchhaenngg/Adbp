"use strict";

var table = new abp.table.server("#auditLog-table", {
    "columnDefs": null,
    "order": [[0, "desc"]],
    'ajax': {
        url: '/zeroanalasy/getAuditLogs',
        data: function data(params) {
            params.search.value = $("#table-search").val();
            return params;
        }
    },
    columns: [{
        data: 'ExecutionTime', render: function render(data, type, full, meta) {
            return abp.timing.datetimeStr(data);
        }
    }, { data: 'UserStr' }, {
        data: 'ServiceName', render: function render(data, type, full, meta) {
            return "<p class=\"text-body\" data-toggle=\"popover\" title=\"ServiceName\" data-content=\"" + data + "\">" + data.split(".").reverse().shift() + "</p>";
        }
    }, { data: 'MethodName' }, {
        data: 'Parameters', render: function render(data, type, full, meta) {

            var htmlStr = "<p class=\"text-body\" data-toggle=\"popover\" title=\"Parameters\" data-content=" + abp.escapeHtml(data) + "\">" + abp.maxDisplay(data, 20, "...") + "</p>";
            return htmlStr;
        }
    }, {
        data: 'ExecutionDuration', render: function render(data, type, full, meta) {
            return data + " ms";
        }
    }, { data: 'ClientIpAddress' }, { data: 'ClientName' }, { data: 'BrowserInfo' }, {
        data: 'Exception', render: function render(data, type, full, meta) {
            if (!data) {
                return data;
            }
            var htmlStr = "<p class=\"text-body\" data-toggle=\"popover\" title=\"Parameters\" data-content=" + abp.escapeHtml(data) + "\">" + abp.maxDisplay(data, 30, "...") + "</p>";
            return htmlStr;
        }
    }]
}).setStyle("rtip");
table.afterDraw = function () {
    $('[data-toggle="popover"]').popover({ 'trigger': 'hover' });
};
table.show();

//table-search
$("#table-search").on("change", function () {
    table.show();
});