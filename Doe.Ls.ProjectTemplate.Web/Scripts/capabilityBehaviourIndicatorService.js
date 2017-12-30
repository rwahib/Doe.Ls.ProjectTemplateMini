
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            hookAdvFilter();

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
                { 'sName': 'CapabilityName.Name', 'mData': 'CapabilityName', 'defaultContent': '' },
                { 'sName': 'CapabilityLevel.LevelName', 'mData': 'CapabilityLevelName', 'defaultContent': '' },
                {'sName':'IndicatorContext','mData':'IndicatorContext', 'defaultContent': ''  },
               
                {
                    'mData': 'CapabilityNameId',
                    'mRender':  function (value, type, row){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="CapabilityBehaviourIndicator/Edit?nameId=' + value + '&levelId=' + row.CapabilityLevelId+ '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' +
                                    '<a href="CapabilityBehaviourIndicator/Details?nameId=' + value + '&levelId=' + row.CapabilityLevelId + '"  class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' +
                                   // '<a href="CapabilityBehaviourIndicator/Delete?nameId=' + value + '&levelId=' + row.CapabilityLevelId + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' +
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
                 { name: "CapabilityNameId", value: $('#CapabilityNameSearchParam').val() },
                 { name: "CapabilityLevelId", value: $('#CapabilityLevelSearchParam').val() }
                 );
            }
        }
    }
    function hookAdvFilter() {
        $('#CapabilityNameSearchParam').off('change')
            .on('change',
                function () {
                    $('.table').dataTable().fnDraw();
                });
        $('#CapabilityLevelSearchParam').off('change')
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

