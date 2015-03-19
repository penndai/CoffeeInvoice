using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;

namespace CoffeeInvoice.Controllers
{
	public class ProductController : Controller
	{
		private InvoiceDB db = new InvoiceDB();
		private int defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);

		// GET: /Provider/

		public ViewResult Index(int? page)
		{
			var products = db.Products.OrderBy(i => i.ProductName).ToList();
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var productsListPaged = products.ToPagedList(currentPageIndex, defaultPageSize);
			return View(productsListPaged);
		}
	}	 
}