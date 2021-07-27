using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using SPAWedding.Core.Models;

namespace SPAWedding.Web.ViewModels
{
    public class DiscountFormViewModel
    {
        [DisplayName("عنوان تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید کمتر از 500 کارکتر باشد")]
        public string Title { get; set; }
        public int DiscountType { get; set; }
        [DisplayName("میزان تخفیف")]
        [Required(ErrorMessage = "لطفا میزان تخفیف را وارد کنید")]
        public long Amount { get; set; }
        public string DeadLine { get; set; }
        public List<int> BrandIds { get; set; }
        public List<int> ProductIds { get; set; }
        public List<int> ProductGroupIds { get; set; }
        public bool IsOffer { get; set; }
        public int? OfferId { get; set; }

        // Edit Props
        public string GroupIdentifier { get; set; }
        public List<Discount> PreviousDiscounts { get; set; }
    }

    public class TestimonialViewModel
    {
        public TestimonialViewModel()
        {
        }

        public TestimonialViewModel(Testimonial testimonial)
        {
            this.Name = testimonial.Name;
            this.Image = testimonial.Image;
            this.Description = testimonial.Description;
            this.PersianDate = new PersianDateTime(testimonial.InsertDate.Value).ToString("dddd d MMMM yyyy");
        }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string PersianDate { get; set; }
    }
    public class ContactUsViewModel
    {
        public StaticContentDetail Map { get; set; }
        public StaticContentDetail Address { get; set; }
        public StaticContentDetail Email { get; set; }
        public StaticContentDetail Phone { get; set; }
    }

    public class CartModel
    {
        public CartModel()
        {
        }

