"use strict";

var abp = abp || {};
(function ($) {
    if (!$) {
        throw "dependency jQuery!";
    }

    function handleHttpStatusNotOkResponse(status) {
        switch (status) {
            case 401:
                abp.ajax.handleUnAuthorizedRequest(abp.ajax.showError(abp.ajax.defaultError401), abp.appPath);
                break;
            case 403:
                abp.ajax.showError(abp.ajax.defaultError403);
                break;
            case 404:
                abp.ajax.showError(abp.ajax.defaultError404);
                break;
            default:
                abp.ajax.showError(abp.ajax.defaultError);
                break;
        }
    }

    abp.httpStatusNotOkResponse = handleHttpStatusNotOkResponse;

    abp.ajax = function (userOptions) {
        userOptions = userOptions || {};
        //第一个参数boolean，true为属性为对象时是否进行深度拷贝（合并对象属性）。
        //为false或extend对象时，不进行深度拷贝，对应属性为对象时，直接完整替换。
        var options = $.extend(true, {}, abp.ajax.defaultOpts, userOptions);
        var oldBeforeSend = options.beforeSend;
        options.beforeSend = function (xhr) {
            if (oldBeforeSend && oldBeforeSend(xhr) === false) {
                return false;
            }

            xhr.setRequestHeader("Pragma", "no-cache");
            xhr.setRequestHeader("Cache-Control", "no-cache");
            xhr.setRequestHeader("Expires", "Sat, 01 Jan 2000 00:00:00 GMT");
        };
        options.success = undefined;
        options.error = undefined;

        return $.Deferred(function ($dfd) {
            $.ajax(options).done(function (data, textStatus, jqXHR) {
                if (data.__abp) {
                    abp.ajax.handleResponse(data, userOptions, $dfd, jqXHR);
                } else {
                    //not wrapped result
                    $dfd.resolve(data);
                    userOptions.success && userOptions.success(data);
                }
            }).fail(function (jqXHR) {
                if (jqXHR.responseJSON && jqXHR.responseJSON.__abp) {
                    abp.ajax.handleResponse(jqXHR.responseJSON, userOptions, $dfd, jqXHR);
                } else {
                    abp.ajax.handleNonAbpErrorResponse(jqXHR, userOptions, $dfd);
                }
            });
        });
    };
    $.extend(abp.ajax, {
        defaultOpts: {
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        },
        defaultError: {
            message: 'An error has occurred!',
            details: 'Error detail not sent by server.'
        },
        defaultError401: {
            message: 'You are not authenticated!',
            details: 'You should be authenticated (sign in) in order to perform this operation.'
        },
        defaultError403: {
            message: 'You are not authorized!',
            details: 'You are not allowed to perform this operation.'
        },
        defaultError404: {
            message: 'Resource not found!',
            details: 'The resource requested could not found on the server.'
        },
        logError: function logError(error) {
            abp.log.error(error);
        },
        showError: function showError(error) {
            if (error.details) {
                return abp.message.error(error.details, error.message);
            } else {
                return abp.message.error(error.message || abp.ajax.defaultError.message);
            }
        },
        handleTargetUrl: function handleTargetUrl(targetUrl) {
            if (!targetUrl) {
                location.href = abp.appPath;
            } else {
                location.href = targetUrl;
            }
        },
        handleNonAbpErrorResponse: function handleNonAbpErrorResponse(jqXHR, userOptions, $dfd) {
            if (userOptions.abpHandleError !== false) {
                handleHttpStatusNotOkResponse(jqXHR.status);
            }

            $dfd.reject.apply(this, arguments);
            userOptions.error && userOptions.error.apply(this, arguments);
        },
        handleUnAuthorizedRequest: function handleUnAuthorizedRequest(messagePromise, targetUrl) {
            if (messagePromise) {
                messagePromise.done(function () {
                    abp.ajax.handleTargetUrl(targetUrl);
                });
            } else {
                abp.ajax.handleTargetUrl(targetUrl);
            }
        },
        handleResponse: function handleResponse(data, userOptions, $dfd, jqXHR) {
            //跳转到某个新页面、
            if (data) {
                if (data.success === true) {
                    $dfd && $dfd.resolve(data.result, data, jqXHR);
                    userOptions.success && userOptions.success(data.result, data, jqXHR);

                    if (data.targetUrl) {
                        abp.ajax.handleTargetUrl(data.targetUrl);
                    }
                } else if (data.success === false) {
                    var messagePromise = null;

                    if (data.error) {
                        if (userOptions.abpHandleError !== false) {
                            messagePromise = abp.ajax.showError(data.error);
                        }
                    } else {
                        data.error = abp.ajax.defaultError;
                    }

                    abp.ajax.logError(data.error);

                    $dfd && $dfd.reject(data.error, jqXHR);
                    userOptions.error && userOptions.error(data.error, jqXHR);

                    if (jqXHR.status === 401 && userOptions.abpHandleError !== false) {
                        abp.ajax.handleUnAuthorizedRequest(messagePromise, data.targetUrl);
                    }
                } else {
                    //not wrapped result
                    $dfd && $dfd.resolve(data, null, jqXHR);
                    userOptions.success && userOptions.success(data, null, jqXHR);
                }
            } else {
                //no data sent to back
                $dfd && $dfd.resolve(jqXHR);
                userOptions.success && userOptions.success(jqXHR);
            }
        },
        blockUI: function blockUI(options) {
            if (options.blockUI) {
                if (options.blockUI === true) {
                    //block whole page
                    abp.ui.setBusy();
                } else {
                    //block an element
                    abp.ui.setBusy(options.blockUI);
                }
            }
        },
        unblockUI: function unblockUI(options) {
            if (options.blockUI) {
                if (options.blockUI === true) {
                    //unblock whole page
                    abp.ui.clearBusy();
                } else {
                    //unblock an element
                    abp.ui.clearBusy(options.blockUI);
                }
            }
        },
        ajaxSendHandler: function ajaxSendHandler(event, request, settings) {
            var token = abp.security.antiForgery.getToken();
            if (!token) {
                return;
            }
            if (!abp.security.antiForgery.shouldSendToken(settings)) {
                return;
            }
            if (!settings.headers || settings.headers[abp.security.antiForgery.tokenHeaderName] === undefined) {
                //上面if条件？怎样才能false????未测到
                request.setRequestHeader(abp.security.antiForgery.tokenHeaderName, token);
            }
        }
    });

    //abp.ajax
    $(document).ajaxSend(function (event, request, settings) {
        return abp.ajax.ajaxSendHandler(event, request, settings);
    });
})(jQuery);