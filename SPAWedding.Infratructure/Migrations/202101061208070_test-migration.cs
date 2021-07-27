namespace SPAWedding.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testmigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ShortTitle", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ShortTitle", c => c.String(maxLength: 300));
        }
    }
}
