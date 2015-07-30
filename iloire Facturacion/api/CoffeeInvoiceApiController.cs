using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CoffeeInvoice.api
{
	[Authorize]
    public class CoffeeInvoiceApiController : ApiController
    {
		private readonly InvoiceDB dbContext = new InvoiceDB();

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.dbContext.Dispose();
			}

			base.Dispose(disposing);
		}

		private async Task<Customer> SearchCustomer(int id)
		{
			return await this.dbContext.Customers.FindAsync(CancellationToken.None, id);
		}

		[ResponseType(typeof(Customer))]
		public async Task<IHttpActionResult> GetCustomer(int id)
		{
			Customer data = await SearchCustomer(id);
			if (data == null)
			{
				return this.NotFound();
			}

			return Ok(data);
		}
    }
}
