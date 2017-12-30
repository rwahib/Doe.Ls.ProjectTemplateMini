using System;
using System.Reflection;
using System.Web.Mvc;

namespace Doe.Ls.EntityBase.MVCExtensions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ReleaseModeBaseAttribute : ActionMethodSelectorAttribute
    {
        public bool AjaxOnly { get; set; }
        public bool PostOnly { get; set; }
        public bool AuthorisedOnly { get; set; }

        #region Overrides of ActionMethodSelectorAttribute

        public override bool IsValidForRequest(ControllerContext ctx, MethodInfo methodInfo)
        {
            if (AjaxOnly)
            {
                if (!ctx.HttpContext.Request.IsAjaxRequest()) return false;
            }

            if (PostOnly)
            {
                if (ctx.HttpContext.Request.HttpMethod != "POST") return false;
            }

            if (AuthorisedOnly)
            {
                if (!ctx.HttpContext.Request.IsAuthenticated) return false;
            }
            return true;
        }

        #endregion
    }
}
