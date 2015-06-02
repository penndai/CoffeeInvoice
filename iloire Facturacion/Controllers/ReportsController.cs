using System.Collections.Generic;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CoffeeInvoice.Models.ViewModel;

namespace CoffeeInvoice.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private InvoiceDB db = new InvoiceDB();

		private TSummary GetTransSummary(DateTime fromDate, DateTime toDate)
		{
			TSummary ts = new TSummary();
			ts.From = fromDate;
			ts.To = toDate;
			if (Session["LoginUser"] != null)
			{
				User user = (User)Session["LoginUser"];				

				ts.SingleTransactions = db.Transactions.Where(x =>x.UserID == user.UserID && x.TimeStamp >= fromDate && x.TimeStamp <= toDate && x.IsPaid).ToList();
				ts.ComboTransactions = db.ComboTransactions.Where(x => x.UserID == user.UserID && x.TimeStamp >= fromDate && x.TimeStamp <= toDate && x.IsPaid).ToList();

				ts.Expense = ts.SingleTransactions.Sum(x => x.Expense)+ts.ComboTransactions.Sum(x=>x.Expense);
				ts.Income = ts.SingleTransactions.Sum(x => x.Income)+ts.ComboTransactions.Sum(x=>x.Income);
				ts.Benefit = ts.SingleTransactions.Sum(x => x.Number * x.Benefit) + ts.ComboTransactions.Sum(x=>x.Benefit);
			}

			return ts;
		}

        private Summary GetSummary(DateTime fromDate, DateTime toDate)
        {
            Summary s = new Summary();

            s.From = fromDate;
            s.To = toDate;

            s.Invoices = (from i in db.Invoices.Include("InvoiceDetails")
                            where i.TimeStamp >= fromDate && i.TimeStamp <= toDate
                            select i).ToList().Where(i=>!i.IsProposal).ToList();

            s.Purchases = (from p in db.Purchases
                             where p.TimeStamp >= fromDate && p.TimeStamp <= toDate
                             select p).ToList();


            s.NetExpense = s.Purchases.Sum(i => i.SubTotal);
            s.GrossExpense = s.Purchases.Sum(i => i.TotalWithVAT);
            
            s.NetIncome = s.Invoices.Sum(i => i.NetTotal);
            s.GrossIncome = s.Invoices.Sum(i=>i.TotalWithVAT);

            s.VATReceived = s.Invoices.Sum(i => i.VATAmount);
            s.VATPaid = s.Purchases.Sum(i => i.VAT);
            s.VATBalance = s.Invoices.Sum(i => i.VATAmount) - s.Purchases.Sum(p => p.VATAmount);

            s.AmountPaid = s.Invoices.Where(i => i.Paid).Sum(i => i.TotalToPay);

            s.AdvancePaymentTaxPaid = s.Invoices.Sum(i => i.AdvancePaymentTaxAmount);
            return s;
        }

		public ActionResult GetLatestTransaction()
		{
			List<CoffeeInvoice.Models.ViewModel.LatestTransaction> latestTrans = new List<Models.ViewModel.LatestTransaction>();
			if (Session["LoginUser"] != null)
			{
				int userid = ((User)Session["LoginUser"]).UserID;
				List<Transaction> latestSingleTrans =
					db.Transactions.Where(x => x.UserID == userid).OrderByDescending(x => x.TimeStamp).Take(10).ToList();

				List<ComboTransaction> latestComboTrans =
					db.ComboTransactions.Where(x => x.UserID == userid).OrderByDescending(x => x.TimeStamp).Take(10).ToList();

				latestTrans = ComboLatestTransaction(latestSingleTrans, latestComboTrans);
			}

			return PartialView("LatestTransactionListPartial", latestTrans);
		}
  
		private List<LatestTransaction> ComboLatestTransaction(List<Transaction> latestSingleTrans, List<ComboTransaction> latestComboTrans)
		{
			List<LatestTransaction> transVM = new List<LatestTransaction>();
			foreach (Transaction single in latestSingleTrans)
			{
				Models.ViewModel.LatestTransaction tranVM = new Models.ViewModel.LatestTransaction();
				tranVM.Benefit = single.Benefit;
				tranVM.CustomerID = single.CustomerID;
				tranVM.Customer = single.Customer.Name;
				tranVM.ID = single.TransactionID;
				tranVM.Product = single.Product.ProductName;
				tranVM.ProductID = single.ProductID;
				tranVM.TotalPay = single.Income * single.Number;
				tranVM.TransactionDate = single.TimeStamp;
				tranVM.IsPaid = single.IsPaid;
				tranVM.PaidDate = single.PaidDateTime;
				transVM.Add(tranVM);
			}

			foreach (ComboTransaction combo in latestComboTrans)
			{
				Models.ViewModel.LatestTransaction tranVM = new Models.ViewModel.LatestTransaction();
				tranVM.ID = combo.ComboTransactionID;
				db.IndividualProductTransactions.Where(
					x => x.ComboTransactionID == combo.ComboTransactionID).ToList().ForEach(x =>
					{
						tranVM.Product = string.Format("{0}\n{1}", tranVM.Product, x.Product.ProductName);
					});

				tranVM.ProductID = -1;
				tranVM.Customer = combo.Customer.Name;
				tranVM.CustomerID = combo.CustomerID;
				tranVM.TransactionDate = combo.TimeStamp.Date;
				tranVM.TotalPay = combo.Income;
				tranVM.Benefit = combo.Benefit;
				tranVM.IsPaid = combo.IsPaid;
				tranVM.PaidDate = combo.PaidDateTime;
				transVM.Add(tranVM);
			}
			return transVM;
		}

		public ActionResult GetOverdueTransaction()
		{
			var overdueTrans = new System.Collections.Generic.List<CoffeeInvoice.Models.ViewModel.OverdueTransactionVM>();

			if (Session["LoginUser"] != null)
			{
				int userid = ((User)Session["LoginUser"]).UserID;

				// Get single transaction 
				System.Collections.Generic.List<Transaction> singleTrans =
				db.Transactions.Where(x=>x.UserID == userid && !x.IsPaid && DbFunctions.AddDays(x.TimeStamp, 3) <= DateTime.Now).OrderBy(x => x.TimeStamp).ToList();
				// Get combo transaction
				System.Collections.Generic.List<ComboTransaction> comboTrans =
					db.ComboTransactions.Where(x => x.UserID == userid && !x.IsPaid && DbFunctions.AddDays(x.TimeStamp, 3) <= DateTime.Now).OrderBy(x => x.TimeStamp).ToList();

				overdueTrans = CombineTrans(singleTrans, comboTrans);
			}

			return PartialView("OverdueTransactionListPartial", overdueTrans);
		}

		private System.Collections.Generic.List<Models.ViewModel.OverdueTransactionVM> CombineTrans(System.Collections.Generic.List<Transaction> singleTrans, System.Collections.Generic.List<ComboTransaction> comboTrans)
		{
			System.Collections.Generic.List<Models.ViewModel.OverdueTransactionVM> transVM = new System.Collections.Generic.List<Models.ViewModel.OverdueTransactionVM>();
			foreach (Transaction single in singleTrans)
			{
				Models.ViewModel.OverdueTransactionVM tranVM = new Models.ViewModel.OverdueTransactionVM();
				tranVM.Benefit = single.Benefit;
				tranVM.CustomerID = single.CustomerID;
				tranVM.Customer = single.Customer.Name;
				tranVM.ID = single.TransactionID;
				tranVM.Product = single.Product.ProductName;
				tranVM.ProductID = single.ProductID;
				tranVM.TotalPay = single.Income*single.Number;
				tranVM.TransactionDate = single.TimeStamp;
				transVM.Add(tranVM);
			}

			foreach (ComboTransaction combo in comboTrans)
			{
				Models.ViewModel.OverdueTransactionVM tranVM = new Models.ViewModel.OverdueTransactionVM();
				tranVM.ID = combo.ComboTransactionID;
				db.IndividualProductTransactions.Where(
					x => x.ComboTransactionID == combo.ComboTransactionID).ToList().ForEach(x => {
						tranVM.Product = string.Format("{0}\n{1}", tranVM.Product,x.Product.ProductName);
					});
				
				tranVM.ProductID = -1;
				tranVM.Customer = combo.Customer.Name;
				tranVM.CustomerID = combo.CustomerID;
				tranVM.TransactionDate = combo.TimeStamp.Date;
				tranVM.TotalPay = combo.Income;
				tranVM.Benefit = combo.Benefit;
				transVM.Add(tranVM);
			}
			return transVM;
		}
        //
        // GET: /Reports/

        public ActionResult QuarterDetails(int? quarter, int? year) {
            return View();
        }

		public ActionResult TransQuarterDetails(int? quarter, int? year)
		{
			return View();
		}

		public ActionResult TransQuarterDetailsPartial(int? quarter, int? year)
		{
			DateTime start, end;

			if (!year.HasValue || !quarter.HasValue)
			{
				int q, y;
				TaxDateHelper.CalculateQuarter(DateTime.Now, out q, out y, out start, out end);
				quarter = q;
				year = y;
			}
			else
			{
				start = TaxDateHelper.GetStartDate(quarter.Value, year.Value);
				end = TaxDateHelper.GetEndDate(quarter.Value, year.Value);
			}

			TransQuarterSummary quarter_Summary = new TransQuarterSummary()
			{
				Year = year.Value,
				Month1 = GetTransSummary(start, start.AddMonths(1).AddDays(-1)),
				Month2 = GetTransSummary(start.AddMonths(1), start.AddMonths(2).AddDays(-1)),
				Month3 = GetTransSummary(start.AddMonths(2), end)
			};

			ViewBag.Year = year.Value;
			ViewBag.Quarter = quarter.Value;

			return PartialView("TransQuarterDetailsPartial", quarter_Summary);
		}

        public ActionResult QuarterDetailsPartial (int? quarter, int? year) {
            DateTime start, end;

            if (!year.HasValue || !quarter.HasValue)
            {
                int q, y;
                TaxDateHelper.CalculateQuarter(DateTime.Now, out q, out y, out start, out end);
                quarter = q;
                year = y;
            }
            else {
                start = TaxDateHelper.GetStartDate(quarter.Value, year.Value);
                end = TaxDateHelper.GetEndDate(quarter.Value, year.Value);
            }

            ViewBag.PurchaseTypes = (from p in db.PurchaseTypes
                               select p).ToList();

            QuarterSummary quarter_Summary = new QuarterSummary()
            {
                Year = year.Value,
                Month1 = GetSummary(start, start.AddMonths(1).AddDays(-1)),
                Month2 = GetSummary(start.AddMonths(1), start.AddMonths(2).AddDays(-1)),
                Month3 = GetSummary(start.AddMonths(2), end)
            };

            ViewBag.Year = year.Value;
            ViewBag.Quarter = quarter.Value;

            return PartialView("QuarterDetailsPartial", quarter_Summary);
        }

		public void TransPeriodSummary(DateTime fromDate, DateTime toDate)
		{
			PeriodSummary("PeriodTransSummary", fromDate, toDate);
		}

        public ActionResult PeriodSummary(string viewname, DateTime fromDate, DateTime toDate)
        {
			if (viewname == "PeriodSummary")
				return PartialView(viewname, GetSummary(fromDate, toDate));
			else
				return PartialView(viewname, GetTransSummary(fromDate, toDate));
        }

        public ActionResult ThisMonthSummary()
        {
            return PeriodSummary("PeriodSummary",
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), 
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                );
        }

		[OutputCache(Duration = 60)]
		public ActionResult TransYearSummary(int id)
		{
			TransYearSummary y = new TransYearSummary();
			y.Q1 = GetTransSummary(TaxDateHelper.GetStartDate(1, id), TaxDateHelper.GetStartDate(2, id).AddDays(-1));
			y.Q1.Year = id;

			y.Q2 = GetTransSummary(TaxDateHelper.GetStartDate(1, id), TaxDateHelper.GetStartDate(3, id).AddDays(-1));
			y.Q2.Year = id;
			
			y.Q3 = GetTransSummary(TaxDateHelper.GetStartDate(1, id), TaxDateHelper.GetStartDate(4, id).AddDays(-1));
			y.Q3.Year = id;

			y.Q4 = GetTransSummary(TaxDateHelper.GetStartDate(1, id), TaxDateHelper.GetStartDate(1, id).AddDays(1).AddDays(-1));
			y.Q4.Year = id;

			return PartialView("TransYearSummary", y);
		}

        [OutputCache(Duration=60)]
        public ActionResult YearSummary(int id)
        {
            YearSummary y=new YearSummary();
            y.Q1 = GetSummary(TaxDateHelper.GetStartDate(1, id), TaxDateHelper.GetStartDate(2, id).AddDays(-1));
            y.Q1.Year = id;
            
            y.Q2 = GetSummary(TaxDateHelper.GetStartDate(2, id), TaxDateHelper.GetStartDate(3, id).AddDays(-1));
            y.Q2.Year = id;
            
            y.Q3 = GetSummary(TaxDateHelper.GetStartDate(3, id), TaxDateHelper.GetStartDate(4, id).AddDays(-1));
            y.Q3.Year = id;
            
            y.Q4 = GetSummary(TaxDateHelper.GetStartDate(4, id), TaxDateHelper.GetStartDate(1, id).AddYears(1).AddDays(-1));
            y.Q4.Year = id;


            return PartialView("YearSummary", y);
        }

		[OutputCache(Duration = 60)]
		public ActionResult ThisQuarterTransSummary() 
		{
			int quarter = 0;
			int year = 0;
			DateTime start;
			DateTime end;

			TaxDateHelper.CalculateQuarter(DateTime.Now, out quarter, out year, out start, out end);
			return PeriodSummary("PeriodTransSummary", start,end);
		}

        [OutputCache(Duration = 60)]
        public ActionResult ThisQuarterSummary()
        {
            int quarter=0;
            int year=0;
            DateTime start;
            DateTime end;
            
            TaxDateHelper.CalculateQuarter(DateTime.Now, out quarter, out year, out start, out end);

            return PeriodSummary("PeriodSummary",start, end);
        }

        [OutputCache(Duration = 60)]
        public ActionResult QuarterSummary(DateTime date)
        {
            int quarter = 0;
            int year = 0;
            DateTime start;
            DateTime end;

            TaxDateHelper.CalculateQuarter(date, out quarter, out year, out start, out end);

            return PeriodSummary("PeriodSummary",start, end);
        }


        public ActionResult ByYear(int id) {
            return View(id);    
        }
    }
}
