
/// <reference path="_references.js" />
/// <reference path="~/Scripts/Framework/jquery-1.12.0.intellisense.js" />
define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {

            $('html').css('overflow-x', 'hidden');
            interactive.hookSelect2Ajax();
            hookLookups();
            hookPopup();
            loadFilterLookups();
            resetFilter();
            validateBasicDetails();

            $("[id*=FilterDivisionCode],[id*=FilterDirectorateId],[id*=FilterBusinessUnitId],[id*=FilterUnitId],[id*=FilterPosStatusCode],[id*=FilterStatusCode").change(function () {
                setUrl();
            });
            if (helper.hasValue($('#OldReportToPositionId').val()) && $('#OldReportToPositionId').val() != "0") {
                $('#ReportToPositionId').select2('data', { id: $('#OldReportToPositionId').val(), text: $('#OldReportToPositionText').val() });
            } else {
                $('#ReportToPositionId').select2('data', { id: null, text: "Select a report to position" });
            }


        },

        dtSettings: {
            'iDisplayLength': getTableLength(),
            'aoColumns': [
                /*{ 'sName': 'PositionId', 'mData': 'PositionId' },*/
                { 'sName': 'PositionNumber', 'mData': 'PositionNumber', 'defaultContent': '' },
                { 'sName': 'PositionTitle', 'mData': 'PositionTitle', 'defaultContent': '' },
                {
                    'sName': 'ParentPosition.PositionTitle', 'mData': 'ParentPositionTitle',
                    'mRender': function (value, type, row) {
                        return '<a href=' + window.appUrl + 'Position/ManageSummary?id=' + row.ReportToPositionId + '>' + value + '</a>';
                    }
                },
                {
                    'sName': 'Unit.UnitName',
                    'mData': 'UnitName',
                    'mRender': function (value, type, row) {
                        return '<a href=' + window.appUrl + 'Unit/Details?id=' + row.UnitId + '>' + value + '</a>';
                    }
                },
                { 'sName': 'RolePositionDescription.GradeCode', 'mData': 'Grade', 'defaultContent': '' },
                { 'sName': 'PositionLevel.PositionLevelName', 'mData': 'PositionLevel', 'defaultContent': '' },
                {
                    'sName': 'RolePositionDescription.DocNumber', 'mData': 'DOCNumber',
                    'mRender': function (value, type, row) {
                        return '<a href=' + window.appUrl + 'RolePositionDescription/Manage?id=' + row.RolePositionDescriptionId + '>' + value + '</a>';
                    }

                },
                {
                    'sName': 'StatusValue.StatusName', 'mData': 'StatusName',
                    'mRender': function (value, type, row) {
                        return "<span class='" + value + "'>" + value + "</span>";
                    }
                },
                { 'sName': 'PositionInformation.EmployeeTypeCode', 'mData': 'EmployeeType', 'defaultContent': '' },
                { 'sName': 'PositionInformation.OccupationTypeCode', 'mData': 'OccupationType', 'defaultContent': '' },
                { 'sName': 'PositionInformation.PositionType', 'mData': 'PositionType', 'defaultContent': '' },

                {
                    'sName': 'LastModifiedDate', 'mData': 'LastModifiedDate', 'mRender': function (value, type, row) {
                        return helper.getDateFromJsonDate(value);
                    }
                },
                {
                    'mData': 'PositionId',
                    'mRender': function (code, type, access) {
                        var rtnValue = "";
                        if (access.Privilege.CanEdit) {
                            rtnValue = rtnValue +
                                '<a href=' + window.appUrl + 'Position/Edit?id=' + code + ' class="btn btn-primary ">Edit</a>';
                        }
                        if (access.Privilege.CanPerformActions) {
                            rtnValue = rtnValue +
                                '<a href=' +
                                window.appUrl +
                                'Position/ManageActions/' +
                                code +
                                ' class="btn btn-primary " >Actions</a>';
                        }
                        rtnValue = rtnValue +
                           '<a href=' +
                           window.appUrl +
                           'Position/ManageSummary?id=' +
                           code +
                           ' class="btn btn-primary ">Summary</a>';



                        return '<div class="btn-group-vertical">' + rtnValue + '<div>';
                    },
                    'bSortable': false
                }
            ],
            'aaSorting': getSorting(),
            'bFilter': true,
            // Use this to define the onDrawCallBack for Data Table
            'fnDrawCallback': function () {
                hookPopup();
            },
            // Use this to set server parameters
            'fnServerParams': function (aoData) {
                aoData.push({
                    name: "DirectorateId", value: $('#FilterDirectorateId').val()
                });

                aoData.push({
                    name: "BusinessUnitId", value: $('#FilterBusinessUnitId').val()
                });
                aoData.push({
                    name: "UnitId", value: $('#FilterUnitId').val()
                });
                aoData.push({
                    name: "PosStatusCode",
                    value: helper.hasValue($('#FilterPosStatusCode').val()) ? $('#FilterPosStatusCode').val().toString() : ''
                    //value: $('#FilterPosStatusCode').val()
                });
                aoData.push({
                    name: "StatusCode",
                    value: helper.hasValue($('#FilterStatusCode').val()) ? $('#FilterStatusCode').val().toString() : ''
                    //value: $('#FilterStatusCode').val()
                });
                aoData.push({
                    name: "DivisionCode", value: $('#FilterDivisionCode').val()
                });
                aoData.push({
                    name: "HasSession", value: $('#HasSession').val()
                });

            }
        }
    };

    function loadFilterLookups() {

        //Functional area ID

        // TODO var selectDir = $('#FilterExecutiveCode');
        var selectDir = $('#FilterDirectorateId');
        var selectBU = $('#FilterBusinessUnitId');
        var selectUnit = $('#FilterUnitId');
        $('#FilterBusinessUnitId').on('change', function () {
            helper.setSelectEmpty(selectUnit);

            disbale(selectUnit, true);
            interactive.setDropdwnOptions(selectUnit, cnt.GetUnits, { BUnitId: $('#FilterBusinessUnitId').val() }, function () {
                disbale(selectBU, false);
                disbale(selectUnit, false);

            });
            $('.table').dataTable().fnDraw();
        });
        //Directorate ID

        $('#FilterDirectorateId').on('change', function () {
            helper.setSelectEmpty(selectBU);
            helper.setSelectEmpty(selectUnit);

            disbale(selectBU, true);
            disbale(selectUnit, true);

            interactive.setDropdwnOptions(selectBU, cnt.GetBusinessUnits, { DirectorateId: $('#FilterDirectorateId').val() }, function () {

                disbale(selectBU, false);
                disbale(selectUnit, false);

            });
            $('.table').dataTable().fnDraw();
        });

        $('#FilterDivisionCode').on('change', function () {
            helper.setSelectEmpty(selectDir);
            helper.setSelectEmpty(selectBU);
            helper.setSelectEmpty(selectUnit);
            disbale(selectDir, true);
            disbale(selectBU, true);
            disbale(selectUnit, true);

            interactive.setDropdwnOptions(selectDir,
                cnt.GetDirectorates,
                {
                    divisionCode: $('#FilterDivisionCode').val()
                }, function () {
                    disbale(selectDir, false);
                    disbale(selectBU, false);
                    disbale(selectUnit, false);

                });

            $('.table').dataTable().fnDraw();
        });
        $('#FilterUnitId,#FilterPosStatusCode, #FilterStatusCode').on('change', function () {
            $('.table').dataTable().fnDraw();
        });
    }

    function setUrl() {
        var divisionCode = $('#FilterDivisionCode').val();
        var directorateId = $('#FilterDirectorateId').val();
        var bUnitId = $('#FilterBusinessUnitId').val();
        var unitId = $('#FilterUnitId').val();

        var posStatusCode = !$('#FilterPosStatusCode').val() ? "" : $('#FilterPosStatusCode').val();
        var statusCode = !$('#FilterStatusCode').val() ? "" : $('#FilterStatusCode').val();

        var bUrl = window.appUrl;
        if ($('#positionListType').val() === 'ApprovalList') {
            bUrl = window.appUrl + "PositionApprovalList";
        } else {
            bUrl = window.appUrl + "Position";
        }


        var queryString = "DirectorateId=" + directorateId;
        queryString = queryString + "&DivisionCode=" + divisionCode;
        queryString = queryString + "&BusinessUnitId=" + bUnitId;
        queryString = queryString + "&UnitId=" + unitId;
        queryString = queryString + "&PosStatusCode=" + posStatusCode;
        queryString = queryString + "&StatusCode=" + statusCode;

        bUrl = bUrl + "?" + queryString;

        window.history.pushState(" ", "Position List", bUrl);
        $("input:text").focus(function () { $(this).select(); });

    }

    function hookLookups() {
        var selectDir = $('#DirectorateId');
        var selectBU = $('#BusinessUnitId');
        var selectUnit = $('#UnitId');
        var selectLocation = $('#LocationId');
        // var selectUnitChief = $('#UnitChiefPositionId');

        if ($('#formpage').val() === 'true') {

        }


        $('#DivisionId')
            .on('change',
                function () {

                    helper.setSelectEmpty(selectDir, "Please select");
                    helper.setSelectEmpty(selectBU, "Please select");
                    helper.setSelectEmpty(selectUnit, "Please select");
                    helper.setSelectEmpty(selectLocation, "Please select");
                    disbale(selectDir, true);
                    disbale(selectBU, true);
                    disbale(selectUnit, true);
                    disbale(selectLocation, true);

                    interactive.setDropdwnOptions(selectDir, cnt.GetDirectorates, {
                        divisionCode: helper.hasValue($('#DivisionId').val()) && $('#DivisionId').val().length > 0 ? $('#DivisionId').val() : 0
                        , displayNumbers: false, currentUser: true
                    }, function () {
                        disbale(selectDir, false);
                        disbale(selectBU, false);
                        disbale(selectUnit, false);
                        disbale(selectLocation, false);

                    });


                    interactive.revalidateFormField(selectBU);
                    interactive.revalidateFormField(selectUnit);
                    interactive.revalidateFormField(selectLocation);

                });
        $('#DirectorateId')
            .on('change',
                function () {
                    helper.setSelectEmpty(selectUnit, "Please select");
                    helper.setSelectEmpty(selectBU, "Please select");
                    helper.setSelectEmpty(selectLocation, "Please select");


                    disbale(selectBU, true);
                    disbale(selectUnit, true);
                    disbale(selectLocation, true);

                    interactive.setDropdwnOptions(selectBU, cnt.GetBusinessUnits, {
                        DirectorateId: helper.hasValue($('#DirectorateId').val()) && $('#DirectorateId').val().length > 0 ? $('#DirectorateId').val() : 0
                        , displayNumbers: false, currentUser: true
                    }, function () {
                        disbale(selectBU, false);
                        disbale(selectUnit, false);
                        disbale(selectLocation, false);
                    });

                    interactive.setDropdwnOptions(selectLocation, cnt.GetLocations, {
                        DirectorateId: helper.hasValue($('#DirectorateId').val()) && $('#DirectorateId').val().length > 0 ? $('#DirectorateId').val() : 0
                        , displayNumbers: false
                    });

                    interactive.revalidateFormField(selectBU);
                    interactive.revalidateFormField(selectUnit);
                    interactive.revalidateFormField(selectLocation);

                });

        $(selectBU).on("change", function (e) {
            helper.setSelectEmpty(selectUnit, "Please select");

            disbale(selectUnit, true);

            interactive.setDropdwnOptions(selectUnit, cnt.GetUnits, { BUnitId: helper.hasValue($('#BusinessUnitId').val()) && $('#BusinessUnitId').val().length > 0 ? $('#BusinessUnitId').val() : 0, displayNumbers: false }
                , function () {
                    disbale(selectUnit, false);
                }
                );
            interactive.revalidateFormField(selectUnit);

            // $('#form-position-basicDetails').bootstrapValidator('resetForm', true);

        });
    }

    function resetFilter() {

        var selectBUnit = $('#FilterBusinessUnitId');
        var selectUnit = $('#FilterUnitId');
        var selectStatus = $('#FilterPosStatusCode');
        var selectStatusCode = $('#FilterStatusCode');
        var selectDivisionCode = $('#FilterDivisionCode');

        var selectDirectorate = $('#FilterDirectorateId');

        $('#btnReset')
            .on('click',
                function () {
                    helper.setSelectEmpty(selectUnit, "All");
                    helper.setSelectEmpty(selectBUnit, "All");
                    helper.setSelectEmpty(selectDirectorate, "All");
                    // helper.setSelectEmpty(selectDivisionCode, "All");
                    selectStatus.select2("val", '');
                    selectStatusCode.select2("val", '');
                    selectDivisionCode.select2("val", '');
                    //selectDirectorate.select2("val", 'All');
                    setUrl();
                    $('.table').dataTable().fnDraw();

                });


    }

    function toggleExistingOrNew() {
        $("#newRD").hide();
        $("#existingRD").hide();

        $("input[name='select']").on("click", function () {

            // $('#form-rolepositiondesc-create').bootstrapValidator('resetForm', true);

            if ($(this).val() === "1") {
                $("#newRD").hide();
                $("#existingRD").show();
                $("#RolePositionDescId").val("");
                $('#RolePositionDescId').select2('data', { id: "", text: "" });

            } else {
                //$("#DocNumberPart1").val("");
                //$("#DocNumberPart2").val("");
                $("#Title").val("");
                $("#newRD").show();
                $("#existingRD").hide();
            }

        });

    }

    function toggleCloneExistingOrNew() {
        $("#newRD").hide();
        $("#existingRD").hide();

        $("input[name='select']").on("click", function () {

            var positionNum = $('#PositionNumber').val();
            $('#form-clonePosition-confirm').bootstrapValidator('resetForm', true);

            if ($(this).val() === "0") {
                $("#newRD").hide();
                $("#existingRD").hide();
                $("#RolePositionDescId").val("");
                $('#RolePositionDescId').select2('data', { id: "", text: "" });
            }
            else if ($(this).val() === "1") {
                $("#newRD").hide();
                $("#existingRD").show();
                $("#RolePositionDescId").val("");
                $('#RolePositionDescId').select2('data', { id: "", text: "" });

            } else {
                //$("#DocNumberPart1").val("");
                //$("#DocNumberPart2").val("");
                $("#Title").val("");
                $("#newRD").show();
                $("#existingRD").hide();
            }
            $('#PositionNumber').val(positionNum);

        });

    }

    function initialSetup() {

        var selectGrade = $('#GradeCode');
        if ($('#DescType').is(':checked')) {
            selectGrade.removeProp("disabled");

        } else {
            selectGrade.attr("disabled", true);
        }

    }

    function loadGradeonDescType() {
        initialSetup();
        $("input[name='DescType']").on("click", function () {
            var selectGrade = $('#GradeCode');
            selectGrade.removeProp("disabled");

            //helper.setSelectEmpty(selectGrade,"Select a Grade");
            var action = cnt.GetGrades;
            api.ajaxGet(action, { descType: $('input:radio[name=DescType]:checked').val() },
                        function (items) {

                            selectGrade.empty();
                            selectGrade.append('<option value="">Select a Grade....</option>');
                            $.each(items,
                                function (id) {
                                    selectGrade.append($('<option/>',
                                    {
                                        value: items[id].Value,
                                        text: items[id].Text
                                    }));
                                });
                            selectGrade.select2();
                            $('#form-rolepositiondesc-create').formValidation(cnt.fv_RevalidateField, 'GradeCode');
                            $('#form-rolepositiondesc-create').formValidation(cnt.fv_RevalidateField, 'Title');
                            $('#form-clonePosition-confirm').formValidation(cnt.fv_RevalidateField, 'GradeCode');
                            $('#form-clonePosition-confirm').formValidation(cnt.fv_RevalidateField, 'Title');

                        });
        });
    }

    function hookPopup() {
        var popupContainer = $('#vlePluginModal');

        if ($('.pop-up').length > 0) {
            interactive.hookPopupButton(
                {}                  // custom validation settings
                , function () {        // modal load callback function
                    toggleExistingOrNew();
                    loadGradeonDescType();
                    interactive.hookSelect2($('#form-rolepositiondesc-create'), $('#GradeCode'));
                    interactive.hookSelect2($('#form-clonePosition-confirm'), $('#GradeCode'));
                    interactive.hookSelect2Ajax();
                    validateRadioButtons();

                }
                , true               // use Ajax Submit Handler
                , function (data) {    // success callback function

                    if (helper.hasValue(data.Data)) {
                        window.location.href = data.Data;
                    }
                }
             //   ,function(data){    // error callback function
           //     }
           //     ,'.pop-up'          // element selector
           //     ,true               // auto hide
            );
        }

        if ($('.clone-action-pop-up').length > 0) {

            interactive.hookPopupButton(
            {}                  // custom validation settings
                , function () {        // modal load callback function
                    toggleCloneExistingOrNew();
                    loadGradeonDescType();
                    interactive.hookSelect2($('#form-clonePosition-confirm'), $('#GradeCode'));
                    interactive.hookSelect2Ajax();
                    validateRadioButtons();
                    if (('#form-clonePosition-confirm').length > 0) {
                        interactive.validateForm($('#form-clonePosition-confirm'), getPositionNumberValidation());
                    }
                }
                , true               // use Ajax Submit Handler
                , function (result, textStatus, jqXhr) {    // success callback function
                    if (helper.hasValue(result.Data)) {
                        $('.error-popup').hide();
                        $('#formTab').hide();
                        $('#lbl-success')
                            .text("Successfully cloned a position, please wait...");
                        $('.success-popup').show(10);
                        window.setTimeout(function () {
                            window.location.href = result.Data;
                        },
                            4000);
                    }
                }
                , function (result, textStatus, jqXhr) {    // error callback function
                    $('#lbl-success').hide();
                    $('#formTab').hide();
                    $('#lbl-error').text("Cloning failed. Please try again later.");
                    $('.error-popup').show(10);
                }
                    , '.clone-action-pop-up'          // element selector
                        , true               // auto hide
            );
        }

        if ($('.action-pop-up').length > 0) {

            interactive.hookPopupButton(
                {}                  // custom validation settings
                , function () {        // modal load callback function
                    interactive.basicInitialisation(popupContainer);
                    interactive.hookSelect2Ajax(popupContainer);
                    $('#dismiss').hide();
                }
                , true               // use Ajax Submit Handler
                , function (result, textStatus, jqXhr) {    // success callback function                  
                    $('.error-popup').hide();
                    $('#formTab').hide();
                    $('#lbl-success')
                        .text(result.Message);
                    if (helper.hasValue(result.HeaderText)) {
                        $('.modal-header h3').text(result.HeaderText);
                    }
                    $('.success-popup').show(500);
                    $('#dismiss').show(500);
                }
                , function (data) {    // error callback function
                }
                , '.action-pop-up'          // element selector
                , false               // auto hide
            );
        }

        if ($('.move-positions-action-pop-up').length > 0) {

            interactive.hookPopupButton(
                {}                  // custom validation settings
                , function () {        // modal load callback function
                    interactive.basicInitialisation();
                    interactive.hookSelect2Ajax();
                    hookCheckUnCheckAll();
                    $('#dismiss').hide();
                }
                , null               // true
                , null
                , null
                , '.move-positions-action-pop-up'          // element selector
                , false               // auto hide
            );
        }

    }

    function disbale(elem, disable) {
        elem.prop('disabled', disable);
    }

    function getTableLength() {
        return ($("#positionListType").val() === "ApprovalList") ? 25 : 10;
    }
    function getSorting() {
        return [
            ($("#positionListType").val() === "ApprovalList") ? 11 : 0,
            ($("#positionListType").val() === "ApprovalList") ? 'desc' : 'asc'
        ];
    }

    function validateRadioButtons() {
        var form = $('#form-rolepositiondesc-create');

        var formClone = $('#form-clonePosition-confirm');
        /*Create form*/
        if (form.length > 0) {
            $('#btn-submit')
                .on("click",
                    function () {
                        if (!$('#existingRP').is(":checked") && !$('#newRP').is(':checked')) {
                            $('#msg').text("Please select a radio button first");
                        }
                    });
            $("input[name='select']")
               .on("click",
                   function () {
                       $('#msg').text("");
                   });
        }
        /*Clone form*/
        if (formClone.length > 0) {
            $('#btn-submit')
                .on("click",
                    function () {
                        if (!$('#currentRP').is(":checked") && !$('#existingRP').is(":checked") && !$('#newRP').is(':checked')) {
                            $('#msg').text("Please select a radio button first");
                            return false;
                        }
                    });
            $("input[name='select']")
               .on("click",
                   function () {
                       $('#msg').text("");

                   });

        }
    }

    function validateBasicDetails() {

        var $theForm = $('#form-position-basicDetails');
        if ($theForm.length > 0) {
            interactive.validateForm($theForm, getPositionNumberValidation());


            $('#PositionNumber')
                .on('change',
                    function () {
                        $theForm.formValidation('enableFieldValidators', 'PositionNumber', true);

                    });
        }
    };

    function getPositionNumberValidation() {
        return {
            fields: {
                PositionNumber: {
                    validators: {
                        remote: {
                            url: cnt.PositionNumberExists,
                            data: function (validator, $field, value) {
                                return {
                                    positionNumber: validator.getFieldElements('PositionNumber').val()
                                };
                            },
                            enabled: false,
                            type: 'POST',
                            delay: 1000,
                            message: 'The position number entered already exists, please try a different number.'
                        }
                    }
                },

                NewPositionNumber: {
                    validators: {
                        remote: {
                            url: cnt.PositionNumberExists,
                            data: function (validator, $field, value) {
                                return {
                                    positionNumber: validator.getFieldElements('NewPositionNumber').val()
                                };
                            },
                            type: 'POST',
                            delay: 1000,
                            message: 'The position number entered already exists, please try a different number.'
                        }
                    }
                }



            }
        };
    }

    function hookCheckUnCheckAll() {

        $('#CheckAll')
            .on('change',
                function () {
                    if ($('#CheckAll').prop('checked') === true) {
                        $('input[name^="checkbox_"]').each(
                            function () {
                                $(this).prop('checked', true);
                            });

                    } else {
                        $('input[name^="checkbox_"]').each(
                             function () {
                                 $(this).prop('checked', false);
                             });
                    }
                });
    }


});

