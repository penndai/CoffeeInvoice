namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserID : DbMigration
    {
		public override void Up()
		{
			AddColumn("dbo.Customers", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.Customers", "UserID", "dbo.Users");

			AddColumn("dbo.Invoices", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.Invoices", "UserID", "dbo.Users");

			AddColumn("dbo.InvoiceDetails", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.InvoiceDetails", "UserID", "dbo.Users");

			AddColumn("dbo.Products", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.Products", "UserID", "dbo.Users");

			AddColumn("dbo.Providers", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.Providers", "UserID", "dbo.Users");

			AddColumn("dbo.Purchases", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.Purchases", "UserID", "dbo.Users");

			AddColumn("dbo.PurchaseTypes", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.PurchaseTypes", "UserID", "dbo.Users");

			AddColumn("dbo.PurchaseProducts", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.PurchaseProducts", "UserID", "dbo.Users");

			AddColumn("dbo.Transactions", "UserID", c => c.Int(nullable: true));
			AddForeignKey("dbo.Transactions", "UserID", "dbo.Users");

		}
    }
}
