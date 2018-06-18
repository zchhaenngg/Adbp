var abp = abp || {};
(function () {
    if (!moment) {
        return;
    }

    abp.libs = abp.libs || {};
    abp.libs.moment = {
        locale: "en-us",//暂时无用
        format: {
            DATE: 'MM/DD/YYYY',
            TIME: "hh:mm:ss",
            DATETIME: "MM/DD/YYYY hh:mm:ss"
        },
        useUtc: false
    };

    moment.locale(abp.libs.moment.locale);

    abp.timing = abp.timing || {};

    /**
     * 时间戳也被称为Unix时间戳（Unix Timestamp）
     * time为时间戳则返回结果一致，字符串则根据时区显示是会有差异
     * @param {any} time
     */
    function getMomentTime(time) {
        if (!time) {
            return null;
        }
        if (abp.libs.moment.useUtc) {
            let localTime = moment.utc(time).toDate();
            return moment(localTime);
        }
        else {
            return moment(time);
        }
    }

    abp.timing.str = function (value, format) {
        return value ? getMomentTime(value).format(format) : "";
    }

    abp.timing.dateStr = function (value) {
        return abp.timing.str(value, abp.libs.moment.format.DATE);
    }

    abp.timing.timeStr = function (value) {
        return abp.timing.str(value, abp.libs.moment.format.TIME);
    }

    abp.timing.datetimeStr = function (value) {
        return abp.timing.str(value, abp.libs.moment.format.DATETIME);
    }

    abp.event.on('abp.dynamicScriptsInitialized', function () {
        abp.libs.moment.format.DATE = abp.setting.get("App.UiDateFormatting").replace(/d/g, 'D').replace(/y/g, 'Y');
        abp.libs.moment.format.TIME = abp.setting.get("App.UiTimeFormatting");
        abp.libs.moment.format.DATETIME = abp.setting.get("App.UiDateTimeFormatting").replace(/d/g, 'D').replace(/y/g, 'Y');
    });
})();