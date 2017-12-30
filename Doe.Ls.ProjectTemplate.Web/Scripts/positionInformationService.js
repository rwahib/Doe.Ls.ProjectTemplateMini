/// <reference path="_references.js" />
/// <reference path="~/Scripts/Framework/jquery-1.12.0.intellisense.js" />
/// <reference path="~/Scripts/Plugins/bootstrap-FormValidation/js/formValidation.js" />
/// <reference path="~/Scripts/Plugins/bootstrapDatepicker/bootstrap-datepicker.js" />
define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {
    var $form = $('#form-positionInformation-basicDetails');
    return {
        initialise: function () {
            validatePositionEndDate();
            hookPopup();

            $('.table').on('draw.dt', function () {
                hookPopup();
            });
        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
                { 'sName':'PositionId','mData':'PositionId', 'defaultContent': ''},
                {'sName':'OlderPositionNumber3','mData':'OlderPositionNumber3', 'defaultContent': ''  },
                {'sName':'OlderPositionNumber1','mData':'OlderPositionNumber1', 'defaultContent': ''  },
                {'sName':'OlderPositionNumber2','mData':'OlderPositionNumber2', 'defaultContent': ''  },
                {'sName':'SchNumber','mData':'SchNumber', 'defaultContent': ''  },
                { 'sName':'PositionTypeCode','mData':'PositionTypeCode', 'defaultContent': ''},
                { 'sName':'EmployeeTypeCode','mData':'EmployeeTypeCode', 'defaultContent': ''},
                {'sName':'PositionNoteId','mData':'PositionNoteId', 'defaultContent': ''  },
                {'sName':'TrimLink','mData':'TrimLink', 'defaultContent': ''  },
                {
                    'sName':'PositionEndDate',
                    'mData': 'PositionEndDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'PositionFTE','mData':'PositionFTE', 'defaultContent': ''  },
                {
                    'mData': 'PositionId',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="PositionInformation/Edit?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="PositionInformation/Details?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="PositionInformation/Delete?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
                                '</div>';
                    },
                    'bSortable': false
                }
            ],
            'aaSorting': [[0,'asc']],
            'bFilter': true,
            // Use this to define the onDrawCallBack for Data Table
            'fnDrawCallback': function () {
                hookPopup();
            },
            // Use this to set server parameters
            //'fnServerParams': function (aoData) {
            //      function to construct Server parameters 
            //}
        }
    }

    function validatePositionEndDate() {
        
        if ($('#PositionTypeCode').val() !== "T") {
            $('#PositionEndDate').val("");
            $('#spn-end-date').hide();
            $('#PositionEndDate').prop('disabled', true);
        }
        $("#PositionTypeCode").on("change", function () {
            if ($('#PositionTypeCode').val() !== "T") {
                $('#PositionEndDate').prop('disabled', true);
                $('#PositionEndDate').val("");
                $('#spn-end-date').hide();
                
            } else {
                $('#PositionEndDate').prop('disabled', false);
                $('#spn-end-date').show();
            }
            if ($('#PositionEndDate').prop('disabled') === true) {
                $('#PositionEndDate').val('');
                $('#PositionEndDate').datepicker('hide');
                $('#PositionEndDate').prop('disabled', false);
                $form.data('formValidation').resetField('PositionEndDate', true);
                $('#PositionEndDate').prop('disabled', true);
            } else {
                $form.data('formValidation').resetField('PositionEndDate', true);
            }
        });


        var customValidateStep = function () {

            
            var newSettings = {
                fields: {
                    PositionEndDate: {
                        validators: {
                            callback: {
                                //  message
                                callback:
                                    function (value, validator) {
                                        var msg = " ";
                                        var positionCode = $('#PositionTypeCode').val();
                                        if (positionCode === "T") {
                                            var dateHasValue = helper.hasValue($('#PositionEndDate').val()) && $('#PositionEndDate').val().length > 1;
                                            if (!dateHasValue) {
                                                msg = "Position end date is required";
                                                return {
                                                    valid: false, // or false
                                                    message: msg
                                                };
                                            }
                                            
                                            return true;
                                        } else {
                                            
                                            return true;
                                        }
                                    }
                            }
                        }
                    }
                 


                }
            };

            interactive.validateForm($form, newSettings);

        }

        customValidateStep();
    }


    function validateEndDates(positionCode) {
      
    }

    function hookPopup(){
        if ($('.pop-up-notes').length > 0) {
            interactive.hookPopupButton(
                {}                  // custom validation settings
                ,function(){        // modal load callback function
                }
                ,true               // use Ajax Submit Handler
                ,function(data){    // success callback function

                    $('.error-popup').hide();
                    $('#formTab-popup').hide();
                    $('#lbl-success')
                        .text('Comment has been successfully updated');
                    $('.success-popup').show(500);

                    location.reload();
                }
                ,function(data){    // error callback function
                }
                ,'.pop-up-notes'          // element selector
                ,true               // auto hide
            );
        }
    }

});

