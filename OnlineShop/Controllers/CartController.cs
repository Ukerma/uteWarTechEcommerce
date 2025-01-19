using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShop.Models.Db;
using OnlineShop.Models.ViewModels;
using System.Security.Claims;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private OnlineShopContext _context;

        public CartController(OnlineShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = GetProductsinCart();
            return View(result);
        }

        public IActionResult ClearCart()
        {
            Response.Cookies.Delete("Cart");
            return Redirect("/");
        }

        /// <summary>
        /// Add or update the shopping cart
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="count">
        ///  If quantity is zero, it means the intention is to remove the item. 
        /// This case is manually handled by us.
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateCart([FromBody] CartViewModel request)
        {

            var product = _context.Products.FirstOrDefault(x => x.Id == request.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            if (product.Qty<request.Count)
            {
                return BadRequest();
            }
            // Retrieve the list of products in the cart using the dedicated function
            var cartItems = GetCartItems();

            var foundProductInCart = cartItems.FirstOrDefault(x => x.ProductId == request.ProductId);

            // If the product is found, it means it is in the cart, and the user intends to change the quantity
            if (foundProductInCart == null)
            {
                var newCartItem = new CartViewModel() { };
                newCartItem.ProductId = request.ProductId;
                newCartItem.Count = request.Count;

                cartItems.Add(newCartItem);
            }
            else
            {
                // If greater than zero, it means the user wants to update the quantity; otherwise, it will be removed from the cart.
                if (request.Count > 0)
                {
                    foundProductInCart.Count = request.Count;
                }
                else
                {
                    cartItems.Remove(foundProductInCart);
                }
            }

            var json = JsonConvert.SerializeObject(cartItems);

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append("Cart", json, option);

            var result = cartItems.Sum(x => x.Count);

            return Ok(result);
        }

        public List<CartViewModel> GetCartItems()
        {
            List<CartViewModel> cartList = new List<CartViewModel>();

            var prevCartItemsString = Request.Cookies["Cart"];

            // If it's not null, it means the cart is not empty, so we need to convert it to a list of view models; 
            // otherwise, we return an empty cart list.
            if (!string.IsNullOrEmpty(prevCartItemsString))
            {
                cartList = JsonConvert.DeserializeObject<List<CartViewModel>>(prevCartItemsString);
            }

            return cartList;
        }

        public List<ProductCartViewModel> GetProductsinCart()
        {
            var cartItems = GetCartItems();

            if (!cartItems.Any())
            {
                return null;
            }

            var cartItemProductIds = cartItems.Select(x => x.ProductId).ToList();
            // Load products into memory
            var products = _context.Products
                .Where(p => cartItemProductIds.Contains(p.Id))
                .ToList();

            // Create the ProductCartViewModel list

            List<ProductCartViewModel> result = new List<ProductCartViewModel>();
            foreach (var item in products)
            {
                var newItem = new ProductCartViewModel
                {
                    Id = item.Id,
                    ImageName = item.ImageName,
                    Price = item.Price - (item.Discount ?? 0),
                    Title = item.Title,
                    Count = cartItems.Single(x => x.ProductId == item.Id).Count,
                    RowSumPrice = (item.Price - (item.Discount ?? 0)) * cartItems.Single(x => x.ProductId == item.Id).Count,
                };

                result.Add(newItem);
            }

            return result;
        }

        public IActionResult SmallCart()
        {
            var result = GetProductsinCart();
            return PartialView(result);
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var order = new Models.Db.Order();

            var shipping = _context.Settings.First().Shipping;
            if (shipping != null)
            {
                order.Shipping = shipping;
            }

            ViewData["Products"] = GetProductsinCart();
            return View(order);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ApplyCouponCode([FromForm] string couponCode)
        {
            var order = new Models.Db.Order();

            var coupon = _context.Coupons.FirstOrDefault(c => c.Code == couponCode);

            if (coupon != null)
            {
                order.CouponCode = coupon.Code;
                order.CouponDiscount = coupon.Discount;
            }
            else
            {
                ViewData["Products"] = GetProductsinCart();
                TempData["message"] = "Coupon not exitst";
                return View("Checkout", order);
            }

            var shipping = _context.Settings.First().Shipping;
            if (shipping != null)
            {
                order.Shipping = shipping;
            }

            ViewData["Products"] = GetProductsinCart();
            return View("Checkout", order);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Models.Db.Order order)
        {
            if (!ModelState.IsValid)
            {

                ViewData["Products"] = GetProductsinCart();

                return View(order);
            }

            //-------------------------------------------------------

            //check and find coupon
            if (!string.IsNullOrEmpty(order.CouponCode))
            {
                var coupon = _context.Coupons.FirstOrDefault(c => c.Code == order.CouponCode);

                if (coupon != null)
                {
                    order.CouponCode = coupon.Code;
                    order.CouponDiscount = coupon.Discount;
                }
                else
                {
                    TempData["message"] = "Coupon not exitst";
                    ViewData["Products"] = GetProductsinCart();

                    return View(order);
                }
            }

            var products = GetProductsinCart();

            order.Shipping = _context.Settings.First().Shipping;
            order.CreateDate = DateTime.Now;
            order.SubTotal = products.Sum(x => x.RowSumPrice);
            order.Total = (order.SubTotal + order.Shipping ?? 0);
            order.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (order.CouponDiscount != null)
            {
                order.Total -= order.CouponDiscount;
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            //-------------------------------------------------------

            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var item in products)
            {
                OrderDetail orderDetailItem = new OrderDetail()
                {
                    Count = item.Count,
                    ProductTitle = item.Title,
                    ProductPrice = (decimal)item.Price,
                    OrderId = order.Id,
                    ProductId = item.Id
                };

                orderDetails.Add(orderDetailItem);
            }
            //-------------------------------------------------------
            _context.OrderDetails.AddRange(orderDetails);
            _context.SaveChanges();

            // Redirect to PayPal
            return Redirect("/");
        }
    }
}
