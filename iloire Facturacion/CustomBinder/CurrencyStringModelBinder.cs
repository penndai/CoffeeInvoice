using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeInvoice.CustomBinder
{
	public class CurrencyStringModelBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelName.StartsWith("TransactionSellAmount"))
			{
				ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
				object actualValue = null;
				if (valueResult != null)
				{
					ModelState modelState = new ModelState { Value = valueResult };

					string valToCheck = valueResult.AttemptedValue;

					actualValue = Convert.ToDecimal(valToCheck.Substring(1), CultureInfo.InvariantCulture);
				}

				return actualValue;
			}

			return base.BindModel(controllerContext, bindingContext);
			//var prefixValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			//if (prefixValue != null)
			//{
			//	var prefix = (String)prefixValue.ConvertTo(typeof(String));
			//	if (!String.IsNullOrEmpty(prefix) && !bindingContext.ModelName.StartsWith("Currency_"))
			//	{
			//		if (String.IsNullOrEmpty(bindingContext.ModelName))
			//		{
			//			bindingContext.ModelName = prefix;
			//		}
			//		else
			//		{
			//			bindingContext.ModelName = prefix + "." + bindingContext.ModelName;

			//			// fall back
			//			if (bindingContext.FallbackToEmptyPrefix &&
			//				!bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
			//			{
			//				bindingContext.ModelName = prefix;
			//			}
			//		}
			//	}
			//}
			//return base.BindModel(controllerContext, bindingContext);
		}
	}
}