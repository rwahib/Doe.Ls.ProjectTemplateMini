
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
                { 'sName':'RoleDescriptionId','mData':'RoleDescriptionId', 'defaultContent': ''},
                {'sName':'Cluster','mData':'Cluster', 'defaultContent': ''  },
                {'sName':'SeniorExecutiveWorkLevelStandards','mData':'SeniorExecutiveWorkLevelStandards', 'defaultContent': ''  },
                {'sName':'ANZSCOCode','mData':'ANZSCOCode', 'defaultContent': ''  },
                {'sName':'PCATCode','mData':'PCATCode', 'defaultContent': ''  },
                {'sName':'AgencyOverview','mData':'AgencyOverview', 'defaultContent': ''  },
                {'sName':'Agency','mData':'Agency', 'defaultContent': ''  },
                {'sName':'AgencyWebsite','mData':'AgencyWebsite', 'defaultContent': ''  },
                {'sName':'RolePrimaryPurpose','mData':'RolePrimaryPurpose', 'defaultContent': ''  },
                {'sName':'KeyAccountabilities','mData':'KeyAccountabilities', 'defaultContent': ''  },
                {'sName':'KeyChallenges','mData':'KeyChallenges', 'defaultContent': ''  },
                {'sName':'DecisionMaking','mData':'DecisionMaking', 'defaultContent': ''  },
                {'sName':'ReportingLine','mData':'ReportingLine', 'defaultContent': ''  },
                {'sName':'DirectReports','mData':'DirectReports', 'defaultContent': ''  },
                {'sName':'BudgetExpenditure','mData':'BudgetExpenditure', 'defaultContent': ''  },
                {'sName':'BudgetExpenditureValue','mData':'BudgetExpenditureValue', 'defaultContent': ''  },
                {'sName':'BudgetExtraNotes','mData':'BudgetExtraNotes', 'defaultContent': ''  },
                {'sName':'EssentialRequirements','mData':'EssentialRequirements', 'defaultContent': ''  },
                {'sName':'RoleCapabilityItems','mData':'RoleCapabilityItems', 'defaultContent': ''  },
                {'sName':'CapabilitySummary','mData':'CapabilitySummary', 'defaultContent': ''  },
                {'sName':'FocusCapabilities','mData':'FocusCapabilities', 'defaultContent': ''  },
                {'sName':'LastModifiedBy','mData':'LastModifiedBy', 'defaultContent': ''  },
                {
                    'sName':'CreatedDate',
                    'mData': 'CreatedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {
                    'sName':'LastModifiedDate',
                    'mData': 'LastModifiedDate',
                    'mRender': function(value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                {'sName':'CreatedBy','mData':'CreatedBy', 'defaultContent': ''  },
                {'sName':'VersionStatus','mData':'VersionStatus', 'defaultContent': ''  },
                {'sName':'OldPDFileName','mData':'OldPDFileName', 'defaultContent': ''  },
                {'sName':'ManagerRole','mData':'ManagerRole', 'defaultContent': ''  },
                {
                    'mData': 'RoleDescriptionId',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="RoleDescription/Edit?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="RoleDescription/Details?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="RoleDescription/Delete?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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

