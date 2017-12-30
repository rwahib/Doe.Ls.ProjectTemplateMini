
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            loadFilter();
            
        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
                {'sName':'LogId','mData':'LogId' },
                {'sName':'Action','mData':'Action', 'defaultContent': ''  },
                {'sName':'Context','mData':'Context', 'defaultContent': ''  },
                {
                    'sName':'CreationDate',
                    'mData': 'CreationDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'Username','mData':'Username', 'defaultContent': ''  },
                { 'sName': 'RoleName', 'mData': 'RoleName', 'defaultContent': '' },
                {'sName':'Note','mData':'Note', 'defaultContent': ''  },
                {
                    'mData': 'LogId',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="GeneralLog/Details?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                '</div>';
                    },
                    'bSortable': false
                }
            ],
            'aaSorting': [[0,'desc']],
            'bFilter': true,
            // Use this to define the onDrawCallBack for Data Table
            'fnDrawCallback': function () {
                hookPopup();
            },
            // Use this to set server parameters
            'fnServerParams': function (aoData) {
                aoData.push({
                    name: "fromDate", value: $('#fromDate').val()
                },
                {
                    name: "toDate", value: $('#toDate').val()
                },
                {
                    name: "logAction", value: $('#logAction').val()
            }
                );
                    
            }
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
    function loadFilter() {
        $('#fromDate, #toDate, #logAction').on('change', function () {
            $('.table').dataTable().fnDraw();
        });

        $('#reset').on('click', function () {
            $('#fromDate').val('');
            $('#toDate').val('');            
            $("#logAction").prop('selectedIndex', 0);
            $('#logAction').select2().trigger('change');
        });
    }

});

