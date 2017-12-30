define(['cnt', 'jquery', 'helper', 'api', 'bootstrap', 'bbox'], function (cnt, $, helper, api, bootstrap, bbox) {

    return {
        initialise: function () {
            window.ajaxErrorDisplayed = false;
            filterDashboard();

            if (helper.hasValue($.fn.dataTable)) {
                $.fn.dataTable.ext.errMode = function (event, jqxhr, settings, thrownError) {
                   
                    bbox.alert('Session expired, please log in or contact your System Administrator for assistance.', function () {
                        //window.location.href = cnt.SignInUrl;
                    });
                };
            }
            //$(document).ajaxError(function (event, jqxhr, settings, thrownError) {

            //    if (!window.ajaxErrorDisplayed) {
            //        window.ajaxErrorDisplayed = true;
            //        event.stopPropagation();
            //        bbox.alert('<p>Error, sorry but something has gong wrong.</p>' +
            //            'You may need to sign in.', function () {
            //                //  window.location.href = cnt.SignInUrl;
            //            });
            //    }
            //}
            //    );

            var currentPage = window.currentPage;
            var currentPage2 = window.currentPage2;

            $.each($('li a[data-name]'),
                function (index, item) {
                    var $item = $(item);
                    var li = $item.parent();
                    li.removeClass('active');
                    var pageName = $item.attr('data-name');
                    if (!helper.hasValue(pageName)) {
                        pageName = $item.text().trim();
                    }

                    if (pageName === currentPage || pageName === currentPage2) {
                        li.addClass('active');
                    }

                });

            hookAdminSettings(helper);
        }
    };

    function hookAdminSettings() {

        $('#utility').on('shown.bs.collapse', function () {
            var url = cnt.SetUtilityStatus;
            var data = { collapsed: false };

            api.ajaxGetHtml(url,
                data,
                function (result) {
                });
        });

        $('#utility').on('hidden.bs.collapse', function () {

            var url = cnt.SetUtilityStatus;
            var data = { collapsed: true };

            api.ajaxGetHtml(url,
                data,
                function (result) {
                });
        });

        var settingButton = $("#edit-mode");

        if (canEdit()) {
            settingButton.removeClass('disabled');
            var mode = settingButton.attr('data-mode');
            if (!helper.hasValue(mode)) {
                settingButton.attr('data-mode', "settings");
                $('#edit-mode span')
                    .removeClass("glyphicon-pencil")
                    .removeClass("glyphicon-cog")
                    .addClass("glyphicon-cog");
                $('#edit-mode span').show();
            }
            $('#edit-mode')
                .on('click',
                    function () {
                        var mode = $(this).attr('data-mode');
                        switch (mode) {
                            case 'settings':
                                $('span', $(this))
                                    .removeClass("glyphicon-pencil")
                                    .removeClass("glyphicon-cog")
                                    .addClass("glyphicon-pencil");
                                $(this).attr('data-mode', "edit-mode");

                                $(document)
                                    .trigger(cnt.Events.vleTinyMce.onSettignsChanged,
                                {
                                    mode: cnt.Events.vleTinyMce.editMode
                                });

                                break;
                            case 'edit-mode':
                                $('span', $(this))
                                    .removeClass("glyphicon-pencil")
                                    .removeClass("glyphicon-cog")
                                    .addClass("glyphicon-cog");
                                $(this).attr('data-mode', "settings");
                                $(document)
                                    .trigger(cnt.Events.vleTinyMce.onSettignsChanged,
                                {
                                    mode: cnt.Events.vleTinyMce.settingsMode
                                });
                                break;
                        }

                    });
        } else {
            settingButton.removeClass('disabled').addClass('disabled');;


        }
    }

    function canEdit() {

        return $("section[data-editable='true']").length > 0;
    }

    function filterDashboard() {
        var searchText = $('input', '#search-dashboard');

        if (searchText.length === 1) {

            var resetBtn = $(':button', '#search-dashboard');


            resetBtn.on('click',
                function() {
                    searchText.val('');
                    applyFilter('');
                });

            searchText.on('change keyup',
                function() {
                    applyFilter($(this).val());
                });

        }
    }
    function applyFilter(term) {
        term = term.trim().toLowerCase();
        var listItems = $('h3.list-group-item-heading', '#dashboard-list');
        var clearItems = function() {
            listItems.each(function() {
                var $item = $(this);
                var $a = $('a', $item);
                var hidden = 'hidden';
                var highlight = 'highlight';

                $item.removeClass(hidden);
                $item.removeClass(highlight);

                $a.removeClass(hidden);
                $a.removeClass(highlight);
                

            });

          
        };

        if (term.length === 0) {
            clearItems();

        } else {
            clearItems();
            listItems.each(function () {
                var text = $(this).text().trim().toLowerCase();
                var $item = $(this);
                var $a = $('a', $item);
                var hidden = 'hidden';
                var highlight = 'highlight';

                if (helper.containTerms(text,term)) {
                    $item.addClass(highlight);
                    $a.addClass(highlight);

                    
                } else {
                    //console.log(text+' hidden');
                    $item.addClass(hidden);
                    $a.addClass(highlight);
                    
                }

               
            });

 
        }

     
    }

   
    
    
});

