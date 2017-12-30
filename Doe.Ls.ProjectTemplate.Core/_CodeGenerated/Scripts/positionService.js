
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function(){
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
                {'sName':'PositionId','mData':'PositionId' },
                { 'sName':'ReportToPositionId','mData':'ReportToPositionId', 'defaultContent': ''},
                {'sName':'PositionNumber','mData':'PositionNumber', 'defaultContent': ''  },
                { 'sName':'RolePositionDescriptionId','mData':'RolePositionDescriptionId', 'defaultContent': ''},
                { 'sName':'UnitId','mData':'UnitId', 'defaultContent': ''},
                {'sName':'PositionTitle','mData':'PositionTitle', 'defaultContent': ''  },
                {'sName':'Description','mData':'Description', 'defaultContent': ''  },
                { 'sName':'PositionLevelId','mData':'PositionLevelId', 'defaultContent': ''},
                { 'sName':'StatusId','mData':'StatusId', 'defaultContent': ''},
                {'sName':'PositionPath','mData':'PositionPath', 'defaultContent': ''  },
                { 'sName':'LocationId','mData':'LocationId', 'defaultContent': ''},
                {
                    'sName':'CreatedDate',
                    'mData': 'CreatedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {
                    'sName':'LastModifiedDate',
                    'mData': 'LastModifiedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'CreatedBy','mData':'CreatedBy', 'defaultContent': ''  },
                {'sName':'LastModifiedBy','mData':'LastModifiedBy', 'defaultContent': ''  },
                {'sName':'DivisionOverview','mData':'DivisionOverview', 'defaultContent': ''  },
                {
                    'mData': 'PositionId',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="Position/Edit?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="Position/Details?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="Position/Delete?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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

    function hookPopup(){
        if($('.pop-up').length>0){
            interactive.hookPopupButton(
               // {}                  // custom validation settings
              //  ,function(){        // modal load callback function
             //   }
             //   ,true               // use Ajax Submit Handler
             //   ,function(data){    // success callback function
             //   }
           //     ,function(data){    // error callback function
           //     }
           //     ,'.pop-up'          // element selector
           //     ,true               // auto hide
            );
        }
    }

});

