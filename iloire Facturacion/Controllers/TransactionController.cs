using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using MvcPaging;

namespace CoffeeInvoice.Controllers
{
	[Authorize]
	public class TransactionController : Controller
	{
		private InvoiceDB db = new InvoiceDB();
		private int defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);

		public ActionResult Index(string filter, int? page, int? pagesize)
		{
			int currentPage = page.HasValue ? page.Value - 1 : 0;
			var transactions = db.Transactions.Include(x => x.Product).Include(x=>x.Customer).ToList();

			IPagedList<Transaction> PagedTransactions = null;
			PagedTransactions = transactions.OrderByDescending(x => x.TimeStamp).ToPagedList(currentPage, pagesize.HasValue ? pagesize.Value : defaultPageSize);

			return View(PagedTransactions);
		}

		public ActionResult Create()
		{
			Transaction t = new Transaction();
			t.TimeStamp = DateTime.Now;
			t.CustomerID = -1;
			t.ProductID = -1;

			ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", t.ProductID);
			ViewBag.Customers = new SelectList(db.Customers.OrderByDescending(x => x.Name), "CustomerID", "Name", t.CustomerID);

			return View(t);
		}
	}
}