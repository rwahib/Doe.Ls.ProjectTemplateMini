
/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            hookAdvFilter();
            $("#GradeCodeSearchParam").addClass("select2picker");
            $("#FocusIdSearchParam").addClass("select2picker");
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
                { 'sName': 'Grade.GradeCode', 'mData': 'GradeName', 'defaultContent': '' },
                { 'sName': 'Focus.FocusName', 'mData': 'FocusName', 'defaultContent': '' },
                { 'sName': 'SelectionCriteria.Criteria', 'mData': 'Criteria', 'defaultContent': '' },
                { 'sName': 'IsMandatory', 'mData': 'IsMandatory', 'defaultContent': '' },
                { 'mData': 'LookupId',
                    'mRender': function(code){
                        return '<div class="btn-group-vertical">' + 
                                    '<a href="LookupFocusGradeCriteria/Edit?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' + 
                                    '<a href="LookupFocusGradeCriteria/Details?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Detail</a>' + 
                                    '<a href="LookupFocusGradeCriteria/Delete?id=' + code + '" class="btn btn-primary pop-up"  data-toggle="modal" data-target="#vlePluginModal">Delete</a>' + 
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
            'fnServerParams': function (aoData) {
                aoData.push(
                 { name: "GradeCode", value: $('#GradeCodeSearchParam').val() }
                 , { name: "FocusId", value: $('#FocusIdSearchParam').val() }
                 );
            }
        }
    }

    function hookAdvFilter() {
        $('#GradeCodeSearchParam').off('change')
            .on('change',
                function () {
                    $('.table').dataTable().fnDraw();
                });

        $('#FocusIdSearchParam').off('change')
           .on('change',
               function () {
                   $('.table').dataTable().fnDraw();
               });
    }

    function applyFocusAndCriteriaFilter() {
        var selectFocus = $('#FocusId');

        $('#GradeCode').off('change').on('change', function () {
            selectFocus.empty();
            selectFocus.append($('<option/>', {
                  value: 0,
                  text: 'Please select Focus...'
              }));

            selectFocus.select2("val", 0);
              $.ajax({
                  type: 'get',
                  dataType: 'json',
                  cache: false,
                  url: cnt.GetFocusList,
                  data: { gradeCode: helper.hasValue($('#GradeCode').val()) && $('#GradeCode').val().length > 0 ? $('#GradeCode').val() : 0 },
                  success: function (items) {

                      $.each(items, function (id) {
                          selectFocus.append($('<option/>', {
                              value: items[id].Value,
                              text: items[id].Text
                          }));

                      });


                  },
                  error: function (errorThrown) {
                      alert('Error - area- ' + errorThrown);
                  }
              });


        });

        var selectcriteria = $('#SelectionCriteriaId');
        $('#FocusId').off('change').on('change', function () {
            selectcriteria.empty();
            selectcriteria.append($('<option/>', {
                value: 0,
                text: 'Please select Criteria...'
            }));

            selectcriteria.select2("val", 0);
            $.ajax({
                type: 'get',
                dataType: 'json',
                cache: false,
                url: cnt.GetCriteriaList,
                data: {gradeCode: helper.hasValue($('#GradeCode').val()) && $('#GradeCode').val().length > 0 ? $('#GradeCode').val() : 0 ,
                     focusId: helper.hasValue($('#FocusId').val()) && $('#FocusId').val().length > 0 ? $('#FocusId').val() : 0
                },
                success: function (items) {
                    $.each(items, function (id) {
                        selectcriteria.append($('<option/>', {
                            value: items[id].Value,
                            text: items[id].Text
                        }));

                    });


                },
                error: function (errorThrown) {
                    alert('Error - area- ' + errorThrown);
                }
            });


        });
    }

    function hookPopup(){
        if ($('.pop-up').length > 0) {
            $("#GradeCodeSearchParam").removeClass("select2picker");
            $("#FocusIdSearchParam").removeClass("select2picker");
            interactive.hookPopupButton(
                {}                  // custom validation settings
                , function () {        // modal load callback function
                    applyFocusAndCriteriaFilter();
                    interactive.hookSelect2();
                }
                //,true               // use Ajax Submit Handler
                //,function(data){    // success callback function
                //}
                //,function(data){    // error callback function
                //}
                //,'.pop-up'          // element selector
                //,true               // auto hide
            );
        }
    }

});

