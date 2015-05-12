using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using System.Data.Entity;

namespace CoffeeInvoice.Controllers
{
	public class SystemConstantController:Controller
	{
		private int defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);
		private InvoiceDB db = new InvoiceDB();

		public ViewResult Index(int? page)
		{
			List<SystemConstants> systemconstants = new List<SystemConstants>();
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

			if (Session["LoginUser"] != null)
			{				
				User user = (User)Session["LoginUser"];
				systemconstants = db.SystemConstants.Where(x => x.UserID == user.UserID).OrderBy(x => x.Constant).ToList();				
			}

			return View(systemconstants.ToPagedList(currentPageIndex, defaultPageSize));
		}

		public ViewResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(SystemConstants data)
		{
			if (ModelState.IsValid)
			{
				if (Session["LoginUser"] != null)
				{
					data.UserID = ((User)Session["LoginUser"]).UserID;
				}

				db.SystemConstants.Add(data);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(data);
		}

		public ActionResult Edit(int id)
		{
			SystemConstants data = db.SystemConstants.Find(id);
			return View(data);
		}

		[HttpPost]
		public ActionResult Edit(SystemConstants data)
		{
			if (ModelState.IsValid)
			{
				db.Entry(data).State = EntityState.Modified;
				data.UserID = ((User)Session["LoginUser"]).UserID;
				db.SaveChanges();
				return RedirectToAction("Index");				
			}
			
			return View(data);
		}
	}
}