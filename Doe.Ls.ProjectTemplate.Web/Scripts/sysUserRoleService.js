/// <reference path="_references.js" />
/// <reference path="~/Scripts/Framework/jquery-1.12.0.intellisense.js" />
define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {
    return {
        initialise: function () {
            var roleId = helper.hasValue($("#RoleId").val()) ? $("#RoleId").val() : $("#filterRoleId").val();
            var formType = $("#formType").val();
            initialiseCore(roleId, formType);


            $('.table').on('draw.dt', function () {
                hookPopup(roleId, formType);
            });
        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
                { 'sName': 'UserId', 'mData': 'UserId', 'defaultContent': '' },
                { 'sName': 'RoleId', 'mData': 'RoleId', 'defaultContent': '' },
                { 'sName': 'DirectorateId', 'mData': 'DirectorateId', 'defaultContent': '' },
                {
                    'sName': 'ActiveFrom',
                    'mData': 'ActiveFrom',
                    'mRender': function (value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {
                    'sName': 'ActiveTo',
                    'mData': 'ActiveTo',
                    'mRender': function (value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                { 'sName': 'Note', 'mData': 'Note', 'defaultContent': '' },
                {
                    'sName': 'CreatedDate',
                    'mData': 'CreatedDate',
                    'mRender': function (value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                { 'sName': 'CreatedBy', 'mData': 'CreatedBy', 'defaultContent': '' },
                {
                    'sName': 'LastModifiedDate',
                    'mData': 'LastModifiedDate',
                    'mRender': function (value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                { 'sName': 'LastModifiedBy', 'mData': 'LastModifiedBy', 'defaultContent': '' },
                {
                    'mData': 'UserId',
                    'mRender': function (code) {
                        return '<div class="btn-group-vertical">' +
                                    '<a href="SysUserRole/Edit?userId=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' +
                                    '<a href="SysUserRole/Details?userId=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' +
                                    '<a href="SysUserRole/Delete?userId=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' +
                                '</div>';
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
            //'fnServerParams': function (aoData) {
            //      function to construct Server parameters 
            //}
        }
    }

    function initialiseCore(roleId, formType) {
       hookPopup(roleId, formType);
    }

    function hookPopup(roleId, formType) {
        if ($('.pop-up').length > 0) {

            var validationSettings = {
                fields: {
                    ActiveTo: {
                        validators: {
                            callback: {
                                callback: function (value, validator, $field) {
                                    return validateActiveFromTo(validator);

                                }
                            }
                        }
                    },
                    ActiveFrom: {
                        validators: {
                            callback: {
                                callback: function (value, validator, $field) {
                                    return validateActiveFromTo(validator);

                                }
                            }
                        }
                    }
                }
            };


            interactive.hookPopupButton(
                validationSettings               // custom validation settings
                ,function(){        // modal load callback function
                    interactive.basicInitialisation('#vlePluginModal');
                    $('#ActiveFrom').on('change', function () {                        
                        $('#ActiveTo').attr('data-startdate', $('#ActiveFrom').val());
                        interactive.hookDatePicker();
                        $(this).closest('form').formValidation(cnt.fv_RevalidateField, "ActiveTo");
                    });

                    $('#ActiveTo').on('change', function () {
                    $(this).closest('form').formValidation(cnt.fv_RevalidateField, "ActiveFrom");
                    });
                }            
            );

        }

    }

    function validateActiveFromTo(validator) {

        var activeFromVal = $('#ActiveFrom').val();
        if (!helper.hasValue(activeFromVal)) {
            validator.updateStatus('ActiveFrom', validator.STATUS_VALID, 'callback');
            return {
                valid: false,    // or false
                message: 'Active from must have a value'
            }
        }
        var activeToVal = $('#ActiveTo').val();
        if (helper.hasValue(activeToVal)) {

            var activeFromDate = helper.parselongDate(activeFromVal);
            var activeToDate = helper.parselongDate(activeToVal);
            if (activeToDate < activeFromDate) {
                validator.updateStatus('ActiveTo', validator.STATUS_VALID, 'callback');
                return {
                    valid: false,    // or false
                    message: 'Active to must greater than active from'
                }
            }

        }

        return {
            valid: true,
            message: 'Invalid active to, active from'
        }
    }
    
  
});

