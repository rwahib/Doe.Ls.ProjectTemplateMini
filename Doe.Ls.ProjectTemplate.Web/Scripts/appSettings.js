define(['helper'],
	function (helper) {
	    return {
	        genericDataTableSettings: {
	            'language': {
	                'lengthMenu': '_MENU_ records per page',
	                'loadingRecords': 'Please wait while loading...'
	            },
	            'iDisplayLength': 10,
	            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
	            'pagingType': 'full_numbers',
	            'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [-1] }],
	            'aaSorting': [[0, 'desc']],
	            'rowCallback': function (row, data, index) {
	                $('td', row)
	                    .each(function() {
	                        if (this.children.length === 0) {
	                            var staticText = $(this).text().trim();
	                            if (!(staticText.length > 0)) {
	                                staticText = '[Not Provided]';
	                            }

	                            $(this).empty();
	                            $(this).append('<span tabindex="0">' + staticText + '</span>');
	                        }
	                    });
	            },
	            'initComplete': function () {
	                //only exec below for IE as search functionality not working in IE9
	                if (navigator.userAgent.indexOf("MSIE") > 0) {
	                    var api = this.api();

	                    $('.dataTables_filter input')
							.off('keyup')
							.on('keyup',
								function (e) {
								    api.search($(this).val()).draw();
								});
	                }
	            }
	        },

	        genericFormValidatorSettings: {
	            framework: 'bootstrap4',
	            icon: {
	                valid: 'fa fa-check',
	                invalid: 'fa fa-times',
	                validating: 'fa fa-refresh'
	            },
	            excluded: [':disabled', ':hidden', ':not(:visible)']
	        },
	    }
	});