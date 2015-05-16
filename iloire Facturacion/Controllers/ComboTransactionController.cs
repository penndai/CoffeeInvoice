﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;

namespace CoffeeInvoice.Controllers
{
	[Authorize]
	public class ComboTransactionController:Controller
	{
		private InvoiceDB db = new InvoiceDB();
		private int defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);

		private decimal _rate;
		private decimal AUDCNYRate
		{
			get { return _rate; }
		}

		public ActionResult Index(string filter, int? page, int? pagesize)
		{
			int currentPage = page.HasValue ? page.Value - 1 : 0;
			IPagedList<ComboTransaction> comboTrans = new List<ComboTransaction>().ToPagedList(currentPage, pagesize.HasValue ? pagesize.Value : defaultPageSize);
			
			if (Session["LoginUser"] != null)
			{
				User user = (User)Session["LoginUser"];
				comboTrans = 
					db.ComboTransactions.Where(x => x.UserID == user.UserID).OrderByDescending(x => x.TimeStamp).ToPagedList(currentPage, pagesize.HasValue ? pagesize.Value : defaultPageSize);

			}

			return View(comboTrans);
		}

		public ActionResult Create()
		{
			ComboTransactionModel ctModel = new ComboTransactionModel();

			if (Session["LoginUser"] != null)
			{
				int userid = ((User)Session["LoginUser"]).UserID;
				ctModel.TimeStamp = DateTime.Now;
				ctModel.Benefit = 0;
				ctModel.Expense = 0;
				ctModel.Income = 0;
				ctModel.IndividualTransactions = new List<IndividualProductTransaction>();
				ctModel.IsPaid = false;
				ctModel.PaidDateTime = null;
				ctModel.TransPortPrice = 0;
				ctModel.UserID = userid;
				ViewBag.Customers = new SelectList(db.Customers.OrderByDescending(x => x.Name), "CustomerID", "Name", ctModel.CustomerID);
			}

			return View(ctModel);
		}

		public ActionResult Edit(int id)
		{
			ComboTransactionModel ctranVM = new ComboTransactionModel();
			ComboTransaction ctran = db.ComboTransactions.Find(id);
			ctranVM.ComboTransactionID = ctran.ComboTransactionID;
			ctranVM.CustomerID = ctran.CustomerID;
			ctranVM.Expense = ctran.Expense;
			ctranVM.Income = ctran.Income;
			ctranVM.IndividualTransactions = db.IndividualProductTransactions.Where(x => x.ComboTransactionID == ctran.ComboTransactionID).OrderByDescending(x => x.IndividualProductTransactionID).ToList();
			ctranVM.IsPaid = ctran.IsPaid;
			ctranVM.PaidDateTime = ctran.PaidDateTime;
			ctranVM.TimeStamp = ctran.TimeStamp;
			ctranVM.TransPortPrice = ctran.TransPortPrice;
			ctranVM.UserID = ctran.UserID;
			ctranVM.Weight = ctran.Weight;
			ViewBag.Customers = new SelectList(db.Customers.OrderByDescending(x => x.Name), "CustomerID", "Name", ctran.CustomerID);
			return View(ctranVM);
		}

		//[HttpPost]
		//public ActionResult Edit(ComboTransactionModel ctranVM)
		//{
		//	if (ModelState.IsValid)
		//	{

		//	}
		//}
	}
}