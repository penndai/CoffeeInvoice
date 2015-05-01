namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setPaidDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "PaidDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transactions", "PaidDateTime", c => c.DateTime(nullable: false));
        }
    }
}