        public CartModel(string json)
        {
            JObject jObject = JObject.Parse(json);
            var jItems = jObject["CartItems"].ToList();
            var cartItems = new List<CartItemModel>();
            foreach (var item in jItems)
            {
                cartItems.Add(new CartItemModel()
                {
                    Id = Convert.ToInt32(item["Id"]),
                    ProductName = (string)item["ProductName"],
                    Image = (string)item["Image"],
                    Price = Convert.ToInt64(item["Price"]),
                    MainFeatureId = Convert.ToInt32(item["MainFeatureId"]),
                    Quantity = Convert.ToInt32(item["Quantity"])
                });
            }

            this.CartItems = cartItems;
            this.TotalPrice = Convert.ToInt64(jObject["TotalPrice"]);
        }
        public List<CartItemModel> CartItems { get; set; }
        public long TotalPrice { get; set; }
    }
    public class CartItemModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public int MainFeatureId { get; set; }
    }
    public class WishListModel
    {
        public WishListModel()
        {
        }

        public WishListModel(string json)
        {
            JObject jObject = JObject.Parse(json);
            var jItems = jObject["WishListItems"].ToList();
            var wishListItems = new List<WishListItemModel>();
            foreach (var item in jItems)
            {
                wishListItems.Add(new WishListItemModel()
                {
                    Id = Convert.ToInt32(item["Id"]),
                    ProductName = (string)item["ProductName"],
                    Image = (string)item["Image"],
                    MinPrice = Convert.ToInt64(item["MinPrice"]),
                    MaxPrice = Convert.ToInt64(item["MaxPrice"]),
                    Quantity = Convert.ToInt32(item["Quantity"])
                });
            }

            this.WishListItems = wishListItems;
        }
        public List<WishListItemModel> WishListItems { get; set; }
    }
    public class WishListItemModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class CommentFormViewModel
    {
        public int? ParentId { get; set; }
        public int? ArticleId { get; set; }
        public int? ProductId { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} باید کمتر از 300 کارکتر باشد")]
        public string Name { get; set; }
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل نا معتبر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} باید کمتر از 400 کارکتر باشد")]
        public string Email { get; set; }
        [Display(Name = "پیام")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800, ErrorMessage = "{0} باید کمتر از 800 کارکتر باشد")]
        public string Message { get; set; }
    }
    public class CheckoutForm
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل نا معتبر")]
        [MaxLength(400, ErrorMessage = "{0} باید کمتر از 400 کارکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} باید کمتر از 400 کارکتر باشد")]
        public string Phone { get; set; }

        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }

        [Display(Name = "کشور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Country { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string City { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PostalCode { get; set; }

        public int GeoDivisionId { get; set; }
        
        [Display(Name = "توضیحات(اختیاری)")]
        [DataType(DataType.MultilineText)]
        [MaxLength(800, ErrorMessage = "{0} باید کمتر از 800 کارکتر باشد")]
        public string Message { get; set; }

        public string DiscountCode { get; set; }
        public string InvoiceNumber { get; set; }
        
    }

    //public class DiscountFormViewModel
    //{
    //    [DisplayName("عنوان تخفیف")]
    //    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    //    [MaxLength(500, ErrorMessage = "{0} باید کمتر از 500 کارکتر باشد")]
    //    public string Title { get; set; }
    //    public int DiscountType { get; set; }
    //    [DisplayName("میزان تخفیف")]
    //    [Required(ErrorMessage = "لطفا میزان تخفیف را وارد کنید")]
    //    public long Amount { get; set; }
    //    public List<int> BrandIds { get; set; }
    //    public List<int> ProductIds { get; set; }
    //    public List<int> ProductGroupIds { get; set; }
    //    public bool IsOffer { get; set; }
    //    public int? OfferId { get; set; }

    //    // Edit Props
    //    public string GroupIdentifier { get; set; }
    //    public List<Discount> PreviousDiscounts { get; set; }
    //}

    public class NavbarViewModel
    {
        public NavbarViewModel()
        {
            MainGroups = new List<ProductGroup>();
            childGroups = new List<ProductGroup>();
        }
        public List<ProductGroup> MainGroups { get; set; }
        public List<ProductGroup> childGroups { get; set; }
    }

    public class AboutViewModel
    {
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string AboutDescription { get; set; }
        public string SignatureImage { get; set; }
        public string Image { get; set; }
    }
    public class FooterViewModel
    {
        public StaticContentDetail Phone { get; set; }
        //public StaticContentDetail Email { get; set; }
        //public StaticContentDetail Address { get; set; }
        public StaticContentDetail Youtube { get; set; }
        public StaticContentDetail Instagram { get; set; }
        public StaticContentDetail Twitter { get; set; }
        public StaticContentDetail Facebook { get; set; }
        public StaticContentDetail Pinterest { get; set; }
        public StaticContentDetail Logo { get; set; }
        //public StaticContentDetail ImplementationShortDescription { get; set; }
        //public StaticContentDetail CompanyServices { get; set; }
    }
    //public class ContactUsViewModel
    //{
    //    public StaticContentDetail Map { get; set; }
    //    public StaticContentDetail ContactInfo { get; set; }
    //    public StaticContentDetail Phone { get; set; }
    //    public StaticContentDetail Email { get; set; }
    //    public StaticContentDetail Address { get; set; }
    //    public StaticContentDetail WorkingHours { get; set; }
    //    public StaticContentDetail Youtube { get; set; }
    //    public StaticContentDetail Instagram { get; set; }
    //    public StaticContentDetail Twitter { get; set; }
    //    public StaticContentDetail Facebook { get; set; }
    //    public StaticContentDetail Pinterest { get; set; }

    //    [MaxLength(600)]
    //    [Display(Name = "نام *")]
    //    [Required(ErrorMessage = "لطفا {0} خود را وارد کنید")]
    //    public string Name { get; set; }

    //    [Display(Name = "ایمیل *")]
    //    [Required(ErrorMessage = "لطفا {0} خود را وارد کنید")]
    //    [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نیست")]
    //    [MaxLength(600)]
    //    public string CustomerEmail { get; set; }

    //    [Display(Name = "پیام شما *")]
    //    [Required(ErrorMessage = "لطفا {0} خود را وارد کنید")]
    //    [DataType(DataType.MultilineText)]
    //    public string Message { get; set; }
    //}

    public class GalleryPageViewModel
    {
        public List<ProductGallery> Images { get; set; }
        //public List<ProductGalleryVideo> Videos { get; set; }
    }

    public class InstaGalleryViewModel
    {
        public InstaGalleryViewModel()
        {
            this.Images = new List<StaticContentDetail>();
        }

        public List<StaticContentDetail> Images { get; set; }
        //public List<ProductGalleryVideo> Videos { get; set; }
    }

    public class HomeTopSliderViewModel
    {
        public HomeTopSliderViewModel()
        {
            this.Slides = new List<StaticContentDetail>();
            this.LogoAndButton = new StaticContentDetail();
        }

        public List<StaticContentDetail> Slides { get; set; }

        public StaticContentDetail LogoAndButton { get; set; }
    }

    public class SocialViewModel
    {
        public StaticContentDetail Facebook { get; set; }

        public StaticContentDetail Twitter { get; set; }

        public StaticContentDetail Instagram { get; set; }

        public StaticContentDetail Youtube { get; set; }

        public StaticContentDetail Pinterest { get; set; }

        public StaticContentDetail Linkedin { get; set; }

    }

    //public class GalleryViewModel
    //{
    //    public GalleryViewModel()
    //    {
    //        GalleryImages = new List<ProductGallery>();
    //    }

    //    public string Image { get; set; }
    //    public List<ProductGallery> GalleryImages { get; set; }
    //}
}