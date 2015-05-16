namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomerIDinIndividualProductTrans : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IndividualProductTransactions", "CustomerID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IndividualProductTransactions", "CustomerID");
        }
    }
}
