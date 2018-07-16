"use strict";

var abp = abp || {};
(function ($) {
    if (!$) {
        throw "require jQuery!";
    }
    if (!$.fn.DataTable) {
        throw "require jquery.datatables!";
    }
    $.fn.dataTable.ext.errMode = 'none';

    var libs = {
        defaultOption: {
            'dom': 'rtp', //'lfrtip'
            "select": 'single', //multi
            "autoWidth": false,
            "columnDefs": [{ // null
                orderable: false,
                className: 'select-checkbox',
                targets: 0
            }]
        },
        isInitialised: function isInitialised(dtSelectorOrdt) {
            //dtSelectorOrdt can be a table node or DataTable
            return $.fn.DataTable.isDataTable(dtSelectorOrdt);
        },
        checkDataTable: function checkDataTable(dt) {
            if (!libs.isInitialised(dt) || !dt.hasOwnProperty("tables")) {
                throw "dt should be DataTable";
            }
        },
        isSelected: function isSelected(dt) {
            //if there there is one or more items in the result set
            libs.checkDataTable(dt);

            return dt.rows('.selected').any();
        },
        isSingleSelected: function isSingleSelected(dt) {
            libs.checkDataTable(dt);

            return dt.rows(".selected").data().length == 1;
        },
        singleSelected: function singleSelected(dt) {
            //if no selected or select more than 1 rows throw error
            libs.checkDataTable(dt);

            var data = dt.rows(".selected").data();
            if (data.length == 0) {
                throw "No row is selected!";
            }
            if (data.length > 1) {
                throw "More than 1 row are selected!";
            }
            return data[0]; //{name:xxxx, description:xxxxx}
        },
        allSelected: function allSelected(dt) {
            //if no selected return []
            libs.checkDataTable(dt);

            return dt.rows(".selected").data().toArray();
        },
        clearSelected: function clearSelected(dt) {
            libs.checkDataTable(dt);

            dt.rows().deselect();
        },
        find: function find(dt, rowSelector) {
            libs.checkDataTable(dt);
            return dt.row(rowSelector).data();
        }
    };

    abp.table = abp.table || {};
    abp.table.client = function () {

        function ClientTable(selector, options) {
            this._selector = selector;
            this._options = $.extend({}, libs.defaultOption, options);
            this._contacts = [];

            $(this._selector).data("adbp_dt", this);
        }

        ClientTable.prototype.data = function () {};

        ClientTable.prototype.setStyle = function (type) {
            if (type === 'full' || type === undefined) {
                this._options["dom"] = "lfrtip";
            } else {
                this._options["dom"] = type;
            }
            return this;
        };

        ClientTable.prototype.clear = function () {
            this.DataTable.clear().draw();
        };
        /**
         * if DataTables event happened, trigger func
         * @param {any} type
         * @param {any} selector
         * @param {e, dt, type, indexes} func
         */
        ClientTable.prototype.contact = function (type, selector, func) {

            this._contacts.push({
                'type': type,
                'selector': selector,
                'func': func
            });
            return this;
        };

        ClientTable.prototype.afterDraw = function () {
            console.log("invoke after draw");
        };

        ClientTable.prototype.show = function () {

            var _this = this;
            function draw(data) {
                if (!_this.DataTable) {
                    //未初始化过
                    if (data) {
                        _this._options.data = data;
                    }
                    _this.DataTable = $(_this._selector).DataTable(_this._options);
                    _this._callContacts();
                } else {
                    _this.DataTable.clear().rows.add(data).draw();
                }
            }
            var defered = this.data();
            if (defered !== undefined && defered.done) {
                return defered.done(function (_ref) {
                    var items = _ref.items;

                    if (items instanceof Array) {
                        draw(items);
                    } else if (arguments[0] instanceof Array) {
                        draw(arguments[0]);
                    } else {
                        throw "result unexpected!";
                    }
                });
            } else {
                draw();
                return $.Deferred(function ($dfd) {
                    $dfd.resolve();
                });
            }
        };

        ClientTable.prototype._callContacts = function () {
            this.checkInitialised();

            var _this = this;
            var drawContacts = this._contacts.filter(function (x) {
                return x.type.includes('draw.dt');
            });
            var selectContacts = this._contacts.filter(function (x) {
                return x.type.includes('select.dt');
            });
            var deselectContacts = this._contacts.filter(function (x) {
                return x.type.includes('deselect.dt');
            });

            _this.afterDraw(); //初始化时，不会触发afterDraw
            this.DataTable.on('draw.dt', function (e, settings) {
                _this.afterDraw();

                drawContacts.forEach(function (contact) {
                    contact.func.call($(contact.selector), e, _this, "table", null);
                });
            });

            this.DataTable.on('select.dt', function (e, dt, type, indexes) {
                selectContacts.forEach(function (contact) {
                    contact.func.call($(contact.selector), e, _this, type, indexes);
                });
            });

            this.DataTable.on('deselect.dt', function (e, dt, type, indexes) {
                deselectContacts.forEach(function (contact) {
                    contact.func.call($(contact.selector), e, _this, type, indexes);
                });
            });
        };

        ClientTable.prototype.isInitialised = function () {
            return !this.DataTable == false;
        };
        /**
         * if not initialised throw error
         */
        ClientTable.prototype.checkInitialised = function () {
            if (!this.isInitialised()) {
                throw "DataTable has not been initialised";
            }
        };

        ClientTable.prototype.isSelected = function () {
            this.checkInitialised();

            return libs.isSelected(this.DataTable);
        };
        ClientTable.prototype.isSingleSelected = function () {
            this.checkInitialised();

            return libs.isSingleSelected(this.DataTable);
        };

        ClientTable.prototype.singleSelected = function () {
            this.checkInitialised();

            return libs.singleSelected(this.DataTable);
        };
        ClientTable.prototype.allSelected = function () {
            this.checkInitialised();

            return libs.allSelected(this.DataTable);
        };

        ClientTable.prototype.clearSelected = function () {
            this.checkInitialised();

            return libs.clearSelected(this.DataTable);
        };

        ClientTable.prototype.search = function (value) {
            this.checkInitialised();

            this.DataTable.search(value).draw();
        };

        ClientTable.prototype.find = function (rowSelector) {
            this.checkInitialised();

            return libs.find(this.DataTable, rowSelector);
        };
        return ClientTable;
    }();

    abp.table.server = function () {
        function ServerTable(selector, options) {

            this._selector = selector;
            this._options = $.extend({
                'serverSide': true
            }, libs.defaultOption, options);

            if (this._options.ajax && !this._options.ajax['type']) {
                this._options.ajax['type'] = 'POST';
            }
            this._contacts = [];

            $(this._selector).data("adbp_dt", this);
            $(this._selector).on('error.dt', function (e, settings, techNote, message) {
                abp.notify.error("please check your permission(table)!");
            });
        }

        ServerTable.prototype.setStyle = function (type) {
            if (type === 'full' || type === undefined) {
                this._options["dom"] = "lfrtip";
            } else {
                this._options["dom"] = type;
            }
            return this;
        };
        /**
         * if DataTables event happened, trigger func
         * @param {any} type
         * @param {any} selector
         * @param {e, dt, type, indexes} func
         */
        ServerTable.prototype.contact = function (type, selector, func) {

            this._contacts.push({
                'type': type,
                'selector': selector,
                'func': func
            });
            return this;
        };
        ServerTable.prototype.isInitialised = function () {
            return !this.DataTable == false;
        };
        /**
        * if not initialised throw error
        */
        ServerTable.prototype.checkInitialised = function () {
            if (!this.isInitialised()) {
                throw "DataTable has not been initialised";
            }
        };
        ServerTable.prototype.isSelected = function () {
            this.checkInitialised();

            return libs.isSelected(this.DataTable);
        };
        ServerTable.prototype.isSingleSelected = function () {
            this.checkInitialised();

            return libs.isSingleSelected(this.DataTable);
        };

        ServerTable.prototype.singleSelected = function () {
            this.checkInitialised();

            return libs.singleSelected(this.DataTable);
        };
        ServerTable.prototype.allSelected = function () {
            this.checkInitialised();

            return libs.allSelected(this.DataTable);
        };

        ServerTable.prototype.clearSelected = function () {
            this.checkInitialised();

            return libs.clearSelected(this.DataTable);
        };

        ServerTable.prototype.search = function (value) {
            this.DataTable.search(value).draw();
        };

        ServerTable.prototype.find = function (rowSelector) {
            this.checkInitialised();

            return libs.find(this.DataTable, rowSelector);
        };

        ServerTable.prototype.afterDraw = function () {
            console.log("invoke after draw");
        };

        //render or reload
        ServerTable.prototype.show = function () {
            var _this = this;
            if (this.isInitialised()) {
                //未初始化过
                return this._refresh();
            } else {
                return this._render();
            }
        };

        ServerTable.prototype._callContacts = function () {
            this.checkInitialised();

            var _this = this;
            var drawContacts = this._contacts.filter(function (x) {
                return x.type.includes('draw.dt');
            });
            var selectContacts = this._contacts.filter(function (x) {
                return x.type.includes('select.dt');
            });
            var deselectContacts = this._contacts.filter(function (x) {
                return x.type.includes('deselect.dt');
            });

            _this.afterDraw(); //初始化时，不会触发afterDraw
            this.DataTable.on('draw.dt', function (e, settings) {
                _this.afterDraw();

                drawContacts.forEach(function (contact) {
                    contact.func.call($(contact.selector), e, _this, "table", null);
                });
            });

            this.DataTable.on('select.dt', function (e, dt, type, indexes) {
                selectContacts.forEach(function (contact) {
                    contact.func.call($(contact.selector), e, _this, type, indexes);
                });
            });

            this.DataTable.on('deselect.dt', function (e, dt, type, indexes) {
                deselectContacts.forEach(function (contact) {
                    contact.func.call($(contact.selector), e, _this, type, indexes);
                });
            });
        };

        ServerTable.prototype._render = function () {

            var _this = this;
            return $.Deferred(function ($dfd) {
                abp.ui.setBusy();
                var options = $.extend({}, _this._options, {
                    initComplete: function initComplete(settings, json) {
                        abp.ui.clearBusy();
                        if ($.isFunction(_this._options.initComplete)) {
                            _this._options.initComplete.call(this, settings, json);
                        }
                        $dfd.resolve(json);
                    }
                });
                _this.DataTable = $(_this._selector).DataTable(options);
                _this._callContacts();
            });
        };
        /**
         * In an environment where the data shown in the table can be updated at the server-side, it is often useful to be able to reload the table, showing the latest data. This method provides exactly that ability, making an Ajax request to the already defined URL (use ajax.url() if you need to alter the URL).
         * 
         */
        ServerTable.prototype._refresh = function () {
            var _this = this;
            abp.ui.setBusy();
            return $.Deferred(function ($dfd) {
                _this.DataTable.ajax.reload(function (result) {
                    abp.ui.clearBusy();
                    $dfd.resolve(result);
                }, true); //Reset(default action or true) or hold the current paging position(false). 
            });
        };

        return ServerTable;
    }();

    abp.table.datatable = function (selector) {
        return $(selector).data("adbp_dt");
    };
})(jQuery);