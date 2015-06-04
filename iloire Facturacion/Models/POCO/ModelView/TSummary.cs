using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TSummary
{
	public int Year { get; set; }

	public DateTime From { get; set; }
	public DateTime To { get; set; }

	public List<Transaction> SingleTransactions { get; set; }
	public List<ComboTransaction> ComboTransactions { get; set; }
	public decimal Transport { get; set; }
	public decimal Expense { get; set; }
	public decimal Income { get; set; }
	public decimal Benefit { get;set;}
}