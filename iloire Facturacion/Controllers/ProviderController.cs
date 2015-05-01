﻿/*
	Iván Loire - www.iloire.com
	Please readme README file for license terms.

	ASP.NET MVC3 ACME Invocing app (demo app for training purposes)
*/
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
    [Authorize]
    public class ProviderController : Controller
    {
        private InvoiceDB db = new InvoiceDB();
        private int defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);

        /*CUSTOM*/
        public ViewResultBase Search(string q, int? page)
        {
            IQueryable<Provider> providers = db.Providers;

            if (q.Length == 1)
            {
                ViewBag.LetraAlfabetica = q;
                //alfabet, first letter
                providers = (from c in db.Providers
                             where c.Name.StartsWith(q)
                             select c);
            }
            else if (q.Length>1)
            {
                //normal search
                providers = (from c in db.Providers
                             where c.Name.IndexOf(q) > -1
                             select c);

            }

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var providersListPaged = providers.OrderBy(i => i.Name).ToPagedList(currentPageIndex, defaultPageSize);

            if (Request.IsAjaxRequest())
                return PartialView("Index", providersListPaged);
            else
                return View("Index", providersListPaged);
        }
        /*END CUSTOM*/

        //
        // GET: /Provider/

        public ViewResult Index(int? page)
        {
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var providersListPaged = new List<Provider>().ToPagedList(currentPageIndex, defaultPageSize);
			if (Session["LoginUser"] != null)
			{
				int UserID = ((User)Session["LoginUser"]).UserID;
			
				var providers = db.Providers.Where(i=>i.UserID == UserID).OrderBy(i => i.Name).ToList();
            
				providersListPaged = providers.ToPagedList(currentPageIndex, defaultPageSize);				
			}

			return View(providersListPaged);
        }

        //
        // GET: /Provider/Details/5

        public ViewResult Details(int id)
        {
            Provider provider = db.Providers.Find(id);
            return View(provider);
        }

        //
        // GET: /Provider/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Provider/Create

        [HttpPost]
        public ActionResult Create(Provider provider)
        {
            if (ModelState.IsValid)
            {
				if (Session["LoginUser"] != null)
				{
					provider.UserID = ((User)Session["LoginUser"]).UserID;					
				}

                db.Providers.Add(provider);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(provider);
        }

		public ActionResult Copy(int id)
		{
			Provider newProvider = new Provider();
			Provider provider = db.Providers.Find(id);
			newProvider.Name = provider.Name;
			newProvider.Phone1 = provider.Phone1;
			newProvider.Phone2 = provider.Phone2;
			newProvider.Purchases = provider.Purchases;
			newProvider.Fax = provider.Fax;
			newProvider.Email = provider.Email;
			newProvider.Address = provider.Address;
			newProvider.City = provider.City;
			newProvider.CompanyNumber = provider.CompanyNumber;
			newProvider.UserID = provider.UserID;
			db.Providers.Add(newProvider);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
        //
        // GET: /Provider/Edit/5
 
        public ActionResult Edit(int id)
        {
            Provider provider = db.Providers.Find(id);
            return View(provider);
        }

        //
        // POST: /Provider/Edit/5

        [HttpPost]
        public ActionResult Edit(Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(provider);
        }

        //
        // GET: /Provider/Delete/5
 
        public ActionResult Delete(int id)
        {
            Provider provider = db.Providers.Find(id);
            return View(provider);
        }

        //
        // POST: /Provider/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Provider provider = db.Providers.Find(id);
            db.Providers.Remove(provider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}