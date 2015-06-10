using CoffeeInvoice.Controllers.AttributeExtend;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeInvoice.Controllers
{
    [Authorize]
	[Localization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }	
    }
}
