
/// <reference path="_references.js" />
/// <reference path="~/Scripts/Framework/jquery-1.12.0.intellisense.js" />
define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function () {
            loadFilterLookups();
            loadUpdateFilterLookups();
        },

        dtSettings: {
            'iDisplayLength': 10,
            'aoColumns': [
                { 'sName': 'UnitId', 'mData': 'UnitId' },
                { 'sName': 'UnitName', 'mData': 'UnitName', 'defaultContent': '' },
                { 'sName': 'BusinessUnit.Directorate.Executive.ExecutiveTitle', 'mData': 'ExecutiveTitle', 'defaultContent': '' },

               { 'sName': 'BusinessUnit.Directorate.DirectorateName', 'mData': 'DirectorateName', 'defaultContent': '' },
                { 'sName': 'BusinessUnit.BUnitName', 'mData': 'BUnitName', 'defaultContent': '' },

               { 'sName': 'FunctionalArea.AreanName', 'mData': 'AreanName', 'defaultContent': '' },
                { 'sName': 'TeamType.TeamTypeName', 'mData': 'TeamTypeName', 'defaultContent': '' },
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
                    'mData': 'UnitId',
                    'mRender': function (code, ype, full) {
                        var edit = full.Privilege.CanEdit ? '' : ' hide';
                        var del = full.Privilege.CanDelete ? '' : ' hide';var read = full.Privilege.CanRead ? '' : ' hide';

                        return '<div class="btn-group-vertical">' +
                                    '<a href="Unit/Edit?id=' + code + '" class="btn btn-primary pop-up ' + edit + '"  data-toggle="modal" data-target="#vlePluginModal">Edit</a>' +
                                    '<a href="Unit/Details?id=' + code + '" class="btn btn-primary pop-up ' + read + '" data-toggle="modal" data-target="#vlePluginModal">Detail</a>' +
                                    '<a href="Unit/Delete?id=' + code + '" class="btn btn-primary pop-up ' + del + '" data-toggle="modal" data-target="#vlePluginModal">Delete</a>' +
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
                    name: "divisionCode",
                    value: $('#filterDivisionCode').val()
                },
                {
                    name: "directorateId",
                    value: $('#filterDirectorateId').val()
                }
                ,
                {
                    name: "BusinessUnitId",
                    value: $('#filterBUnitId').val()
                },
                {
                    name: "funcationalAreaId",
                    value: $('#filterFunctionalAreaId').val()
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
                    
                }
                , function (data) {    // error callback function
                }
                , '.pop-up'          // element selector
                , true               // auto hide
            );
        }
    }

    function loadFilterLookups() {
        var selectDivision = $('#filterDivisionCode');
        var selectDir = $('#filterDirectorateId');

        var selectBu = $('#filterBUnitId');
        var selectFa = $('#filterFunctionalAreaId');
        var link = $('#create-team');
        var table = $('.table');
        selectDivision.on('change', function () {
            helper.setSelectEmpty(selectDir);
            interactive.setDropdwnOptions(selectDir, cnt.GetDirectorates, { divisionCode: selectDivision.val(), displayNumbers: true });
            $('.table').dataTable().fnDraw();            
            link.attr('href', appUrl + 'Unit/Create?divisionCode=' + selectDivision.val());
        });

        
        selectDir.on('change', function () {
           table.dataTable().fnDraw();
            helper.setSelectEmpty(selectBu);
            interactive.setDropdwnOptions(selectBu, cnt.GetBusinessUnits, { directorateId: selectDir.val(), displayNumbers: true });
            helper.setSelectEmpty(selectFa);
            interactive.setDropdwnOptions(selectFa, cnt.GetFunctionalAreas, { directorateId: selectDir.val(), displayNumbers: true });            
            link.attr('href', appUrl + 'Unit/Create?divisionCode=' + selectDivision.val() + '&directorateId=' + selectDir.val());
        });


        selectBu.on('change', function () {
           table.dataTable().fnDraw();                        
           link.attr('href', appUrl + 'Unit/Create?divisionCode=' + selectDivision.val() + '&directorateId=' + selectDir.val() + '&bUnitId=' + selectBu.val() + '&functionalAreaId=' + selectFa.val());
        });

        selectFa.on('change', function () {
           table.dataTable().fnDraw();            
           link.attr('href', appUrl + 'Unit/Create?divisionCode=' + selectDivision.val() + '&directorateId=' + selectDir.val() + '&bUnitId=' + selectBu.val() + '&functionalAreaId=' + selectFa.val());
        });

    }

    function loadUpdateFilterLookups() {

        var divisionSel = $('#DivisionCode');

        var divId = $('#divId').val();

        var selectDir = $('#DirectorateId');

        var dirId = $('#dirId').val();

        var selectBu = $('#BUnitId');

        var selectFa = $('#FunctionalAreaId');

        divisionSel.on('change', function () {
            helper.setSelectEmpty(selectDir);
            interactive.setDropdwnOptions(selectDir, cnt.GetDirectorates, { divisionCode: divisionSel.val(), displayNumbers: true });
        });
        
        selectDir.on('change', function () {
            
            helper.setSelectEmpty(selectBu);
            interactive.setDropdwnOptions(selectBu, cnt.GetBusinessUnits, { directorateId: selectDir.val(), displayNumbers: true });

            helper.setSelectEmpty(selectFa);
            interactive.setDropdwnOptions(selectFa, cnt.GetFunctionalAreas, { directorateId: selectDir.val(), displayNumbers: true });
            
        });

        if (helper.hasValue(divId)) {
            divisionSel.val(divId);
        }
        if (helper.hasValue(dirId)) {
            selectDir.val(dirId);
        }
    }

});

