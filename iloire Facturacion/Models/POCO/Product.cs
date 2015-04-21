using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class Product
{
	[Key]
	public int ProductID
	{
		get;
		set;
	}

	public int UserID { get; set; }
	[Display(Name="Product name")]
	[Required]
	public string ProductName
	{
		get;
		set;
	}

	[DataType(DataType.Currency)]
	[DisplayFormat(DataFormatString = "{0:C}")]
	[Required]
	[Display(Name = "AU Price")]
	public decimal Price
	{
		get;
		set;
	}

	[DataType(DataType.Currency)]
	[DisplayFormat(DataFormatString="{0:C}")]
	[Display(Name = "CNY Price")]
	public decimal? CNYPrice
	{
		get;
		set;
	}

	[DataType(DataType.Currency)]
	[DisplayFormat(DataFormatString="{0:C}")]
	[Display(Name="Sell CNY Price")]
	public decimal? CNYSellPrice
	{
		get;
		set;
	}

	//[System.ComponentModel.DataAnnotations.Schema.ForeignKey("ProviderID")]
	[Display(Name="Provider")]
	public int ProviderID
	{
		get;
		set;
	}

	public virtual User User
	{
		get;
		set;
	}
	public virtual Provider Provider
	{
		get;
		set;
	}	
}