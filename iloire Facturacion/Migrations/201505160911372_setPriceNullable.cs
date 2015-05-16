namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setPriceNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IndividualProductTransactions", "CNYPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.IndividualProductTransactions", "CNYSellPrice", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IndividualProductTransactions", "CNYSellPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.IndividualProductTransactions", "CNYPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
