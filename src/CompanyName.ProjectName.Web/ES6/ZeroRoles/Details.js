(function () {
    var table_members = new abp.table.client("#details-table_members", {
        "order": [[1, "asc"]],
        "columnDefs": null,
        columns: [
            {
                render: function (data, type, full, meta) {
                    return `<button class="btn btn-default btn-xs btn-member_remove" title="删除">
                             <i class="fa fa-times" aria-hidden="true"></i>
                            </button>`;
                }
            },
            { data: 'userName' },
            { data: 'name' },
            { data: 'surname' }
        ]
    }).setStyle("rtip");
    table_members.data = function () {
        return abp.services.app.role.getUserDtosInRole(RoleId);
    }
    table_members.afterDraw = function () {
        $(".btn-member_remove").off("click").on("click", function () {
            if (abp.isDispatched(this, "click")) {
                return;
            }

            let data = table_members.find($(this).closest("tr"));
            abp.services.app.role.removeFromRole(data.id, RoleId).done(function () {
                abp.notify.success("操作成功！");
                table_toadd.show();
                table_members.show();
            });
        });
    }
    table_members.show();

    var table_toadd = new abp.table.client("#details-table_toAdd", {
        "order": [[1, "asc"]],
        "columnDefs": null,
        columns: [
            {
                render: function (data, type, full, meta) {
                    return `<button class="btn btn-default btn-xs btn-member_toAdd" title="添加">
                             <i class="fa fa-plus" aria-hidden="true"></i>
                            </button>`;
                }
            },
            { data: 'userName' },
            { data: 'name' },
            { data: 'surname' }
        ]
    });
    table_toadd.data = function () {
        return abp.services.app.role.getUserDtosNotInRole(RoleId);
    }
    table_toadd.afterDraw = function () {
        $(".btn-member_toAdd").off("click").on("click", function () {
            if (abp.isDispatched(this, "click")) {
                return;
            }

            let data = table_toadd.find($(this).closest("tr"));
            abp.services.app.role.addToRole(data.id, RoleId).done(function () {
                abp.notify.success("操作成功！");
                table_toadd.show();
                table_members.show();
            });
        });
    }
    table_toadd.show();

    $('#table-members_search').on('keyup', function () {
        table_members.search(this.value);
    });

    $('#table-toAdd_search').on('keyup', function () {
        table_toadd.search(this.value);
    });
})();
(function () {
    var table_sysObjects = new abp.table.client("#table-details_sysObjects", {
        "order": [[1, "asc"]],
        "columnDefs": null,
        columns: [
            { data: 'sysObjectName' },
            { data: 'accessLevelStr' }
        ]
    })
        .contact(["draw.dt", "select.dt", "deselect.dt"], '', function (e, dt, type, indexes) {
            if (dt.isSingleSelected()) {
                $("#sysObject-selected").removeClass("d-none");
                $("#sysObject-selected").siblings(".row").addClass("d-none");
                var row = dt.singleSelected();
                initSetting(row);
                //SysObjectName AccessLevelInt
            }
            else {
                $("#sysObject-selected").addClass("d-none");
                $("#sysObject-selected").siblings(".row").removeClass("d-none");
            }
        }).setStyle("rtip");
    table_sysObjects.data = function () {
        return abp.ajax({
            url: "/ZeroRoles/GetSysObjectSettings",
            data: JSON.stringify({
                roleId: RoleId
            })
        });
    }
    table_sysObjects.show();
    $('#table-sysObjects_search').on('keyup', function () {
        table_sysObjects.search(this.value);
    });

    function initSetting({ sysObjectName, accessLevelInt }) {
        $("#SysObject_Name").val(sysObjectName);
        $("#sysObject-selected input[name=AccessLevel]").each(function () {
            let lev = eval($(this).val());
            let isChecked = (accessLevelInt & lev) > 0;
            $(this).prop("checked", isChecked);
        });
    }

    abp.event.on('adbp.formsubmitInitialized', function () {
        let s = $("#sysObject-selected_form").data("adbp_formSubmit");
        s.data = function (params) {
            //对AccessLevel值进行修改
            if (params.AccessLevel) {
                let al = 0;
                for (var i = 0; i < params.AccessLevel.length > 0; i++) {
                    al |= eval(params.AccessLevel[i]);
                }
                params.AccessLevel = al;
            }
        }
    });
})();