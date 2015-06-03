using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeInvoice.Models.ViewModel
{
	[System.ComponentModel.DataAnnotations.Schema.NotMapped]
	public class ComboTransactionWithProductVM:ComboTransaction
	{
		public string Product { get; set; }
	}
}