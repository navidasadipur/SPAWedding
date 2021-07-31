namespace SPAWedding.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyCourseModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "StartDate", c => c.DateTime());
            AddColumn("dbo.Courses", "EndDate", c => c.DateTime());
            AddColumn("dbo.Courses", "DurationInHours", c => c.Long(nullable: false));
            AddColumn("dbo.Courses", "SessionsNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "SessionsNumber");
            DropColumn("dbo.Courses", "DurationInHours");
            DropColumn("dbo.Courses", "EndDate");
            DropColumn("dbo.Courses", "StartDate");
        }
    }
}
