namespace SPAWedding.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescripotionToOfferModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "Description");
        }
    }
}
