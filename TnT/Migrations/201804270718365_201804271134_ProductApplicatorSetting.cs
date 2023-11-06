namespace TnT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201804271134_ProductApplicatorSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductApplicatorSetting",
                c => new
                    {
                        ServerPAID = c.Decimal(nullable: false, precision: 18, scale: 2),
                        S1 = c.Single(nullable: false),
                        S2 = c.Single(nullable: false),
                        S3 = c.Single(nullable: false),
                        S4 = c.Single(nullable: false),
                        S5 = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ServerPAID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductApplicatorSetting");
        }
    }
}
