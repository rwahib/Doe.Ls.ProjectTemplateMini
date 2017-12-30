
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            hookPopup();
            toggleBudgetRadioButtons();
            displayIndicatorContext();
            hookFilterLookups();
            togglePeopleManagement();
            interactive.validateForm($('#form-roleDescription-basicDetails'), getDocNumValidation());
            validateBudgetValue();
            // Uncomment if custom validation is required
            // interactive.hookPopupButton(
            // {}                  // custom validation settings
            // ,function(){        // modal load callback function
            // }
            // ,true               // use Ajax Submit Handler
            // ,function(data){    // success callback function
            // }
            // ,function(data){    // error callback function
            // }
            // ,'.pop-up'          // element selector
            // ,true               // auto hide
            // );
        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
                { 'sName': 'RolePositionDescription.Title', 'mData': 'Title', 'defaultContent': '' },
                { 'sName': 'RolePositionDescription.DocNumber', 'mData': 'DocNumber', 'defaultContent': '' },
                { 'sName': 'RolePositionDescription.Grade.GradeTitle', 'mData': 'GradeTitle', 'defaultContent': '' },
               {
                   'sName': 'RolePositionDescription.DateOfApproval',
                   'mData': 'DateOfApproval',
                   'mRender': function (value) {
                       return value;
                   }
               },
               {
                   'sName': 'RolePositionDescription.StatusValue.StatusName', 'mData': 'Status',
                   'mRender': function (value) {
                       return "<span class='" + value + "'>" + value + "</span>";
                   }
               },
                {
                    'sName': 'RolePositionDescription.LastModifiedDate',
                    'mData': 'LastModifiedDate',
                    'mRender': function (value) {
                        return value;
                    }
                },
                 {
                     'sName': 'LinkedPositions',
                     'mData': 'LinkedPositions',
                     'mRender': function (value, id, row) {
                         var returnValue = '<a href="' + window.appUrl + 'RoleDescription/ManageLinkedPositions?id='
                              + row.RoleDescriptionId + '")>';
                         if (value > 0)
                             returnValue = returnValue + 'Positions (' + value + ')</a>';
                         else {
                             returnValue = returnValue + '</a>';
                         }

                         return returnValue;
                     },
                     'bSortable': false
                 },
                 {
                     'mData': 'RoleDescriptionId',
                     'mRender': function (code, type, access) {
                         var rtnValue = "";
                         if (access.Privilege.CanEdit) {
                             rtnValue = rtnValue +
                                 '<a href=' + window.appUrl + 'RoleDescription/Edit?id=' + code + ' class="btn btn-primary ">Edit</a>';
                         }
                         if (access.Privilege.CanDelete) {
                             rtnValue = rtnValue +
                                 '<a href=' +
                                 window.appUrl +
                                 'RoleDescription/Delete?id=' +
                                 code +
                                 ' class="btn btn-primary " >Delete</a>';
                         }
                         if (access.Privilege.CanPerformActions) {
                             rtnValue = rtnValue +
                                 '<a href=' +
                                 window.appUrl +
                                 'RoleDescription/ManageActions/' +
                                 code +
                                 ' class="btn btn-primary " >Actions</a>';
                         }
                         rtnValue = rtnValue +
                            '<a href=' +
                            window.appUrl +
                            'RoleDescription/ManageSummary?id=' +
                            code +
                            ' class="btn btn-primary ">Summary</a>';



                         return '<div class="btn-group-vertical">' + rtnValue + '<div>';
                     },
                     'bSortable': false
                 }
               
            ],
            'aaSorting': [[0, 'asc']],
            'bFilter': true,
            // Use this to define the onDrawCallBack for Data Table
            'fnDrawCallback': function () {
                hookPopup();
            },
            // Use this to set server parameters
            'fnServerParams': function (aoData) {
                aoData.push({
                    name: "GradeCode",
                    value: helper.hasValue($('#FilterGrade').val()) ? $('#FilterGrade').val().toString() : ''
                });
                aoData.push({
                    name: "StatusCode",
                    value: helper.hasValue($('#FilterStatus').val()) ? $('#FilterStatus').val().toString() : ''
                });
            }
        }
    }

    function hookPopup(){
        var popupContainer = $('#vlePluginModal');
        if ($('.pop-up').length > 0) {
            interactive.hookPopupButton(
                {}                  // custom validation settings
                , function () {        // modal load callback function
                    interactive.hookSelect2Ajax();
                }
                ,false               // use Ajax Submit Handler
                //, function (data) {    // success callback function
                //    $('.error-popup').hide();
                //    $('#formTab').hide();
                //    $('#lbl-success')
                //        .text('Status has been successfully updated');
                //    $('.success-popup').show(500);

                //    $('.table').dataTable().fnDraw();
                //}
                //, function (data) {    // error callback function
                   
                //}
                //,'.pop-up'          // element selector
                //,true               // auto hide
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
    }

    function hookFilterLookups() {
        $('#FilterGrade,#FilterStatus').on('change', function () {
            $('.table').dataTable().fnDraw();
        });
        //Directorate ID
    }

    function toggleBudgetRadioButtons() {
        if ($("#form-roleDescription-Budget").length > 0) {
            
            var selectedValue = $('input[name=BudgetExpenditure]:checked').val();
            if (selectedValue === "Nil") {
                $(".BudgetValuePanel").hide();
                $('#form-roleDescription-Budget').bootstrapValidator('resetForm', true);
            }

            $('input[type=radio][name=BudgetExpenditure]').change(function () {
                if (this.value === 'Nil') {
                    $(".BudgetValuePanel").hide();
                    $('#form-roleDescription-Budget').bootstrapValidator('resetForm', true);
                }
                else  {
                    $(".BudgetValuePanel").show();
                }
            });


        }
    }

 
    function displayIndicatorContext() {

        $("[name^=CapabilityLevelId]").change(function () {

            var nameId = $(this).attr('id').split("_");

            if ($(this).val() > 0) {

                var lvlId = $(this).val();
                api.ajaxGet(cnt.GetIndicatorContext,
                    {
                        LevelId: $(this).val(),
                        NameId: nameId[1]
                    },
                    function (result) {
                        var indContextId = '#indContext_' + nameId[1];
                        if (result.length > 50) {
                            var rs = result;//.substring(0, 50) + "<a target='_blank' href=" + rootUrl + "CapabilityBehaviourIndicator/Details?nameId=" + nameId[1] + "&levelId=" + lvlId + "> View details</a>";
                            $(indContextId).html(rs);
                        } else {
                            $(indContextId).html(result);
                        }

                    });
            } else {
                var selhgId = '#Highlighted_' + nameId[1];
                $(selhgId).prop('checked', false);
                var indContextId = '#indContext_' + nameId[1];
                $(indContextId).html("");
            }
        });

        $("[name^=Selected_]").change(function () {
            var nameId = $(this).attr('id').split("_");
            var selLvlId = '#CapabilityLevelId_' + nameId[1];
            var selhgId = '#Highlighted_' + nameId[1];
            $(selhgId).prop('checked', false);
            $(selLvlId).select2("val", "0");
            var indContextId = '#indContext_' + nameId[1];
            $(indContextId).html("");
            $(this).prop('disabled', true);
        });
    }

    function togglePeopleManagement() {
        if ($('#form-roleDescription-createCapabilities').length > 0) {
            var ckbox = $('#ManagerRole');

            if (ckbox.is(':checked')) {
                $(".PeopleManagement").show();
            } else {
                $(".PeopleManagement").hide();
            }


            $('input').on('click', function () {
                if (ckbox.is(':checked')) {
                   
                    $(".PeopleManagement").show();
                } else {
                   
                    $(".PeopleManagement").hide();
                }
            });

        }
    }


    function getDocNumValidation() {
       return {
            fields: {
                DocNumberPart2: {
                    validators: {
                        remote: {
                            url: cnt.DocNumberExists,
                            data: function (validator, $field, value) {
                                return {
                                    docNumPart1: validator.getFieldElements('DocNumberPart1').val(),
                                    docNumPart2: validator.getFieldElements('DocNumberPart2').val(),
                                    versionNum: validator.getFieldElements('RolePositionDescription.Version').val(),
                                    formType: validator.getFieldElements('FormType').val(),
                                    oldDocNum: validator.getFieldElements('RolePositionDescription.DocNumber').val()
                                };
                            },
                            type: 'POST',
                            delay: 1000,
                            message: 'The same Doc number already exists, please try a different number.'
                        }
                    }
                }

            }
        }
    }

    function validateBudgetValue() {
        var budgetForm = $("#form-roleDescription-Budget");
        if (budgetForm.length > 0) {
            var customValidateStep = function () {
                var newSettings = {
                    fields: {
                        BudgetValue: {
                            validators: {
                                callback: {
                                    //  message
                                    callback:
                                        function (value, validator) {
                                            var budgetValue = $('#BudgetValue').val();
                                            var selectedValue = $('input[name=BudgetExpenditure]:checked').val();
                                            if (selectedValue === "Nil") {
                                                return true;
                                            } else {
                                                if (budgetValue.length > 0 && budgetValue !=="0") {
                                                    return true;
                                                } else {
                                                    return {
                                                        valid: false, // or false
                                                        message: "Please enter a valid budget value"
                                                    };
                                                }
                                            }
                                           
                                       }
                                }
                            }
                        }
                    }
                };

                interactive.validateForm(budgetForm, newSettings);

            }

            customValidateStep();
        }
    }
});

