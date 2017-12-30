define(['cnt', 'jquery', 'helper', 'api', 'appSettings', 'tinymce', 'vle.dataTable'],
    function (cnt, $, helper, api, appSettings, tiny, vleDataTable) {
        var $this = {}; // Contains the self reference to the interactive
        var container = $(document);
        var currentUser;

        return {
            initialise: function () {
                if (!helper.hasValue(currentUser)) {
                    getCurrentUser(function (result) {
                        currentUser = result;
                    });
                }

                (function () {
                    $(document)
                        .on(cnt.Events.interactive.initialisePluginsInProgress,
                        function (event, arg) {
                            // console.info('cnt.Events.interactive.initialisePluginsInProgress called');
                            // console.info(event);
                            // console.info(arg);

                            if (arg.method === cnt.Events.interactive.hookDatePicker) {
                                $(document).data[cnt.Events.interactive.hookDatePicker] = true;
                                // console.info('$(document).data[cnt.Events.interactive.hookDatePicker]');
                                // console.info($(document).data[cnt.Events.interactive.hookDatePicker]);
                            }

                            if (arg.method === cnt.Events.interactive.hookSelect2) {
                                $(document).data[cnt.Events.interactive.hookSelect2] = true;
                                // console.info('$(document).data[cnt.Events.interactive.hookSelect2]');
                                // console.info($(document).data[cnt.Events.interactive.hookSelect2]);
                            }

                            if (arg.method === cnt.Events.interactive.hookTinymce) {
                                $(document).data[cnt.Events.interactive.hookTinymce] = true;
                                // console.info('$(document).data[cnt.Events.interactive.hookTinymce]');
                                // console.info($(document).data[cnt.Events.interactive.hookTinymce]);
                            }

                            if (arg.method === cnt.Events.interactive.hookDataTable) {
                                $(document).data[cnt.Events.interactive.hookDataTable] = true;
                                // console.info('$(document).data[cnt.Events.interactive.hookDataTable]');
                                // console.info($(document).data[cnt.Events.interactive.hookDataTable]);
                            }

                            if (arg.method === cnt.Events.interactive.hookValidation) {
                                $(document).data[cnt.Events.interactive.hookValidation] = true;
                                // console.info('$(document).data[cnt.Events.interactive.hookValidation]');
                                // console.info($(document).data[cnt.Events.interactive.hookValidation]);
                            }

                            if ($(document).data[cnt.Events.interactive.hookDatePicker] &&
                                $(document).data[cnt.Events.interactive.hookSelect2] &&
                                $(document).data[cnt.Events.interactive.hookTinymce] &&
                                $(document).data[cnt.Events.interactive.hookValidation] &&
                                $(document).data[cnt.Events.interactive.hookDataTable]) {

                                if (!$(document).data[cnt.Events.interactive.initialisationDone]) {
                                    $(document).trigger(cnt.Events.interactive.initialisationDone, {});
                                    $(document).data[cnt.Events.interactive.initialisationDone] = true;
                                }
                            }
                        });
                })();


                $this = this;
                initialisePlugins(container);

                this.hookPopupButton(null,
                    function () {
                        var popupContainer = $('#vlePluginModal');
                        $this.basicInitialisation(popupContainer);
                    },
                    true,
                    null,
                    null,
                    '.pop-up',
                    true);
            },
            getCurrentUser: function () {
                return currentUser;
            },

            // function to override the dataTable settings
            applyNewDataTableSettings: function (extendedSettings, $table) {

                vleDataTable.applyNewDataTableSettings(extendedSettings, $table);
            },

            basicInitialisation: function (pluginContainer) {
                basicInitialisation(pluginContainer);
            },

            validateForm: function (form, settings) {
                return validateForm(form, settings);
            },

            destroyValidation: function (form) {
                return destroyValidation(form);
            },

            makeSuccessText: function (msg) {
                return '<label class="success">' + msg + '</label>';
            },

            makeDangerText: function (msg) {
                return '<label class="danger">' + msg + '</label>';
            },

            makeWarningText: function (msg) {
                return '<label class="warning">' + msg + '</label>';
            },

            makeInfoText: function (msg) {
                return '<label class="info">' + msg + '</label>';
            },

            reloadPage: function () {
                window.setTimeout(function () {
                    window.location.reload(true);
                },
                    1000);
            },

            getFormId: function () {
                return $('form').first().attr('id');
            },

            getWrapperId: function () {
                return $('.wrapper').attr('id');
            },

            getParentForm: function (element) {
                var e = $(element);
                var form = e.parents('form').get(0);
                return $(form);
            },

            getClosestElement: function (element, selector) {
                return $(element).closest(selector);
            },

            hookDatePicker: function () {
                return hookDatePicker();
            },

            hookAutoComplete: function (element, callback) {
                if (helper.hasValue(element)) {

                    require(['bsTypeahead'],
                        function () {
                            var jsonObj = element.attr('data-jsonObj');

                            element.typeahead({
                                source: JSON.parse(jsonObj)
                            });
                            if (helper.hasValue(callback)) {
                                callback();
                            }
                        });
                }
            },

            hookTinymce: function (pluginContainer) {
                return hookTinymce(pluginContainer);
            },

            //hookTimePicker: function (element, onChangeCallback) {
            //    return hookTimePicker(element, onChangeCallback);
            //},

            showDialog: function (dialogType, title, message, settings, callback) {
                return showDialog(dialogType, title, message, settings, callback);
            },

            hookPopupButton: function (validatorSettings,
                modalLoadCallBack,
                useAjaxSubmitHandler,
                successCallback,
                errorCallBack,
                elementSelector,
                autoHide) {
                var elements = $('.pop-up');

                if (!helper.hasValue(useAjaxSubmitHandler)) {
                    useAjaxSubmitHandler = true;
                }

                if (!helper.hasValue(autoHide)) {
                    autoHide = true;
                }

                if (helper.hasValue(elementSelector)) {
                    elements = $(elementSelector);
                }

                if (elements.length <= 0) return false;
                elements.each(function () {

                    $(this).attr('data-backdrop', 'static').attr('data-keyboard', 'false');


                });

                elements.off();
                elements.on('click',
                    function (e) {
                        // disabling mouse-wheel click
                        //if (e.which === 2) {
                        //    e.preventDefault();
                        //    return false;
                        //}

                        if (elements.hasClass('disabled')) {
                            return false;
                        }

                        if ($('#vlePluginModal').length > 0) {
                            $('#vlePluginModal').remove();
                        }

                        //$('body')
                        //    .append('<div class="modal fade" tabindex="-1" role="dialog" id="vlePluginModal">' +
                        //    '<div class="modal-dialog">' +
                        //    '<div class="modal-content"></div> <!-- /.modal-content -->' +
                        //    '</div> <!-- /.modal-dialog -->' +
                        //    '</div>');

                        $('body')
                            .append('<div id="vlePluginModal" class="modal fade show" tabindex="-1" role="dialog" aria-labelledby="tempModalLabel" >' +
                            '<div class="modal-dialog" role="document">' +
                            '<div class="modal-content">' +
                                '<div class="progress"> ' +
                                ' <div class="progress-bar progress-bar-striped bg-info" role="progressbar" style="width: 10%" aria-valuenow="10" aria-valuemin="0" aria-valuemax="100"></div></div>' +
                                '</div> <!-- /.modal-content -->' +
                            '</div> <!-- /.modal-dialog -->' +
                            '</div>');

                  

                        $('#vlePluginModal')
                            .on('shown.bs.modal',
                            function (e) {                                
                                var href = $(e.relatedTarget).prop('href');
                                api.ajaxGetHtml(href,
                                    null/*Data*/,
                                    function (content) {/*success*/
                                        var $progressBar = $('.progress-bar', e.target);
                                        $progressBar.parent().hide();
                                        $('.modal-content', e.target).append($(content));
                                        
                                        var remoteFormId = '#' + $('form', content).attr('id');

                                        if ($('div.modal-body').attr('data-modal-custom-width')) {
                                            $('div.modal-dialog').addClass('custom-dialog');
                                        }
                                        

                                        $('.success-popup').hide();

                                        if (helper.hasValue(remoteFormId)) {
                                            if (!helper.hasValue(validatorSettings)) {
                                                validatorSettings = {};
                                            }
                                            var settings = $.extend(
                                                {},
                                                validatorSettings,
                                                {
                                                    framework: 'bootstrap4'
                                                });

                                            var formValidation = validateForm($(remoteFormId), settings);

                                            formValidation.on('success.form.fv',
                                                function(evt) {
                                                    evt.preventDefault();
                                                    var form = $(evt.target);
                                                    var validator = $(evt.target).data('formValidation');
                                                    if (useAjaxSubmitHandler === false) {
                                                        validator.defaultSubmit();
                                                    } else {
                                                        var formData = new FormData();
                                                        var params = form.serializeArray();

                                                        if ($(':file').length > 0) {
                                                            var files = form.find(':file')[0].files;

                                                            $.each(files,
                                                                function(i, file) {
                                                                    // Prefix the name of uploaded files with "uploadedFiles-"
                                                                    // Of course, you can change it to any string
                                                                    formData.append('uploadedFiles-' + i, file);
                                                                });
                                                        }
                                                        $.each(params,
                                                            function(i, val) {
                                                                formData.append(val.name, val.value);
                                                            });

                                                        $.ajax({
                                                            cache: false,
                                                            contentType: false,
                                                            processData: false,
                                                            url: form.attr('action'),
                                                            data: formData,
                                                            type: 'post',
                                                            dataType: 'json',
                                                            success: function(result, textStatus, jqXhr) {
                                                                if (result
                                                                    .Status !==
                                                                    0) { // error                                        
                                                                    var errList = $('.error-popup ul.list-group');
                                                                    errList.empty();
                                                                    $(result.Errors)
                                                                        .each(function(index, error) {
                                                                            errList
                                                                                .append(
                                                                                    '<li class="list-group-item"><label class="validation-error">' +
                                                                                    error +
                                                                                    '</label></li>');
                                                                        });
                                                                } else {
                                                                    if (successCallback != null) {
                                                                        successCallback(result, textStatus, jqXhr);
                                                                        if (autoHide === true)
                                                                            setTimeout(function() {
                                                                                    $('#vlePluginModal').modal('hide');
                                                                                },
                                                                                2000);

                                                                    } else {
                                                                        $('.error-popup').hide();
                                                                        $('#formTab').hide();
                                                                        $('#lbl-success')
                                                                            .text('Data has been successfully updated');
                                                                        if (helper.hasValue(result.HeaderText)) {
                                                                            $('.modal-header h3')
                                                                                .text(result.HeaderText);
                                                                        }
                                                                        $('.success-popup').show(500);

                                                                        if (autoHide === true) {
                                                                            setTimeout(function() {
                                                                                    $('#vlePluginModal').modal('hide');
                                                                                },
                                                                                2000);

                                                                            setTimeout(function() {
                                                                                    location.reload();
                                                                                },
                                                                                2000);
                                                                        }
                                                                    }
                                                                }
                                                            },
                                                            error: function(jqXhr, textStatus, errorThrown) {
                                                                if (errorCallBack != null) {
                                                                    errorCallBack(jqXhr, textStatus, errorThrown);
                                                                }
                                                            }
                                                        });
                                                    }
                                                });
                                        }//only if the modal has form id
                                        if (modalLoadCallBack != null) {
                                            modalLoadCallBack($(remoteFormId));

                                        } else {
                                            var popupContainer = $('#vlePluginModal');

                                            basicInitialisation(popupContainer);

                                        }

                                    },function(ev) {/*error*/
                                        var $progressBar = $('.progress-bar');
                                        $progressBar.hide();
                                        $progressBar.parent().hide();
                                        alert(ev);
                                    });



                            });

                        return true;
                    });


                $('#vlePluginModal')
                    .on('hidden.bs.modal',
                    function () {
                        $('#vlePluginModal').remove();
                        $('.modal-backdrop').remove();
                    });

                return true;
            },


            hookSelect2: function (pluginContainer, selector, settings) {
                return hookSelect2(pluginContainer, selector, settings);
            },

            removeAllValidationAttributes: function ($field) {
                removeAllValidationAttributes($field);
            },

            removeValidatiorsFromContainer: function ($containerElement) {
                removeValidatiorsFromContainer($containerElement);
            },

            hookValidateEmailV2: function (verifyButton, successCallback, errorCallback) {

                $(verifyButton).addClass('disabled');
                $(verifyButton)
                    .on('click',
                    function () {
                        var email = $('input[name*="Email"]').val();
                        var emailElement = $('input[name*="Email"]');
                        var form = $(verifyButton).closest('form');
                        var fieldList = $('[data-binding]', $(form));

                        api.ajaxGet(
                            cnt.GetUserInfoFromAdByEmailUrl,
                            'email=' + email,
                            function (userInfo) {
                                $('input[name*="Email"]').val(userInfo['Email'].toLowerCase());
                                $.each(fieldList,
                                    function () {
                                        var value = userInfo[$(this).attr('data-binding')];
                                        $(this).val($(this).attr('id') === 'UserId' ? value.toLowerCase() : value);
                                    });

                                $(form).formValidation(cnt.fv_RevalidateField, emailElement.prop('name'));
                                $.each(fieldList,
                                    function () {
                                        var fieldName = $(this).prop('name');
                                        form.formValidation(cnt.fv_RevalidateField, fieldName);
                                    });

                                if (helper.hasValue(successCallback)) {
                                    successCallback(userInfo);
                                }
                            },
                            function () {
                                $('input[name*="Email"]').val('_' + email.toLowerCase());
                                $.each(fieldList,
                                    function () {
                                        $(this).val('');
                                    });

                                $(form).formValidation(cnt.fv_RevalidateField, emailElement.prop('name'));
                                $.each(fieldList,
                                    function () {
                                        var fieldName = $(this).prop('name');
                                        form.formValidation(cnt.fv_RevalidateField, fieldName);
                                    });

                                if (helper.hasValue(errorCallback)) {
                                    errorCallback();
                                }
                            });
                    });
            },

            hookSelect2Ajax: function (pluginContainer, targetSelector, newSettings) {
                return hookSelect2Ajax(pluginContainer, targetSelector, newSettings);
            },

            hookSearchLocation: function (pContainer, targetSelector, value) {
                return hookSearchLocation(pContainer, targetSelector, value);
            },

            hookLocationChange: function (pContainer,
                suburbSelector,
                stateSelector,
                postcodeSelector,
                nextFocusSelector,
                isStateSelect2,
                nextFocusSelectorNew) {
                return hookLocationChange(pContainer,
                    suburbSelector,
                    stateSelector,
                    postcodeSelector,
                    nextFocusSelector,
                    isStateSelect2,
                    nextFocusSelectorNew);
            },

            hookSearchContact: function (pContainer, targetSelector, value) {
                return hookSearchContact(pContainer, targetSelector, value);
            },

            hookContactNameChange: function (pContainer, contactSelector, selectorsToUpdate) {
                return hookContactNameChange(pContainer, contactSelector, selectorsToUpdate);
            },

            hookSearchSchool: function (pContainer, targetSelector, valueId, valueText) {
                return hookSearchSchool(pContainer, targetSelector, valueId, valueText);
            },

            hookSchoolChange: function (pContainer, schoolSelector, selectorsToUpdate) {
                return hookSchoolChange(pContainer, schoolSelector, selectorsToUpdate);
            },

            hookRevalidateFieldOnChange: function (pContainer, selector) {
                return hookRevalidateFieldOnChange(pContainer, selector);
            },

            revalidateFormField: function (field) {
                return revalidateFormField(field);
            },

            hookCloseCancelPageForm: function () {
                return hookCloseCancelPageForm();
            },

            hookManageContact: function () {
                return hookManageContact();
            },
            setDropdwnOptions: function (elem, action, data, sucess) {
                return setDropdwnOptions(elem, action, data, sucess);
            }

        };


        function getCurrentUser(successCallback) {
            return api.ajaxGet(cnt.GetCurrentUserUrl, null, successCallback);
        }

        function initialisePlugins(pluginContainer) {
            basicInitialisation(pluginContainer);
            $(document)
                .on(cnt.Events.interactive.initialisePluginsInProgress,
                function (event, arg) {
                    if (arg.method === cnt.Events.interactive.hookValidation) {

                        var needRequire = false;
                        if ($('.table', pluginContainer).not('.notDataTable').length > 0) {
                            needRequire = true;

                            if ($('.ajaxDataTable', pluginContainer).length > 0) {
                                // hooks the ajaxDataTable
                                vleDataTable.applyAjaxDataTable();
                            } else {

                                vleDataTable.applyGenericDataTable(document);
                            }
                            $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookDataTable });

                        }

                        if (!needRequire) $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookDataTable });

                    }

                });

            hookGenericFormValidation(helper, pluginContainer);
        }


        /*------ Show-Hide Functions BEGIN -----*/
        function transformShowHide(container) {
            if ($('section[data-type="show-hide"]', container).length > 0) {
                $('section[data-type="show-hide"]', container).each(function (index) {
                    var sectionId = $(this).prop('id');
                    $('<div class="card-filter" id="show-hide-' + sectionId + '-' + index + '"></div>').insertAfter($(this));
                    var showHide = $('#show-hide-' + sectionId + '-' + index);
                    showHide.append('<div class="panel panel-default">' +
                        '<div class="panel-heading">' +
                        '<h4 class="panel-title">' +
                        '<a  href="#collapse-' + sectionId + '-' + index + '" class="collapsed" data-toggle="collapse">' + $(this).attr('data-heading') + '</a>' +
                        '</h4>' +
                        '</div>' +
                        '<div id="collapse-' + sectionId + '-' + index + '" class="panel-collapse collapse">' +
                        '<div class="panel-body">' +
                        $(this).html() +
                        '</div>' +
                        '</div>' +
                        '</div>'
                    );

                    $(this).remove();
                });
            }

            $('button[class*="show-hide-expand-all"]', container).on('click', function () {
                $('.panel-collapse:not(".in")').collapse('show');
            });

            $('button[class*="show-hide-hide-all"]', container).on('click', function () {
                $('.panel-collapse.in').collapse('hide');
            });

            if ($('button[class*="show-hide-toggle"]', container).length > 0) {
                var toggleButton = $('button[class*="show-hide-toggle"]', container);

                if ($('.panel', container).length > 1) {
                    toggleButton.show();
                } else {
                    toggleButton.hide();
                }

                toggleButton.on('click', function () {
                    $('.panel-collapse').collapse($(this).attr('data-action'));
                    if ($(this).attr('data-action') === 'show') {
                        $(this).attr('data-action', 'hide');
                        $(this).prop('title', 'Collapse All');
                        $(this).text('Collapse All');
                    } else {
                        $(this).attr('data-action', 'show');
                        $(this).prop('title', 'Expand All');
                        $(this).text('Expand All');
                    }

                });
            }
        }


        /*------ Accordion Functions BEGIN------*/
        function transformAccordion() {
            if ($('section[data-type="accordion"]').length > 0) {
                $('section[data-type="accordion"]')
                    .each(function (index) {
                        $('<div class="panel-group" id="accordion' + index + '"></div>').insertAfter($(this));
                        var accordion = $('#accordion' + index);

                        $('section[data-type="panel"]', $(this))
                            .each(function (panelIndex) {
                                accordion.append('<div class="panel panel-primary">' +
                                    '<div class="panel-heading" data-toggle="collapse" data-parent="#' +
                                    accordion.prop('id') +
                                    '" data-target="#Panel' +
                                    panelIndex +
                                    '">' +
                                    '<h4 class="panel-title">' +
                                    $(this).attr('data-heading') +
                                    '<i class="glyphicon glyphicon-chevron-down pull-right"></i></h4>' +
                                    '</div>' +
                                    '<div id="Panel' +
                                    panelIndex +
                                    '" class="panel-body collapse">' +
                                    $(this).html() +
                                    '</div>' +
                                    '</div>');
                            });

                        $('div[id*=Panel]', accordion).first().addClass('in');

                        $(this).hide();
                    });
            }

            hookAccordionChevrons();
            hookAccordionPrint();
            hookAccordionExapandAll();
        }

        function hookAccordionChevrons() {

            $(".accordion-toggle")
                .click(function (ev) {
                    var accordionId = $(this).attr('data-parent');
                    var accordion = $(accordionId);

                    var link = ev.target;
                    var header = $(link).closest(".panel-heading");
                    var chevState = $("i.indicator", header)
                        .toggleClass('glyphicon-chevron-down glyphicon-chevron-right');
                    $("i.indicator", accordion)
                        .not(chevState)
                        .removeClass("glyphicon-chevron-down")
                        .addClass("glyphicon-chevron-right");
                });

        }

        function hookAccordionPrint() {
            $('a.print')
                .on('click',
                function () {

                    var allInIds = [];
                    $('.in')
                        .each(function () {
                            allInIds.push($(this).attr('id'));
                        });

                    $('.panel-body')
                        .each(function () {
                            $(this).addClass('in');
                            $(this).removeAttr("style");
                        });

                    $('p.form-control-plaintext')
                        .each(function () {
                            $(this).after("&nbsp;");
                        });
                    window.print();

                    $('.panel-body')
                        .each(function () {
                            $(this).removeClass('in');
                        });

                    allInIds.forEach(function (value) {
                        $('#' + value).addClass('in');
                        $('#' + value).removeAttr("style");
                    });
                });

        }

        function hookAccordionExapandAll() {
            var active = true;
            $('a.expandAll')
                .on('click',
                function () {
                    if (active) {
                        active = false;
                        $('.panel-body')
                            .each(function () {

                                $(this).addClass('in');
                                $(this).removeAttr("style");
                                //$(this).attr("style", "height:auto;");
                            });
                        $("i.indicator", '#accordion')
                            .removeClass("glyphicon-chevron-right")
                            .addClass("glyphicon-chevron-down");
                        $(this).text('Collapse all');
                    } else {
                        active = true;

                        $('.panel-body')
                            .each(function () {
                                $(this).removeClass('in');
                            });
                        $(this).text('Expand all');
                        $("i.indicator", '#accordion')
                            .removeClass("glyphicon-chevron-down")
                            .addClass("glyphicon-chevron-right");
                    }


                });
        }

        /*------ Accordion Functions END ------*/

        function hookDatePicker() {
            var needRequire = false;
            $('input[type=date]')
                .not('[readonly]')
                .each(function () {
                    $(this).attr("type", "text");
                    $(this).attr("data-date", "date");
                });

            if ($('input[data-date=date]').length > 0) {
                needRequire = true;
                require(['bsDatepicker'],
                    function () {

                        $('input[data-date=date]').each(function () {

                            $(this).datepicker('remove');
                            $(this).datepicker({
                                format: "dd-M-yyyy",
                                autoclose: true,
                                todayBtn: 'linked',
                                startDate: helper.hasValue($(this).attr('data-startdate')) ? $(this).attr('data-startdate') : '01-Jan-1990',
                                endDate: helper.hasValue($(this).attr('data-enddate')) ? $(this).attr('data-enddate') : '01-Jan-9999'
                            })
                                .on('changeDate',
                                function () {

                                    var $form = $(this).closest('form');
                                    if (helper.hasValue($form.data)) {
                                        var fname = $(this).attr('name');
                                        if (!helper.hasValue(fname)) return;
                                        if (!helper.hasValue($form)) return;

                                        $form.formValidation(cnt.fv_RevalidateField, fname);
                                    }
                                });
                        });
                        $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookDatePicker });

                    });
            }
            if (!needRequire) {
                $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookDatePicker });

            }

        }

        function hookGenericFormValidation(helperUtil) {
            var needRequire = false;
            $('form')
                .each(function (index, form) {
                    var settings = {
                        framework: 'bootstrap4',
                        icon: {
                            valid: 'fa fa-check',
                            invalid: 'fa fa-times',
                            validating: 'fa fa-refresh'
                        },
                        excluded: [':disabled', ':hidden', ':not(:visible)']
                    };
                    if (!helperUtil.hasValue($(form).attr('data-custom-validation'))) {
                        needRequire = true;
                        require(['fValidator'],
                            function () {
                                validateForm($(form), settings);
                                $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookValidation });
                            });
                    }
                });

            if (!needRequire) {
                $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookValidation });
            }
        }

        function validateForm(form, extendedSettings) {

            var basicSettings = {
                framework: 'bootstrap4',
                icon: {
                    valid: 'fa fa-check',
                    invalid: 'fa fa-times',
                    validating: 'fa fa-refresh'
                },
                excluded: [
                    function ($field) {
                        if ($field.prop('class').indexOf('tinymce') > 0) {
                            return false;
                        }

                        return $field.is(':hidden') || $field.is(':not(:visible)');
                    }, ':disabled'
                ]
            };

            var newSettings = $.extend(basicSettings, extendedSettings);

            var validator = form.data('formValidation');
            if (helper.hasValue(validator)) {
                try {
                    validator.destroy();
                } catch (err) {
                    alert("Error: " + err);
                }
            }

            try {
                validator = $(form).formValidation(newSettings);
            } catch (err) {
                alert(err);
            }
            return validator;

        }

        function destroyValidation(form) {

            var validator = form.data('formValidation');

            if (helper.hasValue(validator)) {
                try {
                    validator.destroy();
                } catch (err) {
                    alert("Error: " + err);
                }
            }

        }

        function hookSelect2(pContainer, targetSelector, newSettings) {
            var needRequire = false;
            if (!helper.hasValue(targetSelector)) {
                targetSelector = '.select2picker';
            }

            if ($(targetSelector, pContainer).length > 0) {
                needRequire = true;
                require(['bsSelect2'],
                    function () {
                        $(targetSelector, pContainer)
                            .each(function () {
                                var settings = {};

                                if (helper.hasValue(newSettings)) {
                                    $.extend(settings, newSettings);
                                }
                                var placeholder = $(this).attr('placeholder');
                                if (helper.hasValue(placeholder)) {
                                    settings.placeholder = placeholder;
                                }

                                var allowClear = $(this).attr('allowclear');
                                if (helper.hasValue(allowClear)) {
                                    settings.allowClear = allowClear;
                                }

                                var width = $(this).attr('data-width');

                                if (helper.hasValue(width)) {
                                    settings.width = width;
                                }

                                var minimumResultsForSearch = $(this).attr('data-minimumResultsForSearch');
                                if (helper.hasValue(minimumResultsForSearch)) {
                                    settings.minimumResultsForSearch = minimumResultsForSearch;
                                }
                                $(this).select2('destroy').select2(settings);
                                if (settings.initSelection != null) {
                                    $(this).select2('val', []);
                                }
                            });
                        $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookSelect2 });
                    });
            }
            if (!needRequire) $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookSelect2 });
        }

        function hookListGroupHover() {
            // return;
            $('a[disabled]')
                .each(function () {
                    $(this).removeAttr('href');
                });

            $('.list-group-item-heading>a')
                .hover(function () {

                    if (!helper.hasValue($(this).attr('disabled'))) {
                        $(this).addClass('active');
                    } else {
                        $(this).addClass('a-disabled');
                    }
                },
                function () {
                    if (!$(this).hasClass("fixed-active")) {
                        $(this).removeClass('active');
                    }
                });

            $('.list-group')
                .each(function () {
                    if (!helper.hasValue($(this).attr('disabled'))) {
                        $(this).addClass('active');
                    } else {
                        $(this).addClass('listgroup-disabled');
                    }
                },
                function () {
                    $(this).removeClass('active');
                });


        }

        function applyToolTip(apiSrv) {
            $('*[data-tag],*[data-content]')
                .each(function () {
                    var $element = $(this);
                    var content = $element.attr('data-content');
                    var placementSetting = 'left';
                    if (helper.hasValue($element.attr('data-placement'))) {
                        placementSetting = $element.attr('data-placement');
                    }
                    var mouseEventType = 'hover';
                    if (helper.hasValue($element.attr('data-mouseevent'))) {
                        mouseEventType = $element.attr('data-mouseevent');
                    }

                    var delaySetting = '';
                    var hideDelay = 100;
                    if (helper.hasValue($element.attr('data-delay'))) {
                        delaySetting = JSON.parse($element.attr('data-delay'));
                        if (helper.hasValue(delaySetting.hide)) {
                            hideDelay = delaySetting.hide;
                        }
                    }
                    if (helper.hasValue(content)) {
                        $element.attr('data-toggle', 'popover');
                        $element.attr('data-content', content);

                        if (mouseEventType === 'hover') {
                            $element.popover({
                                placement: placementSetting,
                                animation: 'true',
                                trigger: 'manual',
                                html: 'true',
                                delay: delaySetting
                            })
                                .on('mouseenter',
                                function () {
                                    var $this = this;
                                    $(this).popover('show');
                                    $(this)
                                        .siblings('.popover')
                                        .on('mouseleave',
                                        function () {
                                            $($this).popover('hide');
                                        });
                                })
                                .on('mouseleave',
                                function () {
                                    var $this = this;
                                    setTimeout(function () {
                                        if (!$(".popover:hover").length) {
                                            $($this).popover("hide");
                                        }
                                    },
                                        hideDelay);
                                });
                        }

                        if (mouseEventType === 'click') {
                            $element.popover({
                                placement: placementSetting,
                                animation: 'true',
                                html: 'true',
                                delay: delaySetting
                            });
                        }

                        return;
                    }

                    var tagId = $element.attr('data-tag');


                    var result = cnt.EntityList.AjaxResult; // just for intellisense;

                    if (helper.hasValue(tagId)) {
                        apiSrv.ajaxGet(cnt.MessageUrl,
                            'tag=' + tagId,
                            function (message) {
                                result = message;

                                $element.attr('data-toggle', 'popover');
                                $element.attr('data-content', result.Data.MessageContent);

                                $element.popover({
                                    placement: placementSetting,
                                    animation: 'true',
                                    trigger: 'manual',
                                    html: 'true',
                                    delay: delaySetting
                                })
                                    .on("mouseenter",
                                    function () {
                                        var $this = this;
                                        $(this).popover("show");
                                        $(this)
                                            .siblings(".popover")
                                            .on("mouseleave",
                                            function () {
                                                $($this).popover('hide');
                                            });
                                    })
                                    .on("mouseleave",
                                    function () {
                                        var $this = this;
                                        setTimeout(function () {
                                            if (!$(".popover:hover").length) {
                                                $($this).popover("hide");
                                            }
                                        },
                                            hideDelay);
                                    });
                            });
                    }

                });
        }

        function autocompleteOff() {
            var allInputs = $("input:not([autocomplete])");

            $.each(allInputs,
                function () {
                    $(this).attr('autocomplete', 'off');
                });
        }

        function hookDataChangedHandler() {
            if ($("form[data-tracking-enabled='true']").length > 0) {
                var currentform = null;
                $("form[data-tracking-enabled='true']")
                    .each(function () {
                        currentform = $(this);
                        if (!helper.hasValue(currentform)) return true;
                        currentform.on('change keyup keydown',
                            'input, textarea, select',
                            function (e) {
                                if (e.keyCode !== 9) { //set true only when it is not tabbing through the data
                                    currentform.data("dataChanged", true);
                                }
                            });

                        currentform.submit(function () {
                            currentform.data("dataChanged", false);
                            $(document).data('tiny-mce-dataChanged', false);

                        });

                        $(window)
                            .on('beforeunload',
                            function () {
                                if (currentform.data("dataChanged") === true) {
                                    return 'You have not saved your changes.';
                                }
                            });

                    });

                $('[data-dismiss="custom"]').on('click', function () {
                    var container = $('.modal-content');
                    var modalForm = $('form', container);
                    if (helper.hasValue(modalForm) && modalForm.data("dataChanged") === true) {
                        var r = confirm("You have not saved your changes! Do you want to leave this site ?");
                        if (r === false) {
                        } else {
                            $('#vlePluginModal').modal('hide');
                            modalForm.data("dataChanged", false);
                        }
                    } else {
                        $('#vlePluginModal').modal('hide');
                        modalForm.data("dataChanged", false);
                    }
                });

            }



            $(window)
                .on('beforeunload',
                function () {
                    if ($(document).data('tiny-mce-dataChanged') === true) {
                        return 'You have not saved your changes.';
                    }
                });

        }



        function hookTinymce() {
            var needRequire = false;
            if ($('.tinymce, .tinymce-rich, .tinymce-rich-small, .tinymce-simple, .tinymceListOnly').length > 0) {
                needRequire = true;
                require(['vle.tinymce'],
                    function (vleTinyMce) {
                        vleTinyMce.initialise();
                        vleTinyMce.hookTinymce(false); // disable for inline mode unless admin switch on
                        $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookTinymce });
                    }
                );
            }
            if (!needRequire) $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookTinymce });
        }

        function basicInitialisation(pluginContainer) {
            transformShowHide(pluginContainer);
            transformAccordion(pluginContainer);

            hookButtonLinks();
            hookDataChangedHandler(pluginContainer);
            hookListGroupHover(pluginContainer);
            hookDatePicker(pluginContainer);
            hookSelect2(pluginContainer);
            applyToolTip(api);
            autocompleteOff(helper);
            hookTinymce(pluginContainer);
            //hookTimePicker();
            updateAccessibility();

        }
        function updateAccessibility() {
            $('form')
                .each(function (index, form) {
                    if (helper
                        .hasValue($(form).attr('data-null-model')) &&
                        $(form).attr('data-null-model') === 'True') {

                        $('input[type!="hidden"]', $(form)).val('');
                        $('.select2picker option:selected').removeAttr('selected');
                        $('.select2picker option[value=""]').attr('selected', 'selected');
                    }

                    $(this).find('label:not([type="hidden"])').each(function () {
                        if ((typeof $(this).attr('tabindex') === 'undefined')) {
                            $(this).attr('tabindex', 0);

                            if (typeof $(this).attr('aria-label') === 'undefined') {
                                $(this).attr('aria-label', $(this).text().trim());
                            }
                        }

                    });

                    $(this).find('.form-control-plaintext').not('.already-accissible').each(function () {
                        if (typeof $(this).attr('tabindex') === 'undefined') {
                            //if ($(this).html() === $(this).text() || $(':tabbable', $(this).length === 0)) {   // This code conflict with jquery
                            //    $(this).attr('tabindex', 0);
                            //}

                            if ($(this).html() === $(this).text() && $(this).text() === '') {
                                $(this).text('[Not Provided]');
                            }
                            if (typeof $(this).attr('aria-label') === 'undefined') {
                                $(this).attr('aria-label', $(this).text().trim());
                            }
                        }
                    });
                });
        }

        function removeAllValidationAttributes(field) {
            field.removeAttr('required');
            field.removeAttr('data-fv-callback-callback');
            field.removeAttr(' data-fv-emailaddress');

        };

        /*
         * This methods for the form dynamic content that generated dynamically from the server
         * 
         */
        function removeValidatiorsFromContainer($containerElement) {
            var $form = $containerElement.closest('form');

            var formValidation = $form.data('formValidation');

            $(":input:not([type=hidden])", $containerElement)
                .each(function (index) {

                    try {
                        formValidation.removeField($(this));
                    } catch (error) {

                    }
                });
        }

        function hookSelect2Ajax(pContainer, targetSelector, newSettings) {
            var needRequire = false;
            if (!helper.hasValue(targetSelector)) {
                targetSelector = '.select2pickerAjax';
            }

            $('.select2pickerAjax', pContainer).each(function (index, data) {

                var settings = {
                };

                var width = $(this).attr("data-width");

                if (helper.hasValue(width)) {
                    settings.width = width;

                }
                settings.placeholder = "Please select";

                var ajaxattr = $(this).attr('data-ajax');
                var context = $(this).attr('data-context');
                var allowClear = $(this).attr('allowclear');
                if (helper.hasValue(allowClear)) {
                    settings.allowClear = allowClear;
                }
                if (helper.hasValue(ajaxattr)) {//run ajax query
                    var ajaxUrl = $(this).attr('data-ajax');
                    $(this).select2('destroy').select2({
                        minimumInputLength: 2,
                        // placeholder: $(this).attr('placeholder'),
                        multiple: false,
                        id: function (item) {
                            var elmName = $(this).attr('Name');
                            $(this).closest('form').bootstrapValidator('revalidateField', elmName);
                            return item.id;

                        }, initSelection: function (element, callback) {

                        },
                        ajax: {
                            url: ajaxUrl,
                            dataType: 'json',
                            type: "GET",
                            quietMillis: 200,
                            data: function (term) {
                                return {
                                    term: term,
                                    context: context
                                };
                            },
                            results: function (data) {
                                return {
                                    results: $.map(data, function (item) {
                                        //  $(this).attr("data-ref-elmid", item.Value);
                                        return {
                                            text: item.Text,
                                            value: item.Value,
                                            id: item.Value
                                        }
                                    })
                                };
                            }
                        },

                        dropdownCssClass: "bigdrop",
                        escapeMarkup: function (m) { return m; }
                    }).select2('val', []).
                        on('change', function (e) {
                            var targetId = $(this).attr("data-ref-elmid");
                            $('#' + targetId).val($(this).val());
                            var elmName = $(this).attr('Name');
                            $(this).closest('form').bootstrapValidator('revalidateField', elmName);
                            return true;
                        });

                } else {
                    $(this).select2('destroy').select2(settings);
                }
            });

            /* if ($(targetSelector, pContainer).length > 0) {
                 needRequire = true;
                 require(['bsSelect2'],
                     function () {
                         $(targetSelector, pContainer)
                             .each(function () {
                                 var settings = {};
 
                                 if (helper.hasValue(newSettings)) {
                                     $.extend(settings, newSettings);
                                 }
                                 var width = $(this).attr("data-width");
 
                                 if (helper.hasValue(width)) {
                                     settings.width = width;
                                 }
                                 try {
                                     if (settings.initSelection != null)
                                         $(this).select2('destroy').select2(settings).select2('val', []);
                                     else {
                                         $(this).select2('destroy').select2(settings);
                                     }
                                 } catch (err) {
                                     alert(err);
                                 }
 
                             });
                         //$(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookSelect2Ajax });
                     });
             }*/
            //if (!needRequire) $(document).trigger(cnt.Events.interactive.initialisePluginsInProgress, { method: cnt.Events.interactive.hookSelect2Ajax });
        }

        function hookButtonLinks() {
            $.each($('button[data-url]'),
                function (index, item) {
                    var $item = $(item);
                    var target = $item.attr("data-target");
                    var url = $item.attr("data-url");

                    if (helper.hasValue(url)) {
                        $item.on("click",
                            function (e) {
                                if (helper.hasValue(target)) {
                                    if (target === '_blank') {
                                        window.open(url);
                                        e.preventDefault();
                                        return;
                                    } else {
                                        document.location = url;
                                        e.preventDefault();
                                        return;
                                    }
                                }
                            });

                    }


                });
        }

        function hookSearchLocation(pContainer, targetSelector, value, stateValue) {
            //if (!helper.hasValue(stateValue)) {
            //    stateValue = 'NSW';
            //}

            var options = {
                ajax: {
                    url: cnt.GetSuburbUrl,
                    dataType: 'json',
                    data: function (params) {
                        return helper.hasValue(stateValue) ? { suburb: params, state: stateValue } : { suburb: params };
                    },
                    results: function (result, params) {
                        // parse the results into the format expected by Select2
                        // since we are using custom formatting functions we do not need to
                        // alter the remote JSON data, except to indicate that infinite
                        // scrolling can be used
                        //params.page = params.page || 1;
                        //{results:[{id:1, text:'Red'},{id:2, text:'Blue'}], more:true}

                        if (result.error) {
                            alert(error.errorMessage);
                            return false;
                        }
                        return {
                            results: getAusPostAddressItems(result.localities),
                            more: false
                        };
                    },
                    cache: true
                },
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                minimumInputLength: 3,
                createSearchChoice: function (term, data) {
                    if ($(data)
                        .filter(function () {
                            return this.text.localeCompare(term) === 0;
                        })
                        .length ===
                        0) {
                        return {
                            id: term + ' (New)',
                            text: term,
                            newOption: true
                        };
                    } else {
                        return null;

                    }
                },
                templateResult: function () { },
                templateSelection: function () { }
            };
            if (value != null && value.length > 0) {
                options.initSelection = function (element, callback) {
                    callback({ id: value, text: value });
                };
            }
            hookSelect2(pContainer, targetSelector, options);
        }

        function getAusPostAddressItems(localities) {
            var itemisedAddresses = [];
            if (localities === '')
                return itemisedAddresses;
            var addresses = [];
            var localityArray = [];
            if (localities.locality instanceof Array) {
                localityArray = localityArray.concat(localities.locality);
            } else {
                localityArray.push(localities.locality);
            }
            for (var r = 0; r < localityArray.length; r++) {
                var formattedAddress = [localityArray[r].location, localityArray[r].state, localityArray[r].postcode].join(' ');
                if ($.inArray(formattedAddress, addresses) === -1) {
                    addresses.push(formattedAddress);
                    var item = {
                        id: formattedAddress,
                        text: formattedAddress
                    };
                    itemisedAddresses.push(item);
                }
            }
            return itemisedAddresses;
        }

        function hookLocationChange(pContainer, suburbSelector, stateSelector, postcodeSelector, nextFocusSelector, isStateSelect2, nextFocusSelectorNew) {
            isStateSelect2 = isStateSelect2 || false;
            $(suburbSelector).on('select2:select change', function (e) {
                var addressSegments = $(this).val().split(' ');
                var segmentsLengthOffset = 0;
                if (addressSegments[addressSegments.length - 1] === '(New)') {
                    segmentsLengthOffset = 1;
                } else {
                    segmentsLengthOffset = 2;
                    $(postcodeSelector).val(addressSegments[addressSegments.length - 1]);
                    if (isStateSelect2) {
                        $(stateSelector).select2('destroy');
                    }
                    $(stateSelector).val(addressSegments[addressSegments.length - 2]);
                    if (isStateSelect2) {
                        $(stateSelector).select2();
                    }
                }
                $(suburbSelector).select2('destroy');
                var suburbWords = [];
                for (var i = 0; i < addressSegments.length - segmentsLengthOffset; i++) {
                    suburbWords.push(addressSegments[i]);
                }
                var suburbVal = suburbWords.join(' ');
                $(suburbSelector).val(suburbVal);
                hookSearchLocation(pContainer, suburbSelector, suburbVal);
                if (nextFocusSelector != null) {
                    if (addressSegments[addressSegments.length - 1] === '(New)') {
                        if (nextFocusSelectorNew != null) {
                            $(nextFocusSelectorNew).focus();
                        } else {
                            $(nextFocusSelector).focus();
                        }
                    } else {
                        $(nextFocusSelector).focus();
                    }
                }
            });
        }

        function hookSearchContact(pContainer, targetSelector, value) {
            var options = {
                ajax: {
                    url: cnt.SearchContactByLastNameOrFirstName,
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return { name: params };
                    },
                    results: function (result, params) {
                        // parse the results into the format expected by Select2
                        // since we are using custom formatting functions we do not need to
                        // alter the remote JSON data, except to indicate that infinite
                        // scrolling can be used
                        //params.page = params.page || 1;
                        //{results:[{id:1, text:'Red'},{id:2, text:'Blue'}], more:true}

                        if (result.Status === 1) {
                            alert(result.Errors[0]);
                            return false;
                        }
                        return {
                            results: result.Data,
                            more: false
                        };
                    },
                    cache: true,
                    error: function () {
                        alert('hookSelect2Ajax.error');
                    }
                },
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                placeholder: "Search for Last Name or First Name",
                minimumInputLength: 2,
                maximumInputLength: 200,
                templateResult: function () { },
                templateSelection: function () { }
            };
            if (value != null && value.length > 0) {
                options.initSelection = function (element, callback) {
                    callback({ id: value, text: value });
                };
            }
            hookSelect2(pContainer, targetSelector, options);
        }

        function hookContactNameChange(pContainer, contactSelector, selectorsToUpdate) {
            //UserId,Email,PrimaryPhone,SecondaryPhone,SchoolCode,SchoolFullName
            $(contactSelector).on('select2:select change', function () {
                var items = $(this).val().split(';');
                for (var i = 0; i < items.length; i++) {
                    if (items[i].length > 0) {
                        if (selectorsToUpdate[i] === '#SchoolName') {
                            hookSearchSchool(pContainer, selectorsToUpdate[i], items[i - 1], items[i]);
                        } else {
                            $(selectorsToUpdate[i]).val(items[i]);
                            if ($.inArray(selectorsToUpdate[i], ['#Email', '#PrimaryPhone', '#SecondaryPhone']) > -1) {
                                $(selectorsToUpdate[i]).trigger('change');
                            }
                        }
                    }
                }
                revalidateFormField($('#ContactName', pContainer));
                revalidateFormField($('#SchoolCode', pContainer));
            });
        }

        function showDialog(dialogType, title, message, settings, callback) {

            require(['bbox'],
                function (bootbox) {

                    settings = $.extend({ callback: callback }, settings);

                    if (helper.hasValue(title)) {
                        $.extend(settings, { title: title });
                    }
                    if (helper.hasValue(message)) {
                        $.extend(settings, { message: message });
                    }

                    if (dialogType === 'alert') {
                        bootbox.alert(settings);
                    } else if (dialogType === 'confirm') {
                        bootbox.confirm(settings);
                    } else if (dialogType === 'prompt') {
                        bootbox.prompt(settings);
                    } else if (dialogType === 'dialog') {
                        bootbox.dialog(settings);
                    }
                });
        }

        function hookSearchSchool(pContainer, targetSelector, valueId, valueText) {
            var options = {
                ajax: {
                    url: cnt.SearchSchoolByFullName,
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return { fullName: params };
                    },
                    results: function (result, params) {
                        // parse the results into the format expected by Select2
                        // since we are using custom formatting functions we do not need to
                        // alter the remote JSON data, except to indicate that infinite
                        // scrolling can be used
                        //params.page = params.page || 1;
                        //{results:[{id:1, text:'Red'},{id:2, text:'Blue'}], more:true}

                        if (result.Status === 1) {
                            alert(result.Errors[0]);
                            return false;
                        }
                        return {
                            results: result.Data,
                            more: false
                        };
                    },
                    cache: true,
                    error: function () {
                        alert('hookSelect2Ajax.error');
                    }
                },
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                placeholder: "Search for School Name",
                minimumInputLength: 3,
                maximumInputLength: 200,
                templateResult: function () { },
                templateSelection: function () { }
            };
            if (helper.hasValue(valueId)) {
                options.initSelection = function (element, callback) {
                    callback({ id: valueId, text: valueText });
                };
            }
            hookSelect2(pContainer, targetSelector, options);
        }

        function hookSchoolChange(pContainer, schoolSelector, selectorsToUpdate) {
            $(schoolSelector).on('select2:select change',
                function () {
                    var schoolDetails = ($(this).val().split(';').length === 1 ? $(schoolSelector, pContainer).attr('data-details') : $(this).val()).split(';');
                    var fullSelectorList = [
                        '#SchoolCode',
                        '#PostalStreet',
                        '#PostalSuburb',
                        '#PostalState',
                        '#PostalPostCode',
                        '#PrimaryPhone',
                        '#Facsimile'
                    ];
                    for (var s = 0; s < selectorsToUpdate.length; s++) {
                        var index = $.inArray(selectorsToUpdate[s], fullSelectorList);
                        if (index === -1) continue;
                        if (fullSelectorList[index] === '#PostalSuburb') {
                            $(fullSelectorList[index])
                                .empty()
                                .append('<option value=' +
                                schoolDetails[index] +
                                ' selected="selected">' +
                                schoolDetails[index] +
                                '</option>');
                            hookSearchLocation(pContainer, fullSelectorList[index], schoolDetails[index]);
                        } else if ((fullSelectorList[index] === '#PostalState')) {
                            $(fullSelectorList[index]).val(schoolDetails[index]).trigger('change');
                        } else {
                            $(fullSelectorList[index]).val(schoolDetails[index]);
                        }
                    }
                });
        }

        function hookRevalidateFieldOnChange(pContainer, selector) {
            $(selector, pContainer).on('change', function () {
                revalidateFormField($(this));
            }
            );
        }

        function revalidateFormField(field) {
            var form = field.closest('form');
            var fieldName = field.prop('name');
            form.formValidation(cnt.fv_RevalidateField, fieldName);
        }

        function hookCloseCancelPageForm() {
            $('.cancel-page-form')
                .on('click',
                function () {
                    var header = $('h2').text();
                    var title = header.indexOf('Detail') > -1 ? 'Close ' + header : header + ' cancelled';
                    var message = 'You should now close this tab.';
                    showDialog('alert', title, message);
                });
        }

        function hookManageContact() {
            $('#ManageContact').on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                var win = window.open($('#formType').val() === 'Edit' ? cnt.EditContact + '?userid=' + $('#UserId').val() :
                    cnt.CreateContact, '_blank', '');
                if (win) {
                    win.focus();
                }
            });
        }

        function setDropdwnOptions(elem, action, data, sucess) {
            api.ajaxGet(action, data,
                function (items) {
                    $.each(items, function (id) {
                        elem.append($('<option/>', {
                            value: items[id].Value,
                            text: items[id].Text

                        }));

                    });
                    if (helper.hasValue(sucess)) {
                        sucess();
                    }
                }

            );
            elem.select2("val", "0");
        }
    });