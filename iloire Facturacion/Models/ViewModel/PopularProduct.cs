using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


	[NotMapped]
	public class PopularProduct
	{
		public int ProductID { get; set; }
		public string Product { get; set; }
		public int Count { get; set; }
		public decimal TotalValue { get; set; }


	}
