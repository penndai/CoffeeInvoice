
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using System.Text;
using CoffeeInvoice.Models.ViewModel;
using Microsoft.International.Converters.PinYinConverter;

namespace CoffeeInvoice.Controllers
{
   [Authorize]
    public class CustomerController : Controller
    {
        private int defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);
        private InvoiceDB db = new InvoiceDB();
	
        public ViewResultBase Search(string q, int? page)
        {
			List<Customer> customers = db.Customers.OrderBy(c => c.Name).ToList();
			List<CustomerWithPinYinName> customer_PinYin = new List<CustomerWithPinYinName>();
			if (Session["LoginUser"] != null)
			{
				foreach (Customer cus in customers)
				{
					CustomerWithPinYinName cusPY = new CustomerWithPinYinName();
					cusPY.Address = cus.Address;
					cusPY.City = cus.City;
					cusPY.CompanyNumber = cus.CompanyNumber;
					cusPY.ContactPerson = cus.ContactPerson;
					cusPY.CP = cus.CP;
					cusPY.CustomerID = cus.CustomerID;
					cusPY.Email = cus.Email;
					cusPY.Fax = cus.Fax;
					//cusPY.Invoices = cus.Invoices;
					cusPY.Name = cus.Name;
					cusPY.Notes = cus.Notes;
					cusPY.Phone1 = cus.Phone1;
					cusPY.Phone2 = cus.Phone2;
					cusPY.User = cus.User;
					cusPY.UserID = cus.UserID;
					
					string r = string.Empty;
					foreach (char obj in cusPY.Name)
					{
						try
						{
							ChineseChar chineseChar = new ChineseChar(obj);
							string t = chineseChar.Pinyins[0].ToString();
							r += t.Substring(0, 1);
						}
						catch
						{
							r += obj.ToString();
						}
					}
										
					cusPY.PinYin = r;
					customer_PinYin.Add(cusPY);
				}

				if (q.Length == 1)//alphabetical search, first letter
				{
					ViewBag.LetraAlfabetica = q;

					customer_PinYin = customer_PinYin.Where(c => c.PinYin.StartsWith(q)).ToList();
				}
				else if (q.Length > 1)
				{
					//normal search
					customer_PinYin = customer_PinYin.Where(c => c.PinYin.IndexOf(q) > -1).ToList();
				}
			}

            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var customersListPaged = customer_PinYin.OrderBy(i => i.Name).ToPagedList(currentPageIndex, defaultPageSize);

			if (Request.IsAjaxRequest())
				return PartialView("Index", customersListPaged);
			else
                return View("Index", customersListPaged);
        }

        /*END CUSTOM*/
        

        // GET: /Customer/

        public ViewResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var customers = db.Customers.OrderBy(c => c.Name).ToList();
			List<CustomerWithPinYinName> customer_PinYin = new List<CustomerWithPinYinName>();

			if (Session["LoginUser"] != null)
			{
				User user = (User)Session["LoginUser"];
				customers = customers.Where(x => x.UserID == user.UserID).ToList();
				
				foreach (Customer cus in customers)
				{
					CustomerWithPinYinName cusPY = new CustomerWithPinYinName();
					cusPY.Address = cus.Address;
					cusPY.City = cus.City;
					cusPY.CompanyNumber = cus.CompanyNumber;
					cusPY.ContactPerson = cus.ContactPerson;
					cusPY.CP = cus.CP;
					cusPY.CustomerID = cus.CustomerID;
					cusPY.Email = cus.Email;
					cusPY.Fax = cus.Fax;
					cusPY.Invoices = cus.Invoices;
					cusPY.Name = cus.Name;
					cusPY.Notes = cus.Notes;
					cusPY.Phone1 = cus.Phone1;
					cusPY.Phone2 = cus.Phone2;
					cusPY.User = cus.User;
					cusPY.UserID = cus.UserID;					

					string r = string.Empty;
					foreach (char obj in cusPY.Name)
					{
						try
						{
							ChineseChar chineseChar = new ChineseChar(obj);
							string t = chineseChar.Pinyins[0].ToString();
							r += t.Substring(0, 1);
						}
						catch
						{
							r += obj.ToString();
						}
					}

					cusPY.PinYin = r;
					
					customer_PinYin.Add(cusPY);
				}
			}

