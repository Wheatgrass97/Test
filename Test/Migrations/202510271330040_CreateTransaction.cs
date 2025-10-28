namespace Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 50),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyCode = c.String(maxLength: 3),
                        CreatedDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transaction");
        }
    }
}
