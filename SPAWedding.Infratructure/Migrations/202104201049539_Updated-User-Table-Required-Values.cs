namespace SPAWedding.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedUserTableRequiredValues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 300));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 300));
        }
    }
}
