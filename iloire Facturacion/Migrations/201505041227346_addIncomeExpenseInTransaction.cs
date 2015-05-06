namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIncomeExpenseInTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Expense", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "Income", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Transactions", "Benefit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Benefit");
            DropColumn("dbo.Transactions", "Income");
            DropColumn("dbo.Transactions", "Expense");
        }
    }
}
