 

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
                {'sName':'GradeCode','mData':'GradeCode' },
                {'sName':'GradeTitle','mData':'GradeTitle', 'defaultContent': ''  },
                {'sName':'Award','mData':'Award', 'defaultContent': ''  },
                {'sName':'AwardMaxRates','mData':'AwardMaxRates', 'defaultContent': ''  },
                {'sName':'TeachingBased','mData':'TeachingBased', 'defaultContent': ''  },
                {'sName':'GradeType','mData':'GradeType', 'defaultContent': ''  },
                { 'sName':'StatusId','mData':'StatusId', 'defaultContent': ''},
                {'sName':'Message','mData':'Message', 'defaultContent': ''  },
                {
                    'mData': 'GradeCode',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="Grade/Edit?gradeCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="Grade/Details?gradeCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="Grade/Delete?gradeCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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

