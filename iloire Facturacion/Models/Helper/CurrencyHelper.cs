using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeInvoice.Models.Helper
{
	public static class CurrencyHelper
	{
		public static string Currency(this HtmlHelper helper, decimal data, string locale = "en-US", bool woCurrency = false)
		{
			var culture = new System.Globalization.CultureInfo(locale);

			if (woCurrency || (helper.ViewData["woCurrency"] != null && (bool)helper.ViewData["woCurrency"]))
				return data.ToString(culture);

			return data.ToString("C", culture);
		}
	}
}