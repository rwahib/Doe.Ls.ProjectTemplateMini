/// <reference path="_references.js" />
define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            $('.table').on('draw.dt', function () {
                hookPopup();
            });

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
                {'sName':'PositionLevelId','mData':'PositionLevelId' },
                {'sName':'PositionLevelName','mData':'PositionLevelName', 'defaultContent': ''  },
                {'sName':'CustomClass','mData':'CustomClass', 'defaultContent': ''  },
                {
                    'mData': 'PositionLevelId',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="PositionLevel/Edit?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="PositionLevel/Details?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="PositionLevel/Delete?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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

