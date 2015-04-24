using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using Microsoft.AspNet.Identity;

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

			if (Session["LoginUser"] != null)
			{
				User user = (User)Session["LoginUser"];
				transactions = transactions.Where(x => x.UserID == user.UserID).ToList();
			}

			IPagedList<TransactionModel> PagedTransactions = new List<TransactionModel>().ToPagedList(currentPage, pagesize.HasValue ? pagesize.Value : defaultPageSize);

			foreach (Transaction t in transactions)
			{
				TransactionModel tm = new TransactionModel();
				tm.Customer = t.Customer;
				tm.CustomerID = t.CustomerID;
				tm.Number = t.Number;
				tm.Product = t.Product;
				tm.ProductID = t.ProductID;
				tm.TimeStamp = t.TimeStamp;
				tm.TransactionID = t.TransactionID;
				tm.User = t.User;
				tm.UserID = t.UserID;

				if (t.Number > 0 && t.Product.CNYSellPrice.HasValue)  
				{
					tm.TransactionSellAmount = t.Number * t.Product.CNYSellPrice.Value;
				}
				PagedTransactions.Add(tm);
			}

			PagedTransactions = PagedTransactions.OrderByDescending(x => x.TimeStamp).ToPagedList(currentPage, pagesize.HasValue ? pagesize.Value : defaultPageSize);
					//transactions.OrderByDescending(x => x.TimeStamp).ToPagedList(currentPage, pagesize.HasValue ? pagesize.Value : defaultPageSize);

			ViewBag.AUPrice = PagedTransactions.Sum(x => x.Product.Price);
			ViewBag.CNYPrice = PagedTransactions.Sum(x=>x.Product.CNYPrice);
			ViewBag.SellCNYPrice = PagedTransactions.Sum(x=>x.Product.CNYSellPrice);
			return View(PagedTransactions);
		}

		public ActionResult Edit(int id)
		{
			Transaction t = db.Transactions.Find(id);
			TransactionModel tm = new TransactionModel();
			tm.Customer = t.Customer;
			tm.CustomerID = t.CustomerID;
			tm.Number = t.Number;
			tm.Product = t.Product;
			tm.ProductID = t.ProductID;
			tm.TimeStamp = t.TimeStamp;
			tm.TransactionID = t.TransactionID;
			tm.User = t.User;
			tm.UserID = t.UserID;

			ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", t.ProductID);
			ViewBag.Customers = new SelectList(db.Customers.OrderByDescending(x => x.Name), "CustomerID", "Name", t.CustomerID);

			return View(tm);
		}

		[HttpPost]
		public ActionResult Edit(TransactionModel tm)
		{
			if (ModelState.IsValid)
			{
				Transaction t = db.Transactions.Find(tm.TransactionID);
				t.Number = tm.Number;
				db.Entry<Transaction>(t).State = EntityState.Modified;
				if (Session["LoginUser"] != null)
				{
					t.UserID = ((User)Session["LoginUser"]).UserID;
				}
				db.SaveChanges();

				return RedirectToAction("Index");
			}
			else
			{
				ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", tm.ProductID);
				ViewBag.Customers = new SelectList(db.Customers.OrderByDescending(x => x.Name), "CustomerID", "Name", tm.CustomerID);

				return View(tm);
			}
		}

		public ActionResult Create()
		{
			TransactionModel t = new TransactionModel();
			t.TimeStamp = DateTime.Now;
			t.CustomerID = -1;
			t.ProductID = -1;
			t.Number = 1;
			ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", t.ProductID);
			ViewBag.Customers = new SelectList(db.Customers.OrderByDescending(x => x.Name), "CustomerID", "Name", t.CustomerID);

			return View(t);
		}

		[HttpPost]
		public ActionResult Create(TransactionModel tm, int? Products, int? Customers)
		{
			if (ModelState.IsValid)
			{
				PurchaseProduct pp = new PurchaseProduct();
				Transaction t = new Transaction();

				t.Customer = tm.Customer;
				t.CustomerID = tm.CustomerID;
				t.Number = tm.Number;
				t.Product = tm.Product;
				t.ProductID = tm.ProductID;
				t.TimeStamp = tm.TimeStamp;
				t.TransactionID = tm.TransactionID;
				t.User = tm.User;
				t.UserID = tm.UserID;
				pp.CustomerID = tm.CustomerID;
				pp.ProductID = tm.ProductID;
				
				pp.ProviderID = db.Products.Find(tm.ProductID).ProviderID;
				pp.TimeStamp = DateTime.Now;
				
				if (Products.HasValue)
				{
					t.ProductID = Products.Value;
				}

				if (Customers.HasValue)
					t.CustomerID = Customers.Value;

				if(Session["LoginUser"] !=null)
				{
					t.UserID = ((User)Session["LoginUser"]).UserID;
					pp.UserID = t.UserID; 
				}

				db.PurchaseProducts.Add(pp);
				db.Transactions.Add(t);


				db.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				return View(tm);
			}				
		}
	}
}