			return View(customer_PinYin.ToPagedList(currentPageIndex, defaultPageSize));
        }

		public PartialViewResult MostTop10Customers(int top)
		{
			List<MostTopCustomer> topCustomers = new List<MostTopCustomer>();
			if (Session["LoginUser"] != null)
			{
				User user = (User)Session["LoginUser"];
				var singleTransmostPopularProds =
					db.Transactions.Where(x => x.UserID == user.UserID).GroupBy(x => new { CustomerID = x.CustomerID, CustomerName = x.Customer.Name }).Select(x => new MostTopCustomer { CustomerID = x.Key.CustomerID, CustomerName = x.Key.CustomerName, TotalPaid = x.Sum(y => y.Product.CNYSellPrice.Value) }).ToList();

				List<MostTopCustomer> mostPopularProds = GetComboTransactionTopCustomer(user.UserID);
				topCustomers.AddRange(singleTransmostPopularProds);
				foreach (MostTopCustomer cus in mostPopularProds)
				{
					MostTopCustomer mtc = singleTransmostPopularProds.Where(x => x.CustomerID == cus.CustomerID).FirstOrDefault();
					if (mtc != null)
					{
						mtc.TotalPaid += cus.TotalPaid;
					}
					else
					{
						topCustomers.Add(cus);
					}
				}

				topCustomers = topCustomers.OrderByDescending(x => x.TotalPaid).Take(top).ToList();
			}
			return PartialView("TopCustomersPartial", topCustomers);
		}
  
		private List<MostTopCustomer> GetComboTransactionTopCustomer(int userID)
		{
			List<MostTopCustomer> rtn =
				db.ComboTransactions.Where(x=>x.UserID==userID).GroupBy(
				x => new { CustomerID = x.CustomerID, CustomerName = x.Customer.Name }).
					Select(
					x => new MostTopCustomer { CustomerID = x.Key.CustomerID, CustomerName = x.Key.CustomerName, TotalPaid = x.Sum(y => y.Income) }).ToList();

			return rtn;
		}

		public ActionResult Copy(int id)
		{
			Customer customer = db.Customers.Find(id);
			Customer target = new Customer();

			if (customer != null)
			{
				target.Address = customer.Address;
				target.City = customer.City;
				target.CompanyNumber = customer.CompanyNumber;
				target.ContactPerson = customer.ContactPerson;
				target.CP = customer.CP;
				target.Email = customer.Email;
				target.Fax = customer.Fax;
				target.Name = customer.Name;
				target.Notes = customer.Notes;
				target.Phone1 = customer.Phone1;
				target.Phone2 = customer.Phone2;
				target.UserID = customer.UserID;

				db.Customers.Add(target);
				db.SaveChanges();
			}

			return RedirectToAction("Index");
		}
		//
        // GET: /Customer/Details/5

        public ViewResult Details(int id)
        {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            return PartialView();
        } 

        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
				if (Session["LoginUser"] != null)
				{
					customer.UserID = ((User)Session["LoginUser"]).UserID;
				}

                db.Customers.Add(customer);
                db.SaveChanges();
                //return list of customers as it is ajax request
				return PartialView("CustomerListPartial", db.Customers.Where(x => x.UserID == customer.UserID).OrderBy(c => c.Name).ToPagedList(0, defaultPageSize));
                //return RedirectToAction("Index");  
            }
            this.Response.StatusCode = 400;
            return PartialView(customer);
        }
        
        //
        // GET: /Customer/Edit/5
 
        public ActionResult Edit(int id)
        {
            Customer customer = db.Customers.Find(id);
            return PartialView(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
				customer.UserID = ((User)Session["LoginUser"]).UserID;
                db.SaveChanges();
                //return RedirectToAction("Index");
                //return list of customers as it is ajax request
                return PartialView("CustomerListPartial", db.Customers.Where(x=>x.UserID == customer.UserID).OrderBy(c => c.Name).ToPagedList(0, defaultPageSize));
            }
            this.Response.StatusCode = 400;
            return PartialView(customer);
        }

        //
        // GET: /Customer/Delete/5
 
        public ActionResult Delete(int id)
        {
            Customer customer = db.Customers.Find(id);
            return View(customer);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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