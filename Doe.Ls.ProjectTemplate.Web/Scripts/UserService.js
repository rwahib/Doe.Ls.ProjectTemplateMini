
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function() {          
        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
                { 'sName': 'UserId', 'mData': 'UserName' },
                {'sName':'FirstName','mData':'FirstName', 'defaultContent': ''  },
                { 'sName': 'LastName', 'mData': 'SurName', 'defaultContent': '' },

                {
                    'sName': 'Email', 'mData': 'Email', 'defaultContent': '', 'mRender': function (mail) {
                        return '<a href="mailto:'+mail+'?Subject='+window.appUrl+'">' + mail + '</a>';
                    }

                },
                { 'sName': 'PrimaryPhone', 'mData': 'Phone', 'defaultContent': '' },
                { 'sName': 'CurrentRole', 'mData': 'CurrentRole', 'defaultContent': '', 'bSortable': false },
                { 'sName': 'Roles', 'mData': 'DisplayRoles', 'defaultContent': '', 'bSortable': false },
                {
                    'mData': 'UserName',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="User/Details?userId=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
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
            }
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

