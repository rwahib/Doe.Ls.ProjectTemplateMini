using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Validation;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI
{
    /// <summary>
    /// This class name is misleading as we use the ViewData
    /// </summary>
    public static class ViewBagWrapper
    {
        public static void SetGeneralObject(string name, object obj, ViewDataDictionary bag)
        {
            bag[name] = obj;
        }
        public static T GetGeneralObject<T>(string name, ViewDataDictionary bag) 
        {
            return ViewDataWrapper.GetValue<T>(name, bag);
        }
        
        public static class UserInfoExtensionBag
        {
            public static UserInfoExtension GetCurrentUser(ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<UserInfoExtension>("CurrentUser", bag) ?? new UserInfoExtension();
            }

            public static void SetCurrentUser(UserInfoExtension uExt, ViewDataDictionary bag)
            {
                if (uExt != null)
                {
                    bag["CurrentUser"] = uExt;
                }
            }

            public static void SetTargetUser(UserInfoExtension uExt, ViewDataDictionary bag)
            {
                if (uExt != null)
                {
                    bag["TargetUser"] = uExt;
                }
            }

            public static UserInfoExtension GetTargetUser(ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<UserInfoExtension>("TargetUser", bag) ?? new UserInfoExtension();
            }
        }

        public static class ErrorBag
        {
            public static IEnumerable<DbValidationError> GetErrors(ViewDataDictionary bag)
            {
                var errors = ViewDataWrapper.GetValue<List<DbValidationError>>("Errors", bag) ??
                            new List<DbValidationError>();
                return errors;
            }

            public static void SetErrors(IEnumerable<DbValidationError> errors, ViewDataDictionary bag)
            {
                bag["Errors"] = errors;
            }
        }

        public static class SampleUserBag
        {
            public static void SetSampleUserByRole(Enums.UserRole role, string[] value, ViewDataDictionary bag)
            {
                bag[role.ToString()] = value;
            }
            public static string[] GetSampleUserByRole(Enums.UserRole role, ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<string[]>(role.ToString(), bag);
            }
        }

        public static class TaskBag
        {
            public static IUserTask GetCurrentTask(ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<IUserTask>("Task", bag);
            }

            public static void SetTask(IUserTask userTask, ViewDataDictionary bag)
            {
                bag["Task"] = userTask;
            }
        }
        public static class WorkflowBag
            {
            public static IWorkflowEngine GetWorkflowEngine(ViewDataDictionary bag)
                {
                return ViewDataWrapper.GetValue<IWorkflowEngine>("WorkflowEngine", bag);
                }

            public static void SetWorkflowEngine(IWorkflowEngine engine, ViewDataDictionary bag)
                {
                bag["WorkflowEngine"] = engine;
                }
            }

        public static class GlobalServiceRepository
        {
            public static ServiceRepository GetRepository(ViewDataDictionary viewData)
            {
                return GetGeneralObject<ServiceRepository>("ServiceRepository", viewData);
            }

            public static void SetRepository(ServiceRepository repository, ViewDataDictionary viewData)
            {
                SetGeneralObject("ServiceRepository", repository, viewData);
            }
        }
        public static class InfoBag
        {
            public static string GetTitle(ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<string>("Title", bag);

            }
            public static string GetDescription(ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<string>("Description", bag);

            }

            public static void SetTitle(string title, ViewDataDictionary bag)
            {
                bag["Title"] = title;
            }
            public static void SetDescription(string description, ViewDataDictionary bag)
            {
                bag["Description"] = description;
            }
        }

        public static class ListBag
        {
            public static IEnumerable<SelectListItem> GetList(string listName, ViewDataDictionary bag)
            {
                return bag[listName] as IEnumerable<SelectListItem>;
            }


            public static void SetList(string listName, IEnumerable<SelectListItem> items, ViewDataDictionary bag)
            {
                bag[listName] = items;
            }

            public static Object GetCapabilityGroupList(ViewDataDictionary bag)
            {
                return bag["capabilityGroup"] as IEnumerable<Object>;
            }
            public static void SetCapabilityGroupList(IEnumerable<Object> capabilityGroup, ViewDataDictionary bag)
            {
                bag["capabilityGroup"] = capabilityGroup;
            }


            public static Object GetSelectedCapabilityList(ViewDataDictionary bag)
            {
                return bag["SelectedCapabilityList"] as IEnumerable<Object>;
            }
            public static void SetSelectedCapabilityList(IEnumerable<Object> selectedCapabilityList, ViewDataDictionary bag)
            {
                bag["SelectedCapabilityList"] = selectedCapabilityList;
            }
        }

        public static class FormOperations
        {
            public static bool GetNullModel(ViewDataDictionary bag)
            {
                var result = bag["NullModel"] != null && (bool)bag["NullModel"];
                return result;
            }

            public static void SetNullModel(bool val, ViewDataDictionary bag)
            {
                bag["NullModel"] = val;
            }

            public static FormType GetFormType(ViewDataDictionary bag)
            {
                var result = bag["FormType"];
                FormType val;
                if (result == null || !Enum.TryParse(result.ToString(), out val))
                {
                    return FormType.Details;
                }
                return val;
            }

            public static void SetFormType(FormType formType, ViewDataDictionary bag)
            {
                bag["FormType"] = formType;
            }

            public static RequestType GetRequestType(ViewDataDictionary bag)
            {
                var result = bag["RequestType"];
                RequestType val;
                if (result == null || !Enum.TryParse(result.ToString(), out val))
                {
                    return RequestType.Direct;
                }
                return val;
            }

            public static void SetRequestType(RequestType requestType, ViewDataDictionary bag)
            {
                bag["RequestType"] = requestType;
            }

            public static FormMethod GetFormMethod(ViewDataDictionary bag)
            {
                var result = bag["FormMethod"];
                FormMethod val;
                if (result == null || !Enum.TryParse(result.ToString(), out val))
                {
                    return FormMethod.Get;
                }
                return val;
            }
            public static void SetFormMethod(FormMethod formMethod, ViewDataDictionary bag)
            {
                bag["FormMethod"] = formMethod;
            }

        }

        public static class EntityInfo
        {
            public static EntityType GetEntityType(ViewDataDictionary bag)
            {
                return bag["EntityType"] as EntityType;
            }

            public static void SetEntityType(EntityType entityType, ViewDataDictionary bag)
            {
                bag["EntityType"] = entityType;
            }

           
        }

        public static class DbContextBag
        {
            public static DbContext GetDbContext(ViewDataDictionary bag)
            {
                return bag["DbContext"] as DbContext;
            }

            public static void SetDbContext(DbContext ctx, ViewDataDictionary bag)
            {
                bag["DbContext"] = ctx;
            }
        }


        public static class LayoutBag
            {
            public static bool HasFooter(ViewDataDictionary bag)
                {
                if (bag["HasFooter"] == null)
                {
                    bag["HasFooter"] = true;
                }
                return (bool)bag["HasFooter"];
                }

            public static void SetHasFooter(bool hasFooter, ViewDataDictionary bag)
                {
                bag["HasFooter"] = hasFooter;
                }

            public static bool IsCollapsed(HttpSessionStateBase session)
                {
                if (session["utility-collapse"] == null)
                {
                    session["utility-collapse"] = false;

                }
                return (bool) session["utility-collapse"];
                }

            public static void SetUtilityStatus(bool collapsed,HttpSessionStateBase session)
            {
                session["utility-collapse"] = collapsed;
                
           }

            }


        public static class VariableBag
        {
            public static void SetStringVariable(string name, string value, ViewDataDictionary bag)
            {
                bag[name] = value;
            }

            public static string GetStringVariable(string name, ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<string>(name, bag);
            }

            public static void SetIntVariable(string name, int value, ViewDataDictionary bag)
            {
                bag[name] = value;
            }

            public static int GetIntVariable(string name, ViewDataDictionary bag)
            {
                return bag[name] is int ? (int)bag[name] : 0;
            }

            public static void SetBoolVariable(string name, bool value, ViewDataDictionary bag)
            {
                bag[name] = value;
            }

            public static bool GetBoolVariable(string name, ViewDataDictionary bag)
            {
                return bag[name] is bool && (bool)bag[name];
            }

            public static void SetDateTimeVariable(string name, DateTime value, ViewDataDictionary bag)
            {
                bag[name] = value;
            }

            public static DateTime GetDateTimeVariable(string name, ViewDataDictionary bag)
            {
                return bag[name] is DateTime ? (DateTime)bag[name] : DateTime.MinValue;
            }
        }


        public static class  PositionDescBag
        {
            public static Enums.PositionDescWizardStep GetPositionDescWizardBag( ViewDataDictionary bag)
            {
               return ViewDataWrapper.GetValue<Enums.PositionDescWizardStep>("PositionDescWizardStep", bag);
            }
            public static void SetPositionDescWizardBag(Enums.PositionDescWizardStep value, ViewDataDictionary bag)
            {
                bag["PositionDescWizardStep"] = value;
            }
            public static IEnumerable<LookupFocusGradeCriteria> GetSelectionCriteria(string listName, ViewDataDictionary bag)
            {
                return bag[listName] as IEnumerable<LookupFocusGradeCriteria>;
            }


            public static void SetSelectionCriteria(string listName, LookupFocusGradeCriteria[] items, ViewDataDictionary bag)
            {
                bag[listName] = items;
            }
        }

        public static class RoleDescBag
        {
            public static Enums.RoleDescWizardStep GetRoleDescWizardBag(ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<Enums.RoleDescWizardStep>("RoleDescWizardStep", bag);
            }

            public static void SetRoleDescWizardBag(Enums.RoleDescWizardStep value, ViewDataDictionary bag)
            {
                bag["RoleDescWizardStep"] = value;
            }


        }


        public static class PositionBag
        {
            public static Enums.PositionWizardStep GetPositionWizardBag(ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<Enums.PositionWizardStep>("PositionWizardStep", bag);
            }

            public static void SetPositionWizardBag(Enums.PositionWizardStep value, ViewDataDictionary bag)
            {
                bag["PositionWizardStep"] = value;
            }

            public static Enums.DescriptionType GetDescriptionTypeBag(ViewDataDictionary bag)
            {
                return ViewDataWrapper.GetValue<Enums.DescriptionType>("DescriptionType", bag);
            }

            public static void SetDescriptionTypeBag(Enums.DescriptionType value, ViewDataDictionary bag)
            {
                bag["DescriptionType"] = value;
            }

            public static List<PositionLight> GetPositionListModel(ViewDataDictionary viewData)
            {
                return viewData["PositionList"] as List<PositionLight>;
            }

            public static void SetPositionListModel(List<PositionLight> positionList, ViewDataDictionary viewData)
            {
                viewData["PositionList"] = positionList;
            }

        }

        public class ValidationTaskBag
            {
            public static void SetTasks(List<RolePositionDescTask> rolePositiondescTask, ViewDataDictionary bag)
                {
                bag["RolePositiondescTask"] = rolePositiondescTask;
                }

            public static List<RolePositionDescTask> GetTasks(ViewDataDictionary bag)
                {
                return bag["RolePositiondescTask"] as List<RolePositionDescTask>;
                }
            }

        public class MessageListBag
        {
            public static void SetMessageService(MessageService messageService, ViewDataDictionary bag)
                {
                bag["MessageService"] = messageService;
                }

            public static MessageService GetMessageService(ViewDataDictionary bag)
                {
                return bag["MessageService"] as MessageService;
                }
            }
        }
}