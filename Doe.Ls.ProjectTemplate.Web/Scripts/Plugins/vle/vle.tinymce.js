define(["cnt", "jquery", "helper", "api", "tinymce", "interactive"],
    function(cnt, $, helper, api, tinyDummy, interactive) {

        return {
            initialise: function() {
                var $this = this;

                $(document)
                    .on(cnt.Events.vleTinyMce.onSettignsChanged,
                        function(type, arg) {
                            if (arg.mode === cnt.Events.vleTinyMce.editMode) {
                                $this.hookTinymce(true);
                            } else {
                                $this.hookTinymce(false);
                            }
                        })
                    .on(cnt.Events.vleTinyMce.onEditorChanged,
                        function(type, argument) {

                            var commandToolBox = $(".section-content-manage", argument.element.parent());
                            var saveButton = $('button[name="save"]', commandToolBox);
                            saveButton.prop("disabled", false);
                            var element = $(".section-content-body", saveButton.closest(".section-content"));
                            element.data["dirty"] = true;
                        });

                $('button[name="save"]', $(".section-content-manage"))
                    .on("click",
                        function() {
                            var saveButton = $(this);

                            var element = $(".section-content-body", saveButton.closest(".section-content"));

                            var sectionId = element.prop("id");
                            //$(document).data("tiny-mce-dataChanged", false);
                            saveContent(sectionId,
                                function(result) {
                                    if (result.Status === 0) {
                                        saveButton.prop("disabled", true);
                                        element.data["dirty"] = false;
                                        
                                    } else {
                                        interactive.showDialog("alert",
                                            "error",
                                            '<span class="text-danger">' + result.Message + "</span>");
                                    }
                                },
                                function(result) {
                                    interactive.showDialog("alert",
                                        "error",
                                        '<span class="text-danger">' + result.Message + "</span>");

                                });

                        });

                $('button[name="refresh"]', $(".section-content-manage"))
                    .on("click",
                        function() {
                            var refreshButton = $(this);
                            var saveButton = $('button[name="save"]', $(".section-content-manage"));

                            var element = $(".section-content-body", refreshButton.closest(".section-content"));


                            if (element.data["dirty"]) {
                                interactive.showDialog("confirm",
                                    "Warning!",
                                    "Are you sure you want to discard any change and get latest saved version?",
                                    null,
                                    function(data) {
                                        if (data) {
                                            //$(document).data("tiny-mce-dataChanged", false);
                                            getContent(getSectionBodyKey(element),
                                                function(result) {
                                                    if (result.Status === 0) {
                                                        element.html(result.Data);
                                                        saveButton.prop("disabled", true);
                                                        element.data["dirty"] = false;
                                                        
                                                    } else {
                                                        interactive.showDialog("alert",
                                                            "error",
                                                            '<span class="text-danger">' + result.Message + "</span>");
                                                    }
                                                },
                                                function(result) {
                                                    interactive.showDialog("alert",
                                                        "error",
                                                        '<span class="text-danger">' + result + "</span>");
                                                }
                                            );
                                        }


                                    });

                            } else {
                                //$(document).data("tiny-mce-dataChanged", false);
                                getContent(getSectionBodyKey(element),
                                    function(result) {
                                        if (result.Status === 0) {
                                            element.html(result.Data);
                                            saveButton.prop("disabled", true);
                                            element.data["dirty"] = false;
                                          
                                        } else {
                                            interactive.showDialog("alert",
                                                "error",
                                                '<span class="text-danger">' + result.Message + "</span>");
                                        }
                                    },
                                    function(result) {
                                        interactive.showDialog("alert",
                                            "error",
                                            '<span class="text-danger">' + result + "</span>");
                                    }
                                );
                            }
                        });

                $('button[name="refresh-page"]', $(".section-content-manage"))
                    .on("click",
                        function() {
                            var refreshButton = $(this);

                            var element = $(".section-content-body", refreshButton.closest(".section-content"));

                            if (element.data["dirty"]) {
                                interactive.showDialog("confirm",
                                    "Warning!",
                                    "Are you sure you want to discard any change and refresh the whole page?",
                                    null,
                                    function(data) {
                                        if (data) {
                                            //$(document).data("tiny-mce-dataChanged", false);
                                            interactive.reloadPage();
                                        }

                                    });

                            } else {
                                //$(document).data("tiny-mce-dataChanged", false);
                                interactive.reloadPage();
                            }
                        });

                $('button[name="save-draft"]', $(".section-content-manage"))
                    .on("click",
                        function() {
                            var saveDraftButton = $(this);
                            var element = $(".section-content-body", saveDraftButton.closest(".section-content"));
                            var sectionKey = getSectionBodyKey(element);

                            interactive.showDialog("prompt",
                                "Provide new title",
                                null,
                                null,
                                function(data) {

                                    if (!helper.hasValue(data) || $.trim(data) === "") {
                                        interactive.showDialog("alert",
                                            "error",
                                            '<span class="text-danger">' + "Title could not be empty" + "</span>");
                                        return;
                                    }


                                    if (helper.hasValue(data)) {
                                        var title = data;


                                        saveDraftContent(sectionKey,
                                            title,
                                            function(result) {

                                                if (result.Status === 0) {
                                                    var newSectionKey = result.Data;
                                                    interactive.showDialog("alert",
                                                        "success",
                                                        '<div class="text-success">Draft record successfully saved' +
                                                        '<p><a href="' +
                                                        window.appUrl +
                                                        "AppPage/PageSample?sectionKey=" +
                                                        newSectionKey +
                                                        '" >Click here to access the new draft version</a> </p>' +
                                                        "</div>");

                                                } else {
                                                    interactive.showDialog("alert",
                                                        "error",
                                                        '<span class="text-danger">' + result.Message + "</span>");
                                                }
                                            },
                                            function(result) {
                                                interactive.showDialog("alert",
                                                    "error",
                                                    '<span class="text-danger">' + result.Message + "</span>");

                                            });
                                    }

                                });


                        }
                    );
            },

            hookTinymce: function(editMode) {
                if ($(".tinymce, .tinymce-rich, .tinymce-rich-small, .tinymce-simple,.tinymceListOnly").length > 0) {

                    var clearEditors = function() {
                        if (typeof (tinymce) != "undefined") {
                            if (tinymce.editors !== null || tinymce.activeEditor == null) {
                                tinymce.editors = []; // remove any existing references
                            }
                        }
                    };
                    var tinyDefaultSettings = getTinyDefaultSettings();

                    var tinyRichSettings = getTinyRichSettings(tinyDefaultSettings);
                    var tinyRichSmallSettings = getTinyRichSmallSettings(tinyDefaultSettings);

                    var tinyListOnly = getTinyListOnlySettings(tinyDefaultSettings);

                    if ($(".tinymce").length > 0) {

                        clearEditors();
                        tinymce.init(tinyDefaultSettings);
                    }
                    if ($(".tinymceListOnly").length > 0) {

                        clearEditors();
                        tinymce.init(tinyListOnly);
                    } if ($(".tinymce-rich-small").length > 0) {

                        clearEditors();
                        tinymce.init(tinyRichSmallSettings);
                    }

                    if ($(".tinymce-rich").length > 0) {
                        clearEditors();
                        $(".tinymce-rich")
                            .each(function() {
                                var $el = $(this);
                                var attribInLine = $el.attr("data-inline");
                                var attributeBasic = $el.attr("data-basic");
                                
                                var settings = {};
                                if (helper.hasValue(attributeBasic) && attributeBasic.toLowerCase() === "true") {
                                    settings = tinyDefaultSettings;
                                } else {
                                    settings = tinyRichSettings;
                                }

                                var commandToolBox = $(".section-content-manage", $el.parent());


                                if (helper.hasValue(attribInLine) && attribInLine.toLowerCase() === "true") {
                                   if (editMode) {
                                        var key = getSectionBodyKey($el);
                                        tinymce.init($.extend(settings,
                                        {
                                            inline: true,                                          
                                            selector: "#" + $el.prop("id"),
                                            fixed_toolbar_container: "#section-content-manage-key-" + key,
                                            
                                        }));
                                       
                                      commandToolBox.show(1000);
                                      
                                   } else {

                                        commandToolBox.hide(1000);
                                    }


                                } else {
                                    $.extend(settings,
                                    {
                                        fixed_toolbar_container: null
                                    });
                                    tinymce.init($.extend(settings,
                                    {
                                        inline: false,
                                        selector: "#" + $el.prop("id")
                                    }));
                                    $("#vlePluginModal")
                                        .on("shown.bs.modal",
                                            function() {
                                                $(document).off("focusin.modal");
                                            });

                                }
                            });
                    }

                }

            },

            saveContent: function(sectionId, callback, errorCallback) {
                saveContent(sectionId,callback, errorCallback);
            }
        };

        function log(name, obj) {
            if (name === "NEW") {
                console.info("-------------------");
            } else {
                console.info(name + " : " + obj);
                //console.info('$' + name + ' : ' + $(obj));
            }
        }

        function fullTraceElement(element) {
            //log('NEW');
            //log("element",element);
            //log("innerHTML",element.innerHTML);
            //log("outerHTML",element.outerHTML);
            //log("childNodes", element.childNodes);
            ////log("childNodes.length", element.childNodes.length);
            //log("children", element.children);
            //log("className", element.className);
            //log("style", element.style);
            //log("id",$(element).prop('id'));
            //log("name",$(element).prop('name'));
            //log("closest('div.mce-content-body')",$(element).closest('div.mce-content-body'));
            //log('NEW');
        };

        function getTinyDefaultSettings() {            
            return {
                setup: function(editor) {
                    editor.on("NodeChange",
                        function(e) {

                            var element = e.element;

                            var container = $(element).closest("div.mce-content-body");
                            var factory = new EditorParserFactory();
                            var parser = factory.getEditorParser(element, container);
                            if (parser != null) {
                                parser.update(element, container, editor);
                            }


                        });

                    editor.on("keyup change",
                        function (e) {

                            console.log(e);

                            editor.save();
                            var targetElement = editor.getElement();
                            var fieldName = $(targetElement).attr("name");
                            if (helper.hasValue($(targetElement).closest("form").data("formValidation"))) {
                                $(targetElement).closest("form").formValidation(cnt.fv_RevalidateField, fieldName);
                            }

                            //$(document).data("tiny-mce-dataChanged", true);
                            
                            $(document)
                                .trigger(cnt.Events.vleTinyMce.onEditorChanged,
                                { element: $(targetElement), editor: editor });
                        });

                    editor.on("blur",
                        function(e) {
                            return false;
                        });
                },
                'paste_as_text': true,
                selector: ".tinymce",
                resize: "both",
                height: "150px",
                browser_spellcheck: true,
                object_resizing: "img",
                skin: "lightgray",
                menu: {
                    // this is the complete default configuration
                    edit: {
                        title: "Edit",
                        items: "undo redo | cut copy paste pastetext | selectall"
                    },
                    insert: {
                        title: "Insert",
                        items: "link hr"
                    },
                    view: {
                        title: "View",
                        items: "visualaid"
                    },
                    format: {
                        title: "Text Formatting",
                        items: "bold italic strikethrough superscript subscript | removeformat"
                    },
                    tools: {
                        title: "Tools",
                        items: ""
                    }
                },

                plugins: [
                    "autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                    "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                    "save table contextmenu directionality emoticons template paste textcolor embed"
                ],

                toolbar1:
                    "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | link | removeformat",

                table_grid: true,
                table_default_styles: { fontWeight: "bold" },

                templates: [
                    {
                        title: "Datatable header row",
                        description: "Datatable with header row and caption",
                        url: window.appUrl + "Scripts/js-templates/data-table-header-row.html"
                    },
                    {
                        title: "Datatable header column",
                        description: "Datatable with header column and caption",
                        url: window.appUrl + "Scripts/js-templates/data-table-header-column.html"
                    },
                    {
                        title: "Call-out box",
                        description: "Call-out box",
                        url: window.appUrl + "Scripts/js-templates/call-out-box.html"
                    },
                    {
                        title: "Blockquote",
                        description: "Blockquote with author",
                        url: window.appUrl + "Scripts/js-templates/blockquote.html"
                    },
                    {
                        title: "Show/hide",
                        description: "show and hide section",
                        url: window.appUrl + "Scripts/js-templates/collapsed.html"
                    },
                    {
                        title: "Tabs x2",
                        description: "2 tabs across",
                        url: window.appUrl + "Scripts/js-templates/tabs2.html"
                    },
                    {
                        title: "Tabs x3",
                        description: "3 tabs across",
                        url: window.appUrl + "Scripts/js-templates/tabs3.html"
                    },
                    {
                        title: "Tabs x4",
                        description: "4 tabs across",
                        url: window.appUrl + "Scripts/js-templates/tabs4.html"
                    },
                    {
                        title: "Banner image",
                        description: "large banner with image text and link",
                        url: window.appUrl + "Scripts/js-templates/banner-image.html"
                    },
                    {
                        title: "Highlights",
                        description: "3 highlights across",
                        url: window.appUrl + "Scripts/js-templates/highlights.html"
                    },
                    {
                        title: "Grouped links (1 group, 2 columns)",
                        description: "grouped links (1 group, 2 columns)",
                        url: window.appUrl + "Scripts/js-templates/links-1group-2columns.html"
                    },
                    {
                        title: "Grouped links (1 group, 3 columns)",
                        description: "grouped links (1 group, 3 columns)",
                        url: window.appUrl + "Scripts/js-templates/links-1group-3columns.html"
                    },
                    {
                        title: "Anchor box",
                        description: "Table of content for page",
                        url: window.appUrl + "Scripts/js-templates/anchor-box.html"
                    }
                ],

                convert_urls: false

            };
        }

        function getTinyRichSettings(tinyDefaultSettings) {
            var tinyRich = {};
            $.extend(tinyRich,
                tinyDefaultSettings,
                {
                    selector: ".tinymce-rich",
                    height: "500px",

                    toolbar2:
                        "bullist numlist outdent indent | image media preview" +
                            " template table  visualblocks    search replace   anchor  hr code embed",
                    image_advtab: true,                    
                    image_caption: true,
                    formats: { handbookFormat: { inline: "span", classes: "number" } },
                    style_formats_merge: false,
                    style_formats: [
                        {
                            title: "Heading",
                            items: [
                                { title: "Heading 2", format: "h2" },
                                { title: "Heading 3", format: "h3" },
                            ]
                        },
                        {
                            title: "Paragraph",
                            items: [
                                { title: "Normal", block: "p", "classes": "normal" },
                                { title: "Lead", block: "p", "classes": "lead" },
                            ]
                        },
                        {
                            title: "Blocks",
                            items: [
                                { title: "Paragraph", format: "p" },
                                { title: "Blockquote", format: "blockquote" },
                                { title: "Div", format: "div" },
                                { title: "Pre", format: "pre" }
                            ]
                        }
                    ],
                    contextmenu_never_use_native: true,
                    contextmenu:
                        "link image inserttable | undo redo | styleselect | bold italic underline strikethrough superscript subscript | alignleft aligncenter alignright alignjustify | link | format | removeformat",

                });
            return tinyRich;
        }

        function getTinyRichSmallSettings(tinyDefaultSettings) {
            var tinyRichSmall = {};
            $.extend(tinyRichSmall,
                tinyDefaultSettings,
                {
                    menu:{},
                    selector: ".tinymce-rich-small",
                    height: "150px",
                    
                    style_formats: [
                        {
                            title: "Heading",
                            items: [
                                 { title: "Heading 1", format: "h1" },
                                { title: "Heading 2", format: "h2" },
                                { title: "Heading 3", format: "h3" },
                            ]
                        }
                    ], 
                    contextmenu_never_use_native: true,
                    contextmenu:
                        "link undo redo | bold italic underline strikethrough superscript subscript | alignleft aligncenter alignright alignjustify | link | format | removeformat"

                });
            return tinyRichSmall;
        }
        function getTinyListOnlySettings(tinyDefaultSettings) {
            var tinyRich = {};
            $.extend(tinyRich,
                tinyDefaultSettings,
                {
                    selector: ".tinymceListOnly",
                    'paste_as_text': true,
                    height: "250px",
                    menu: {},
                    plugins: "advlist, advlist, link,paste",
                    advlist_bullet_styles: "default",
                    toolbar1: "bullist",
                    toolbar2: "",
                    templates: ""
                });
            return tinyRich;
        }

        function saveContent(sectionId, callback, errorCallback) {

            var content = tinymce.get(sectionId).getContent();

            var id = getSectionBodyKey($("#" + sectionId));

            api.ajaxPost(cnt.SaveContentUrl,
                { id: id, content: content },
                function(result) {
                    if (helper.hasValue(callback)) {
                        callback(result);
                    }
                },
                function(result) {
                    if (helper.hasValue(errorCallback)) {
                        errorCallback(result);
                    }
                    
                });
        }

        function getContent(id, callback, errorCallback) {
            api.ajaxPost(cnt.GetContentByIdUrl,
                { id: id },
                function(result) {
                    callback(result);
                },
                function(result) {
                    errorCallback(result);
                });
        }

        function getSectionBodyKey($element) {
            return $element.attr("data-section-key");
        }

        function saveDraftContent(id, title, callback, errorCallback) {
            api.ajaxPost(cnt.SaveDraftUrl,
                { id: id, title: title },
                function(result) {
                    callback(result);
                },
                function(result) {
                    errorCallback(result);
                });
        }

        // class 
        function EditorParserFactory() {

            this.getEditorParser = function(element, editorContainer) {
                var parser = new ShowHideEditor();
                if (parser.match(element, editorContainer)) {
                    return parser;

                }
                return null;
            };
        }

        // class 
        function ShowHideEditor() {

            this.update = function(element, editorContainer, editor) {
                if ($(element).get(0).tagName === "A") {
                    if ($(element).attr("href").indexOf("#collapse1") >= 0) {
                        var newId = ("_g_" + helper.GUID()).substring(0, 10);
                        $(element).attr("href", "#" + newId);
                        $(element).attr("data-mce-href", "#" + newId);

                        $(("#collapse1")).attr("id", newId);

                        editor.save();
                        $(document)
                            .trigger(cnt.Events.vleTinyMce.onEditorChanged, { element: $(element), editor: editor });
                    }

                }
            };
            this.match = function(element, editorContainer) {

                if ($(element).get(0).tagName === "A") {
                    if ($(element).attr("href").indexOf("#collapse1") >= 0) {
                        return true;
                    }
                }
                return false;
            };
        }

    });