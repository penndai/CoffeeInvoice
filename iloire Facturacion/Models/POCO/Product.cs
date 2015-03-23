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

	[Display(Name="Product name")]
	[Required]
	public string ProductName
	{
		get;
		set;
	}

	[DisplayFormat(DataFormatString = "{0:C}")]
	public double Price
	{
		get;
		set;
	}

	[DisplayFormat(DataFormatString="{0:C}")]
	public double? CNYPrice
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

	public virtual Provider Provider
	{
		get;
		set;
	}
}