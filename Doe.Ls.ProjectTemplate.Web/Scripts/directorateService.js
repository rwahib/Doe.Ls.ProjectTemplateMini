
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function() {
            loadFilterLookups();
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
                {'sName':'DirectorateId','mData':'DirectorateId' },
                { 'sName': 'Executive.ExecutiveTitle', 'mData': 'ExecutiveTitle', 'defaultContent': '' },
                {'sName':'DirectorateName','mData':'DirectorateName', 'defaultContent': ''  },
                { 'sName': 'StatusValue.StatusId', 'mData': 'Status', 'defaultContent': '' },
                {'sName':'DirectorateOrder','mData':'DirectorateOrder', 'defaultContent': ''  },                
                {
                    'sName':'LastModifiedDate',
                    'mData': 'LastModifiedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'LastModifiedBy','mData':'LastModifiedBy', 'defaultContent': ''  },
                {
                    'mData': 'DirectorateId',
                    'mRender': function (code,ype, full) {
                        var edit = full.Privilege.CanEdit ? '' : ' hide';
                        var del = full.Privilege.CanDelete ? '' : ' hide';
                        var read= full.Privilege.CanRead ? '' : ' hide';
                        
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="Directorate/Edit?id=' + code + '" class="btn btn-primary pop-up '+edit+'"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="Directorate/Details?id=' + code + '" class="btn btn-primary pop-up '+read+'" data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="Directorate/Delete?id=' + code + '" class="btn btn-primary pop-up '+del +'" data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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
                    name: "divisionCode", value: $('#executiveCode').val()
                });
            }
        }
    }

    function hookPopup(){
        if($('.pop-up').length>0){
            interactive.hookPopupButton(
                {}                  // custom validation settings
                ,function(){        // modal load callback function
                    interactive.basicInitialisation('#vlePluginModal');
                }
                ,true               // use Ajax Submit Handler
                ,function(data){    // success callback function
                    $('.error-popup').hide();
                    $('#formTab').hide();
                    $('#lbl-success')
                        .text('Data has been successfully updated');                   
                    $('.success-popup').show(500);
                    
                    $('.table').dataTable().fnDraw();
                }
                ,function(data){    // error callback function
                }
                ,'.pop-up'          // element selector
                ,true               // auto hide
            );
        }
    }
    function loadFilterLookups() {        
        $('#executiveCode').on('change', function () {
            var link = $('#creat-directorate');
            link.attr('href', appUrl + 'Directorate/Create?ExecutiveCode=' + $('#executiveCode').val());
            $('.table').dataTable().fnDraw();
        });
    }
});

