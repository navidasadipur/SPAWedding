namespace SPAWedding.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UncommentDescriptionInOfferAndUncommentCompanyNameAndCountryAndCityAndDescription : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Offers", "Description", c => c.String());
            //AddColumn("dbo.Invoices", "CompanyName", c => c.String());
            //AddColumn("dbo.Invoices", "Country", c => c.String());
            //AddColumn("dbo.Invoices", "City", c => c.String());
            //AddColumn("dbo.Invoices", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "Description");
            DropColumn("dbo.Invoices", "City");
            DropColumn("dbo.Invoices", "Country");
            DropColumn("dbo.Invoices", "CompanyName");
            DropColumn("dbo.Offers", "Description");
        }
    }
}
