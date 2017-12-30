define([],
	function () {

	    return {
	        //googleApiSearchUrl: 'http://maps.googleapis.com/maps/api/geocode/json',
	        //auspostApiSearchUrl: 'https://digitalapi.auspost.com.au/postcode/search.json',
	        //auspostApiKey: 'cfbd9124-bccf-42d2-b3c1-5763a41595fa',
	        fv_UpdateStatus: 'updateStatus',
	        fv_ValidateField: 'validateField',
	        fv_RevalidateField: 'revalidateField',
	        fv_ValidationStatus_NotValidated: 'NOT_VALIDATED',

	        GetUserInfoFromAdByEmailUrl: window.appUrl + 'AppApi/GetUserInfoFromAdByEmail',
	        GetCurrentUserUrl: window.appUrl + 'AppApi/GetCurrentUser',
	        SetUtilityStatus: window.appUrl + 'appApi/SetUtilityStatus',
	        AssetUrl: window.appUrl + 'File/',
	        GetSchoolBySchoolCode: window.appUrl + 'School/GetBySchoolCode',
	       
	        GetFocusList: window.appUrl + "LookupFocusGradeCriteria/GetFocusList",
	        GetCriteriaList: window.appUrl + "LookupFocusGradeCriteria/GetCriteriaList",

	        GetIndicatorContext: window.appUrl + '/CapabilityBehaviourIndicator/GetBehaviourIndicators',
	        GetGrades: window.appUrl + '/Grade/GetGrades',
	        GetLocations: window.appUrl + '/Location/GetLocations',
	        //for chart

	        GetPositions: window.appUrl + '/Position/GetPositions',
	        SignInUrl: window.appUrl + 'Account/DoeSignIn',
	        LoadChartForPreview: window.appUrl + '/Position/LoadChartForPreview',
	        GetUnits: window.appUrl + '/Unit/GetUnits',
	        GetFunctionalAreas: window.appUrl + '/FunctionalArea/GetFunctionalAreas',
	        GetBusinessUnits: window.appUrl + '/BusinessUnit/GetBusinessUnits',
	        GetExecutiveCodes: window.appUrl + '/Executive/GetExecutiveCodes',
	        GetDirectorates: window.appUrl + '/Directorate/GetDirectorates',
	        PositionNumberExists: window.appUrl + '/Position/PositionNumberExists',
	        DocNumberExists: window.appUrl + '/RoleDescription/DocNumberExists',
	       // GetUnitChiefPositionNumbers: window.appUrl+'/Position/GetUnitChiefPositionNumbers',
	        EntityList: {
	            AjaxResult: {
	                "TotalCount": 0,
	                "Data": {
	                    "Tag": "***",
	                    "MessageTitle": "***",
	                    "EntityId": 0,
	                    "EnityContextId": "NA",
	                    "MessageContent": "*****",
	                    "MessageTypeId": "0",
	                    "ModeId": "0",
	                    "Note": null
	                },
	                "Status": 1,
	                "Message": null,
	                "Errors": null
	            },

	            GeneralLog: {
	                LogId: 0,
	                Action: '',
	                Context: '',
	                CreationDate: '',
	                Username: '',
	                Note: '',
	            }
	        },

	        Events: {
	            vleTinyMce: {
	                onSettignsChanged: 'vle.onSettignsChanged',
	                onEditorChanged: 'vle.onEditorChanged',
	                editMode: 'edit-mode',
	                settingsMode: 'settings'
	            },
	            interactive: {
	                initialisationDone: 'vle.interactive.initialisationDone',
	                basicInitialisationDone: 'vle.interactive.basicInitialisationDone',
	                initialisePluginsInProgress: 'vle.interactive.initialisePluginsInProgress',

	                hookDatePicker: 'hookDatePicker',
	                hookSelect2: 'hookSelect2',
	                hookTinymce: 'hookTinymce',
	                hookValidation: 'hookValidation',
	                hookDataTable: 'hookDataTable',
	                hookSelect2Ajax: 'hookSelect2Ajax'

	            }
	        },

	        UserRole: {
	            Guest: '-1',
	            SystemAdministrator: '10',
	            SsuAdministrator: '20',
	            SsuPowerUser: '30',
	            AssociationAdministrator: '40',
	            AssociationPowerUser: '50'
	        },

	        Association: {
	            NA: '-2',
	            ALL: '-1'
	        },

	        EventType: {
	            None: 0,
	            Draw: 1,
	            Result: 2,
	            SchoolHolidays: 3,
	            PublicHoliday: 4,
	            Carnival: 5,
	            Championship: 6,
	            Trial: 7
	        },

	        EventStatus: {
	            Planned: 1,
	            Published: 2,
	            Postponed: 3,
	            Cancelled: 4,
	            Updated: 5,
	            Tentative: 6
	        },

	        EventAction: {
	            DiscardChanges: 0,
	            SaveDraft: 1,
	            SaveTentative: 2,
	            SavePublish: 3,
	            SaveUpdate: 4,
	            DeleteEvent: 5,
	            CancelEvent: 6,
	            CloneEvent: 7,
	            CreateChildEvent: 8
	        },

	        QueryName: {
	            TermId: 't',
	            FromTermId: 'ft',
	            ToTermId: 'tt',

	            StartDate: 'sd',
	            EndDate: 'ed',

	            EventTypeId: 'et',

	            AssociationId: 'a',

	            SportId: 'sp',
	            ActivityId: 'act',

	            StatusId: 'st',
	            PL: 'pl',
	            StateEvents: 'se',

	            NationalEventsPrimary: 'nep',
	            NationalEventsSecondary: 'nes',
	            UseMainLayout: 'ml',

	            ViewAsTerm: 'vat',
	            ViewAsWeek: 'vaw',
	            ViewAsDay: 'vad'
	        }
	    }
	});

