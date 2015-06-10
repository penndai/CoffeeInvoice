using CoffeeInvoice.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CoffeeInvoice.CustomBinder;

namespace CoffeeInvoice
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
			
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			

            routes.MapRoute(
            "Invoice",
            "Invoice/{action}/{id}", 
            new { controller = "Invoice", action = "Index", id = UrlParameter.Optional, proposal = false }, // Valores predeterminados de parámetro
			new[] { "CoffeeInvoice.Controllers" }
            );


            routes.MapRoute(
              "Proposal",
              "Proposal/{action}/{id}", 
              new { controller = "Invoice", action = "Index", id = UrlParameter.Optional, proposal=true }, // Valores predeterminados de parámetro
			  new[] { "CoffeeInvoice.Controllers" }
          );

			routes.MapRoute(
			 "Localization", // Route name
			 "{lang}/{controller}/{action}/{id}", // URL with parameters
			 new { lang="zh",controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
			 new[] { "CoffeeInvoice.Controllers" }
			);

            routes.MapRoute(
                "Default", 
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, 
				new[] {"CoffeeInvoice.Controllers"}
            );

        }

        protected void Application_Start()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["DropDatabaseOnChange"] == "1")
            {
                //Set initializer to populate data on database creation
                System.Data.Entity.Database.SetInitializer(new EntitiesContextInitializer());
            }

			
            AreaRegistration.RegisterAllAreas();
			ModelBinders.Binders.Add(typeof(Nullable<DateTime>), new DateTimeBinder());
			//ModelBinders.Binders.Add(typeof(string), new CurrencyStringModelBinder());
			GlobalConfiguration.Configure(WebApiConfig.Register);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}