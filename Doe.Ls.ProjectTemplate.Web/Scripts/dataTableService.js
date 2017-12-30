/// <reference path="_references.js" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function (dataTable) {
            dataTable.each(function () {
                var ajaxTable = $(this);
                var serviceType = ajaxTable.attr('data-service');

                var ajaxUrl = ajaxTable.attr('data-url');

                if (!helper.hasValue(ajaxUrl)) {
                    ajaxUrl = '';
                }

                applyDataTableSettings(serviceType, function (dataTableSettings) {
                    var generalSettings = getGeneralDataTableSettings(ajaxTable, ajaxUrl);
                    var newSettings = $.extend(generalSettings, dataTableSettings);
                    interactive.applyNewDataTableSettings(newSettings, ajaxTable);
                });

            });
        }
    }

    function applyDataTableSettings(serviceType, callBack) {

        switch (serviceType) {

            case 'appEntityTypeService':
                require(['appEntityTypeService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'appMessageService':
                require(['appMessageService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'appObjectInfoService':
                require(['appObjectInfoService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'capabilityBehaviourIndicatorService':
                require(['capabilityBehaviourIndicatorService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'capabilityGroupService':
                require(['capabilityGroupService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'capabilityLevelService':
                require(['capabilityLevelService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'capabilityNameService':
                require(['capabilityNameService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'costCentreDetailService':
                require(['costCentreDetailService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'directorateService':
                require(['directorateService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'employeeService':
                require(['employeeService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'employeePositionService':
                require(['employeePositionService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'employeeTypeService':
                require(['employeeTypeService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'executiveService':
                require(['executiveService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'focusService':
                require(['focusService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'functionalAreaService':
                require(['functionalAreaService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'generalLogService':
                require(['generalLogService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'globalItemService':
                require(['globalItemService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'gradeService':
                require(['gradeService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'keyRelationshipService':
                require(['keyRelationshipService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'locationService':
                require(['locationService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'lookupFocusGradeCriteriaService':
                require(['lookupFocusGradeCriteriaService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'occupationTypeService':
                require(['occupationTypeService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'positionService':
                require(['positionService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'positionDescriptionService':
                require(['positionDescriptionService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'positionFocusCriteriaService':
                require(['positionFocusCriteriaService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'positionInformationService':
                require(['positionInformationService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'positionLevelService':
                require(['positionLevelService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'positionNoteService':
                require(['positionNoteService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'positionStatusValueService':
                require(['positionStatusValueService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'positionTypeService':
                require(['positionTypeService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'relationshipScopeService':
                require(['relationshipScopeService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'roleCapabilityService':
                require(['roleCapabilityService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'roleDescCapabilityMatrixService':
                require(['roleDescCapabilityMatrixService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'roleDescriptionService':
                require(['roleDescriptionService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'rolePositionDescriptionService':
                require(['rolePositionDescriptionService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'selectionCriteriaService':
                require(['selectionCriteriaService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'statusValueService':
                require(['statusValueService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'sysRoleService':
                require(['sysRoleService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'UserService':
                require(['UserService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'sysUserRoleService':
                require(['sysUserRoleService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            case 'unitService':

                require(['unitService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;
            case 'businessUnitService':
                require(['businessUnitService'], function (service) {
                    if (helper.hasValue(callBack)) {
                        callBack(service.dtSettings);
                    }
                });
                break;

            default:
                break;
        }
    }

    function getGeneralDataTableSettings(dataTable, ajaxUrl) {
        return {
            'language': {
                'lengthMenu': ' _MENU_ records per page',
                'loadingRecords': 'Please wait - loading...'
            },
            'bProcessing': true,
            'bServerSide': true,
            'bPaginate': true,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            'nPaginationType': 'fullNumbers',
            'sAjaxSource': ajaxUrl,
            'initComplete': function () {
                //only exec below for IE as search functionality not working in IE9
                if (navigator.userAgent.indexOf("MSIE") > 0) {
                    var datatableApi = this.api();
                    $('.dataTables_filter input').unbind('keyup').bind('keyup', function (e) {
                        datatableApi.search($(this).val()).draw();
                    });
                }
                $(dataTable).show();
            }
        };
    }
});