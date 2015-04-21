using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;

public class PurchaseType
{
    public int PurchaseTypeID { get; set; }
	public int UserID { get; set; }
    [Required]
    public string Name { get; set; }

    public string Descr { get; set; }

	public virtual User User { get; set; }
    public virtual ICollection<Purchase> Purchases { get; set; }
}