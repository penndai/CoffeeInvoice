namespace CoffeeInvoice.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSomething : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComboTransactions", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddForeignKey("dbo.IndividualProductTransactions", "ComboTransactionID", "dbo.ComboTransactions", "ComboTransactionID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IndividualProductTransactions", "ComboTransactionID", "dbo.ComboTransactions");
            DropColumn("dbo.ComboTransactions", "Discriminator");
        }
    }
}
