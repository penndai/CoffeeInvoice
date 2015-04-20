namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtimestampProcedure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TimeStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "TimeStamp");
        }
    }
}
