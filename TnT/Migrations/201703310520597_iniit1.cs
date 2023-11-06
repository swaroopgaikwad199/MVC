namespace TnT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iniit1 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.X_TracelinkUIDStore",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            TLRequestId = c.Int(nullable: false),
            //            SerialNo = c.String(),
            //            IsUsed = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.X_TracelinkUIDStore");
        }
    }
}
