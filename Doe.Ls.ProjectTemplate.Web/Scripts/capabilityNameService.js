
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            hookAdvFilter();
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
                {'sName':'CapabilityNameId','mData':'CapabilityNameId' },
                {'sName':'Name','mData':'Name', 'defaultContent': ''  },
                {'sName':'CapabilityDescription','mData':'CapabilityDescription', 'defaultContent': ''  },
                { 'sName': 'CapabilityGroup.GroupName', 'mData': 'CapabilityGroupName', 'defaultContent': '' },
                {
                    'mData': 'CapabilityNameId',
                    'mRender': function (code, i, row) {
                        if (!row.CanDelete) {
                            return '<div class="btn-group-vertical">' +
                                   '<a href="CapabilityName/Edit?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' +
                                   '<a href="CapabilityName/Details?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' +
                                   '</div>';
                        }
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="CapabilityName/Edit?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="CapabilityName/Details?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="CapabilityName/Delete?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' +
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
            'fnServerParams': function (aoData) {
                aoData.push(
                 { name: "CapabilityGroupId", value: $('#CapabilityGroupSearchParam').val() }
                
                 );
            }
        }
    }
    function hookAdvFilter() {
        $('#CapabilityGroupSearchParam').off('change')
            .on('change',
                function () {
                    $('.table').dataTable().fnDraw();
                });
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

