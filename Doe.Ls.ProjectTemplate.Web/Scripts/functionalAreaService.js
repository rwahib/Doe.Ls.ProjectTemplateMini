/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            loadFilterLookups();
            loadUpdateFilterLookups();
        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
                { 'sName': 'FuncationalAreaId', 'mData': 'FuncationalAreaId' },
                { 'sName': 'AreanName', 'mData': 'AreanName', 'defaultContent': '' },
                { 'sName': 'Directorate.Executive.ExecutiveTitle', 'mData': 'ExecutiveTitle', 'defaultContent': '' },
                { 'sName': 'Directorate.DirectorateName', 'mData': 'DirectorateName', 'defaultContent': '' },

                { 'sName': 'StatusValue.StatusName', 'mData': 'StatusName', 'defaultContent': '' },
                {
                    'sName': 'LastModifiedDate',
                    'mData': 'LastModifiedDate',
                    'mRender': function (value) {
                        return helper.getDateTimeFromJsonDate(value);
                    }
                },
                { 'sName': 'LastModifiedBy', 'mData': 'LastModifiedBy', 'defaultContent': '' },
                {
                    'mData': 'FuncationalAreaId',
                    'mRender': function (code, ype, full) {
                        var edit = full.Privilege.CanEdit ? '' : ' hide';
                        var del = full.Privilege.CanDelete ? '' : ' hide';
                        var read = full.Privilege.CanRead ? '' : ' hide';

                        return '<div class="btn-group-vertical">' +
                                    '<a href="FunctionalArea/Edit?id=' + code + '" class="btn btn-primary pop-up ' + edit + '"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' +
                                    '<a href="FunctionalArea/Details?id=' + code + '" class="btn btn-primary pop-up ' + read + '" data-toggle="modal" data-target="#vlePluginModal">Detail</a>' +
                                    '<a href="FunctionalArea/Delete?id=' + code + '" class="btn btn-primary pop-up ' + del + '" data-toggle="modal" data-target="#vlePluginModal">Delete</a>' +
                                '</div>';
                    },
                    'bSortable': false
                }

            ],
            'aaSorting': [[0, 'asc']],
            'bFilter': true,
            // Use this to define the onDrawCallBack for Data Table
            'fnDrawCallback': function () {
                hookPopup();
            },
            // Use this to set server parameters
            'fnServerParams': function (aoData) {
                aoData.push({
                    name: "divisionCode", value: $('#filterDivisionCode').val()
                }, {
                    name: "directorateId", value: $('#filterDirectorateId').val()
                }

                );
            }
        }
    }

    function hookPopup() {
        if ($('.pop-up').length > 0) {
            interactive.hookPopupButton(
                {}                  // custom validation settings
                , function () {        // modal load callback function
                    interactive.basicInitialisation('#vlePluginModal');
                    loadUpdateFilterLookups();
                }
                , true               // use Ajax Submit Handler
                , function (data) {    // success callback function
                    $('.error-popup').hide();
                    $('#formTab').hide();
                    $('#lbl-success')
                        .text('Data has been successfully updated');
                    $('.success-popup').show(500);

                    $('.table').dataTable().fnDraw();

                    $('.table').dataTable().fnDraw();
                }
                , function (data) {    // error callback function
                }
                , '.pop-up'          // element selector
                , true               // auto hide
            );
        }
    }

    function loadFilterLookups() {
        var selectDir = $('#filterDirectorateId');

        $('#filterDivisionCode').on('change', function () {
            helper.setSelectEmpty(selectDir);
            interactive.setDropdwnOptions(selectDir, cnt.GetDirectorates, { divisionCode: $('#filterDivisionCode').val(), displayNumbers: true });
            $('.table').dataTable().fnDraw();
            var link = $('#create-function-area');
            link.attr('href', appUrl + 'FunctionalArea/Create?divisionCode=' + $('#filterDivisionCode').val());
        });


        $('#filterExecutiveCode').on('change', function () {
            $('.table').dataTable().fnDraw();
        });

        $('#filterDirectorateId').on('change', function () {
            $('.table').dataTable().fnDraw();

            var link = $('#create-function-area');
            link.attr('href', appUrl + 'FunctionalArea/Create?divisionCode=' + $('#filterDivisionCode').val() + '&directorateId=' + $('#filterDirectorateId').val());
        });
    }

    function loadUpdateFilterLookups() {
        var divCode = $('#ExecutiveCod').val();
        var selectDir = $('#DirectorateId');
        var divisionSel = $('#DivisionCode');


        divisionSel.on('change', function () {
            helper.setSelectEmpty(selectDir);
            interactive.setDropdwnOptions(selectDir, cnt.GetDirectorates, { divisionCode: divisionSel.val(), displayNumbers: true });
        });

        if (helper.hasValue(divCode)) {
            divisionSel.val(divCode);
        }


    }

});

