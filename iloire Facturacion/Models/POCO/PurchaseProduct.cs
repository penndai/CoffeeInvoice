using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

public class PurchaseProduct
{
	[Key]
	public int PurchaseProductID { get; set; }

	[Required]
	public int CustomerID { get; set; }
	public virtual Customer Customer { get; set; }


	public int ProductID { get; set; }
	public virtual Product Product { get; set; }

	public int ProviderID { get; set; }
	public virtual Provider Provider { get; set; }

	[DisplayName("Created")]
	public DateTime TimeStamp { get; set; }
}
