using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPaging;

public class ComboTransactionModel : ComboTransaction
{
	public IList<IndividualProductTransaction> IndividualTransactions { get; set; }
}