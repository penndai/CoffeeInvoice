using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


	public class Transaction
	{
		[Key]
		public int TransactionID { get; set; }
		public int UserID { get; set; }
		[ForeignKey("Product")]
		public int ProductID { get; set; }

		public decimal Price
		{
			get;
			set;
		}
		public decimal CNYPrice
		{
			get;
			set;
		}

		public decimal CNYSellPrice
		{
			get;
			set;
		}
	

		[Range(1,100)]
		[DisplayFormat(DataFormatString = "{0:D}", ApplyFormatInEditMode = true)]
		public int Number { get; set; }

		[ForeignKey("Customer")]
		public int CustomerID { get; set; }
		public bool IsPaid { get;set;}
		public DateTime? PaidDateTime { get; set; }
		public DateTime TimeStamp { get; set; }

		public decimal Weight { get; set; }
		public decimal TransPortPrice { get; set; }
		public decimal Expense { get; set; }
		public decimal Income { get; set; }
		public decimal Benefit { get; set; }

		public virtual User User { get; set; }
		public virtual Product Product { get; set; }
		public virtual Customer Customer { get; set; }
	}
