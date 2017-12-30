using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.Settings;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    public class NotificationController : AppControllerBase
    {
        public ActionResult WebEmail(string email)
         {
             var emailMessage = new EmailMessage
             {
                 To = email,
                 From = CurrentUser.Email
             };

             return View(emailMessage);
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult WebEmail(EmailMessage emailMessage)
         {
             if (!Request.IsAjaxRequest())
             {
                 throw new InvalidOperationException("This should be an Ajax request");
             }

             var ajaxResult = new Result();
             var errors = new List<DbValidationError>();

             try
             {
                 if (ProjectTemplateSettings.Site.IsTestSite)
                 {
                     emailMessage.To = CurrentUser.Email;
                 }

                 ServiceRepository.EmailService().SendEmail(emailMessage);

                 ajaxResult.Status = Status.Success;
                 ajaxResult.Message = "Success";
             }
             catch (Exception exception)
             {
                 LogException(exception);
                 errors.Add(new DbValidationError("Database updates ", "Oops! something went wrong  " + exception.Message));

                 ajaxResult.Status = Status.Error;
                 ajaxResult.Message = "Errors";
                 ajaxResult.AddErrors(errors);
             }

             return Json(ajaxResult);
         }
    }
}