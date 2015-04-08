namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTransaction : DbMigration
    {
        public override void Up()
        {
            
            
           
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.CustomerID);
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Transactions", "CustomerID", "dbo.Customers");
           
            DropIndex("dbo.Transactions", new[] { "CustomerID" });
            DropIndex("dbo.Transactions", new[] { "ProductID" });
          
            DropTable("dbo.Transactions");
           
        }
    }
}
