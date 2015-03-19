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

	public string ProductName
	{
		get;
		set;
	}

	public double Price
	{
		get;
		set;
	}

	//[System.ComponentModel.DataAnnotations.Schema.ForeignKey("ProviderID")]
	public int ProviderID
	{
		get;
		set;
	}
}