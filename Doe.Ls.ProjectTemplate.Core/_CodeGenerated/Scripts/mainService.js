 
 


/// <reference path="_references.js" />

define(['interactive', 'helper'], function (interactive, helper) {

    return {
        initialise: function(){
            interactive.initialise();
          
            var frmId = interactive.getFormId();
            var wrapperId = interactive.getWrapperId();

            if (!helper.hasValue(frmId)) {
                frmId = '';
            }

            if(!helper.hasValue(wrapperId)){
                wrapperId = '';
            }

                if (wrapperId.indexOf('wrapper-AppEntityType') > -1 || frmId.indexOf('form-appEntityType') > -1){
                    require(['appEntityTypeService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-AppObjectInfo') > -1 || frmId.indexOf('form-appObjectInfo') > -1){
                    require(['appObjectInfoService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-BusinessUnit') > -1 || frmId.indexOf('form-businessUnit') > -1){
                    require(['businessUnitService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-CapabilityBehaviourIndicator') > -1 || frmId.indexOf('form-capabilityBehaviourIndicator') > -1){
                    require(['capabilityBehaviourIndicatorService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-CapabilityGroup') > -1 || frmId.indexOf('form-capabilityGroup') > -1){
                    require(['capabilityGroupService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-CapabilityLevel') > -1 || frmId.indexOf('form-capabilityLevel') > -1){
                    require(['capabilityLevelService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-CapabilityName') > -1 || frmId.indexOf('form-capabilityName') > -1){
                    require(['capabilityNameService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-CostCentreDetail') > -1 || frmId.indexOf('form-costCentreDetail') > -1){
                    require(['costCentreDetailService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-Directorate') > -1 || frmId.indexOf('form-directorate') > -1){
                    require(['directorateService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-Employee') > -1 || frmId.indexOf('form-employee') > -1){
                    require(['employeeService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-EmployeePosition') > -1 || frmId.indexOf('form-employeePosition') > -1){
                    require(['employeePositionService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-EmployeeType') > -1 || frmId.indexOf('form-employeeType') > -1){
                    require(['employeeTypeService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-Executive') > -1 || frmId.indexOf('form-executive') > -1){
                    require(['executiveService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-Focus') > -1 || frmId.indexOf('form-focus') > -1){
                    require(['focusService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-FunctionalArea') > -1 || frmId.indexOf('form-functionalArea') > -1){
                    require(['functionalAreaService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-GeneralLog') > -1 || frmId.indexOf('form-generalLog') > -1){
                    require(['generalLogService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-GlobalItem') > -1 || frmId.indexOf('form-globalItem') > -1){
                    require(['globalItemService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-GlobalSetting') > -1 || frmId.indexOf('form-globalSetting') > -1){
                    require(['globalSettingService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-Grade') > -1 || frmId.indexOf('form-grade') > -1){
                    require(['gradeService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-HierarchyLevel') > -1 || frmId.indexOf('form-hierarchyLevel') > -1){
                    require(['hierarchyLevelService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-KeyRelationship') > -1 || frmId.indexOf('form-keyRelationship') > -1){
                    require(['keyRelationshipService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-Location') > -1 || frmId.indexOf('form-location') > -1){
                    require(['locationService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-LookupFocusGradeCriteria') > -1 || frmId.indexOf('form-lookupFocusGradeCriteria') > -1){
                    require(['lookupFocusGradeCriteriaService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-OccupationType') > -1 || frmId.indexOf('form-occupationType') > -1){
                    require(['occupationTypeService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-OrgLevel') > -1 || frmId.indexOf('form-orgLevel') > -1){
                    require(['orgLevelService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-PositionDescription') > -1 || frmId.indexOf('form-positionDescription') > -1){
                    require(['positionDescriptionService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-PositionFocusCriteria') > -1 || frmId.indexOf('form-positionFocusCriteria') > -1){
                    require(['positionFocusCriteriaService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-PositionHistory') > -1 || frmId.indexOf('form-positionHistory') > -1){
                    require(['positionHistoryService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-PositionInformation') > -1 || frmId.indexOf('form-positionInformation') > -1){
                    require(['positionInformationService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-PositionLevel') > -1 || frmId.indexOf('form-positionLevel') > -1){
                    require(['positionLevelService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-PositionNote') > -1 || frmId.indexOf('form-positionNote') > -1){
                    require(['positionNoteService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-PositionStatusValue') > -1 || frmId.indexOf('form-positionStatusValue') > -1){
                    require(['positionStatusValueService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-PositionType') > -1 || frmId.indexOf('form-positionType') > -1){
                    require(['positionTypeService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-RelationshipScope') > -1 || frmId.indexOf('form-relationshipScope') > -1){
                    require(['relationshipScopeService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-RoleCapability') > -1 || frmId.indexOf('form-roleCapability') > -1){
                    require(['roleCapabilityService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-RoleDescCapabilityMatrix') > -1 || frmId.indexOf('form-roleDescCapabilityMatrix') > -1){
                    require(['roleDescCapabilityMatrixService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-RoleDescription') > -1 || frmId.indexOf('form-roleDescription') > -1){
                    require(['roleDescriptionService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-RolePositionDescription') > -1 || frmId.indexOf('form-rolePositionDescription') > -1){
                    require(['rolePositionDescriptionService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-RolePositionDescriptionHistory') > -1 || frmId.indexOf('form-rolePositionDescriptionHistory') > -1){
                    require(['rolePositionDescriptionHistoryService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-ScriptHistory') > -1 || frmId.indexOf('form-scriptHistory') > -1){
                    require(['scriptHistoryService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-SelectionCriteria') > -1 || frmId.indexOf('form-selectionCriteria') > -1){
                    require(['selectionCriteriaService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-StatusValue') > -1 || frmId.indexOf('form-statusValue') > -1){
                    require(['statusValueService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-SysMessage') > -1 || frmId.indexOf('form-sysMessage') > -1){
                    require(['sysMessageService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-SysMsgCategory') > -1 || frmId.indexOf('form-sysMsgCategory') > -1){
                    require(['sysMsgCategoryService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-SysRole') > -1 || frmId.indexOf('form-sysRole') > -1){
                    require(['sysRoleService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-SysUser') > -1 || frmId.indexOf('form-sysUser') > -1){
                    require(['sysUserService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-SysUserRole') > -1 || frmId.indexOf('form-sysUserRole') > -1){
                    require(['sysUserRoleService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-TeamType') > -1 || frmId.indexOf('form-teamType') > -1){
                    require(['teamTypeService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-TrimRecord') > -1 || frmId.indexOf('form-trimRecord') > -1){
                    require(['trimRecordService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-Unit') > -1 || frmId.indexOf('form-unit') > -1){
                    require(['unitService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-WfAction') > -1 || frmId.indexOf('form-wfAction') > -1){
                    require(['wfActionService'], function(service){
                        service.initialise();
                    });
                }

                if (wrapperId.indexOf('wrapper-Position') > -1 || frmId.indexOf('form-position') > -1){
                    require(['positionService'], function(service){
                        service.initialise();
                    });
                }

                

            $('#loading-div').hide();
            $('#main-container').show();
            $('footer').show();
        }
    }
});