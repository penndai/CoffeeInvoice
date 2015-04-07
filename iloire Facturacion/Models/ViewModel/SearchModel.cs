using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoffeeInvoice.Models.ViewModel
{
	public class SearchModel
	{
		public string Action
		{
			get;
			set;
		}

		public string text
		{
			get;
			set;
		}
		
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? from
		{
			get;
			set;
		}

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? to
		{
			get;
			set;
		}
	}
}