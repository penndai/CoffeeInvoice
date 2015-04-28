using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CoffeeInvoice.Models.Helper
{
	public static class TextBoxCurrencyReadOnlyExtension
	{
		public static MvcHtmlString TextBoxCurrencyReadOnly<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper, Expression<System.Func<TModel, TProperty>> fieldExpression, object htmlAttributes)
		{
			// get the metdata
			ModelMetadata fieldmetadata = ModelMetadata.FromLambdaExpression(fieldExpression, htmlHelper.ViewData);

			// get the field name
			var fieldName = ExpressionHelper.GetExpressionText(fieldExpression);

			// get full field (template may have prefix)
			var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

			// build simple textbox html
			var tag = new TagBuilder("input");			
			tag.Attributes.Add("type", "text");
			tag.Attributes.Add("name", fullName);
			// get value
			if (fieldmetadata.Model != null && Convert.ToDecimal(fieldmetadata.Model)>0)
			{
				var value = fieldmetadata.Model.ToString();
				tag.Attributes.Add("value", CurrencyHelper.Currency(htmlHelper, Convert.ToDecimal(value), "zh-CN"));
			}
			else
			{
				tag.Attributes.Add("class", "negative");
				tag.Attributes.Add("value", 0.ToString("C", new System.Globalization.CultureInfo("zh-CN")));
			}
			// add passed html attributes

			var dict = new RouteValueDictionary(htmlAttributes);
			tag.MergeAttributes(dict);

			// add validation attributes
			if (!string.IsNullOrEmpty(fieldName))
				tag.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(fieldName));

			var html = tag.ToString(TagRenderMode.SelfClosing);
			return MvcHtmlString.Create(html);
		}
	}
}