namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNumberTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Number");
        }
    }
}
