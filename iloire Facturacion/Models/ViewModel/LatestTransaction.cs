using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeInvoice.Models.ViewModel
{
	public class LatestTransaction
	{
		public int ID
		{
			get;
			set;
		}

		public int CustomerID
		{
			get;
			set;
		}

		public string Customer
		{
			get;
			set;
		}
		public int ProductID
		{
			get;
			set;
		}

		public string Product
		{
			get;
			set;
		}

		public decimal TotalPay
		{
			get;
			set;
		}

		public decimal Benefit
		{
			get;
			set;
		}

		public DateTime TransactionDate
		{
			get;
			set;
		}
		public bool IsPaid
		{
			get;
			set;
		}
		public DateTime? PaidDate
		{
			get;
			set;
		}
	}
}