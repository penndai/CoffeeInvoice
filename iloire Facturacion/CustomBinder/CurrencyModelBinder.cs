using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeInvoice.CustomBinder
{
	public class CurrencyModelBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			object actualValue = null;
			if (valueResult != null)
			{
				ModelState modelState = new ModelState { Value = valueResult };

				try
				{
					string valToCheck = valueResult.AttemptedValue;
					if (valToCheck == string.Empty)
					{
						actualValue = null;
					}
					else
					{
						actualValue = Convert.ToDecimal(valToCheck.Replace("$", string.Empty).Replace("￥", string.Empty), CultureInfo.InvariantCulture);
					}
				}
				catch (FormatException e)
				{
					modelState.Errors.Add(e);
				}

			}
			return actualValue;
		}
	}
}