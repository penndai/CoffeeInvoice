using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeInvoice.Controllers
{
	public class IndividualProductTransactionController : Controller
	{
		private InvoiceDB db = new InvoiceDB();
		private int defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);

		[HttpPost]
		public ActionResult Create(IndividualProductTransaction ipTran, int? ProductID, int CustomerID)
		{
			if (ModelState.IsValid)
			{
				if (Session["LoginUser"] != null)
				{
					int UserID = ((User)Session["LoginUser"]).UserID;					
					
					if (ProductID.HasValue)
					{						
						ipTran.ProductID = ProductID.Value;
						Product product =  db.Products.First(x => x.ProductID == ProductID.Value);
						ipTran.Price =product.Price;
						ipTran.CNYPrice = product.CNYPrice;
						ipTran.CNYSellPrice = product.CNYSellPrice;

						if (ipTran.ComboTransactionID == 0)
						{
							ComboTransaction cbTrans = new ComboTransaction();
							cbTrans.Benefit = (ipTran.CNYSellPrice.Value - ipTran.CNYPrice.Value) * ipTran.Number;
							cbTrans.CustomerID = ipTran.CustomerID;
							cbTrans.Expense = ipTran.CNYPrice.Value * ipTran.Number;//output
							cbTrans.Income = ipTran.TotalPrice;//income
							cbTrans.TimeStamp = DateTime.Now;
							cbTrans.UserID = UserID;
							db.ComboTransactions.Add(cbTrans);

							db.SaveChanges();
							ipTran.ComboTransactionID = cbTrans.ComboTransactionID;
						}
						else
						{
							//update combo transaction benefit and expense, income
							ComboTransaction cbTrans = db.ComboTransactions.Find(ipTran.ComboTransactionID);
							cbTrans.Benefit += (ipTran.CNYSellPrice.Value - ipTran.CNYPrice.Value) * ipTran.Number;
							cbTrans.Expense += ipTran.CNYPrice.Value * ipTran.Number;//output
							cbTrans.Income += ipTran.TotalPrice;//income
							db.Entry(cbTrans).State = System.Data.Entity.EntityState.Modified;
							db.SaveChanges();
						}
					}


					db.IndividualProductTransactions.Add(ipTran);
					db.SaveChanges();
					return RedirectToAction("Edit", "ComboTransaction", new { id = ipTran.ComboTransactionID });
				}
			}
			ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", ipTran.ProductID);

			return View(ipTran);
		}

		public ActionResult Create(int ComboTransactionID, int CustomerID)
		{
			IndividualProductTransaction ipTran = new IndividualProductTransaction();
			ipTran.ComboTransactionID = ComboTransactionID;

			if (Session["LoginUser"] != null)
			{
				int userid = ((User)Session["LoginUser"]).UserID;

				Product prd = db.Products.Where(x => x.UserID == userid).FirstOrDefault();
				if (prd != null)
				{
					ipTran.ProductID = prd.ProductID;
					ipTran.Product = prd;
					ipTran.CNYPrice = prd.CNYPrice.Value;
					ipTran.CNYSellPrice = prd.CNYSellPrice.Value;
					ipTran.Price = prd.Price;
				}
				else
				{
					ipTran.Price = 0;
					ipTran.CNYSellPrice = 0;
					ipTran.CNYPrice = 0;
				}

				ipTran.CustomerID = CustomerID;
				ipTran.Number = 1;
				ipTran.TotalPrice = ipTran.Number * ipTran.CNYSellPrice.Value;

				ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", ipTran.ProductID);
			}

			return View(ipTran);
		}

		public ActionResult Delete(int IndividualProductTransactionID, int ComboTransactionID)
		{
			if (Session["LoginUser"] != null)
			{
				IndividualProductTransaction ipTran = db.IndividualProductTransactions.Find(IndividualProductTransactionID);
				if (ipTran != null)
				{
					ComboTransaction comboTran = db.ComboTransactions.Find(ComboTransactionID);
					
					comboTran.Benefit -= (ipTran.CNYSellPrice.Value - ipTran.CNYPrice.Value) * ipTran.Number;
					comboTran.Expense -= ipTran.CNYPrice.Value * ipTran.Number;//output
					comboTran.Income -= ipTran.TotalPrice;//income

					db.IndividualProductTransactions.Remove(ipTran);
					db.Entry(comboTran).State = System.Data.Entity.EntityState.Modified;
					db.SaveChanges();
				}
			}

			return RedirectToAction("Edit", "ComboTransaction", new { id = ComboTransactionID });
		}		

		[HttpPost]
		public ActionResult Edit(IndividualProductTransaction ipTranVM, int ProductID)
		{
			if (Session["LoginUser"] != null)
			{
				if (ModelState.IsValid)
				{
					IndividualProductTransaction ipTranDB = db.IndividualProductTransactions.Find(ipTranVM.IndividualProductTransactionID);
					if (ipTranDB != null)
					{
						ipTranDB.Number = ipTranVM.Number;
						ipTranDB.TotalPrice = ipTranVM.TotalPrice;

						db.Entry(ipTranDB).State = System.Data.Entity.EntityState.Modified;
						db.SaveChanges();

						return RedirectToAction("Edit", "ComboTransaction", new { id =ipTranDB.ComboTransactionID}); ;
					}
					else
						return RedirectToAction("Index");
				}
				else
				{
					ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", ProductID);
					return RedirectToAction("Index");
				}
			}
			else
			{
				return RedirectToAction("Index","ComboTransaction");
			}
		}

		public ActionResult Edit(int IndividualProductTransactionID)
		{
			IndividualProductTransaction ipTran = new IndividualProductTransaction();
			if (Session["LoginUser"] != null)
			{
				ipTran = db.IndividualProductTransactions.Find(IndividualProductTransactionID);
				if (ipTran != null)
				{
					ViewBag.Products = new SelectList(db.Products.OrderByDescending(x => x.ProductName), "ProductID", "ProductName", ipTran.ProductID);
				}
			}

			return View(ipTran);
		}

		public JsonResult GetProductPrice(int? ProductID, int Number)
		{
			decimal? price = 0;
			decimal totalPrice = 0;

			
			price = db.Products.Find(ProductID).CNYSellPrice;
			totalPrice = (price.Value * Number);
			var data = new { UnitPrice = price.Value, TransactionTotalPrice = totalPrice};
			return Json(data, JsonRequestBehavior.AllowGet);
		}
	}
}