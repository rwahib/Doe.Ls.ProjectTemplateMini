using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class NewLabelExtensions
    {

        public static MvcHtmlString LabelHtmlStringFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return LabelHtmlStringFor(html, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString LabelHtmlStringFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("label");
            foreach (var key in htmlAttributes.Keys)
            {
                tag.Attributes.Add(key.Replace("_", "-"), htmlAttributes[key].ToString());
            }

            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString SpanFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var propertyName = metadata.PropertyName;
            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            var span = new TagBuilder("span");
            span.Attributes.Add("class", "label");
            span.Attributes.Add("id", propertyName);

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                span.MergeAttributes(attributes);
            }
            span.SetInnerText(labelText);
            return new MvcHtmlString(span.ToString());
        }


        public static MvcHtmlString Span<TModel>(this HtmlHelper<TModel> helper, string value, string name = "", object htmlAttributes = null)
        {
            var labelText = value;
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            var span = new TagBuilder("span");

            if (!string.IsNullOrWhiteSpace(name))
            {
                span.Attributes.Add("name", name);

            }

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                span.MergeAttributes(attributes);
            }
            span.SetInnerText(labelText);
            return new MvcHtmlString(span.ToString());
        }

        public static MvcHtmlString ReadOnlyDisabledTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {/*
            MvcHtmlString html = default(MvcHtmlString);

            if (editable)
            {
                html = Html.InputExtensions.TextBoxFor(htmlHelper, expression);
            }
            else
            {*/
            var html = Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "readOnly", @readonly = "readonly", @disabled = "disabled" });
            // }
            return html;
        }
    }
}
