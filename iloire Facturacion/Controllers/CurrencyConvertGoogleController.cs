using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace CoffeeInvoice.Controllers
{
    public class CurrencyConvertYahooController : ApiController
    {
        // GET api/currencyconvertgoogle
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

		[Route("api/ConvertCurrency/{inCurrency}/{outCurrency}/{invalue}")]
		[HttpGet]
		public decimal ConvertCurrency(string inCurrency, string outCurrency, int invalue)
		{
			string yahooURL = System.Configuration.ConfigurationManager.AppSettings["yahooURL"];			
			WebClient wclient = new WebClient();
			string url = string.Format(yahooURL, inCurrency.ToUpper(), outCurrency.ToUpper(), invalue);
			string response = wclient.DownloadString(url);


			string[] val = Regex.Split(response, ",");

			decimal rate = Convert.ToDecimal(Math.Round(float.Parse(val[1], System.Globalization.CultureInfo.InvariantCulture) + 0.2, 2));			
			return rate * invalue;
		}

        // GET api/currencyconvertgoogle/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/currencyconvertgoogle
        public void Post([FromBody]string value)
        {
        }

        // PUT api/currencyconvertgoogle/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/currencyconvertgoogle/5
        public void Delete(int id)
        {
        }
    }
}
