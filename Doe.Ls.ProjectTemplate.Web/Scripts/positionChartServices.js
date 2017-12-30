var orgDiagram = null;
var treeItems = {};
var m_timer = null;

define(['cnt', 'jquery', 'helper', 'interactive', 'primitives', 'api', 'pdfkit', 'fs', 'blobstream', 'jQueryLayout', 'bpEditor', 'jPrint', 'fValidator', 'bsSelect2'], function (cnt, $, helper, interactive, pr, api) {
    return {
        initialise: function () {
            displaySearchBox(false);
            loadLookups();

            initialiseChart();

            resetFilter();

            $("[id*=DivisionCode],[id*=DirectorateId],[id*=BusinessUnitId],[id*=UnitId]").change(function () {
                initAndUpdateChart();
            });

            $('#download-pdf').on("click", function () {
                pdfView();
            });

        }
    }

    function initAndUpdateChart() {

        initialiseChart();

        var qString = "DivisionCode=" + $('#ChartDivisionCode').val() + "&DirectorateId=" + $('#ChartDirectorateId').val() +
                   "&BusinessUnitId=" + $('#ChartBusinessUnitId').val() +
                   "&UnitId=" + $('#ChartUnitId').val();

        window.history.pushState(" ", "Organisational chart", window.appUrl + "Position/InitiateChartLoading?" + qString);

    }

    function initialiseChart() {
        $('#main-container').removeClass("container-fluid");

        if ($('#LoadPreview').val() === 'true') {

            var options = new primitives.orgdiagram.Config();
            options.hasSelectorCheckbox = primitives.common.Enabled.False;
            options.hasButtons = primitives.common.Enabled.False;
            options.defaultTemplateName = "UserTemplateContact";
            options.childrenPlacementType = primitives.common.ChildrenPlacementType.Horizontal;
            options.leavesPlacementType = primitives.common.ChildrenPlacementType.Matrix;
            options.pageFitMode = primitives.common.PageFitMode.PrintPreview;
            //options.printPreviewPageSize = new primitives.common.Size(700, 600);
            // options.templates = [getPrintTemplate()];
            //options.onItemRender = onTemplateRender;

            var jsonArr = [];
            var chartHeader1 = "Preview";
            for (var i = 0; i < posListJsonObj.length; i++) {
                jsonArr[i] = new primitives.orgdiagram.ItemConfig(
                {
                    id: posListJsonObj[i].id,
                    parent: posListJsonObj[i].parent,
                    title: posListJsonObj[i].title,
                    description: posListJsonObj[i].description,
                    // groupTitle: posListJsonObj[i].groupTitle,
                    itemTitleColor: posListJsonObj[i].itemTitleColor,
                    // groupTitleColor: posListJsonObj[i].itemTitleColor,
                    itemType: posListJsonObj[i].itemType,
                    //templateName: "printTemplate",
                    labelFontFamily: "Arial",
                    labelFontSize: "12px"
                });
                if (posListJsonObj[i].parent == null) {
                    chartHeader1 = posListJsonObj[i].positionTitle;
                }
            }
            $('#previewHeader').text(chartHeader1);
            $('#download-pdf').show();
            options.items = jsonArr;
            jQuery("#basicdiagram").orgDiagram(options);

        } else {

            $('#main-container').removeClass("container-fluid");
            var orgEditorConfig = new primitives.orgeditor.Config();
            orgEditorConfig.connectorType = primitives.common.ConnectorType.Squared;
            orgEditorConfig.itemSize = new primitives.common.Size(100, 90);
            orgEditorConfig.showLabels = primitives.common.Enabled.False;
            orgEditorConfig.pageFitMode = 6;
            if ($('#NoOfLevels').val() == "2") {
                orgEditorConfig.leavesPlacementType = primitives.common.ChildrenPlacementType.Horizontal;
            } else {
                orgEditorConfig.leavesPlacementType = primitives.common.ChildrenPlacementType.Auto;
            }
            if ($("#canEdit").val() == "False") {
                orgEditorConfig.readOnlyMode = true;
            }
            var selectorEditor = jQuery("#basicdiagram");
            var divProcessing = $('#Processing');
            divProcessing.show();
            divProcessing.html('<img src="' + window.appUrl + 'Images/prog.gif"/> ');
            //divProcessing.html('');
            // divProcessing.css('display', 'none');
            var positionFilterParams = {
                DivisionCode: $('#ChartDivisionCode').val(),
                DirectorateId: $('#ChartDirectorateId').val(),
                BusinessUnitId: $('#ChartBusinessUnitId').val(),
                UnitId: $('#ChartUnitId').val(),
                NoOfLevels: $('#NoOfLevels').val()
            }
            // positionFilterParams = JSON.stringify({ 'positionFilterParams': positionFilterParams });

            api.ajaxGet(cnt.GetPositions,
                positionFilterParams
            ,
                  function (items) {
                      orgEditorConfig.items = items;

                      jQuery("#basicdiagram").bpOrgEditor(orgEditorConfig);
                      selectorEditor.bpOrgEditor("option", {
                          items: items,
                          cursorItem: 0
                      });

                      selectorEditor.bpOrgEditor("update");
                      $('div[name=options-button]').addClass("none");
                      $('div[name=json-button]').addClass("none");

                      divProcessing.hide();
                      $("[name*=options-button]").remove();
                      $("[name*=json-button]").remove();
                      $('#NoOfLevels').val(0);

                      $("label[for='search100']").html("Search by Position# in current view ");
                      $("label[for='search100']").addClass("chart-search-lbl");
                      $('#search100').addClass("chart-search-input");
                      $('#search100').attr('tabindex', 1);
                      $('.orgdiagrambutton').addClass("btnContainerSize");

                      // $('li.orgdiagrambutton').attr('tabindex', 0);
                      $('span.ui-button-icon-primary').attr('tabindex', 0);
                      if (helper.hasValue(items)) {
                          displaySearchBox(items.length > 0);
                      } else {
                          displaySearchBox(false);
                      }

                  });
            $(window).resize(function () {
                onWindowResize(orgEditorConfig);
            });

            /*  orgEditorConfig.onSave = function (element) {
                 
                 
                      var config1 = jQuery("#basicdiagram").bpOrgEditor("option");
                      // Can change report to positionId from here using ajax call, Diagram will be loaded automatically
                      // What will happen to Unit chief ReportTo and unit chief Position Number
                      //Read config option and store chart changes #3#
                  };*/

        }
    }

    function pdfView() {
        var PdfDocument = require('pdfkit');
        var blobStream = require('blobstream');
        var file = "chart";
        var chartHeader = "Organisation chart ";
        var jsonArr = [];
        for (var i = 0; i < posListJsonObj.length; i++) {
            jsonArr[i] = new primitives.orgdiagram.ItemConfig(
            {
                id: posListJsonObj[i].id,
                parent: posListJsonObj[i].parent,
                title: "          " + posListJsonObj[i].title,
                description: posListJsonObj[i].description,
                //groupTitle: posListJsonObj[i].groupTitle,
                itemTitleColor: posListJsonObj[i].itemTitleColor,
                //groupTitleColor: posListJsonObj[i].itemTitleColor,
                //groupTitleColor: "#4169E1",
                templateName: "printTemplate",
                itemType: posListJsonObj[i].itemType,
                //templateName: "positionTemplate",
                labelFontFamily: "Arial",
                labelFontSize: "12px"
            });

            if (posListJsonObj[i].parent == null) {
                file = posListJsonObj[i].unitName;
                chartHeader = posListJsonObj[i].positionTitle;
            }
        }

        var chartdiagram = primitives.pdf.orgdiagram.Plugin({
            items: jsonArr,
            cursorItem: null,
            hasSelectorCheckbox: primitives.common.Enabled.False,
            childrenPlacementType: primitives.common.ChildrenPlacementType.Horizontal,
            leavesPlacementType: primitives.common.ChildrenPlacementType.Matrix,
            //templates: [getPrintTemplate()],
            // onItemRender: onTemplateRender
            //templates: [getPositionTemplate()],
            // onItemRender: onTemplateRender
        });

        chartdiagram.labelFontFamily = "Arial";
        chartdiagram.labelFontSize = "12px";
        var chartdiagramsize = chartdiagram.getSize();

        var doc = new PdfDocument({ size: [chartdiagramsize.width + 50, chartdiagramsize.height + 90] });
        var stream = doc.pipe(blobStream());

        doc.save();

        // draw some text
        doc.fontSize(15)
            .text(chartHeader, 50, 50);
        doc.fontSize(12)
          .text("Chart generated at " + new Date().toLocaleDateString() + " " + new Date().toLocaleTimeString(), 50, 100);

        chartdiagram.draw(doc, 50, 100);


        doc.restore();

        doc.end();
        stream.on('finish', function () {

            var string = stream.toBlob('application/pdf');
            window.saveAs(string, file + '.pdf');
        });
        /*  stream.on('finish', function () {
              var string = stream.toBlobURL('application/pdf');
              $('#wrapper-PositionChart').hide();
              jQuery('#previewpanel').attr('src', string);
          });*/
    }

    function onWindowResize(orgEditorConfig) {
        if (m_timer == null) {
            m_timer = window.setTimeout(function () {
                resizePlaceholder();
                jQuery("#basicdiagram").bpOrgEditor("update", primitives.orgdiagram.UpdateMode.Refresh);
                window.clearTimeout(m_timer);
                m_timer = null;
            }, 300);
        }
    }

    function resizePlaceholder() {
        var bodyWidth = $(window).width() - 40;
        var bodyHeight = $(window).height() - 20;
        var leftalgn = 250 - 40;
        jQuery("#basicdiagram").css(
        {
            "width": bodyWidth + "px",
            "height": bodyHeight + "px"
        });
        /* jQuery(".chartrow ").css(
        {
            "margin-left":"auto"
        });*/

    }

    function resetFilter() {

        var selectBu = $('#ChartBusinessUnitId');
        var selectUnit = $('#ChartUnitId');
        var selectDirectorate = $('#ChartDirectorateId');
        var selectDivisionCode = $('#ChartDivisionCode');

        $('#btnFilterReset')
            .on('click',
                function () {

                    if (selectDivisionCode.val() === '') {
                        $('#msg').text("Please select a division first");
                    }

                    helper.setSelectEmpty(selectUnit);
                    helper.setSelectEmpty(selectBu);
                    helper.setSelectEmpty(selectDirectorate);

                    initAndUpdateChart();
                });

    }

    function loadLookups() {

        //Functional area ID
        var selectDir = $('#ChartDirectorateId');
        var selectBu = $('#ChartBusinessUnitId');
        var selectUnit = $('#ChartUnitId');

        $('#ChartDirectorateId option').lenth == 1 ? disbale(selectDir, true) : disbale(selectDir, false);
        $('#ChartBusinessUnitId option').lenth == 1 ? disbale(selectBu, true) : disbale(selectBu, false);
        $('#ChartUnitId option').lenth == 1 ? disbale(selectUnit, true) : disbale(selectUnit, false);


$('#ChartUnitId').on('change', function () {
         selectUnit.parent().removeClass("hi_select");
     });
        $('#ChartBusinessUnitId').on('change', function () {
		 selectUnit.parent().removeClass("hi_select");
         $(".hi_select").removeClass("hi_select");
            helper.setSelectEmpty(selectUnit, "Please select unit");
            disbale(selectUnit, true);

            if ($('#ChartBusinessUnitId').val().length > 1) {
                interactive.setDropdwnOptions(selectUnit, cnt.GetUnits, { bUnitId: $('#ChartBusinessUnitId').val(), fromChart: true }, function () {
                    highLightBgColor(selectUnit);
                    disbale(selectUnit, false);
                });
            }
        });

        //Directorate ID
        $('#ChartDirectorateId').on('change', function () {
		 selectBu.parent().removeClass("hi_select");
         $(".hi_select").removeClass("hi_select");
            helper.setSelectEmpty(selectBu, "Please select business unit");
            helper.setSelectEmpty(selectUnit, "Please select unit");
            disbale(selectBu, true);
            disbale(selectUnit, true);
            if ($('#ChartDirectorateId').val().length > 1) {
                interactive.setDropdwnOptions(selectBu, cnt.GetBusinessUnits, { directorateId: $('#ChartDirectorateId').val(), fromChart: true }, function () {
                    highLightBgColor(selectBu);
                    disbale(selectBu, false);
                    disbale(selectUnit, true);
                });
            }

        });

        $('#ChartDivisionCode').on('change', function () {
		  selectDir.parent().removeClass("hi_select");
	    $(".hi_select").removeClass("hi_select");
            if ($('#ChartDivisionCode').val() !== '') {
                $('#msg').text("");
            }
            disbale(selectDir, true);
            disbale(selectBu, true);
            disbale(selectUnit, true);

            helper.setSelectEmpty(selectDir, "Please select directorate");
            helper.setSelectEmpty(selectBu, "Please select business unit");
            helper.setSelectEmpty(selectUnit, "Please select unit");
            if ($('#ChartDivisionCode').val().length > 1) {
                interactive.setDropdwnOptions(selectDir, cnt.GetDirectorates, { divisionCode: $('#ChartDivisionCode').val(), fromChart: true }, function () {
                    disbale(selectDir, false);
                    highLightBgColor(selectDir);
                    disbale(selectBu, true);
                    disbale(selectUnit, true);
                });
            }

        });
    }

    function highLightBgColor(elementObj) {
	 
        elementObj.parent().addClass("hi_select");
        //elementObj.parent().css('backgroundColor', 'hsl(180, 100%, 50%)');

        //    var d = 1000;
        //    for (var i = 50; i <= 94; i = i + 2) {
        //        d += 30;
        //        (function (ii, dd) {
        //            setTimeout(function () {
        //                //174, 72%, 56%
        //                var xx = 100 - ii-6; var x1 = 180 - xx;
        //                console.log("ii : "+ii+" xx : "+xx)
        //                elementObj.parent().css('backgroundColor', 'hsl('+174 +',' + xx + '%,' + ii + '%)');
        //                //if (i >= 100) {
        //                //    elementObj.parent().css('backgroundColor', 'hsl(0,0%,94%)');

        //                //}
        //            }, dd);
        //        })(i, d);

        //    }

    }

        function disbale(elem, disable) {
        elem.prop('disabled', disable);
    }

    function getPrintTemplate() {
        var result = new primitives.orgdiagram.TemplateConfig();
        result.name = "printTemplate";

        var buttons = [];
        /* buttons.push(new primitives.orgdiagram.ButtonConfig("delete", "ui-icon-close", "Delete"));
         buttons.push(new primitives.orgdiagram.ButtonConfig("properties", "ui-icon-gear", "Info"));
         buttons.push(new primitives.orgdiagram.ButtonConfig("add", "ui-icon-person", "Add"));
         buttons.push(new primitives.orgdiagram.ButtonConfig("notice", "ui-icon-notice", "Notice"));*/
        result.buttons = buttons;

        result.itemSize = new primitives.common.Size(102, 90);
        result.minimizedItemSize = new primitives.common.Size(2, 2);
        result.highlightPadding = new primitives.common.Thickness(2, 2, 2, 2);

        var itemTemplate = jQuery(
          '<div class="bp-item bp-corner-all bt-item-frame">'
            + '<div name="titleBackground" class="bp-item bp-corner-all bp-title-frame" style="top: 2px; left: 2px; width: 100px; height: 20px;">'
                + '<div name="title" class="bp-item bp-title" style="top: 3px; left: 2px; width: 98px; height: 18px;font-size:bold">'
                + '</div>'
            + '</div>'


            + '<div name="description" class="bp-item" style="top: 20px; left: 13px; width: 98px; height: 76px; font-size: 10px;"></div>'
        + '</div>'
        ).css({
            width: result.itemSize.width + "px",
            height: result.itemSize.height + "px"
        }).addClass("bp-item bp-corner-all bt-item-frame");
        result.itemTemplate = itemTemplate.wrap('<div>').parent().html();

        return result;
    }

    function onTemplateRender(event, data) {
        switch (data.renderingMode) {
            case primitives.common.RenderingMode.Create:
                /* Initialize widgets here */
                break;
            case primitives.common.RenderingMode.Update:
                /* Update widgets here */
                break;
        }

        var itemConfig = data.context;

        if (data.templateName == "printTemplate") {

            data.element.find("[name=titleBackground]").css({ "background": itemConfig.itemTitleColor });

            var fields = ["title", "description"];

            for (var index = 0; index < fields.length; index++) {
                var field = fields[index];

                var element = data.element.find("[name=" + field + "]");
                if (element.text() != itemConfig[field]) {
                    element.text(itemConfig[field]);
                }
            }
            // $('.bp-grouptitle-frame').hide();
        }
    }

    function onHighlightChanging(event, data) {
        //var target = jQuery(event.originalEvent.target);
        console.log(data.context.title);

    }

    function onMouseClick(event, data) {

        var target = jQuery(event.originalEvent.target);
        if (target.hasClass("btn") || target.parent(".btn").length > 0) {
            var button = target.hasClass("btn") ? target : target.parent(".btn");


            var buttonUrl = button.data("buttonurl");
            if (buttonUrl != null) {
                var win = window.open(window.appUrl + buttonUrl + "?positionId=" + data.context.id, "_blank");
                win.focus();
            }

            data.cancel = true;
        }
    }

    function displaySearchBox(show) {

        if (!show) {
            $('.chart-search-lbl').hide();
            $('#search100').hide();
        } else {
            $('.chart-search-lbl').show();
            $('#search100').show();
        }
    }
}
);