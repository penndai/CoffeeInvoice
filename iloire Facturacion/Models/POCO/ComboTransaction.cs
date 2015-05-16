using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

public class ComboTransaction
{
	[Key]
	public int ComboTransactionID { get; set; }

	[ForeignKey("User")]
	public int UserID
	{
		get;
		set;
	}

	[ForeignKey("Customer")]
	public int CustomerID { get; set; }

	public bool IsPaid { get; set; }
	public DateTime? PaidDateTime { get; set; }
	public DateTime TimeStamp { get; set; }
	public decimal Weight { get; set; }
	public decimal TransPortPrice { get; set; }
	public decimal Expense { get; set; }
	public decimal Income { get; set; }
	public decimal Benefit { get; set; }

	public virtual Customer Customer { get; set; }
	public virtual User User { get; set; }
}