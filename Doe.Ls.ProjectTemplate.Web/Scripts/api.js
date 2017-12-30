/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive'], function ($, cnt, helper, interactive) {
    return {
        postForm: function (form, callback) {
            var url = $('#ajax-url-id', form).val();
            $.ajax({
                cache: false,
                url: url,
                data: form.serialize(),
                type: "post",
                dataType: "json",
                async: false,

                success: callback,
                fail: function (response) {
                    alert("Error has been caught" + response);
                }
            });

        },

        ajaxGet: function (url, data, success, error) {
            $.ajax({
                cache: false,
                url: url,
                data: data,
                type: 'get',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (e) {
                    if (helper.hasValue(success)) {
                        success(e);
                    }
                },
                error: function (e) {
                    if (helper.hasValue(error))
                        error(e);
                }
            });

        },

        ajaxGetHtml: function (url, data, success, error, progress) {
            $.ajax({
                cache: false,
                url: url,
                data: data,
                type: 'get',
                success: function (e) {
                    if (helper.hasValue(success)) {
                        success(e);
                    }
                },
                error: function (e) {
                    if (helper.hasValue(error))
                        error(e);
                },
                progress: function () {
                    if (helper.hasValue(progress)) {
                        progress();
                    }
                }
            });

        },
        // old 
        ajaxPost: function (url, data, success, error) {
            $.ajax({
                cache: false,
                url: url,
                data: data,
                type: 'POST',
                //contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (e) {
                    if (helper.hasValue(success)) {
                        success(e);
                    }
                },
                error: function (e) {
                    if (helper.hasValue(error)) {
                        error(e);
                    }
                }
            });

        },

        ajaxPostV2: function (url, data, success, error, dataType) {
            if (!helper.hasValue(dataType)) {
                dataType = 'json';
            }
            $.ajax({
                cache: false,
                url: url,
                data: data,
                type: 'POST',
                //contentType: "application/json; charset=utf-8",
                dataType: dataType,
                async: true,
                success: function (e) {
                    if (helper.hasValue(success)) {
                        success(e);
                    }
                },
                error: function (e) {
                    if (helper.hasValue(error)) {
                        error(e);
                    }
                }
            });

        }
    }

});

