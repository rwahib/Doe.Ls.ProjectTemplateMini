
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
                {'sName':'ItemCode','mData':'ItemCode' },
                {'sName':'ItemName','mData':'ItemName', 'defaultContent': ''  },
                {'sName':'ItemDescription','mData':'ItemDescription', 'defaultContent': ''  },
                {'sName':'ItemContent','mData':'ItemContent', 'defaultContent': ''  },
                {'sName':'CreatedBy','mData':'CreatedBy', 'defaultContent': ''  },
                {
                    'sName':'CreatedDate',
                    'mData': 'CreatedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'LastupdatedBy','mData':'LastupdatedBy', 'defaultContent': ''  },
                {
                    'sName':'LastupdatedDate',
                    'mData': 'LastupdatedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {
                    'mData': 'ItemCode',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="Item/Edit?itemCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="Item/Details?itemCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="Item/Delete?itemCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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

