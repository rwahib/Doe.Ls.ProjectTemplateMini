
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
                {'sName':'ExecutiveCod','mData':'ExecutiveCod' },
                {'sName':'ExecutiveTitle','mData':'ExecutiveTitle', 'defaultContent': ''  },
                {'sName':'ExecutiveDescription','mData':'ExecutiveDescription', 'defaultContent': ''  },
                {'sName':'CustomClass','mData':'CustomClass', 'defaultContent': ''  },
                { 'sName':'StatusId','mData':'StatusId', 'defaultContent': ''},
                {
                    'sName':'CreatedDate',
                    'mData': 'CreatedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'CreatedBy','mData':'CreatedBy', 'defaultContent': ''  },
                {
                    'sName':'LastModifiedDate',
                    'mData': 'LastModifiedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'LastModifiedBy','mData':'LastModifiedBy', 'defaultContent': ''  },
                {'sName':'DefaultExecutiveOverview','mData':'DefaultExecutiveOverview', 'defaultContent': ''  },
                {
                    'mData': 'ExecutiveCod',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="Executive/Edit?executiveCod=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="Executive/Details?executiveCod=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="Executive/Delete?executiveCod=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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

