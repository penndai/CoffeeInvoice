using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

public class IndividualProductTransaction
{
	[Key]
	public int IndividualProductTransactionID
	{
		get;
		set;
	}

	[ForeignKey("ComboTransaction")]
	public int ComboTransactionID { get; set; }

	public decimal Price
	{
		get;
		set;
	}
	public decimal? CNYPrice
	{
		get;
		set;
	}

	public decimal? CNYSellPrice
	{
		get;
		set;
	}

	public decimal TotalPrice
	{
		get;
		set;
	}

	[Range(1, 100)]
	[DisplayFormat(DataFormatString = "{0:D}", ApplyFormatInEditMode = true)]
	public int Number { get; set; }

	[ForeignKey("Product")]
	public int ProductID { get; set; }

	public int CustomerID { get; set; }
	public virtual ComboTransaction ComboTransaction { get; set; }
	public virtual Product Product { get; set; }

}