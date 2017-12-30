using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Routing;


namespace System.Web.Mvc
{
    public static class NewLinkExtension
    {
        public static MvcHtmlString LinkFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string text, string name = null, object htmlAttributes = null)
        {
            if (htmlAttributes != null)
            {
                htmlAttributes = new RouteValueDictionary(htmlAttributes);
            }

            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            var url = metadata.Model.ToString();
            if (String.IsNullOrEmpty(labelText) || String.IsNullOrEmpty(text))
            {
                throw new ValidationException("Internal error",
                                   new InvalidOperationException("text and property value are requireds "));
            }

            var tag = new TagBuilder("a");

            tag.Attributes.Add("href", url);
            if (!String.IsNullOrEmpty(name))
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

            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(text);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }


        public static MvcHtmlString Link(this HtmlHelper html, string href, string text, string name = null, object htmlAttributes = null)
        {
            if (htmlAttributes != null)
            {
                htmlAttributes = new RouteValueDictionary(htmlAttributes);
            }

            var tag = new TagBuilder("a");

            tag.Attributes.Add("href", href);
            if (!String.IsNullOrEmpty(name))
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

            //tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(text));
            tag.SetInnerText(text);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}