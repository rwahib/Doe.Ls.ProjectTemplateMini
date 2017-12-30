using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class ContentExtension
    {
        
        public static MvcHtmlString ContentWithHeader(this HtmlHelper html, string header, string body, string name = null, object htmlAttributes = null)
        {
            if (htmlAttributes != null)
            {
                htmlAttributes = new RouteValueDictionary(htmlAttributes);
            }

            var tag = new TagBuilder("div");

            if (!string.IsNullOrWhiteSpace(name))
            {
                tag.Attributes.Add("name", name);    
            }
            
            if (htmlAttributes != null)
            {
                foreach (var key in (htmlAttributes as RouteValueDictionary).Keys)
                {
                    tag.Attributes.Add(key.Replace("_", "-"), (htmlAttributes as RouteValueDictionary)[key].ToString());
                }
            }
            tag.AddCssClass("content-wrapper");
            var content = string.Format("<header class=\"content-header\"><h5>" + header + "</h5></header>" + "<p class=\"content-body\">" + body + "</p>", header, body);

            tag.InnerHtml = content;
            
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}