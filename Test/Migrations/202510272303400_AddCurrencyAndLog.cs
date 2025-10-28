namespace Test.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrencyAndLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.ErrorLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ErrorMessage = c.String(),
                        Parameters = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AlterColumn("dbo.Transaction", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transaction", "Status", c => c.String());
            DropTable("dbo.ErrorLog");
            DropTable("dbo.Currency");
        }
    }
}
