
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
                {'sName':'LocationId','mData':'LocationId' },
                { 'sName': 'Name', 'mData': 'Name', 'defaultContent': '' },
                 { 'sName': 'Directorate.Executive.ExecutiveTitle', 'mData': 'ExecutiveTitle', 'defaultContent': '' },
                { 'sName': 'Directorate.DirectorateName', 'mData': 'DirectorateName', 'defaultContent': '' },

                {
                    'sName':'LastModifiedDate',
                    'mData': 'LastModifiedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'LastModifiedBy','mData':'LastModifiedBy', 'defaultContent': ''  },
                  {
                      'mData': 'LocationId',
                      'mRender': function (code,ype, full) {
                          var edit = full.Privilege.CanEdit ? '' : ' hide';
                          var del = full.Privilege.CanDelete ? '' : ' hide';
                          var read= full.Privilege.CanRead ? '' : ' hide';
                        
                          return '<div class="btn-group-vertical">' + 
                                      '<a href="Location/Edit?id=' + code + '" class="btn btn-primary pop-up '+edit+'"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                      '<a href="Location/Details?id=' + code + '" class="btn btn-primary pop-up '+read+'" data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                      '<a href="Location/Delete?id=' + code + '" class="btn btn-primary pop-up '+del +'" data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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

