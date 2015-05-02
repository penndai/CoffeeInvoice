using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

	[NotMapped]
	public class MostTopCustomer
	{
		public int CustomerID
		{ get; set; 
		}

		public string CustomerName
		{
			get;
			set;
		}

		public decimal TotalPaid { get; set; }
	}
