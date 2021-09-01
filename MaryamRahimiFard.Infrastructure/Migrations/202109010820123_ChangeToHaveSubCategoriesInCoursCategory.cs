namespace MaryamRahimiFard.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeToHaveSubCategoriesInCoursCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CourseCategories", "ParentId", c => c.Int());
            CreateIndex("dbo.CourseCategories", "ParentId");
            AddForeignKey("dbo.CourseCategories", "ParentId", "dbo.CourseCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseCategories", "ParentId", "dbo.CourseCategories");
            DropIndex("dbo.CourseCategories", new[] { "ParentId" });
            DropColumn("dbo.CourseCategories", "ParentId");
        }
    }
}
