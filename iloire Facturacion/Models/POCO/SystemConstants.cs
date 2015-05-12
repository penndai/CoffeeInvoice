using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

public class SystemConstants
{
	[Key]
	public int ConstantID { get; set; }
	public int UserID { get; set; }
	public string Constant { get; set; }
	public decimal ConstrantValue { get; set; }
}