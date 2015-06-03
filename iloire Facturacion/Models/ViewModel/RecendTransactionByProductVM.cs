using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeInvoice.Models.ViewModel
{
	public class RecendTransactionByProductVM
	{
		public List<Transaction> RecentSingleTrans { get; set; }
		public List<ComboTransactionWithProductVM> RecentComboTrans { get; set; }
	}
}