/**
 * Basic Primitives ASP.NET BPOrgDiagram
 *
 * (c) Basic Primitives Inc
 *
 *
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 */

Type.registerNamespace("BasicPrimitives");

BasicPrimitives.UserTemplates = {};

BasicPrimitives.UserTemplates["UserTemplateContact"] = {
    getTemplate: function () {
        var result = new primitives.orgdiagram.TemplateConfig(),
			itemTemplate;
        result.name = "UserTemplateContact";
        result.itemSize = new primitives.common.Size(180, 120);
        result.minimizedItemSize = new primitives.common.Size(4, 4);
        result.highlightPadding = new primitives.common.Thickness(2, 2, 2, 2);


        itemTemplate = jQuery(
		  '<div class="bp-item bp-corner-all bt-item-frame">'
			+ '<div name="titleBackground" class="bp-item bp-corner-all bp-title-frame" style="top: 2px; left: 2px; width: 176px; height: 20px;">'
				+ '<div name="title" class="bp-item bp-title" style="top: 3px; left: 6px; width: 168px; height: 18px;">'
				+ '</div>'
			+ '</div>'
			+ '<div class="bp-item bp-photo-frame" style="top: 26px; left: 2px; width: 50px; height: 60px;">'
				+ '<img name="photo" style="height=60px; width=50px;" />'
			+ '</div>'
			+ '<div name="phone" class="bp-item" style="top: 26px; left: 56px; width: 122px; height: 18px; font-size: 12px;"></div>'
			+ '<div name="email" class="bp-item" style="top: 44px; left: 56px; width: 122px; height: 18px; font-size: 12px;"></div>'
			+ '<div name="description" class="bp-item" style="top: 62px; left: 56px; width: 122px; height: 52px; font-size: 10px;"></div>'
		+ '</div>'
		).css({
		    width: result.itemSize.width + "px",
		    height: result.itemSize.height + "px"
		});
        var bootStrapVerticalButtonsGroup = jQuery("<div></div>")
           .css({
               position: "absolute",
               overflow: "hidden",
               top: result.cursorPadding.top + "px",
               left: (result.itemSize.width + result.cursorPadding.left + 10) + "px",
               width: "35px",
               height: (result.itemSize.height + 1) + "px"
           }).addClass("btn-group btn-group-vertical");

        bootStrapVerticalButtonsGroup.append('<button class="btn btn-small" data-buttonname="info" type="button"><i class="icon-info-sign"></i></button>');
        bootStrapVerticalButtonsGroup.append('<button class="btn btn-small" data-buttonname="edit" type="button"><i class="icon-edit"></i></button>');
        bootStrapVerticalButtonsGroup.append('<button class="btn btn-small" data-buttonname="remove" type="button"><i class="icon-remove"></i></button>');
        bootStrapVerticalButtonsGroup.append('<button class="btn btn-small" data-buttonname="user" type="button"><i class="icon-user"></i></button>');

        itemTemplate.append(bootStrapVerticalButtonsGroup);
        result.itemTemplate = itemTemplate.wrap('<div>').parent().html();

        return result;
    },
    onRender: function (event, data, config) {
        var itemConfig = data.context;

        switch (data.renderingMode) {
            case primitives.common.RenderingMode.Create:
                /* Initialize widgets here */
                break;
            case primitives.common.RenderingMode.Update:
                /* Update widgets here */
                break;
        }

        data.element.find("[name=photo]").attr({ "src": itemConfig.image });
        data.element.find("[name=titleBackground]").css({ "background": itemConfig.itemTitleColor });
        data.element.find("[name=title]").css({ "color": primitives.common.highestContrast(itemConfig.itemTitleColor, config.itemTitleSecondFontColor, config.itemTitleFirstFontColor) });
        data.element.find("[name=phone]").text(itemConfig.item["Phone"]);
        data.element.find("[name=email]").text(itemConfig.item["Email"]);
        data.element.find("[name=description]").text(itemConfig.item["Description"]);
        data.element.find("[name=title]").text(itemConfig.item["Title"]);
    }
};
