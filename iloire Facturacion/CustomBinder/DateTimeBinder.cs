using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeInvoice.CustomBinder
{

	public class DateTimeBinder : IModelBinder
	{
		#region IModelBinder Members
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			DateTime dateTime;
			if (DateTime.TryParse(controllerContext.HttpContext.Request.QueryString["dateTime"], CultureInfo.GetCultureInfo("en-AU"), DateTimeStyles.None, out dateTime))
				return dateTime;
			//else
			return new DateTime();//or another appropriate default ;
		}
		#endregion
	}

}