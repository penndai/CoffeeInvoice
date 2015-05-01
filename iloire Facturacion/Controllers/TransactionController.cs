using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using Microsoft.AspNet.Identity;
using CoffeeInvoice.Models.Helper;
using System.Data.Entity.Core.Objects;

namespace CoffeeInvoice.Controllers
{
	[Authorize]
	public class TransactionController : Controller
	{
		private InvoiceDB db = new InvoiceDB();
		private int defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);

		public PartialViewResult LatestTransactions()
		{
			List<Transaction> trans = new List<Transaction>();
			if (Session["LoginUser"] != null)
			{
				User user = (User)Session["LoginUser"];
				trans = 
					db.Transactions.Include(x => x.Customer).Include(x => x.Product).Where(x => x.UserID == user.UserID).OrderByDescending(x => x.TimeStamp).ToList();
				return PartialView("TransactionListPartial", trans);
			}
			else
			{
				return PartialView("TransactionListPartial", trans);
			}
		}

		public ActionResult Index(string filter, int? page, int? pagesize)
		{
			int currentPage = page.HasValue ? page.Value - 1 : 0;

			var transactions = db.Transactions.Include(x => x.Product).Include(x => x.Customer).ToList();

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
			ViewBag.CNYPrice = PagedTransactions.Sum(x => x.Product.CNYSellPrice);
			ViewBag.SellCNYPrice = PagedTransactions.Sum(x =>x.Number * x.Product.CNYSellPrice);
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
			tm.IsPaid = t.IsPaid;
			tm.PaidDateTime = t.PaidDateTime;
			if (t.Number > 0 && t.Product.CNYSellPrice.HasValue)
			{
				tm.TransactionSellAmount = t.Number * t.Product.CNYSellPrice.Value;
			}
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
				t.ProductID = tm.ProductID;
				t.Number = tm.Number;
				t.IsPaid = tm.IsPaid;
				if(t.IsPaid)
					t.PaidDateTime = tm.PaidDateTime;
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

		public JsonResult GetProductPrice(int? ProductID, int Number, string culture)
		{
			decimal? price = 0;
			decimal totalPrice = 0;

			var culInfo = new System.Globalization.CultureInfo(culture);
			price = db.Products.Find(ProductID).CNYSellPrice;
			totalPrice = (price.Value * Number);
			var data = new { UnitPrice = price.Value.ToString("C", culInfo), TransactionTotalPrice = totalPrice.ToString("C", culInfo) };
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Create()
		{
			TransactionModel t = new TransactionModel();
			t.TimeStamp = DateTime.Now;
			t.CustomerID = -1;
			t.ProductID = 1;
			t.Number = 1;

			t.Product = db.Products.Find(t.ProductID);
			t.TransactionSellAmount = t.Number * t.Product.CNYSellPrice.Value;
			ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", t.ProductID);
			ViewBag.Customers = new SelectList(db.Customers.OrderByDescending(x => x.Name), "CustomerID", "Name", t.CustomerID);

			return View(t);
		}

		[HttpPost]
		public ActionResult Create(TransactionModel tm, int? ProductID, int? CustomerID)
		{
			if (ModelState.IsValid)
			{
				PurchaseProduct pp = new PurchaseProduct();
				Transaction t = new Transaction();

				//t.Customer = tm.Customer;
				t.CustomerID = tm.CustomerID;
				t.Number = tm.Number;
				//t.Product = tm.Product;
				t.ProductID = tm.ProductID;
				t.TimeStamp = tm.TimeStamp;
				t.TransactionID = tm.TransactionID;
				t.IsPaid = tm.IsPaid;

				if (t.IsPaid)
					t.PaidDateTime = tm.PaidDateTime;
				//t.User = tm.User;
				t.UserID = tm.UserID;
				pp.CustomerID = tm.CustomerID;
				pp.ProductID = tm.ProductID;
				
				pp.ProviderID = db.Products.Find(tm.ProductID).ProviderID;
				pp.TimeStamp = DateTime.Now;

				if (ProductID.HasValue)
				{
					t.ProductID = ProductID.Value;
				}

				if (CustomerID.HasValue)
					t.CustomerID = CustomerID.Value;

				if (Session["LoginUser"] != null)
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

		public PartialViewResult OverDueTransaction()
		{
			var overdueTrans = new List<Transaction>();
			if (Session["LoginUser"] != null)
			{
				int userid = ((User)Session["LoginUser"]).UserID;

				overdueTrans = db.Transactions.Where(x => x.UserID == userid && !x.IsPaid && EntityFunctions.AddDays(x.TimeStamp,3) <= DateTime.Now).OrderBy(x=>x.TimeStamp).ToList();
			}

			return PartialView("TransactionListPartial",overdueTrans);
		}
	}
}