namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductIDinIndividualTransactionProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IndividualProductTransactions", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.IndividualProductTransactions", new[] { "Product_ProductID" });
            RenameColumn(table: "dbo.IndividualProductTransactions", name: "Product_ProductID", newName: "ProductID");
            AlterColumn("dbo.IndividualProductTransactions", "ProductID", c => c.Int(nullable: false));
            CreateIndex("dbo.IndividualProductTransactions", "ProductID");
            AddForeignKey("dbo.IndividualProductTransactions", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IndividualProductTransactions", "ProductID", "dbo.Products");
            DropIndex("dbo.IndividualProductTransactions", new[] { "ProductID" });
            AlterColumn("dbo.IndividualProductTransactions", "ProductID", c => c.Int());
            RenameColumn(table: "dbo.IndividualProductTransactions", name: "ProductID", newName: "Product_ProductID");
            CreateIndex("dbo.IndividualProductTransactions", "Product_ProductID");
            AddForeignKey("dbo.IndividualProductTransactions", "Product_ProductID", "dbo.Products", "ProductID");
        }
    }
}
