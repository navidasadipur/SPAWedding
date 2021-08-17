using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MaryamRahimiFard.Core.Models;
using MaryamRahimiFard.Core.Utility;
using MaryamRahimiFard.Infrastructure.Repositories;
using MaryamRahimiFard.Infratructure.Repositories;
using MaryamRahimiFard.Infratructure.Services;
using MaryamRahimiFard.Web.ViewModels;

namespace MaryamRahimiFard.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DiscountsRepository _discountRepo;
        private readonly StaticContentDetailsRepository _staticContentRepo;
        private readonly OffersRepository _offersRepo;
        private readonly ProductService _productService;
        private readonly TestimonialsRepository _testimonialRepo;
        private readonly PartnersRepository _partnersRepo;
        private readonly ArticlesRepository _articlesRepo;
        private readonly ContactFormsRepository _contactFormRepo;
        private readonly ProductGroupsRepository _productGroupRepo;
        private readonly OurTeamRepository _ourTeamRepo;
        private readonly FaqGroupsRepository _faqGroupsRepo;
        private readonly EmailSubscriptionRepository _emailSubscriptionRepo;
        private readonly CertificatesRepository _certificatesRepo;
        private readonly CoursesRepository _coursesRepo;
        private readonly GalleriesRepository _galleryRepo;
        private readonly ProductsRepository _productsRepo;

        public HomeController(
            StaticContentDetailsRepository staticContentRepo
            , OffersRepository offersRepo
            , ProductsRepository productsRepo
            , ProductService productService
            , TestimonialsRepository testimonialRepo
            , PartnersRepository partnersRepo
            , ArticlesRepository articlesRepo
            , DiscountsRepository discountsRepo
            , EmailSubscriptionRepository emailSubscriptionRepo
            , ContactFormsRepository contactFormRepo
            , ProductGroupsRepository productGroupRepo
            , OurTeamRepository ourTeamRepo
            , FaqGroupsRepository faqGroupsRepo
            , CertificatesRepository certificatesRepo
            , CoursesRepository coursesRepo
            , GalleriesRepository galleryRepo
            )
        {
            _discountRepo = discountsRepo;
            _staticContentRepo = staticContentRepo;
            _offersRepo = offersRepo;
            _productService = productService;
            _testimonialRepo = testimonialRepo;
            _partnersRepo = partnersRepo;
            _articlesRepo = articlesRepo;
            _contactFormRepo = contactFormRepo;
            _productGroupRepo = productGroupRepo;
            _ourTeamRepo = ourTeamRepo;
            _faqGroupsRepo = faqGroupsRepo;
            _emailSubscriptionRepo = emailSubscriptionRepo;
            _certificatesRepo = certificatesRepo;
            _coursesRepo = coursesRepo;
            _galleryRepo = galleryRepo;
            _productsRepo = productsRepo;
        }

        public ActionResult Index()
        {

            var popup = _staticContentRepo.GetSingleContentByTypeId((int)StaticContentTypes.Popup);

            ViewBag.ContactUs = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HomeContactUs);

            return View(popup);
        }

        public ActionResult MainTitleSection()
        {
            ViewBag.MainTitle = _staticContentRepo.GetStaticContentDetail((int)StaticContents.MainTitleDescriptionHeaderFooter).ShortDescription;

            return PartialView();
        }

        public ActionResult HeaderSection()
        {

            //var allMainGroups = _productGroupRepo.GetMainProductGroups();

            //foreach (var group in allMainGroups)
            //{
            //    group.Children = _productGroupRepo.GetChildrenProductGroups(group.Id);
            //}

            //ViewBag.LogoImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo).Image;

            ////WishList 
            //var wishListModel = new WishListModel();

            //HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            //if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            //{
            //    string cartJsonStr = cartCookie.Values["wishList"];
            //    wishListModel = new WishListModel(cartJsonStr);
            //}

            //if (wishListModel.WishListItems != null)
            //{
            //    ViewBag.WishListCount = wishListModel.WishListItems.Count();
            //}

            ViewBag.MainTitle = _staticContentRepo.GetStaticContentDetail((int)StaticContents.MainTitleDescriptionHeaderFooter).ShortDescription;

            ViewBag.HeaderImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HeaderBackGroundImage).Image;

            return PartialView(/*allMainGroups*/);
        }

        //public ActionResult MobileHeaderSection()
        //{

        //    var allMainGroups = _productGroupRepo.GetMainProductGroups();

        //    foreach (var group in allMainGroups)
        //    {
        //        group.Children = _productGroupRepo.GetChildrenProductGroups(group.Id);
        //    }

        //    ViewBag.LogoImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo).Image;

        //    var wishListModel = new WishListModel();

        //    HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

        //    if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
        //    {
        //        string cartJsonStr = cartCookie.Values["wishList"];
        //        wishListModel = new WishListModel(cartJsonStr);
        //    }

        //    if (wishListModel.WishListItems != null)
        //    {
        //        ViewBag.WishListCount = wishListModel.WishListItems.Count();
        //    }

        //    return PartialView(allMainGroups);
        //}

        //public ActionResult FooterTopSection()
        //{
        //    return PartialView();
        //}

        public ActionResult FooterSection()
        {
            var vm = new FooterViewModel()
            {
                Address = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Address),
                Email = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Email),
                Phone = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Phone),
                Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook),
                LinkedIn = _staticContentRepo.GetStaticContentDetail((int)StaticContents.LinkedIn),
                Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter),
                Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram),
                Youtube = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Youtube),
                Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest),
            };

            ViewBag.MainTitle = _staticContentRepo.GetStaticContentDetail((int)StaticContents.MainTitleDescriptionHeaderFooter);

            return PartialView(vm);
        }

        //public ActionResult CartSection()
        //{
        //    var cartModel = new CartModel();

        //    HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

        //    if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
        //    {
        //        string cartJsonStr = cartCookie.Values["cart"];
        //        cartModel = new CartModel(cartJsonStr);
        //    }
        //    return PartialView(cartModel);
        //}

        //public ActionResult WishListSection()
        //{
        //    var wishListModel = new WishListModel();

        //    HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

        //    if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
        //    {
        //        string cartJsonStr = cartCookie.Values["wishList"];
        //        wishListModel = new WishListModel(cartJsonStr);
        //    }

        //    return PartialView(wishListModel);
        //}

        public ActionResult HomeTopSliderSection()
        {
            var content = new List<StaticContentDetail>();

            content = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.HomeTopSlider);

            return PartialView(content);
        }

        public ActionResult HomeUnderSliderSection()
        {
            var content = new List<StaticContentDetail>();

            content = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.HomeOurServicesUnderSlieder);

            ViewBag.HomeUnderSliderTitle = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HomeUnderSliderTitle);

            return PartialView(content);
        }

        public ActionResult HomeAboutFounderSection()
        {
            var content = new StaticContentDetail();

            content = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HomeAbout);

            return PartialView(content);
        }

        public ActionResult HomeCounterSection()
        {
            var content = new List<StaticContentDetail>();

            content = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.HomeCounters);

            ViewBag.BackGroundImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HomeCounterBackGroundImage).Image;

            return PartialView(content);
        }

        public ActionResult HomeCourseProperties()
        {
            var content = new List<StaticContentDetail>();

            content = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.HomeCourseProperties);

            return PartialView(content);
        }

        public ActionResult HomeNewCoursesSection(int take)
        {
            var model = _coursesRepo.GetLatestCourses(take);

            ViewBag.HomeNewCourses = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HomeNewCourses);

            return PartialView(model);
        }

        public ActionResult HomeGallerySection()
        {
            var AllImages = _galleryRepo.GetAll();

            ViewBag.Gallery = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Gallery);

            return PartialView(AllImages);
        }

        public ActionResult HomeNewArticlesSection(int take)
        {
            var model = _articlesRepo.GetLatestArticles(take);

            ViewBag.HomeNewArticles = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HomeNewArticles);

            return PartialView(model);
        }

        //public ActionResult OffersSection()
        //{
        //    var offers = _offersRepo.GetAll();
        //    offers = offers.OrderBy(o => o.Id).ToList();
        //    return PartialView(offers);
        //}

        //public ActionResult TopSoldProductsSection(int take)
        //{
        //    var products = _productService.GetTopSoldProductsWithPrice(take);
        //    var vm = new List<ProductWithPriceViewModel>();
        //    foreach (var product in products)
        //    {
        //        var tempVm = new ProductWithPriceViewModel(product);

        //        var group = _productGroupRepo.GetGroupByProductId(product.Id);

        //        tempVm.ProductGroupId = group.Id;

        //        tempVm.ProductGroupTitle = group.Title;

        //        vm.Add(tempVm);
        //    }

        //    return PartialView(vm);
        //}

        //public ActionResult TestimonialsSection()
        //{
        //    var testimonials = _testimonialRepo.GetAll();
        //    var vm = testimonials.Select(testimonial => new TestimonialViewModel(testimonial)).ToList();

        //    return PartialView(vm);
        //}

        //public ActionResult LatestProductsSection(int take)
        //{
        //    var products = _productService.GetLatestProductsWithPrice(take);
        //    var vm = new List<ProductWithPriceViewModel>();
        //    foreach (var product in products)
        //    {
        //        var tempVm = new ProductWithPriceViewModel(product);

        //        var group = _productGroupRepo.GetGroupByProductId(product.Id);

        //        tempVm.ProductGroupId = group.Id;

        //        tempVm.ProductGroupTitle = group.Title;

        //        vm.Add(tempVm);
        //    }

        //    return PartialView(vm);
        //}

        //public ActionResult HighRateProductsSection(int take)
        //{
        //    var products = _productService.GetHighRatedProductsWithPrice(take);
        //    var vm = new List<ProductWithPriceViewModel>();
        //    foreach (var product in products)
        //        vm.Add(new ProductWithPriceViewModel(product));

        //    return PartialView(vm);
        //}


        public ActionResult LatestArticlesSection(int take)
        {
            //var articles = _articlesRepo.GetLatestArticles(3);
            //var vm = articles.Select(item => new LatestArticlesViewModel(item)).ToList();

            //return PartialView(vm);

            var vm = new List<LatestArticlesViewModel>();
            var articles = _articlesRepo.GetTopArticles(take);
            foreach (var item in articles)
                vm.Add(new LatestArticlesViewModel(item));

            return PartialView(vm);
        }


        //public ActionResult DiscountSection()
        //{
        //    var discountItems = _discountRepo.GetProductsWithDiscount();
        //    var products = new List<DiscountProductViewModel>();
        //    foreach(var item in discountItems)
        //    {
        //        var product = new DiscountProductViewModel();
        //        product.Price = _productService.GetProductPrice(item.Product);
        //        product.PriceAfterDiscount = _productService.GetProductPriceAfterDiscount(item.Product);
        //        product.ProductId = item.ProductId.Value;
        //        product.Image = item.Product.Image;
        //        product.DiscountType = item.DiscountType;
        //        product.Amount = item.Amount;
        //        product.Title = item.Title;
        //        product.ShortTitle = item.Product.ShortTitle;
        //        product.DeadLine = item.DeadLine;

        //        products.Add(product);
        //    }

        //    return PartialView(products);
        //}

        [Route("Faq")]
        public ActionResult Faq()
        {
            var model = _faqGroupsRepo.GetAllFaqGroupsWithFaqs();

            ViewBag.Faq = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Faq);

            return View(model);
        }

        [Route("About")]
        public ActionResult About()
        {
            var about = _staticContentRepo.GetStaticContentDetail((int)StaticContents.AboutDescription);

            var model = new AboutViewModel()
            {
                Title = about.Title,
                AboutDescription = about.Description,
                Image = about.Image,
            };

            return View(model);
        }

        public ActionResult AboutNewClassesSection(int take)
        {
            var model = _coursesRepo.GetLatestCourses(take);

            return PartialView(model);
        }

        public ActionResult OurTeamsSection()
        {
            var ourTeam = _ourTeamRepo.GetAll();

            return PartialView(ourTeam);
        }

        public ActionResult PartnersSection()
        {
            var partners = _partnersRepo.GetAll();
            return PartialView(partners);
        }

        [Route("ContactUs")]
        public ActionResult ContactUs()
        {
            var map = _staticContentRepo.GetStaticContentDetail((int)StaticContents.ContactUsMap);
            var phone = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Phone);
            var email = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Email);
            var address = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Address);
            var vm = new ContactUsViewModel()
            {
                Map = map,
                Phone = phone,
                Email = email,
                Address = address
            };

            ViewBag.ContactUs = _staticContentRepo.GetStaticContentDetail((int)StaticContents.ContactUs);

            return View(vm);
        }

        public ActionResult ContactSocialsSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook);
            model.Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter);
            model.Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest);
            model.Youtube = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Youtube);
            model.Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram);

            return PartialView(model);
        }

        public ActionResult ContactUsForm()
        {
            var model = new ContactForm();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ContactUsForm(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                _contactFormRepo.Add(contactForm);
                return RedirectToAction("ContactUsSummary");
            }
            return RedirectToAction("ContactUs");
        }

        public ActionResult ContactUsSummary()
        {
            return View();
        }

        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string vImagePath = String.Empty;
            string vMessage = String.Empty;
            string vFilePath = String.Empty;
            string vOutput = String.Empty;
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var vFileName = DateTime.Now.ToString("yyyyMMdd-HHMMssff") +
                                    Path.GetExtension(upload.FileName).ToLower();
                    var vFolderPath = Server.MapPath("/Upload/");
                    if (!Directory.Exists(vFolderPath))
                    {
                        Directory.CreateDirectory(vFolderPath);
                    }
                    vFilePath = Path.Combine(vFolderPath, vFileName);
                    upload.SaveAs(vFilePath);
                    vImagePath = Url.Content("/Upload/" + vFileName);
                    vMessage = "Image was saved correctly";
                }
            }
            catch
            {
                vMessage = "There was an issue uploading";
            }
            vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + vImagePath + "\", \"" + vMessage + "\");</script></body></html>";
            return Content(vOutput);
        }

        [Route("NotFound")]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmailSubscription(string Email)
        {
            var email = "";
            var isValid = true;
            try
            {
                //email = collection["Email"];

                email = Email;
            }
            catch
            {

            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                isValid = addr.Address == email;
            }
            catch
            {
                isValid = false;
            }

            if (isValid)
            {
                EmailSubscription emailSubscription = new EmailSubscription();
                emailSubscription.Email = email;
                emailSubscription.IsDeleted = false;
                emailSubscription.InsertDate = DateTime.Now;

                _emailSubscriptionRepo.Create(emailSubscription);
            }

            ViewBag.Added = isValid;

            return View();
        }

        [Route("Certificate")]
        public ActionResult Certificate()
        {
            var certificates = _certificatesRepo.GetAll();

            ViewBag.Certificate = _staticContentRepo.GetStaticContentDetail((int)StaticContents.certificate);

            return View(certificates);
        }

        [Route("Gallery")]
        public ActionResult Gallery()
        {
            var AllImages = _galleryRepo.GetAll();

            ViewBag.Gallery = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Gallery);

            return View(AllImages);
        }
    }
}