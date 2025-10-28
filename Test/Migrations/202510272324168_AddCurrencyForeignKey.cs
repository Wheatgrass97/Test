namespace Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrencyForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Transaction", "CurrencyCode");
            AddForeignKey("dbo.Transaction", "CurrencyCode", "dbo.Currency", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transaction", "CurrencyCode", "dbo.Currency");
            DropIndex("dbo.Transaction", new[] { "CurrencyCode" });
        }
    }
}
