using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoffeeInvoice.Models.ViewModel
{
	[NotMapped]
	public class CustomerWithPinYinName : Customer
	{
		public string PinYin { get; set; }
	}
}