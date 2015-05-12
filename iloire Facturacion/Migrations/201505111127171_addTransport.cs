namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTransport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemConstants",
                c => new
                    {
                        ConstantID = c.Int(nullable: false, identity: true),
                        Constant = c.String(),
                        ConstrantValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ConstantID);
            
            AddColumn("dbo.Transactions", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Weight");
            DropTable("dbo.SystemConstants");
        }
    }
}
