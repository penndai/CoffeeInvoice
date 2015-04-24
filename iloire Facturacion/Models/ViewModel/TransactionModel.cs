using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

[NotMapped]
public class TransactionModel : Transaction
{
	/// <summary>
	/// total amount for this transaction
	/// </summary>
	public decimal TransactionSellAmount { get; set; }
}