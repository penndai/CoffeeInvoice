namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductWeightPriceForTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransPortPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "TransPortPrice");
        }
    }
}
