


using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class KeyRelationshipController : AppControllerBase
    {
        private KeyRelationshipRepository _repository = null;
        public KeyRelationshipRepository Repository
        {
            get
            {

                return _repository = _repository ?? ServiceRepository.KeyRelationshipRepository();

            }

        }


        public ActionResult Index()
        {
            var keyRelationships = Repository.List();
            return View(keyRelationships.ToList());
        }

        //
        // GET: /KeyRelationship/Details/5
        public ActionResult Details(int id = 0, int roleDescriptionId = 0, bool? ajax = false)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var keyRelationship = Repository.GetKeyRelationshipWithRoleDescById(id);

            if (keyRelationship == null)
            {
                var msg = MessageHelper.NotFoundMessage("key relationship");
                throw new HttpException(msg);
            }
            if (Request.IsAjaxRequest())
            {
                return View("Details-modal", keyRelationship);
            }
            else
            {
                return View(keyRelationship);
            }


        }

        public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg)
        {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();

            try
            {
                var keyRelationships = Repository.List();
                IQueryable<KeyRelationship> displayedKeyRelationships;

                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedKeyRelationships = Repository.FilterKeyRelationships(keyRelationships, searchArgs);
                }
                else
                {
                    displayedKeyRelationships = CustomOrderBy.CustomSort(keyRelationships, arg);
                }

                var totalRecord = displayedKeyRelationships.Count();
                var totalDisplayRecord = displayedKeyRelationships.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedKeyRelationships = displayedKeyRelationships.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedKeyRelationships.AsEnumerable().ToArray().Select(ent => ent.To(new KeyRelationshipLight()));

                dataTableResult.sEcho = arg.sEcho;
                dataTableResult.iTotalRecords = totalRecord;
                dataTableResult.iTotalDisplayRecords = totalDisplayRecord;
                dataTableResult.aaData = result;
                dataTableResult.Status = Status.Success;
                dataTableResult.Message = "Success";
            }
            catch (Exception exception)
            {
                LogException(exception);

                dataTableResult.Status = Status.Error;
                dataTableResult.Message = "Errors";
                var msg = MessageHelper.ErrorOccured();
                dataTableResult.AddError(new DbValidationError("Error", msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
        }

        // GET: /KeyRelationship/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();

            try
            {
                var keyRelationshipLight = Repository.GetEntityByKey(id).To(new KeyRelationshipLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = keyRelationshipLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogException(exception);
                ajaxResult.Status = Status.Error;
                ajaxResult.Message = "Errors";
                var msg = MessageHelper.ErrorOccured();
                ajaxResult.AddError(new DbValidationError("Error", msg + exception.Message));
                //ajaxResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            }

        }



        // GET: /KeyRelationship/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var keyRelationship = Repository.GetKeyRelationshipWithRoleDescById(id);
            if (keyRelationship == null)
            {
                var msg = MessageHelper.NotFoundMessage("key relationship");
                throw new HttpException(msg);
            }
            if (Request.IsAjaxRequest())
            {
                return View("Delete-modal", keyRelationship);
            }
            else
            {
                return View(keyRelationship);
            }


        }
        //
        // GET: /KeyRelationship/Create
        public ActionResult Create(int roleDescId)
        {
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest())
            {
                return View("Create-modal", new KeyRelationship { RoleDescriptionId = roleDescId });
            }
            else
            {
                return View(new KeyRelationship { RoleDescriptionId = roleDescId });
            }
        }

        //
        // POST: /KeyRelationship/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KeyRelationship keyRelationship)
        {

            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();

            RemoveStates();
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    Repository.Insert(keyRelationship);

                    //Add to RolePositionDesc history
                    var rpd =
                        ServiceRepository.RolePositionDescriptionRepository()
                            .BaseList()
                            .SingleOrDefault(r => r.RolePositionDescId == keyRelationship.RoleDescriptionId);

                    if (rpd != null && rpd.StatusId != (int)Enums.StatusValue.Draft)
                    {
                        var history = new RolePositionDescriptionHistory
                        {
                            RolePositionDescId = keyRelationship.RoleDescriptionId,
                            Action =
                                Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Create) + " key relationship",
                            StatusFrom = Enum.GetName(typeof(Enums.StatusValue), rpd.StatusId),

                            StatusTo =
                                Enum.GetName(typeof(Enums.StatusValue), rpd.StatusId),
                            AdditionalInfo = "Created key relationship for this role description",
                            CreatedBy = CurrentUser.UserName,
                            CreatedDate = DateTime.Now
                        };
                        
                        ServiceRepository.RolePositionDescriptionHistoryRepository().Insert(history);
                    }
                }
                catch (Exception exception)
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error", "Oops! something went wrong.  " + exception.Message));

                    if ((Request.IsAjaxRequest()))
                    {
                        ajaxResult.Status = Status.Error;
                        ajaxResult.Message = "Errors";
                        ajaxResult.AddErrors(errors);
                        return Json(ajaxResult);
                    }
                    else
                    {

                        ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                    }

                }
                CreateLookups();
                if (Request.IsAjaxRequest())
                {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";
                    return Json(ajaxResult);
                }
                return RedirectToAction("ManageKeyRelationships", "RoleDescription", new { id = keyRelationship.RoleDescriptionId });

            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    ajaxResult.Status = Status.Error;
                    ajaxResult.Message = "Errors";
                    ajaxResult.AddErrors(errors);
                    return Json(ajaxResult);
                }
                else
                {
                    ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                    CreateLookups();
                    return View(keyRelationship);
                }

            }

        }


        //
        // GET: /KeyRelationship/Edit/5
        public ActionResult Edit(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var keyRelationship = Repository.GetKeyRelationshipWithRoleDescById(id);
            if (keyRelationship == null)
            {
                var msg = MessageHelper.NotFoundMessage("key relationship");
                throw new HttpException(msg);
            }
            CreateLookups();

            if (Request.IsAjaxRequest())
            {
                return View("Edit-modal", keyRelationship);
            }
            else
            {
                return View(keyRelationship);
            }
        }

        //
        // POST: /KeyRelationship/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KeyRelationship keyRelationship)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var keyRelationshipDb = Repository.BaseList().
                SingleOrDefault(k => k.KeyRelationshipId == keyRelationship.KeyRelationshipId);

            keyRelationship.ModifiedUserName = keyRelationshipDb.ModifiedUserName;
            keyRelationship.DateCreated = keyRelationshipDb.DateCreated;
            keyRelationship.LastUpdated = keyRelationshipDb.LastUpdated;

            var ajaxResult = new Result();
            var trailingList = keyRelationship.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            
            RemoveStates();

            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if (ModelState.IsValid)
            {
                var oldKeyRelationship = Repository.GetPrimitiveById(keyRelationship.KeyRelationshipId);

                keyRelationship.DateCreated = oldKeyRelationship.DateCreated;
                keyRelationship.RoleDescriptionId = oldKeyRelationship.RoleDescriptionId;

                Repository.SetPropertyValuesFrom(ref oldKeyRelationship, keyRelationship);

                try
                {
                    oldKeyRelationship.ModifiedUserName = CurrentUser.UserName;

                    Repository.Update(oldKeyRelationship);

                    //Add to RolePositionDesc history
                    var rpd =
                        ServiceRepository.RolePositionDescriptionRepository()
                            .BaseList()
                            .SingleOrDefault(r => r.RolePositionDescId == keyRelationship.RoleDescriptionId);
                    if (rpd != null && rpd.StatusId != (int)Enums.StatusValue.Draft)
                    {
                        var history = new RolePositionDescriptionHistory
                        {
                            RolePositionDescId = keyRelationship.RoleDescriptionId,
                            Action ="Updated key relationship",
                            StatusFrom = Enum.GetName(typeof(Enums.StatusValue), rpd.StatusId),
                            StatusTo =
                                Enum.GetName(typeof(Enums.StatusValue), rpd.StatusId),
                            AdditionalInfo = "Updated key relationship for this role description. KeyRelationshipId = " + keyRelationship.KeyRelationshipId,
                            CreatedBy = CurrentUser.UserName,
                            CreatedDate = DateTime.Now
                        };

                        ServiceRepository.RolePositionDescriptionHistoryRepository().Insert(history);
                    }

                }
                catch (Exception exception)
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error", "Oops! something went wrong. " + exception.Message));

                    if ((Request.IsAjaxRequest()))
                    {
                        ajaxResult.Status = Status.Error;
                        ajaxResult.Message = "Errors";
                        ajaxResult.AddErrors(errors);
                        return Json(ajaxResult);
                    }
                    else
                    {
                        ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                    }

                }

                CreateLookups();

                if (Request.IsAjaxRequest())
                {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";

                    return Json(ajaxResult);
                }
                return RedirectToAction("ManageKeyRelationships", "RoleDescription", new { id = keyRelationship.RoleDescriptionId });
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    ajaxResult.Status = Status.Error;
                    ajaxResult.Message = "Errors";
                    ajaxResult.AddErrors(errors);
                    return Json(ajaxResult);
                }
                else
                {
                    ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                    CreateLookups();
                    return View(keyRelationship);
                }
            }
        }

        //
        // POST: /KeyRelationship/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(KeyRelationship keyRelationship)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();
            var oldKeyRelationship = Repository.GetEntityByEntityKey(keyRelationship);
            try
            {
                Repository.Delete(oldKeyRelationship);
            }
            catch (Exception exception)
            {
                LogException(exception);

                var errors = Repository.GetBackendValidationErrors().ToList();

                errors.Add(new DbValidationError("", string.Format(GetMessage("ERR-GENERAL").MessageFormat, exception.Message)));
                errors.Add(new DbValidationError("", GetMessage("ERR-GENERAL-DELETE-HAS-CHILDS").MessageFormat));

                if ((Request.IsAjaxRequest()))
                {
                    ajaxResult.Status = Status.Error;
                    ajaxResult.Message = "Errors";
                    ajaxResult.AddErrors(errors);

                    return Json(ajaxResult);
                }
                else
                {
                    ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                }

            }

            CreateLookups();

            if (Request.IsAjaxRequest())
            {
                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";

                return Json(ajaxResult);
            }

            return RedirectToAction("ManageKeyRelationships", "RoleDescription", new { id = keyRelationship.RoleDescriptionId });
        }
        private void RemoveStates()
        {
            ModelState.Remove("ModifiedUserName");
            ModelState.Remove("LastUpdated");
            ModelState.Remove("DateCreated");
        }

        private void CreateLookups()
        {

            var enity = T4Helper.GetEntityType("KeyRelationship", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

            //var roleDescriptionItems=ServiceRepository.RoleDescriptionRepository()
            //            .List().ToArray()
            //            .Select(pe => new SelectListItemExtension { Value = pe.RoleDescriptionId.ToString(), Text =pe.OldPDFileName})
            //            .ToArray();

            //ViewBagWrapper.ListBag.SetList("roleDescriptionItems",roleDescriptionItems,ViewData);


            var relationshipScopeItems = ServiceRepository.RelationshipScopeRepository()
                      .List().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.ScopeId.ToString(), Text = pe.ScopeTitle })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("relationshipScopeItems", relationshipScopeItems, ViewData);


        }



        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}