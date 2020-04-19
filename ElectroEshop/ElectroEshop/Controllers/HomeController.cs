using ElectroEshop.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElectroEshop.Controllers
{
    public class HomeController : Controller
    {
        ElectroEshopEntities db = new ElectroEshopEntities();

        public ApplicationSignInManager SignInManager;
        public ApplicationUserManager UserManager;

        public HomeController()
        {
        }
        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        private int pagesize = 6, pageindex = 1;

        public ActionResult Index()
        {
            countWishItems();

            HomeListsViewModel homeListsViewModel = new HomeListsViewModel()
            {
                newLaptopsList = db.Products.Where(p => p.fk_category_id == (int)CategoryEnum.Laptops).OrderByDescending(p => p.product_createDate).Take(5).ToList(),
                newSmartphonesList = db.Products.Where(p => p.fk_category_id == (int)CategoryEnum.Smartphones).OrderByDescending(p => p.product_createDate).Take(5).ToList(),
                newCamerasList = db.Products.Where(p => p.fk_category_id == (int)CategoryEnum.Cameras).OrderByDescending(p => p.product_createDate).Take(5).ToList(),
                newAccessoriesList = db.Products.Where(p => p.fk_category_id == (int)CategoryEnum.Accessories).OrderByDescending(p => p.product_createDate).Take(5).ToList(),

                tendLaptopsList = db.Products.Where(p => p.fk_category_id == (int)CategoryEnum.Laptops).OrderByDescending(p => p.product_soldQuantity).Take(5).ToList(),
                tendSmartphonesList = db.Products.Where(p => p.fk_category_id == (int)CategoryEnum.Smartphones).OrderByDescending(p => p.product_soldQuantity).Take(5).ToList(),
                tendCamerasList = db.Products.Where(p => p.fk_category_id == (int)CategoryEnum.Cameras).OrderByDescending(p => p.product_soldQuantity).Take(5).ToList(),
                tendAccessoriesList = db.Products.Where(p => p.fk_category_id == (int)CategoryEnum.Accessories).OrderByDescending(p => p.product_soldQuantity).Take(5).ToList(),

                topSellingList = db.Products.OrderByDescending(p => p.product_soldQuantity).Take(6).ToList()
            };
            return View(homeListsViewModel);
        }
        public ActionResult Categories(int? page)
        {
            throw new InvalidOperationException("Logfile cannot be read-only");
            var productList = getProducts(true, page);

            return View(productList);
        }
        public ActionResult Laptops(int? page)
        {
            var productList = getProducts(false, page, (int)CategoryEnum.Laptops);
            return View(productList);
        }
        public ActionResult Smartphones(int? page)
        {
            var productList = getProducts(false, page, (int)CategoryEnum.Smartphones);
            return View(productList);
        }
        public ActionResult Cameras(int? page)
        {
            var productList = getProducts(false, page, (int)CategoryEnum.Cameras);
            return View(productList);
        }
        public ActionResult Accessories(int? page)
        {
            var productList = getProducts(false, page, (int)CategoryEnum.Accessories);
            return View(productList);
        }
        [HttpGet]
        public ActionResult Search(string searchOption, int? page, string search = "")
        {
            pagesize = 8;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            List<Product> allProducts = db.Products.Where(p => p.product_name.Contains(search)).ToList();
            List<Product> filtedProducts = new List<Product>();
            switch (searchOption)
            {
                case "1":
                    allProducts = allProducts.Where(p => p.fk_category_id == (int)CategoryEnum.Laptops).ToList();
                    break;
                case "2":
                    allProducts = allProducts.Where(p => p.fk_category_id == (int)CategoryEnum.Smartphones).ToList();
                    break;
                case "3":
                    allProducts = allProducts.Where(p => p.fk_category_id == (int)CategoryEnum.Cameras).ToList();
                    break;
                case "4":
                    allProducts = allProducts.Where(p => p.fk_category_id == (int)CategoryEnum.Accessories).ToList();
                    break;
                default:
                    break;
            }

            if (allProducts.Count <= 0 || search == "")
            {
                SearchViewModel emptyModel = new SearchViewModel()
                {
                    totalCount = allProducts.Count(),
                    serachWord = search
                };
                return View(emptyModel);
            }
            SearchViewModel searchModel = new SearchViewModel()
            {
                totalCount = allProducts.Count(),
                serachWord = search
            };
            var userId = User.Identity.GetUserId();
            List<ProductsForList> productsList = new List<ProductsForList>();
            foreach (var pro in allProducts)
            {
                ProductsForList product = new ProductsForList()
                {
                    product = pro,
                    category_name = db.Categories.Where(c => c.category_id == pro.fk_category_id).FirstOrDefault().category_name,
                };
                if (userId != null)
                {
                    var item = db.WishItems.Where(u => u.fk_user_id == userId && u.fk_product_id == pro.product_id).FirstOrDefault();
                    if (item != null)
                    {
                        product.inWishList = true;
                    }
                }
                productsList.Add(product);
            }
            searchModel.products = productsList.ToPagedList(pageindex, pagesize);
            return View(searchModel);
        }
        public ActionResult Product(int? page, int pro_id = 1)
        {

            var product = db.Products.Where(p => p.product_id == pro_id).FirstOrDefault();
            if(product == null)
            {
                return RedirectToAction("ProductNotFound", "Error", new { id = pro_id });
            }
            ProductViewModel model = new ProductViewModel()
            {
                product = product,
                reviewsCount = db.Reviewers.Where(r => r.fk_product_id == product.product_id).Count(),
                category_name = db.Categories.Where(c => c.category_id == product.fk_category_id).FirstOrDefault().category_name
            };
            if (product.product_color != null)
            {
                string[] arrayColor = product.product_color.Split(',');
                foreach (var color in arrayColor)
                {
                    model.colors.Add(color);
                }
                ViewBag.selectColor = new SelectList(model.colors);
            }
            model.photos = db.Photos.Where(p => p.fk_product_id == pro_id).ToList();

            var reviewersModel = db.Reviewers.Where(r => r.fk_product_id == pro_id).ToList();
            decimal totalCount = reviewersModel.Count();
            decimal count = 0;
            var models = new CountRating();
            models.avgRating = 0;
            models.stars = new int[5];
            models.percentStars = new int[5];
            if (reviewersModel.Any())
            {
                models.avgRating = count / totalCount;
                foreach (var review in reviewersModel)
                {
                    count += (int)review.reviewer_rating;
                }
                for (var i = 0; i < 5; i++)
                {
                    models.stars[i] = reviewersModel.Where(r => r.reviewer_rating == i + 1).Count();
                }
                for (var i = 0; i < 5; i++)
                {
                    models.percentStars[i] = models.stars[i] * 100 / (int)totalCount;
                }
            }
            model.countRating = models;

            pagesize = 3;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            model.reviewers = reviewersModel.ToPagedList(pageindex, pagesize);

            model.relatedProducts = db.Products.Where(p => p.fk_category_id == product.fk_category_id).Take(4).ToList();

            return View(model);
        }
        public ActionResult ViewCart()
        {
            return View();
        }
        public ActionResult Checkout()
        {
            var userId = User.Identity.GetUserId();
            AspNetUser user;
            if (userId != null)
            {
                user = db.AspNetUsers.Where(u => u.Id == userId).FirstOrDefault();
            }
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                AdditionalShippingAddress ship = null;
                if (model.DifferentAddressBool)
                {
                    ship = new AdditionalShippingAddress()
                    {
                        shipping_firstName = model.SPfirstName,
                        shipping_lastName = model.SPlastName,
                        shipping_email = model.SPemail,
                        shipping_address = model.SPaddress,
                        shipping_createDate = DateTime.Now,
                        shipping_city = model.SPcity,
                        shipping_country = model.SPcountry,
                        shipping_zipCode = model.SPzipCode,
                        shipping_telephone = model.SPtelephone,

                    };
                    db.AdditionalShippingAddresses.Add(ship);
                    ship = db.AdditionalShippingAddresses.Where(a => a.shipping_email == ship.shipping_email).OrderByDescending(d => d.shipping_createDate).FirstOrDefault();
                }
                ApplicationUser user = null;
                if (model.CreateAccountBool)
                {
                    user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Address = model.Address, City = model.City, Country = model.Country, ZipCode = model.ZipCode, PhoneNumber = model.Telephone };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        return View(model);
                    }
                }
                var bill = new BillingAddress
                {
                    billing_firstName = model.FirstName,
                    billing_lastName = model.LastName,
                    billing_email = model.Email,
                    billing_address = model.Address,
                    billing_city = model.City,
                    billing_country = model.Country,
                    billing_zipCode = model.ZipCode,
                    billing_telephone = model.Telephone,
                    billing_createDate = DateTime.Now,
                    billing_note = model.Note
                };
                if (user != null)
                {
                    bill.fk_user_id = user.Id;
                }
                db.BillingAddresses.Add(bill);
                db.SaveChanges();
                bill = db.BillingAddresses.Where(b => b.billing_email == bill.billing_email).OrderByDescending(d => d.billing_createDate).FirstOrDefault();

                decimal? TotalPrice = 0;

                var ord = new Order
                {
                    order_createDate = DateTime.Now,
                    order_totalPrice = null,
                    order_shippingPrice = null,
                    order_status = "RESERVATION",
                    fk_billing_id = bill.billing_id,
                };
                if (user != null)
                {
                    ord.fk_user_id = user.Id;
                }
                if (ship != null)
                {
                    ord.fk_shipping_id = ship.shipping_id;
                }
                db.Orders.Add(ord);
                db.SaveChanges();
                ord = db.Orders.Where(o => o.fk_billing_id == bill.billing_id).OrderByDescending(o => o.order_createDate).FirstOrDefault();

                List<CartItem> cart = (List<CartItem>)Session["cart"];
                if (cart != null)
                {
                    foreach (var item in cart)
                    {
                        var line = new ItemOrder
                        {
                            item_quantity = item.Quantity,
                            item_discount = item.Discount,
                            item_quantityPrice = item.Price,
                            fk_product_id = item.Id,
                            fk_order_id = ord.order_id
                        };
                        TotalPrice += line.item_quantityPrice;
                        db.ItemOrders.Add(line);
                        db.SaveChanges();

                        var pro = db.Products.Where(p => p.product_id == item.Id).FirstOrDefault();
                        if (pro != null)
                        {
                            pro.product_quantity = pro.product_quantity - line.item_quantity;
                            pro.product_soldQuantity = pro.product_quantity + line.item_quantity;
                            db.SaveChanges();
                        }
                    }
                }
                ord.order_totalPrice = TotalPrice;
                db.SaveChanges();

                return RedirectToAction("Payment");
            }
            return View(model);
        }
        [Authorize]
        public ActionResult WishList(int? page)
        {
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            var userId = User.Identity.GetUserId();

            var wishList = db.WishItems.Where(p => p.fk_user_id == userId).ToList();
            List<ProductsForList> productsList = new List<ProductsForList>();
            foreach (var pro in wishList)
            {
                var prod = db.Products.Where(p => p.product_id == pro.fk_product_id).FirstOrDefault();
                ProductsForList product = new ProductsForList()
                {
                    product = prod,
                    category_name = db.Categories.Where(c => c.category_id == prod.fk_category_id).FirstOrDefault().category_name
                };
                productsList.Add(product);
            }
            WishViewModel wishViewModel = new WishViewModel();
            wishViewModel.totalCount = productsList.Count();
            wishViewModel.products = productsList.ToPagedList(pageindex, pagesize);

            return View(wishViewModel);
        }

        public ActionResult Payment()
        {
            return View();
        }
        public JsonResult rating(string clientName, string clientEmail, string clientText, int Rating, int productId)
        {
            Reviewer reviewer = new Reviewer()
            {
                reviewer_name = clientName,
                reviewer_email = clientEmail,
                reviewer_comment = clientText,
                reviewer_date = DateTime.Now,
                reviewer_rating = (byte)Rating,
                fk_product_id = productId
            };
            db.Reviewers.Add(reviewer);

            var product = db.Products.Where(p => p.product_id == productId).FirstOrDefault();
            var productReviewers = db.Reviewers.Where(r => r.fk_product_id == product.product_id).ToList();
            if (product != null && productReviewers.Any())
            {
                decimal totalCount = productReviewers.Count();
                decimal count = 0;
                foreach (var review in productReviewers)
                {
                    count += (int)review.reviewer_rating;
                }
                product.avg_rating = count / totalCount;
            }
            db.SaveChanges();

            return Json(new { success = true, responseText = "Your message successfuly sent!" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddToCart(int pro_id, string category = null)
        {
            var photo = db.Photos.Where(p => p.fk_product_id == pro_id).FirstOrDefault();
            var pro = db.Products.Find(pro_id);
            var product_category = db.Categories.Where(c => c.category_id == pro.fk_category_id).FirstOrDefault();
            if (Session["cart"] == null)
            {
                var cart = new List<CartItem>();
                cart.Add(new CartItem()
                {
                    Id = pro.product_id,
                    Category = product_category.category_name,
                    ProductName = pro.product_name,
                    Price = (decimal)pro.product_discountprice,
                    Discount = pro.product_discount,
                    Photo = pro.product_mainPhoto,
                    Color = pro.product_color,
                    Size = pro.product_size,

                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<CartItem> cart = (List<CartItem>)Session["cart"];
                int addedItem = 0;
                foreach (var item in cart)
                {
                    if (item.Id == pro.product_id)
                    {
                        var exist = cart.Where(i => i.Id == pro.product_id).FirstOrDefault();
                        exist.Quantity++;
                        addedItem++;
                        break;
                    }
                }
                if (addedItem == 0)
                {
                    cart.Add(new CartItem()
                    {
                        Id = pro.product_id,
                        Category = product_category.category_name,
                        ProductName = pro.product_name,
                        Price = (decimal)pro.product_discountprice,
                        Discount = pro.product_discount,
                        Photo = pro.product_mainPhoto,
                        Color = pro.product_color,
                        Size = pro.product_size,

                        Quantity = 1
                    });
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("ViewCart");
        }
        public ActionResult IncreaseQty(int id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            foreach (var pro in cart)
            {
                if (pro.Id == id)
                {
                    pro.Quantity++;
                }
            }
            return RedirectToAction("ViewCart");
        }
        public ActionResult DecreaseQty(int id)
        {
            List<CartItem> cart = (List<CartItem>)Session["cart"];
            foreach (var pro in cart)
            {
                if (pro.Id == id)
                {
                    if (pro.Quantity > 1)
                    {
                        pro.Quantity--;
                        break;
                    }
                    else
                    {
                        cart.Remove(pro);
                        break;
                    }
                }
            }
            Session["cart"] = cart;
            return RedirectToAction("ViewCart");
        }
        [Authorize]
        public ActionResult RemoveFromWishList(int pro_id)
        {
            var pro = db.Products.Find(pro_id);
            var userId = User.Identity.GetUserId();

            if (userId == null || pro == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var product = db.WishItems.Where(w => w.fk_user_id == userId && w.fk_product_id == pro.product_id).FirstOrDefault();

            db.WishItems.Remove(product);
            db.SaveChanges();
            return RedirectToAction("wishlist");
        }
        [Authorize]
        public ActionResult AddToWishList(int pro_id)
        {
            var pro = db.Products.Find(pro_id);
            var userId = User.Identity.GetUserId();

            var itemExist = db.WishItems.Where(w=>w.fk_product_id == pro.product_id && w.fk_user_id == userId).FirstOrDefault();
            if (itemExist != null)
            {
                RedirectToAction("wishlist");
            }

            if (userId == null || pro == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var list = new WishItem { fk_product_id = pro.product_id, fk_user_id = userId };

            var nan = db.WishItems.Add(list);
            db.SaveChanges();

            return RedirectToAction("wishlist");
        }
        private ProductListViewModel getProducts(bool withCategories, int? page, int categoryId = 0)
        {
            ProductListViewModel productList = new ProductListViewModel();
            // aside bar: filter categories
            if (withCategories)
            {
                var catogories = db.Categories.ToList();
                foreach (var category in catogories)
                {
                    var categoryWithCount = new CategoryWithCount()
                    {
                        category = category,
                        countCategories = db.Products.Where(p => p.fk_category_id == category.category_id).Count()
                    };
                    productList.categoriesWithCount.Add(categoryWithCount);
                }
            }
            // aside bar: filter brands
            var brands = new List<Brand>();
            switch (categoryId)
            {
                case 1:
                    brands = db.Brands.Where(b => b.fk_cat_id == categoryId).ToList();
                    break;
                case 2:
                    brands = db.Brands.Where(b => b.fk_cat_id == categoryId).ToList();
                    break;
                case 3:
                    brands = db.Brands.Where(b => b.fk_cat_id == categoryId).ToList();
                    break;
                case 4:
                    brands = db.Brands.Where(b => b.fk_cat_id == categoryId).ToList();
                    break;
                default:
                    brands = db.Brands.Take(5).ToList();
                    break;
            }

            foreach (var brand in brands)
            {
                var brandWithCount = new BrandWithCount()
                {
                    brand = brand,
                    countBrands = db.Products.Where(p => p.fk_brand_id == brand.brand_id).Count()
                };
                productList.brandsWithCount.Add(brandWithCount);
            }

            var models = new List<Product>();
            switch (categoryId)
            {
                case 1:
                    models = db.Products.Where(p => p.fk_category_id == categoryId).ToList();
                    productList.Category = CategoryEnum.Laptops.ToString();
                    break;
                case 2:
                    models = db.Products.Where(p => p.fk_category_id == categoryId).ToList();
                    productList.Category = CategoryEnum.Smartphones.ToString();
                    break;
                case 3:
                    models = db.Products.Where(p => p.fk_category_id == categoryId).ToList();
                    productList.Category = CategoryEnum.Cameras.ToString();
                    break;
                case 4:
                    models = db.Products.Where(p => p.fk_category_id == categoryId).ToList();
                    productList.Category = CategoryEnum.Accessories.ToString();
                    break;
                default:
                    models = db.Products.ToList();
                    break;
            }
            List<ProductsForList> ProductsList = new List<ProductsForList>();
            var userId = User.Identity.GetUserId();
            foreach (var product in models)
            {
                ProductsForList pro = new ProductsForList()
                {
                    product = product,
                    category_name = db.Categories.Where(c => c.category_id == product.fk_category_id).FirstOrDefault().category_name
                };
                if (userId != null)
                {
                    var item = db.WishItems.Where(u => u.fk_user_id == userId && u.fk_product_id == product.product_id).FirstOrDefault();
                    if (item != null)
                    {
                        pro.inWishList = true;
                    }
                }
                ProductsList.Add(pro);
            }
            // Top 3 Products
            var top3Models = db.Products.OrderByDescending(p => p.product_soldQuantity).Take(3);
            List<ProductsForList> topProducts = new List<ProductsForList>();
            foreach (var product in top3Models)
            {
                ProductsForList topPro = new ProductsForList()
                {
                    product = product,
                    category_name = db.Categories.Where(c => c.category_id == product.fk_category_id).FirstOrDefault().category_name
                };
                topProducts.Add(topPro);
            }
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

            productList.totalProducts = ProductsList.Count();
            productList.products = ProductsList.ToPagedList(pageindex, pagesize);
            productList.topProducts = topProducts;

            return productList;
        }
        public ActionResult filterAjax(string categoriesString = null, string brandsString = null,
          int? changedMinPrice = null, int? changedMaxPrice = null, int changedSorting = 6, int? changedPage = 6)
        {

            ProductListViewModel productList = new ProductListViewModel();

            if (categoriesString == "" && brandsString == "" && changedMinPrice == 1 && changedMaxPrice == 9999 && changedSorting == 6)
            {
                var allProducts = db.Products.ToList();
                List<ProductsForList> SameProductsList = new List<ProductsForList>();
                foreach (var product in allProducts)
                {
                    ProductsForList pro = new ProductsForList()
                    {
                        product = product,
                        category_name = db.Categories.Where(c => c.category_id == product.fk_category_id).FirstOrDefault().category_name
                    };
                    SameProductsList.Add(pro);
                }
                productList.products = SameProductsList.ToPagedList(1, 6);
                return PartialView("_Store", productList);
            }
            List<Product> categoryProducts = new List<Product>();
            string[] checkedCategories = categoriesString.Split('+');

            if (categoriesString != "")
            {
                foreach (string category in checkedCategories)
                {
                    var foundCategory = db.Categories.Where(c => c.category_name.ToLower() == category.ToLower()).FirstOrDefault();
                    if (foundCategory != null)
                    {
                        var products = db.Products.Where(p => p.fk_category_id == foundCategory.category_id).ToList();
                        categoryProducts.AddRange(products);
                    }
                }
            }
            else
            {
                categoryProducts = db.Products.ToList();
            }

            List<Product> filteredProducts = new List<Product>();
            string[] checkedBrands = brandsString.Split('+');

            if (brandsString != "")
            {
                foreach (string brand in checkedBrands)
                {
                    var foundBrand = db.Brands.Where(b => b.brand_name.ToLower() == brand.ToLower()).FirstOrDefault();
                    if (foundBrand != null)
                    {
                        var products = categoryProducts.Where(p => p.fk_brand_id == foundBrand.brand_id).ToList();
                        filteredProducts.AddRange(products);
                    }
                }
            }
            else
            {
                filteredProducts = categoryProducts;
            }

            if (changedMinPrice != 1 || changedMaxPrice != 9999)
            {
                filteredProducts = filteredProducts.Where(p => p.product_price > changedMinPrice && p.product_price < changedMaxPrice).ToList();
            }

            switch (changedSorting)
            {
                case 0:
                    // break
                    break;
                case 1:
                    //Popular
                    break;
                case 2:
                    //Price(Low To High)
                    filteredProducts = filteredProducts.OrderBy(p => p.product_discountprice).ToList();
                    // So on...
                    break;
                case 3:
                    //Price(High To Low)
                    filteredProducts = filteredProducts.OrderByDescending(p => p.product_discountprice).ToList();
                    break;
                case 4:
                    //Discount(High To Low)
                    filteredProducts = filteredProducts.OrderByDescending(p => p.product_discount).ToList();
                    break;
                default:
                    break;
            }
            List<ProductsForList> ProductsList = new List<ProductsForList>();
            foreach (var product in filteredProducts)
            {
                ProductsForList pro = new ProductsForList()
                {
                    product = product,
                    category_name = db.Categories.Where(c => c.category_id == product.fk_category_id).FirstOrDefault().category_name
                };
                ProductsList.Add(pro);
            }
            productList.products = ProductsList.ToPagedList(pageindex, (int)changedPage);
            return PartialView("_Store", productList);
        }
        private void countWishItems()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                if (Session["wishCart"] == null)
                {
                    var wishCart = new WishCount();
                    wishCart.Count = db.WishItems.Where(p => p.fk_user_id == userId).Count();
                    Session["wishCart"] = wishCart;
                }
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            ViewResult view = new ViewResult();
            view.ViewName = "Error";
            view.ViewBag.errorTitle = "Error";
            //view.ViewBag.errorMessage = "Message: " filterContext.Exception.Message;
            filterContext.Result = view;
            filterContext.ExceptionHandled = true;
        }
    }
}

//public ActionResult Reviewer(int? page, int id = 1)
//{
//    ProductViewModel model = new ProductViewModel();
//    var reviewersModel = db.Reviewers.Where(r => r.fk_product_id == 1).ToList();

//    pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
//    model.reviewers = reviewersModel.ToPagedList(pageindex, 3);
//    return PartialView("_Reviews", model);
//}
//public ActionResult RatingStars(int id = 1)
//{
//    ProductViewModel model = new ProductViewModel();
//    var reviewersModel = db.Reviewers.Where(r => r.fk_product_id == 1).ToList();
//    var totalCount = reviewersModel.Count();
//    int count = 0;
//    foreach (var review in reviewersModel)
//    {
//        count += (int)review.reviewer_rating;
//    }
//    model.countRating.avgRating = count / totalCount;
//    model.countRating.stars = new int[5];
//    model.countRating.percentStars = new int[5];

//    for (var i = 0; i < 5; i++)
//    {
//        model.countRating.stars[i] = reviewersModel.Where(r => r.reviewer_rating == i + 1).Count();
//    }
//    for (var i = 0; i < 5; i++)
//    {
//        model.countRating.percentStars[i] = model.countRating.stars[i] * 100 / totalCount;
//    }

//    return PartialView("_RatingStars", model);
//}