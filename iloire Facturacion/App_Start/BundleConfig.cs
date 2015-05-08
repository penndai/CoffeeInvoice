using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CoffeeInvoice.App_Start
{
	public static class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/scripts/globalization").Include("~/Scripts/globalize*","~/Scripts/globalize/cultures/globalize*"));
			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.min", "~/Content/Site.css", "~/Content/justfied-nav.css"));
		}
	}
}