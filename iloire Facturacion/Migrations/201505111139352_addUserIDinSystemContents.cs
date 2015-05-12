namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserIDinSystemContents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemConstants", "UserID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemConstants", "UserID");
        }
    }
}
