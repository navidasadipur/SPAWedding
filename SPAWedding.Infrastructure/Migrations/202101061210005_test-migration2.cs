namespace MaryamRahimiFard.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testmigration2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ShortTitle", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ShortTitle", c => c.String(nullable: false, maxLength: 300));
        }
    }
}
