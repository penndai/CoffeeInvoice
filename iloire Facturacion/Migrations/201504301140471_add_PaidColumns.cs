namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_PaidColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "IsPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Transactions", "PaidDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "PaidDateTime");
            DropColumn("dbo.Transactions", "IsPaid");
        }
    }
}
