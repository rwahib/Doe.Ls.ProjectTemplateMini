
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function() {
            hookPopup();
            $('.table')
                .on('draw.dt',
                    function() {
                        hookPopup();
                    });


        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
            {
                'sName': 'RelationshipScope.ScopeTitle', 'mData': 'ScopeName', 'mRender': function (data) {
                    if (data === 'External') {
                        return "<div class='external scopeHeading'>" + data + "</div>";
                    }
                    else if (data === 'Internal') {
                        return "<div class='internal scopeHeading'>" + data + "</div>";
                    }
                    else if (data === 'Ministerial') {
                        return "<div class='scopeHeading'>" + data + "</div>";
                    }
                    return data;
              }
            },
            { 'sName': 'Who', 'mData': 'Who', 'defaultContent': '' },
            {
                'sName': 'Why',
                'mData': 'Why',
                'mRender': function (data) {
                    return "<div class='datatableRender'>" + data + "</div>";
                }
},
                {
                    'sName':'LastUpdated',
                    'mData': 'LastUpdated',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {
                    'mData': 'KeyRelationshipId',
                    'mRender': function(code, row, data){
                        return '<div class="btn-group-vertical">' +
                            '<a href="' + window.appUrl + 'KeyRelationship/Edit?id=' + code + '" class="btn btn-primary pop-up-key-relationship"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' +
                                    '<a href="' + window.appUrl + 'KeyRelationship/Details?id=' + code + '" class="btn btn-primary pop-up-key-relationship"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' +
                                    '<a href="' + window.appUrl + 'KeyRelationship/Delete?id=' + code + '" class="btn btn-primary pop-up-key-relationship"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' +
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

    function hookPopup() {
        
        if ($('.pop-up-key-relationship').length > 0) {
        interactive.hookPopupButton(
                {}                  // custom validation settings
                , function () {        // modal load callback function
                    interactive.basicInitialisation('#vlePluginModal');                    
                }
             ,true               // use Ajax Submit Handler
             , function (data) {    // success callback function
                 $('.error-popup').hide();
                 $('#formTab').hide();
                 $('#lbl-success')
                     .text('Data has been successfully updated');
                 $('.success-popup').show(500);
                 $('.table').dataTable().fnDraw();
             }
           ,null
           , '.pop-up-key-relationship'          // element selector
           ,true               // auto hide
            );
        }
    }

});

