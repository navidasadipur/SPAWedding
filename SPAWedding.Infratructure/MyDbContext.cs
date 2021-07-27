using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using SPAWedding.Core.Models;

namespace SPAWedding.Infrastructure
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<ArticleHeadLine> ArticleHeadLines { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<StaticContentType> StaticContentTypes { get; set; }
        public DbSet<StaticContentDetail> StaticContentDetails { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<FaqGroup> FaqGroups { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductFeatureValue> ProductFeatureValues { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<ProductGroupBrand> ProductGroupBrands { get; set; }
        public DbSet<ProductGroupFeature> ProductGroupFeatures { get; set; }
        public DbSet<ProductMainFeature> ProductMainFeatures { get; set; }
        public DbSet<SimilarProduct> SimilarProducts { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<PerfumeNote> PerfumeNotes { get; set; }
        public DbSet<AdditionalFeature> AdditionalFeatures { get; set; }
        public DbSet<SubFeature> SubFeatures { get; set; }
        public DbSet<EPayment> EPayments { get; set; }
        public DbSet<PaymentAccount> PaymentAccounts { get; set; }
        public DbSet<EPaymentLog> EPaymentLogs { get; set; }
        public DbSet<GeoDivision> GeoDivisions { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<EmailSubscription> EmailSubscriptions { get; set; }
        public DbSet<SMSLog> SMSLogs { get; set; }
        public DbSet<OurTeam> OurTeams { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

    }
}
