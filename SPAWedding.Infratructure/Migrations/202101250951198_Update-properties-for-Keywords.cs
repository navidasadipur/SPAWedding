namespace SPAWedding.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatepropertiesforKeywords : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Keywords", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Keywords", c => c.String());
        }
    }
}
