
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            hookPopup();
            hookSelectionCriteria();
            hookFilterLookups();

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
                   'sName': 'RolePositionDescription.StatusValue.StatusName', 'mData': 'StatusValue',
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
                         var returnValue = '<a href="' + window.appUrl + 'PositionDescription/ManageLinkedPositions?id='
                              + row.PositionDescriptionId + '")>';
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
                     'mData': 'PositionDescriptionId',
                     'mRender': function (code, type, access) {
                         var rtnValue = "";
                         if (access.Privilege.CanEdit) {
                             rtnValue = rtnValue +
                                 '<a href=' + window.appUrl + 'PositionDescription/Edit?id=' + code + ' class="btn btn-primary ">Edit</a>';
                         }
                         if (access.Privilege.CanDelete) {
                             rtnValue = rtnValue +
                                 '<a href=' +
                                 window.appUrl +
                                 'PositionDescription/Delete?id=' +
                                 code +
                                 ' class="btn btn-primary " >Delete</a>';
                         }
                         if (access.Privilege.CanPerformActions) {
                             rtnValue = rtnValue +
                                 '<a href=' +
                                 window.appUrl +
                                 'PositionDescription/ManageActions/' +
                                 code +
                                 ' class="btn btn-primary " >Actions</a>';
                         }
                         rtnValue = rtnValue +
                            '<a href=' +
                            window.appUrl +
                            'PositionDescription/ManageSummary?id=' +
                            code +
                            ' class="btn btn-primary ">Summary</a>';



                         return '<div class="btn-group-vertical">' + rtnValue + '<div>';
                     },
                     'bSortable': false
                 },
                //{
                //    'mData': 'PositionDescriptionId',
                //    'mRender': function (code, id, row) {
                //        if (row.Privilege.CanEdit==true) {
                //            return '<div class="btn-group-vertical">' +
                //                   '<a href="' + window.appUrl + 'PositionDescription/Edit?id=' + code + '" class="btn btn-primary">Edit</a>' +
                //                   '<a href="' + window.appUrl + 'PositionDescription/ManageSummary?id=' + code + '" class="btn btn-primary">Detail</a>' +
                //                  // '<a href="' + window.appUrl + 'PositionDescription/Delete?id=' + code + '" class="btn btn-primary">Delete</a>' +
                //               '</div>';
                //        }

                //        return '<div class="btn-group-vertical">' + 
                //                  //  '<a href="' + window.appUrl + 'PositionDescription/Edit?id=' + code + '" class="btn btn-primary">Edit</a>' +
                //                    '<a href="' + window.appUrl + 'PositionDescription/ManageSummary?id=' + code + '" class="btn btn-primary">Detail</a>' +
                //                   // '<a href="' + window.appUrl + 'PositionDescription/Delete?id=' + code + '" class="btn btn-primary">Delete</a>' +
                //                '</div>';
                //    },
                //    'bSortable': false
                //}
            ],
            'aaSorting': [[0,'asc']],
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

    //data-id="StatementOfDuties"
    //id=tinymce

    function hookFilterLookups() {
        $('#FilterGrade,#FilterStatus').on('change', function () {
            $('.table').dataTable().fnDraw();
        });
        //Directorate ID
    }

    function hookSelectionCriteria() {
        $('#item_custom').on('click', function () {
            if (this.checked) {
                $("#CustomChkBoxVal").val("true");
                $("#customCriteria").removeAttr("disabled");
                // $('#customCriteria').prop("disabled", false);
            } else {
                $("#customCriteria").val("");
                $("#CustomChkBoxVal").val("false");
                $("#customCriteria").attr("disabled", "disabled");
            }
        });
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
             //   ,function(data){    // success callback function
             //   }
           //     ,function(data){    // error callback function
           //     }
           //     ,'.pop-up'          // element selector
           //     ,true               // auto hide
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
                    console.log(result);
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

});

