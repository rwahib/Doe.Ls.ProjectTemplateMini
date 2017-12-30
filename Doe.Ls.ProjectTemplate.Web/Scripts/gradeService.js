
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function(){
            hookLookups();
        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
                {'sName':'GradeCode','mData':'GradeCode' },
                { 'sName': 'GradeTitle', 'mData': 'GradeTitle', 'defaultContent': '' },
                { 'sName': 'GradeType', 'mData': 'GradeType', 'defaultContent': '' },
                {'sName':'Award','mData':'Award', 'defaultContent': ''  },                
                { 'sName': 'StatusValue.StatusName', 'mData': 'StatusName', 'defaultContent': '' },
                {
                    'mData': 'GradeCode',
                    'mRender': function (code, ype, full) {
                        var edit = full.Privilege.CanEdit ? '' : ' hide';
                        var del = full.Privilege.CanDelete ? '' : ' hide';
                        var read = full.Privilege.CanRead ? '' : ' hide';
                        return '<div class="btn-group-vertical">' +
                                  '<a href="Grade/Edit?gradeCode=' + code + '" class="btn btn-primary pop-up ' + edit + '"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' +
                                  '<a href="Grade/Details?gradeCode=' + code + '" class="btn btn-primary pop-up ' + read + '" data-toggle="modal" data-target="#vlePluginModal">Detail</a>' +
                                  '<a href="Grade/Delete?gradeCode=' + code + '" class="btn btn-primary pop-up ' + del + '" data-toggle="modal" data-target="#vlePluginModal">Delete</a>' +
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
            'fnServerParams': function (aoData) {
                aoData.push({
                    name: "GradeType", value: $('#GradeType').val()
                }, {
                    name: "StatusCode",
                    value: helper.hasValue($('#FilterStatusCode').val()) ? $('#FilterStatusCode').val().toString() : null                 
                }
                 );
            }
        }
    }

    function hookPopup() {
        if ($('.pop-up').length > 0) {
            interactive.hookPopupButton(
                {}                  // custom validation settings
                , function () {        // modal load callback function
                    interactive.basicInitialisation('#vlePluginModal');
                    
                }
                , true               // use Ajax Submit Handler
                , function (data) {    // success callback function
                    $('.error-popup').hide();
                    $('#formTab').hide();
                    $('#lbl-success')
                        .text('Data has been successfully updated');
                    $('.success-popup').show(500);

                    $('.table').dataTable().fnDraw();

                    $('.table').dataTable().fnDraw();
                }
                , function (data) {    // error callback function
                }
                , '.pop-up'          // element selector
                , true               // auto hide
            );
        }
    }
    function hookLookups() {
        console.log($('#GradeType').val());
        console.log($('#FilterStatusCode').val());
        
        $('#GradeType, #FilterStatusCode')
            .on('change',
                function () {

                    console.log('Fired !!!!');

                    $('.table').dataTable().fnDraw();

                });

        
    }

});

