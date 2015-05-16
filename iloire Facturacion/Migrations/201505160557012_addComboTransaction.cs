namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addComboTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComboTransactions",
                c => new
                    {
                        ComboTransactionID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        PaidDateTime = c.DateTime(),
                        TimeStamp = c.DateTime(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransPortPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Expense = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Income = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Benefit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ComboTransactionID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.IndividualProductTransactions",
                c => new
                    {
                        IndividualProductTransactionID = c.Int(nullable: false, identity: true),
                        ComboTransactionID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CNYPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CNYSellPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Number = c.Int(nullable: false),
                        Product_ProductID = c.Int(),
                    })
                .PrimaryKey(t => t.IndividualProductTransactionID)
                .ForeignKey("dbo.ComboTransactions", t => t.ComboTransactionID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ProductID)
                .Index(t => t.ComboTransactionID)
                .Index(t => t.Product_ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IndividualProductTransactions", "Product_ProductID", "dbo.Products");
            DropForeignKey("dbo.IndividualProductTransactions", "ComboTransactionID", "dbo.ComboTransactions");
            DropForeignKey("dbo.ComboTransactions", "UserID", "dbo.Users");
            DropForeignKey("dbo.ComboTransactions", "CustomerID", "dbo.Customers");
            DropIndex("dbo.IndividualProductTransactions", new[] { "Product_ProductID" });
            DropIndex("dbo.IndividualProductTransactions", new[] { "ComboTransactionID" });
            DropIndex("dbo.ComboTransactions", new[] { "CustomerID" });
            DropIndex("dbo.ComboTransactions", new[] { "UserID" });
            DropTable("dbo.IndividualProductTransactions");
            DropTable("dbo.ComboTransactions");
        }
    }
}
