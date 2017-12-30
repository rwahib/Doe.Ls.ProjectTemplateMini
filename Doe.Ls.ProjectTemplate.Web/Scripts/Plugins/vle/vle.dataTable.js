define(['appSettings', 'interactive', 'helper', 'datatables', 'bsSelect2'], function (appSettings, interactive, helper) {
	return {
	    applyGenericDataTable: function (pluginContainer) {
            
	        var extendedDataTableSettings = appSettings.genericDataTableSettings;

	       
	        $('.table', pluginContainer).not('.notDataTable').not('.ajaxDataTable').dataTable(extendedDataTableSettings);

			hookDataTableSelect();
		    
		},

		applyAjaxDataTable: function(callback) {
			require(['dataTableService'],
				function(service) {
				    service.initialise($('.ajaxDataTable'));
				    if (helper.hasValue(callback)) {
				        callback();
				    }
				});
		},

		applyNewDataTableSettings: function (extendedSettings, $table, callback) {

		    
            

		    var newSettings = $.extend(appSettings.genericDataTableSettings, extendedSettings);
			$table.dataTable().fnDestroy();

			$table
				.on('processing.dt',
					function(e, settings, processing) {

						var divProcessing = $('#DataTables_Table_0_processing');
					    if (!divProcessing.hasClass('loading-container')) divProcessing.addClass('divProcessing');
						if (processing) {
							divProcessing.html('<img src="' + window.appUrl + 'Images/prog.gif" /> ');
						} else {
							divProcessing.html('');
						}

					})
				.dataTable(newSettings);

			// hook records per page to select2
			hookDataTableSelect();
			if (helper.hasValue(callback)) {
				callback();
			}
		}
	};

	function hookDataTableSelect() {
		if ($('select[name*=_length]').length > 0) {
			$('select[name*=_length]').select2({ minimumResultsForSearch: -1 });
		}
	}
});