(function () {

    require.config({
        baseUrl: window.appUrl + 'Scripts/',        
        paths: {
            jquery: 'Framework/jquery-3.2.1.min',
            //'bootstrap-core': 'Framework/bootstrap-4/dist/js/bootstrap',
            'bootstrap': 'Framework/bootstrap-4/js/bootstrap.bundle.min',
            
            //'bootstrap': 'Plugins/bootstrap-accessibility/js/bootstrap-accessibility',

            //'bs4-util': 'Framework/bootstrap-4/dist/js/util',

            //'vendor-popper':'Framework/bootstrap-4/dist/js/popper.min',

            'bbox': 'Plugins/bootbox/bootbox',

            'datatables': 'Plugins/bootstrapDatatable/datatables.min',
            'datatables.net': 'Plugins/bootstrapDatatable/DataTables-1.10.11/js/jquery.dataTables.min',
            'datatables.net-bs': 'Plugins/bootstrapDatatable/DataTables-1.10.11/js/dataTables.bootstrap.min',
            'datatables.net-responsive': 'Plugins/bootstrapDatatable/Responsive-2.0.2/js/dataTables.responsive.min',
            'datatables.net-responsive-bs': 'Plugins/bootstrapDatatable/Responsive-2.0.2/js/responsive.bootstrap.min',
            
            bsDatepicker: 'Plugins/bootstrapDatepicker/bootstrap-datepicker',
            bsSelect2: 'Plugins/bootstrapSelect2/select2',
            bsTypeahead: 'Plugins/bootstrapTypeahead/bootstrap3-typeahead',

            formValidator: 'Plugins/bsFormValidation/js/formValidation.min',
            fValidator: 'Plugins/bsFormValidation/js/Framework/bootstrap4.min',

            bsMultiselect: 'Plugins/bootstrap-multiselect/js/bootstrap-multiselect',
            prettify: 'Plugins/bootstrap-multiselect/js/prettify',
            bsProgress: 'Plugins/bootstrapProgressBar/bootstrap-progressbar.min',
        	tinymce: 'Plugins/tinymce/js/tinymce/tinymce.min',
            moment: 'Plugins/jQueryCalendar/moment.min',
            fullCalendar: 'Plugins/jQueryCalendar/jquery-fullcalendar.min',
            bsTimepicker: 'Plugins/bootstrap-timepicker/bootstrap-timepicker',
            bsSwitcher: 'Plugins/bootstrap-switch/js/bootstrap-switch.min',
            'vle.tinymce': 'Plugins/vle/vle.tinymce',
            'vle.layout': 'Plugins/vle/vle.layout',
            'vle.dataTable': 'Plugins/vle/vle.dataTable',

            primitives: 'Plugins/Primitives/primitives/primitives.latest',
            bpEditor: 'Plugins/primitives/primitives/bporgeditor.latest',
            jQueryUI: 'Plugins/Primitives/jquery-ui-1.10.0.custom/jquery-ui-1.10.2.custom.min',
            jQueryLayout: 'Plugins/Primitives/jquery-ui-1.10.0.custom/jquery.layout-latest.min',
            jPrint: 'Plugins/primitives/jquery.printElement.min',
            pdfkit: 'Plugins/PDFKit/pdfkit',
            blobstream: 'Plugins/PDFKit/blob-stream',
            fs: 'Plugins/PDFKit/FileSaver.min'
        },
        shim: {
            //'jquery': {
            //    deps: ['modernizr']
            //},

            //'bs4-util': {
            //    deps: ['jquery']
            //},

            'bootstrap': {
                deps: ['jquery'/*'bs4-util','vendor-popper'*/]
            },

            'bbox': {
				deps: ['jquery', 'bootstrap']
			},

            'datatables': {
                deps: ['jquery', 'bootstrap', 'datatables.net', 'datatables.net-bs', 'datatables.net-responsive', 'datatables.net-responsive-bs']
            },

            'bsDatepicker': {
                deps: ['jquery', 'bootstrap']
            },

            'bsSelect2': {
                deps: ['jquery', 'bootstrap']
            },

            'bsTypeahead': {
                deps: ['jquery', 'bootstrap']
            },
            
            'formValidator': {
                deps: ['jquery']
            },

            'fValidator': {
                deps: ['jquery', 'bootstrap', 'formValidator']
            },

            'prettify': {
                deps: ['jquery', 'bootstrap']
            },

            'bsMultiselect': {
                deps: ['jquery', 'bootstrap', 'prettify']
            },

            'bsProgress': {
                deps: ['jquery', 'bootstrap']
            },

            'tinymce': {
                deps: ['jquery']
            },

            'moment': {
                deps: ['jquery']
            },

            'fullCalendar': {
                deps: ['jquery', 'moment']
            },
            'bsTimepicker': {
                deps: ['jquery', 'bootstrap']
            },
            'bsSwitcher': {
                deps: ['jquery', 'bootstrap']
            },
            'jQueryUI': {
                deps: ['jquery']
            },

            'pdfkit': {
                deps: ['jquery', 'fs']
            },

            'blobstream': {
                deps: ['jquery']
            },

            'fs': {
                deps: ['jquery']
            }
,
            'primitives': {
                deps: ['jquery', 'jQueryUI']
            },
            'bpEditor': {
                deps: ['jquery', 'primitives']
            },
            'jQueryLayout': {
                deps: ['jquery']
            },
            'jPrint': {
                deps: ['jquery']
            }
        }
    });


    require(['jquery', 'mainService'/*, 'modernizr', 'placeHolder'*/], function ($, main) {
        $(function () {
            main.initialise();
        });
    });
})();