using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Web.WebPages.Html;

namespace ElectroEshop.CustomExtensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> ex, Func<object, HelperResult> template)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(ex);
            var propertyName = htmlFieldName.Split('.').Last();
            var label = new TagBuilder("label");
            label.Attributes["for"] = TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName));
            label.InnerHtml = string.Format(
                "{0} {1}",
                propertyName,
                template(null).ToHtmlString()
            );
            return MvcHtmlString.Create(label.ToString());
        }
    }
}