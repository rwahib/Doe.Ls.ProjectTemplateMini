 

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
                { 'sName':'GradeCode','mData':'GradeCode', 'defaultContent': ''},
                {'sName':'Foundational_Min','mData':'Foundational_Min', 'defaultContent': ''  },
                {'sName':'Foundational_Max','mData':'Foundational_Max', 'defaultContent': ''  },
                {'sName':'Intermediate_Min','mData':'Intermediate_Min', 'defaultContent': ''  },
                {'sName':'Intermediate_Max','mData':'Intermediate_Max', 'defaultContent': ''  },
                {'sName':'Adept_Min','mData':'Adept_Min', 'defaultContent': ''  },
                {'sName':'Adept_Max','mData':'Adept_Max', 'defaultContent': ''  },
                {'sName':'Advanced_Min','mData':'Advanced_Min', 'defaultContent': ''  },
                {'sName':'Advanced_Max','mData':'Advanced_Max', 'defaultContent': ''  },
                {'sName':'HighlyAdvanced_Min','mData':'HighlyAdvanced_Min', 'defaultContent': ''  },
                {'sName':'HighlyAdvanced_Max','mData':'HighlyAdvanced_Max', 'defaultContent': ''  },
                {'sName':'FocusCapabilities_Min','mData':'FocusCapabilities_Min', 'defaultContent': ''  },
                {'sName':'FocusCapabilities_Max','mData':'FocusCapabilities_Max', 'defaultContent': ''  },
                {'sName':'Notes','mData':'Notes', 'defaultContent': ''  },
                {
                    'mData': 'GradeCode',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="RoleDescCapabilityMatrix/Edit?gradeCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="RoleDescCapabilityMatrix/Details?gradeCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="RoleDescCapabilityMatrix/Delete?gradeCode=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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

