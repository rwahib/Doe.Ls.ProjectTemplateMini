using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {

    [HasAnyAdminRole]
    public partial class GeneralLogController : AppControllerBase
        {
        private GeneralLogRepository _repository = null;
        public GeneralLogRepository Repository
            {
            get
                {

                return _repository = _repository ?? ServiceRepository.GeneralLogRepository();

                }

            }


        public ActionResult Index()
            {
            var generalLogs = Repository.List();
            var list = generalLogs.Take(1);
            CreateLookups();
            return View(list);
            }

        //
        // GET: /GeneralLog/Details/5
        public ActionResult Details(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var generalLog = Repository.GetEntityByKey(id);
            if(generalLog == null)
                {
                var msg = MessageHelper.NotFoundMessage("general log");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Details-modal", generalLog);
                }
            else
                {
                return View(generalLog);
                }


            }

        public ActionResult ListJson([FromUri] GeneralLogArgument arg)
            {
            if (!string.IsNullOrWhiteSpace(arg.LogAction)&& arg.LogAction.ToLower().StartsWith("all"))
            {
                arg.LogAction = "";
            }
            InitialiseArgument(arg);

            var dataTableResult = new DataTableResult();

            try
                {
                var generalLogs = Repository.List();
                var displayedGeneralLogs= generalLogs;

                if(!string.IsNullOrWhiteSpace(arg.sSearch) || !string.IsNullOrWhiteSpace(arg.LogAction) ||arg.FromDate.HasValue||arg.ToDate.HasValue)
                    {
                    
                    displayedGeneralLogs = Repository.FilterGeneralLogs(displayedGeneralLogs, arg);
                    
                    }
               
                displayedGeneralLogs = CustomOrderBy.CustomSort(displayedGeneralLogs, arg);

                var totalRecord = displayedGeneralLogs.Count();
                var totalDisplayRecord = displayedGeneralLogs.Count();

                if(arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedGeneralLogs = displayedGeneralLogs.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedGeneralLogs.AsEnumerable().ToArray().Select(ent =>
                {

                    var light = ent.To(new GeneralLogLight());
                    light.RoleName = ent.SysRole.RoleTitle;
                    return light;
                }
                );

                dataTableResult.sEcho = arg.sEcho;
                dataTableResult.iTotalRecords = totalRecord;
                dataTableResult.iTotalDisplayRecords = totalDisplayRecord;
                dataTableResult.aaData = result;
                dataTableResult.Status = Status.Success;
                dataTableResult.Message = "Success";
                }
            catch(Exception exception)
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

        // GET: /GeneralLog/Details/5
        public ActionResult DetailsJson(int id = 0)
            {
            var ajaxResult = new Result();

            try
                {
                var generalLogLight = Repository.GetEntityByKey(id).To(new GeneralLogLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = generalLogLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
                }
            catch(Exception exception)
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
        private void CreateLookups()
            {
            
            var actionListItems = EnumExtension.GetIntegerValues<Enums.LogActions>().Select(val =>
            {
                var actionValue = (Enums.LogActions) val;

                return new SelectListItemExtension
                {
                    Text = actionValue.GetDescription(),
                    Value = actionValue.GetDescription()
                };
            }
                );


            ViewBagWrapper.ListBag.SetList("actionListItems", actionListItems, ViewData);
            }

        protected override void Dispose(bool disposing)
            {

            base.Dispose(disposing);
            }
        }
    }