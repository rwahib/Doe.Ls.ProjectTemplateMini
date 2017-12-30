using System;
using System.Globalization;
using System.Web.Mvc;

namespace Doe.Ls.EntityBase.MVCExtensions
{
    public class ETagAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetETag(GetGlobalToken());
            base.OnResultExecuted(filterContext);
        }

        private string GetGlobalToken()
        {
            return DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
        }
    }
}
