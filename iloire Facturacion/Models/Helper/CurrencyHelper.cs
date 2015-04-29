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

			if (data > 0)
			{
				if (data < 800)
					return data.ToString("C", culture);
				else
					return string.Format("<p class=\"green\">{0}</p>", data.ToString("C", culture));
			}
			else
				return string.Format("<span class=\"negative\">{0}</span>", data.ToString("C", culture));
		}
	}
}