namespace MaryamRahimiFard.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeDurationAndSessionsNumberNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "DurationInHours", c => c.Long());
            AlterColumn("dbo.Courses", "SessionsNumber", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "SessionsNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Courses", "DurationInHours", c => c.Long(nullable: false));
        }
    }
}
