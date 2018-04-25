 

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
                { 'sName':'PositionStatusCode','mData':'PositionStatusCode', 'defaultContent': ''},
                { 'sName':'OccupationTypeCode','mData':'OccupationTypeCode', 'defaultContent': ''},
                {'sName':'OtherOverview','mData':'OtherOverview', 'defaultContent': ''  },
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

