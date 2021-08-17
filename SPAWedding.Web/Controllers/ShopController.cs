using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using MaryamRahimiFard.Core.Models;
using MaryamRahimiFard.Core.Utility;
using MaryamRahimiFard.Infrastructure.Repositories;
using MaryamRahimiFard.Infratructure.Dtos.Product;
using MaryamRahimiFard.Infratructure.Repositories;
using MaryamRahimiFard.Infratructure.Services;
using MaryamRahimiFard.Web.Providers;
using MaryamRahimiFard.Web.ViewModels;

namespace MaryamRahimiFard.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProductService _productService;
        private readonly ProductsRepository _productsRepo;
        private readonly ProductGroupsRepository _productGroupRepo;
        private readonly FeaturesRepository _featuresRepo;
        private readonly BrandsRepository _brandsRepo;
        private readonly ProductGalleriesRepository _productGalleryRepo;
        private readonly ProductCommentsRepository _productCommentsRepository;
        private readonly ProductMainFeaturesRepository _productMainFeaturesRepo;
        private readonly ProductFeatureValuesRepository _productFeatureValueRepo;
        private readonly GeoDivisionsRepository _geoDivisionRepo;
        private readonly StaticContentDetailsRepository _staticContentRepo;
        private readonly InvoicesRepository _invoicesRepository;
        private UsersRepository _usersRepo;
        private CustomersRepository _customerRepo;
        private ShoppingRepository _shoppingRepo;
        private SMSLogRepository _smsLogRepo;
        private readonly DiscountsRepository _discountsRepo;

        public ShopController(ProductService productService,
            ProductGroupsRepository productGroupRepo,
            FeaturesRepository featuresRepo,
            BrandsRepository brandsRepo, ProductsRepository productsRepo,
            ProductGalleriesRepository productGalleryRepo,
            ProductCommentsRepository productCommentsRepository,
            ProductMainFeaturesRepository productMainFeaturesRepo,
            ProductFeatureValuesRepository productFeatureValueRepo,
            GeoDivisionsRepository geoDivisionRepo,
            UsersRepository usersRepo,
            CustomersRepository customerRepo,
            ShoppingRepository shoppingRepo,
            StaticContentDetailsRepository staticContentDetailsRepository,
            InvoicesRepository invoicesRepository,
            SMSLogRepository smsLogRepository,
            DiscountsRepository discountsRepo
            )
        {
            _productService = productService;
            _productGroupRepo = productGroupRepo;
            _featuresRepo = featuresRepo;
            _brandsRepo = brandsRepo;
            _productsRepo = productsRepo;
            _productGalleryRepo = productGalleryRepo;
            _productCommentsRepository = productCommentsRepository;
            _productMainFeaturesRepo = productMainFeaturesRepo;
            _productFeatureValueRepo = productFeatureValueRepo;
            _geoDivisionRepo = geoDivisionRepo;
            _usersRepo = usersRepo;
            _customerRepo = customerRepo;
            _shoppingRepo = shoppingRepo;
            _staticContentRepo = staticContentDetailsRepository;
            _invoicesRepository = invoicesRepository;
            _smsLogRepo = smsLogRepository;
            _discountsRepo = discountsRepo;
        }
        // GET: Products
        [Route("Shop/")]
        [Route("Shop/ProductList/{id}/{title}")]
        [Route("Shop/ProductList/{id}")]
        [Route("Shop/ProductList")]
        [Route("Shop/ProductList/Search/{searchString}")]
        public ActionResult Index(int? id, string searchString = null, string groupIds = null, string productIds = null, string brandIds = null)
        {
            var vm = new ProductListViewModel();
            vm.SelectedGroupId = id ?? 0;
            var productGroups = new List<ProductGroup>();
            var banner = "";
            if (id == null)
            {
                vm.Features = _featuresRepo.GetAllFeatures();
                vm.Brands = _brandsRepo.GetAll();

                var childrenGroups = _productGroupRepo.GetChildrenProductGroups();
                vm.ProductGroups = childrenGroups;
                ViewBag.Title = "محصولات";
                try
                {
                    banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ محصولات").Image;
                    banner = "/Files/StaticContentImages/Image/" + banner;
                }
                catch
                {

                }
            }
            else
            {
                //banner = _productGroupRepo.GetProductGroup(id.Value).Image;
                //banner = "/Files/ProductGroupImages/Image/" + banner;


                vm.Features = _featuresRepo.GetAllGroupFeatures(id.Value);
                vm.Brands = _brandsRepo.GetAllGroupBrands(id.Value);
                var selectedProductGroup = _productGroupRepo.Get(id.Value);
                var childrenGroups = _productGroupRepo.GetChildrenProductGroups(id.Value);

                //vm.Features = _featuresRepo.GetAllFeatures();
                //vm.Brands = _brandsRepo.GetAll();
                //var selectedProductGroup = _productGroupRepo.Get(id.Value);
                //var childrenGroups = _productGroupRepo.GetChildrenProductGroups();

                vm.ProductGroups = childrenGroups;
                ViewBag.ProductGroupName = selectedProductGroup.Title;
                ViewBag.ProductGroupId = selectedProductGroup.Id;
                ViewBag.Title = $"محصولات {selectedProductGroup.Title}";
            }

            //if(banner.Equals(""))
            //{
            //    banner = _staticContentRepo.GetSingleContentDetailByTitle("سربرگ محصولات").Image;
            //    banner = "/Files/StaticContentImages/Image/" + banner;
            //}

            if (searchString != null)
                ViewBag.SearchString = searchString;

            if (groupIds != null)
                ViewBag.GroupIds = groupIds;

            if (productIds != null)
                ViewBag.ProductIds = productIds;

            if (brandIds != null)
                ViewBag.BrandIds = brandIds;

            ViewBag.SearchString = searchString;

            ViewBag.Banner = banner;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(vm);
        }

        [Route("ProductsGrid")]
        public ActionResult ProductsGrid(GridViewModel grid)
        {
            var products = new List<Product>();

            var brandsIntArr = new List<int>();

            if (string.IsNullOrEmpty(grid.brands) == false)
            {
                var brandsArr = grid.brands.Split('-').ToList();
                brandsArr.ForEach(b => brandsIntArr.Add(Convert.ToInt32(b)));
            }

            var subFeaturesIntArr = new List<int>();
            if (string.IsNullOrEmpty(grid.subFeatures) == false)
            {
                var subFeaturesArr = grid.subFeatures.Split('-').ToList();
                subFeaturesArr.ForEach(b => subFeaturesIntArr.Add(Convert.ToInt32(b)));
            }

            products = _productService.GetProductsGrid(grid.categoryId, brandsIntArr, subFeaturesIntArr, grid.priceFrom, grid.priceTo, grid.searchString);

            #region Get Products Base on Group, Brand and Products of "offer"

            var allSearchedTargetProducts = new List<Product>();

            if (grid.GroupIds != null || grid.ProductIds != null || grid.BrandIds != null)
            {
                //search based on multiple group ids
                if (string.IsNullOrEmpty(grid.GroupIds) == false)
                {
                    var groupIdsIntArr = new List<int>();

                    var groupIdsArr = grid.GroupIds.Split('-').ToList();
                    groupIdsArr.ForEach(g => groupIdsIntArr.Add(Convert.ToInt32(g)));

                    var allTargetGroups = new List<ProductGroup>();

                    foreach (var id in groupIdsIntArr)
                    {
                        var group = _productGroupRepo.GetProductGroup(id);

                        allTargetGroups.Add(group);
                    }

                    foreach (var group in allTargetGroups)
                    {
                        if (group != null)
                        {
                            var allProductsOfOneGroup = _productsRepo.getProductsByGroupId(group.Id);

                            foreach (var product in allProductsOfOneGroup)
                            {
                                allSearchedTargetProducts.Add(product);
                            }
                        }
                    }
                }

                //search based on multiple brand ids
                if (string.IsNullOrEmpty(grid.BrandIds) == false)
                {
                    var brandIdsIntArr = new List<int>();

                    var brandIdsArr = grid.BrandIds.Split('-').ToList();
                    brandIdsArr.ForEach(b => brandIdsIntArr.Add(Convert.ToInt32(b)));

                    var allTargetBrands = new List<Brand>();

                    foreach (var id in brandIdsIntArr)
                    {
                        var brand = _brandsRepo.GetBrand(id);

                        allTargetBrands.Add(brand);
                    }

                    foreach (var brand in allTargetBrands)
                    {
                        if (brand != null)
                        {
                            var allProductsOfOneBrand = _productsRepo.getProductsByBrandId(brand.Id);

                            foreach (var product in allProductsOfOneBrand)
                            {
                                allSearchedTargetProducts.Add(product);
                            }
                        }
                    }
                }

                //search based on multiple product ids
                if (string.IsNullOrEmpty(grid.ProductIds) == false)
                {
                    var productIdsIntArr = new List<int>();

                    var productIdsArr = grid.ProductIds.Split('-').ToList();
                    productIdsArr.ForEach(b => productIdsIntArr.Add(Convert.ToInt32(b)));

                    foreach (var id in productIdsIntArr)
                    {
                        var product = _productsRepo.GetProduct(id);

                        //if product not found in allSearchedTargetProducts
                        if (!allSearchedTargetProducts.Contains(product))
                        {
                            allSearchedTargetProducts.Add(product);
                        }
                    }
                }

                products = allSearchedTargetProducts;
            }

            #endregion

            #region Sorting

            if (grid.sort != "date")
            {
                switch (grid.sort)
                {
                    case "name":
                        products = products.OrderBy(p => p.Title).ToList();
                        break;
                    case "sale":
                        products = products.OrderByDescending(p => _productService.GetProductSoldCount(p)).ToList();
                        break;
                    case "price-high-to-low":
                        products = products.OrderByDescending(p => _productService.GetProductPriceAfterDiscount(p)).ToList();
                        break;
                    case "price-low-to-high":
                        products = products.OrderBy(p => _productService.GetProductPriceAfterDiscount(p)).ToList();
                        break;
                }
            }
            #endregion



            var count = products.Count;
            var skip = grid.pageNumber * grid.take - grid.take;
            int pageCount = (int)Math.Ceiling((double)count / grid.take);
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = grid.pageNumber;

            products = products.Skip(skip).Take(grid.take).ToList();

            var vm = new List<ProductWithPriceDto>();
            foreach (var product in products)
                vm.Add(_productService.CreateProductWithPriceDto(product));

            return PartialView(vm);
        }

        [Route("offer")]
        public ActionResult Offer(int offerId = 0)
        {
            var allGroupIds = new List<int>();
            var allProductIds = new List<int>();
            var allBrandIds = new List<int>();

            if (offerId == 0)
            {
                return RedirectToAction("Index");
            }

            var allOfferDiscounts = _discountsRepo.GetAllOfferDiscountsByOfferId(offerId);

            foreach (var discount in allOfferDiscounts)
            {
                if (discount.ProductGroupId != null)
                {
                    allGroupIds.Add(discount.ProductGroupId.Value);
                }
                else if (discount.ProductId != null)
                {
                    var product = _productsRepo.GetProduct(discount.ProductId.Value);

                    allProductIds.Add(product.Id);
                }
                else if (discount.BrandId != null)
                {
                    allBrandIds.Add(discount.BrandId.Value);
                }
            }

            //if (discount.ProductGroupId != null)
            //{
            //    groupId = discount.ProductGroupId.Value;
            //}
            //else if (discount.BrandId != null)
            //{
            //    brandId = discount.BrandId.Value.ToString();

            //    //var allGroups = _productGroupsRepo.GetAllProductGroups();

            //    //foreach (var group in allGroups)
            //    //{
            //    //    group.ProductGroupBrands = _productGroupsRepo.GetProductGroupBrands(group.Id);

            //    //    var allGorupBrands = group.ProductGroupBrands.Where(gb => gb.IsDeleted == false && gb.BrandId == discount.BrandId).ToList();

            //    //    if (allGorupBrands.Count() != 0)
            //    //    {
            //    //        groupId = allGorupBrands.Select(gb => gb.ProductGroupId).FirstOrDefault();
            //    //    }
            //    //}
            //}
            //else
            //{
            //    var allGroups = _productGroupsRepo.GetAllProductGroupsWithProducts();

            //    foreach (var group in allGroups)
            //    {
            //        if (group.Products.Count() != 0)
            //        {
            //            allProducts = group.Products.Where(p => p.IsDeleted == false && p.Id == discount.ProductId).ToList();
            //        }

            //        if (allProducts.Count() != 0)
            //        {
            //            groupId = allProducts.Select(p => p.ProductGroupId).FirstOrDefault().Value;
            //        }
            //    }
            //}

            var allGroupIdsStr = string.Join("-", allGroupIds);

            var allProductIdsStr = string.Join("-", allProductIds);

            var allBrandIdsStr = string.Join("-", allBrandIds);

            return RedirectToAction("Index", new { groupIds = allGroupIdsStr, productIds = allProductIdsStr, brandIds = allBrandIdsStr});
        }

        [Route("Shop/Product/{id}/{title}")]
        [Route("Shop/Product/{id}")]
        [Route("Shop/ProductDetails/{id}")]
        public ActionResult ProductDetails(int id)
        {
            var product = _productsRepo.GetProduct(id);

            var productGroup = new ProductGroup();

            if (product.ProductGroupId != null)
            {
                productGroup = _productGroupRepo.GetProductGroup(product.ProductGroupId.Value);
            }

            var productGallery = _productGalleryRepo.GetProductGalleries(id);
            var productMainFeatures = _productMainFeaturesRepo.GetProductMainFeatures(id);
            var productFeatureValues = _productFeatureValueRepo.GetProductFeatures(id);
            var price = _productService.GetProductPrice(product);
            var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product);
            var productComments = _productCommentsRepository.GetProductComments(id);
            var productCommentsVm = new List<ProductCommentViewModel>();

            foreach (var item in productComments)
                productCommentsVm.Add(new ProductCommentViewModel(item));

            var banner = "";
            if (product.ProductGroupId != null)
                banner = _productGroupRepo.GetProductGroup(product.ProductGroupId.Value).Image;

            ViewBag.Banner = banner;

            var vm = new ProductDetailsViewModel()
            {
                Product = product,
                Group = productGroup,
                ProductGalleries = productGallery,
                ProductMainFeatures = productMainFeatures,
                ProductFeatureValues = productFeatureValues,
                Price = price,
                PriceAfterDiscount = priceAfterDiscount,
                ProductComments = productCommentsVm
            };

            ViewBag.StartNotes = _productsRepo.GetProductPerfumeNotes(product.Id).Where(n=>n.PerfumeNoteType == PerfumeNoteType.Beginning).ToList();
            ViewBag.MidNotes = _productsRepo.GetProductPerfumeNotes(product.Id).Where(n => n.PerfumeNoteType == PerfumeNoteType.Middle).ToList();
            ViewBag.EndNotes = _productsRepo.GetProductPerfumeNotes(product.Id).Where(n => n.PerfumeNoteType == PerfumeNoteType.Ending).ToList();
            ViewBag.ProductColors = _productsRepo.GetProductColors(product.Id);
            ViewBag.ProductVolumes = _productsRepo.GetProductPerfumeVolumes(product.Id);

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            ViewBag.CallForProductNumber = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Phone);

            return View(vm);
        }

        public ActionResult RelatedProductsSection(int productId, int take)
        {
            var products = _productService.GetRelatedProducts(productId, take);
            return PartialView(products);
        }

        public ActionResult ComplementaryProductsSection(int productId, int take=10)
        {
            var type = "complementary";

            var products = _productService.GetComplementaryProducts(productId, take);
            if (products.Count == 0)
            {
                products = _productService.GetRelatedProducts(productId, take);
                type = "related";
            }

            ViewBag.ViewType = type;
            return PartialView(products);
        }

        [HttpPost]
        public ActionResult PostComment(CommentFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                var comment = new ProductComment()
                {
                    ProductId = form.ProductId.Value,
                    //ParentId = form.ParentId,
                    Name = form.Name,
                    Email = form.Email,
                    Message = form.Message,
                    AddedDate = DateTime.Now,
                };
                _productCommentsRepository.Add(comment);
                return RedirectToAction("ContactUsSummary", "Home");
            }
            return RedirectToAction("ProductDetails", new { id = form.ProductId });
        }

        public string GetProductPrice(int productId, int mainFeatureId)
        {
            var product = _productsRepo.Get(productId);
            var price = _productService.GetProductPrice(product, mainFeatureId);
            var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product, mainFeatureId);
            var result = new
            {
                price = price.ToString("##,###"),
                priceAfterDiscount = priceAfterDiscount.ToString("##,###")
            };
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return jsonStr;
        }

        [HttpPost]
        public string AddToCart(int productId, int? mainFeatureId, int count=1)
        {

            CartResponse cartResponse = new CartResponse();
            cartResponse.Message = "success";

            count = count <= 0 ? 1 : count;
            var cartModel = new CartModel();
            var cartItemsModel = new List<CartItemModel>();

            #region Checking for cookie
            HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

            if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
            {
                string cartJsonStr = cartCookie.Values["cart"];
                cartModel = new CartModel(cartJsonStr);
                cartItemsModel = cartModel.CartItems;
            }
            #endregion

            ProductWithPriceDto product;
            int productStockCount;
            if (mainFeatureId == null)
            {
                mainFeatureId = _productMainFeaturesRepo.GetByProductId(productId).Id;
            }
            product = _productService.CreateProductWithPriceDto(productId, mainFeatureId.Value);
            productStockCount = _productService.GetProductStockCount(productId, mainFeatureId.Value);

            if (productStockCount > 0)
            {
                if (cartItemsModel.Any(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value))
                {
                    if (((cartItemsModel.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value).Quantity) + count) <= productStockCount)
                    {
                        cartItemsModel.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value).Quantity += count;
                        cartModel.TotalPrice += (product.PriceAfterDiscount * count);
                    }
                    else
                    {
                        cartResponse.Message = "finished";
                    }
                }
                else
                {
                    if (productStockCount >= count)
                    {
                        cartItemsModel.Add(new CartItemModel()
                        {
                            Id = product.Id,
                            ProductName = product.ShortTitle,
                            Price = product.PriceAfterDiscount,
                            Quantity = count,
                            MainFeatureId = mainFeatureId.Value,
                            Image = product.Image
                        });
                        cartModel.TotalPrice += (product.PriceAfterDiscount * count);
                    }
                    else
                    {
                        cartResponse.Message = "finished";
                    }
                }
                cartModel.CartItems = cartItemsModel;
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cartModel);
                cartCookie.Values.Set("cart", jsonStr);

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;

                Response.Cookies.Add(cartCookie);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(cartResponse);
        }
        [HttpPost]
        public string RemoveFromCart(int productId, int? mainFeatureId, string complete = null)
        {
            var cartModel = new CartModel();

            #region Checking for cookie
            HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

            if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
            {
                string cartJsonStr = cartCookie.Values["cart"];
                cartModel = new CartModel(cartJsonStr);
            }
            #endregion

            if (cartModel.CartItems.Any(i => i.Id == productId && i.MainFeatureId == mainFeatureId))
            {
                var itemToRemove = cartModel.CartItems.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId);
                if (complete == "true" || itemToRemove.Quantity < 2)
                {
                    cartModel.TotalPrice -= itemToRemove.Price * itemToRemove.Quantity;
                    cartModel.CartItems.Remove(itemToRemove);
                }
                else if (complete == "false")
                {
                    cartModel.TotalPrice -= itemToRemove.Price;
                    cartModel.CartItems.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId).Quantity -= 1;
                }
            }
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cartModel);
            cartCookie.Values.Set("cart", jsonStr);
            cartCookie.Expires = DateTime.Now.AddHours(12);
            cartCookie.SameSite = SameSiteMode.Lax;
            Response.Cookies.Add(cartCookie);



            CartResponse cartResponse = new CartResponse();
            cartResponse.Message = "success";
            cartResponse.CartItemCount = cartModel.CartItems.Count;

            return Newtonsoft.Json.JsonConvert.SerializeObject(cartResponse);
        }
        [HttpPost]
        public void AddToWishList(int productId)
        {
            var withListModel = new WishListModel();
            var withListItemsModel = new List<WishListItemModel>();

            #region Checking for cookie
            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                withListModel = new WishListModel(cartJsonStr);
                withListItemsModel = withListModel.WishListItems;
            }
            #endregion

            var product = _productsRepo.GetProduct(productId);
            if (withListItemsModel.Any(i => i.Id == productId) == false)
            {
                withListItemsModel.Add(new WishListItemModel()
                {
                    Id = product.Id,
                    ProductName = product.ShortTitle,
                    Image = product.Image,
                    MinPrice = product.ProductMainFeatures.Select(pmf => pmf.Price).Min(),
                    MaxPrice = product.ProductMainFeatures.Select(pmf => pmf.Price).Max(),
                    Quantity = product.ProductMainFeatures.Select(pmf => pmf.Quantity).Max(),
                });
            }
            withListModel.WishListItems = withListItemsModel;
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(withListModel);
            cartCookie.Values.Set("wishList", jsonStr);

            cartCookie.Expires = DateTime.Now.AddHours(12);
            cartCookie.SameSite = SameSiteMode.Lax;

            Response.Cookies.Add(cartCookie);
        }
        [HttpPost]
        public void RemoveFromWishList(int productId)
        {
            var withListModel = new WishListModel();

            #region Checking for cookie
            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                withListModel = new WishListModel(cartJsonStr);
            }
            #endregion

            if (withListModel.WishListItems.Any(i => i.Id == productId))
            {
                var itemToRemove = withListModel.WishListItems.FirstOrDefault(i => i.Id == productId);
                withListModel.WishListItems.Remove(itemToRemove);
            }
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(withListModel);
            cartCookie.Values.Set("wishList", jsonStr);
            cartCookie.Expires = DateTime.Now.AddHours(12);
            cartCookie.SameSite = SameSiteMode.Lax;
            Response.Cookies.Add(cartCookie);
        }
        public ActionResult WishList()
        {
            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View();
        }
        public ActionResult WishListTable()
        {
            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            return PartialView(wishListModel);
        }
        public ActionResult Cart()
        {
            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View();
        }
        public ActionResult CartTable()
        {
            var cartModel = new CartModel();

            HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

            if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
            {
                string cartJsonStr = cartCookie.Values["cart"];
                cartModel = new CartModel(cartJsonStr);
            }

            return PartialView(cartModel);
        }

        [CustomerAuthorize]
        public ActionResult Checkout(string invoiceNumber = "")
        {
            var cartModel = new CartModel();
            cartModel.CartItems = new List<CartItemModel>();
            List<string> errors = new List<string>();
            long totalPrice = 0;
            string discountCode = "";
            long discountAmount = 0;
            var customer = _customerRepo.GetCurrentCustomer();

            if (invoiceNumber.Equals(""))
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                {
                    string cartJsonStr = cartCookie.Values["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }

                var cartItems = cartModel.CartItems??new List<CartItemModel>();
                foreach (var item in cartItems)
                {
                    
                    var mainFeature = _productMainFeaturesRepo.GetLastActiveMainFeature(item.Id, item.MainFeatureId);
                    var product = _productService.CreateProductWithPriceDto(item.Id, item.MainFeatureId);

                    var productPrice = product.PriceAfterDiscount > 0 ? product.PriceAfterDiscount : product.Price;

                    if (mainFeature==null || (productPrice != item.Price))
                    {
                        errors.Add("ویژگی های محصول '" + item.ProductName + "' تغییر کرده و امکان ثبت سفارش وجود ندارد. لطفا این محصول را از سبد خود حذف و در صورت تمایل مجدد از فروشگاه آن را انتخاب کنید.");
                    }

                    var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                    if (item.Quantity > productStockCount)
                    {
                        errors.Add("در حال حاظر تنها " + productStockCount + " از محصول " + item.ProductName + "در انبار موجود است. لطفا سبد خرید خود را به روز کنید.");

                    }

                    item.Price = product.PriceAfterDiscount;
                    totalPrice += item.Quantity * item.Price;
                }

                cartModel.CartItems = cartItems;
                cartModel.TotalPrice = totalPrice;
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cartModel);
                cartCookie.Values.Set("cart", jsonStr);

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;

                Response.Cookies.Add(cartCookie);
                invoiceNumber = GenerateInvoiceNumber();
            } // new order
            else
            {

                // Reading from database 
                var invoice = _invoicesRepository.GetInvoice(invoiceNumber, customer.Id);


                if(DateTime.Now.Subtract(invoice.AddedDate).TotalDays > 1)
                {
                    return Redirect("/Shop/Expired");
                }
                
                if(invoice.DiscountAmount > 0)
                {
                    discountAmount = invoice.DiscountAmount;
                    discountCode = invoice.DiscountCode.DiscountCodeStr;

                }

                foreach(var item in invoice.InvoiceItems)
                {
                    var product = _productService.CreateProductWithPriceDto(item.ProductId, item.MainFeatureId);
                    var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                    if (item.Quantity > productStockCount)
                    {
                        errors.Add("امکان ثبت این سفارش وجود ندارد. در حال حاظر تنها تعداد " + productStockCount + " مورد از محصول " + product.ShortTitle + " در انبار موجود است. ");

                    }

                    CartItemModel cartItem = new CartItemModel();
                    cartItem.Id = item.ProductId;
                    cartItem.Quantity = item.Quantity;
                    cartItem.MainFeatureId = item.MainFeatureId;
                    cartItem.Price = item.Price;
                    cartItem.ProductName = product.ShortTitle;
                    cartItem.Image = product.Image;

                    totalPrice += cartItem.Quantity * cartItem.Price;


                    cartModel.CartItems.Add(cartItem);
                }
                cartModel.TotalPrice = totalPrice;
            } // existing order


            cartModel.TotalPrice -= discountAmount;

            ViewBag.Today = new PersianDateTime(DateTime.Now).ToString("dddd d MMMM yyyy");
            ViewBag.InvoiceNumber = invoiceNumber;
            ViewBag.Errors = errors;
            ViewBag.DiscountCode = discountCode;
            ViewBag.DiscountAmount = discountAmount;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(cartModel);
        }

        [CustomerAuthorize]
        [HttpPost]
        public ActionResult Checkout(CheckoutForm checkoutForm)
        {

            if (ModelState.IsValid)
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");
                var customer = _customerRepo.GetCurrentCustomer();


                // Update customer info with latest information
                customer.Address = checkoutForm.Address;
                _customerRepo.Update(customer);

                // checkin' for discount code validity
                var discountCode = _shoppingRepo.GetActiveDiscountCode(checkoutForm.DiscountCode, customer.Id);
                long discountCodeAmount = 0;
                int? discountCodeId = null;
                if (discountCode != null)
                {
                    discountCodeAmount = discountCode.Value;
                    discountCodeId = discountCode.Id;
                }

                // Add a new (if not exists, according to invoice number) or update invoice
                var cartModel = new CartModel();
                long totalPricebeforeDiscountCode = 0;

                Invoice currentInvoice =  _invoicesRepository.GetInvoice(checkoutForm.InvoiceNumber);
                if (currentInvoice == null)
                {
                    // adding new order
                    currentInvoice = new Invoice();
                    currentInvoice.InvoiceItems = new List<InvoiceItem>();

                    currentInvoice.AddedDate = DateTime.Now;
                    currentInvoice.CustomerId = customer.Id;
                    currentInvoice.CustomerName = checkoutForm.Name;
                    currentInvoice.CompanyName = checkoutForm.CompanyName;
                    currentInvoice.Country = checkoutForm.Country;
                    currentInvoice.GeoDivisionId = checkoutForm.GeoDivisionId;
                    currentInvoice.City = checkoutForm.City;
                    currentInvoice.Address = checkoutForm.Address;
                    currentInvoice.Phone = checkoutForm.Phone;
                    currentInvoice.PostalCode = checkoutForm.PostalCode;
                    currentInvoice.Email = checkoutForm.Email;
                    currentInvoice.IsPayed = false;
                    currentInvoice.InvoiceNumber = checkoutForm.InvoiceNumber;


                    // calculate price for order and adding items to invoice

                    if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                    {
                        string cartJsonStr = cartCookie.Values["cart"];
                        cartModel = new CartModel(cartJsonStr);
                    }
                    else
                    {
                        return Redirect("/Shop/Checkout"); // basket is empty
                    }

                    var cartItems = cartModel.CartItems;
                    foreach (var item in cartItems)
                    {
                        InvoiceItem invoiceItem = new InvoiceItem();

                        var mainFeature = _productMainFeaturesRepo.GetLastActiveMainFeature(item.Id, item.MainFeatureId);
                        var product = _productService.CreateProductWithPriceDto(item.Id, item.MainFeatureId);

                        var productPrice = product.PriceAfterDiscount > 0 ? product.PriceAfterDiscount : product.Price;

                        if (mainFeature == null || (productPrice != item.Price))
                        {
                            // product features have changed
                            return Redirect("/Shop/Checkout");
                        }

                        var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                        if (item.Quantity > productStockCount)
                        {
                            return Redirect("/Shop/Checkout"); // out of product 

                        }

                        item.Price = product.PriceAfterDiscount;
                        totalPricebeforeDiscountCode += item.Quantity * item.Price;

                        invoiceItem.Quantity = item.Quantity;
                        invoiceItem.Price = item.Price;
                        invoiceItem.ProductId = product.Id;
                        invoiceItem.MainFeatureId = item.MainFeatureId;
                        invoiceItem.InsertDate = DateTime.Now;
                        invoiceItem.InsertUser = customer.User.UserName;

                        currentInvoice.InvoiceItems.Add(invoiceItem);

                    }

                    currentInvoice.DiscountAmount = discountCodeAmount;
                    currentInvoice.TotalPriceBeforeDiscount = totalPricebeforeDiscountCode;
                    currentInvoice.TotalPrice = totalPricebeforeDiscountCode - discountCodeAmount;
                    currentInvoice.DiscountCodeId = discountCodeId;

                    _invoicesRepository.Add(currentInvoice);

                    if (discountCode != null) _shoppingRepo.DeactiveDiscountCode(discountCode.Id); // deactive discount code



                    // remove all items from cart
                    cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");
                    var cart = new CartModel();
                    cart.CartItems = new List<CartItemModel>();
                    cart.TotalPrice = 0;
                    var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cart);
                    cartCookie.Values.Set("cart", jsonStr);

                    cartCookie.Expires = DateTime.Now.AddHours(-12);
                    cartCookie.SameSite = SameSiteMode.Lax;

                    Response.Cookies.Add(cartCookie);

                }
                else
                {
                    // using existing order
                    if (DateTime.Now.Subtract(currentInvoice.AddedDate).TotalDays > 1) // check if invoice is not expired
                    {
                        return Redirect("/Shop/Expired");
                    }

                    // recalculate price for order

                    foreach (var item in currentInvoice.InvoiceItems)
                    {
                        var product = _productService.CreateProductWithPriceDto(item.ProductId, item.MainFeatureId);
                        var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                        if (item.Quantity > productStockCount)
                        {
                            return Redirect("/Shop/Checkout"); // out of product 
                        }
                        item.Price = product.PriceAfterDiscount;
                        totalPricebeforeDiscountCode += item.Quantity * item.Price;


                    }

                    // updating info
                    currentInvoice.AddedDate = DateTime.Now;
                    currentInvoice.CustomerId = customer.Id;
                    currentInvoice.CustomerName = checkoutForm.Name;
                    currentInvoice.CompanyName = checkoutForm.CompanyName;
                    currentInvoice.Country = checkoutForm.Country;
                    currentInvoice.GeoDivisionId = checkoutForm.GeoDivisionId;
                    currentInvoice.City = checkoutForm.City;
                    currentInvoice.Address = checkoutForm.Address;
                    currentInvoice.Phone = checkoutForm.Phone;
                    currentInvoice.PostalCode = checkoutForm.PostalCode;
                    currentInvoice.Email = checkoutForm.Email;
                    currentInvoice.IsPayed = false;

                    if(discountCode!=null)
                    {
                        currentInvoice.DiscountAmount = discountCodeAmount;
                        currentInvoice.DiscountCodeId = discountCodeId;
                    }

                    currentInvoice.TotalPriceBeforeDiscount = totalPricebeforeDiscountCode;
                    currentInvoice.TotalPrice = totalPricebeforeDiscountCode - currentInvoice.DiscountAmount;

                    _invoicesRepository.Update(currentInvoice);
                    if (discountCode != null) _shoppingRepo.DeactiveDiscountCode(discountCode.Id); // deactive discount code

                }

            }

            return Redirect("/Shop/ConfirmOrder/?invoiceNumber="+checkoutForm.InvoiceNumber);
        }

        [CustomerAuthorize]
        public ActionResult ConfirmOrder(string invoiceNumber = "")
        {
            if (invoiceNumber.Equals(""))
                return Redirect("/Shop/Checkout");


            var customer = _customerRepo.GetCurrentCustomer();
            var invoice = _invoicesRepository.GetInvoice(invoiceNumber, customer.Id);


            var cartModel = new CartModel();
            cartModel.CartItems = new List<CartItemModel>();
            long totalPrice = 0;
            string discountCode = "";
            long discountAmount = 0;




            if (DateTime.Now.Subtract(invoice.AddedDate).TotalDays > 1)
            {
                return Redirect("/Shop/Expired");
            }

            if (invoice.DiscountAmount > 0)
            {
                discountAmount = invoice.DiscountAmount;
                discountCode = invoice.DiscountCode.DiscountCodeStr;

            }

            foreach (var item in invoice.InvoiceItems)
            {
                var product = _productService.CreateProductWithPriceDto(item.ProductId, item.MainFeatureId);
                var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                if (item.Quantity > productStockCount)
                {
                    return Redirect("/Shop/Expired"); // since order been registered we can't change the number of products and because we don't have enough products in the stock, we can't process the order

                }

                CartItemModel cartItem = new CartItemModel();
                cartItem.Id = item.ProductId;
                cartItem.Quantity = item.Quantity;
                cartItem.MainFeatureId = item.MainFeatureId;
                cartItem.Price = item.Price;
                cartItem.ProductName = product.ShortTitle;
                cartItem.Image = product.Image;

                totalPrice += cartItem.Quantity * cartItem.Price;


                cartModel.CartItems.Add(cartItem);
            }
            cartModel.TotalPrice = totalPrice;


            cartModel.TotalPrice -= discountAmount;


            ViewBag.CustomerInfoView = new CustomerInfoView(invoice , _geoDivisionRepo);
            ViewBag.Today = new PersianDateTime(invoice.AddedDate).ToString("dddd d MMMM yyyy");
            ViewBag.InvoiceNumber = invoiceNumber;
            ViewBag.DiscountCode = discountCode;
            ViewBag.DiscountAmount = discountAmount;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(cartModel);


        }

        public ActionResult Expired()
        {
            return View();
        }

        [CustomerAuthorize]
        public ActionResult CheckoutForm(string invoiceNumber)
        {
            var customer = _customerRepo.GetCurrentCustomer();
            Invoice invoice = _invoicesRepository.GetLatestInvoice(customer.Id);

            CheckoutForm CheckoutForm = new CheckoutForm();
            CheckoutForm.InvoiceNumber = invoiceNumber;
            CheckoutForm.Address = invoice!=null? invoice.Address : customer.Address;
            CheckoutForm.Email = invoice != null ? invoice.Email : customer.User.Email;
            CheckoutForm.Name = invoice != null ? invoice.CustomerName : customer.User.FirstName + " " + customer.User.LastName;
            CheckoutForm.PostalCode = invoice != null ? invoice.PostalCode : customer.PostalCode;
            CheckoutForm.Phone = invoice != null ? invoice.Phone : customer.User.PhoneNumber;
            CheckoutForm.GeoDivisionId = invoice != null ? invoice.GeoDivisionId.Value : (customer.GeoDivisionId?? 1);
            CheckoutForm.DiscountCode = invoice != null ? (invoice.DiscountCode != null ? invoice.DiscountCode.DiscountCodeStr : "") : "";


            ViewBag.GeoDivisionIds = new SelectList(_geoDivisionRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", CheckoutForm.GeoDivisionId);

            return PartialView(CheckoutForm);
        }

        public string GenerateInvoiceNumber()
        {
            var bytes = Guid.NewGuid().ToByteArray();
            var code = "";
            for (int i = 0; code.Length <= 16 && i < bytes.Length; i++)
            {
                code += (bytes[i] % 10).ToString();
            }

            return code;
        }

        [HttpPost]
        public string ApplyDiscountCode(string discountCodeStr, string invoiceNumber = "")
        {
            long finalPrice = 0;
            long discountAmount = 0;
            var invoice = _invoicesRepository.GetInvoice(invoiceNumber);

            if (invoice == null)
            {
                var cartModel = new CartModel();

                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                {
                    string cartJsonStr = cartCookie.Values["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }

                finalPrice = cartModel.TotalPrice;
            }
            else
            {
                finalPrice = invoice.TotalPrice;
            }

            DiscountCodeResponseViewModel discountCodeResponse = new DiscountCodeResponseViewModel();

            // first we need to check if "discountCode" is valid
            var customer = _customerRepo.GetCurrentCustomer();

            if(customer==null)
            {
                discountCodeResponse.Response = "login";
                discountCodeResponse.FinalPrice = finalPrice;
                return Newtonsoft.Json.JsonConvert.SerializeObject(discountCodeResponse);
            }

            // then we recalculate the cart items' price with regard to discountCode price
            var discountCode = _shoppingRepo.GetActiveDiscountCode(discountCodeStr, customer.Id);
            if (discountCode == null)
            {
                discountCodeResponse.Response = "invalid";
            }
            else
            {
                finalPrice -= discountCode.Value;
                discountAmount = discountCode.Value;
                discountCodeResponse.Response = "valid";
            }



            discountCodeResponse.FinalPrice = finalPrice;
            discountCodeResponse.DiscountAmount = discountAmount;

            return Newtonsoft.Json.JsonConvert.SerializeObject(discountCodeResponse);

        }

        public ActionResult SocialsSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook);
            model.Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter);
            model.Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest);
            model.Youtube = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Youtube);
            model.Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram);

            return PartialView(model);
        }

        public ActionResult ProductDetailsDescrioptinSection(int productId)
        {
            var product = _productsRepo.GetProduct(productId);
            //var productGallery = _productGalleryRepo.GetProductGalleries(productId);
            //var productMainFeatures = _productMainFeaturesRepo.GetProductMainFeatures(productId);
            //var productFeatureValues = _productFeatureValueRepo.GetProductFeatures(productId);
            //var price = _productService.GetProductPrice(product);
            //var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product);
            var productComments = _productCommentsRepository.GetProductComments(productId);
            var productCommentsVm = new List<ProductCommentViewModel>();

            foreach (var item in productComments)
                productCommentsVm.Add(new ProductCommentViewModel(item));

            var banner = "";
            if (product.ProductGroupId != null)
                banner = _productGroupRepo.GetProductGroup(product.ProductGroupId.Value).Image;

            ViewBag.Banner = banner;

            var vm = new ProductDetailsViewModel()
            {
                Product = product,
                //ProductGalleries = productGallery,
                //ProductMainFeatures = productMainFeatures,
                //ProductFeatureValues = productFeatureValues,
                //Price = price,
                //PriceAfterDiscount = priceAfterDiscount,
                //ProductComments = productCommentsVm
            };

            return PartialView(vm);
        }

        public ActionResult ProductFeaturesSection(int productId)
        {
            var product = _productsRepo.GetProduct(productId);
            //var productGallery = _productGalleryRepo.GetProductGalleries(productId);
            //var productMainFeatures = _productMainFeaturesRepo.GetProductMainFeatures(productId);
            var productFeatureValues = _productFeatureValueRepo.GetProductFeatures(productId);
            //var price = _productService.GetProductPrice(product);
            //var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product);
            //var productComments = _productCommentsRepository.GetProductComments(productId);
            //var productCommentsVm = new List<ProductCommentViewModel>();

            //foreach (var item in productComments)
            //    productCommentsVm.Add(new ProductCommentViewModel(item));

            var banner = "";
            if (product.ProductGroupId != null)
                banner = _productGroupRepo.GetProductGroup(product.ProductGroupId.Value).Image;

            ViewBag.Banner = banner;

            var vm = new ProductDetailsViewModel()
            {
                Product = product,
                //ProductGalleries = productGallery,
                //ProductMainFeatures = productMainFeatures,
                ProductFeatureValues = productFeatureValues,
                //Price = price,
                //PriceAfterDiscount = priceAfterDiscount,
                //ProductComments = productCommentsVm
            };

            return PartialView(vm);
        }

        public ActionResult ProductDetailsBrandSection(int productId)
        {
            var product = _productsRepo.GetProduct(productId);
            //var productGallery = _productGalleryRepo.GetProductGalleries(productId);
            //var productMainFeatures = _productMainFeaturesRepo.GetProductMainFeatures(productId);
            //var productFeatureValues = _productFeatureValueRepo.GetProductFeatures(productId);
            //var price = _productService.GetProductPrice(product);
            //var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product);
            var productComments = _productCommentsRepository.GetProductComments(productId);
            var productCommentsVm = new List<ProductCommentViewModel>();

            foreach (var item in productComments)
                productCommentsVm.Add(new ProductCommentViewModel(item));

            var banner = "";
            if (product.ProductGroupId != null)
                banner = _productGroupRepo.GetProductGroup(product.ProductGroupId.Value).Image;

            ViewBag.Banner = banner;

            var vm = new ProductDetailsViewModel()
            {
                Product = product,
                //ProductGalleries = productGallery,
                //ProductMainFeatures = productMainFeatures,
                //ProductFeatureValues = productFeatureValues,
                //Price = price,
                //PriceAfterDiscount = priceAfterDiscount,
                //ProductComments = productCommentsVm
            };

            return PartialView(vm);
        }
    }
}