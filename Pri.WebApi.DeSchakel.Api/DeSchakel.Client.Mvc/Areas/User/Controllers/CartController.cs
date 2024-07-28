using DeSchakel.Client.Mvc.Areas.User.Models.Account;
using DeSchakel.Client.Mvc.Areas.User.Viewmodels;
using DeSchakelApi.Consumer.Events;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeSchakel.Client.Mvc.Areas.User.Controllers
{
    [Area("User")]
    public class CartController : Controller
    {
        private readonly IEventApiService _eventApiService;

        public CartController(IEventApiService eventApiService)
        {
            _eventApiService = eventApiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            CartIndexViewModel cartIndexViewModel = new CartIndexViewModel
            {
                CartItems = new()
            };
            // key.contains  ???
            if (User.Identity.IsAuthenticated &&  HttpContext.Session.Keys.Contains("SessionCartList"))
            {
                cartIndexViewModel.CartItems = JsonConvert.DeserializeObject<List<CartItemModel>>(HttpContext.Session.GetString("SessionCartList"));
            }
            // calculate total
            cartIndexViewModel.Total = cartIndexViewModel.CartItems.Sum(c => c.Price * c.Quantity);
            // calculate total quantity in cart and put in session
            HttpContext.Session.SetInt32("NumberOfItems", cartIndexViewModel.CartItems.Sum(c => c.Quantity));

            if (cartIndexViewModel.CartItems.Count() > 0)
            {
                // put into the cookie 
                //add to cookie
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMinutes(15)
                };
                HttpContext.Response.Cookies.Append("SessionCartList", HttpContext.Session.GetString("SessionCartList"), cookieOptions);
            }
            else
            {
                // Delete the cookie
                HttpContext.Response.Cookies.Delete("SessionCartList");
            }
            return View(cartIndexViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {

            var result = await _eventApiService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Errors);
            }
            var performance = result.Data;
            CartAddViewModel cartAddViewModel = new CartAddViewModel
            {
                CartItem = new CartItemModel
                {
                    Id = performance.Id,
                    Title = performance.Title,
                    ImageString = performance.Imagestring,
                    Price = 20,
                    Quantity = 1
                }
            };
            cartAddViewModel.CartItem.ItemsPrice = cartAddViewModel.CartItem.Price * cartAddViewModel.CartItem.Quantity;
            return View(cartAddViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CartAddViewModel cartAddViewModel)
        {
            List<CartItemModel> cartItems = new List<CartItemModel>();
            if (HttpContext.Session.Keys.Contains("SessionCartList"))
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItemModel>>(HttpContext.Session.GetString("SessionCartList"));
            }

            if (!ModelState.IsValid)
            {
                return View(cartAddViewModel);
            }

            // need to refill the CartItemModel
            CartItemModel newCartItem = new CartItemModel
            {
                Id = cartAddViewModel.Id,
                Title = cartAddViewModel.CartItem.Title,
                ImageString = cartAddViewModel.CartItem.ImageString,
                Price = 20,
                Quantity = cartAddViewModel.CartItem.Quantity
            };
            // check if cartItem alrady exits in the cart
            CartItemModel searchedCartItem = cartItems.FirstOrDefault(c => c.Id == cartAddViewModel.Id);
            if (searchedCartItem != null)
            {
                newCartItem.Quantity += searchedCartItem.Quantity;
                cartItems.Remove(searchedCartItem);
            }
            newCartItem.ItemsPrice = newCartItem.Price * newCartItem.Quantity;

            cartItems.Add(newCartItem);
            HttpContext.Session.SetString("SessionCartList", JsonConvert.SerializeObject(cartItems));
            HttpContext.Session.SetInt32("NumberOfItems", cartItems.Sum(c => c.Quantity));
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> ConfirmRemove(int id)
        {

            // look up in list from session
            // Get list of all cartItems
            List<CartItemModel> cartItems = new List<CartItemModel>();
            if (HttpContext.Session.Keys.Contains("SessionCartList"))
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItemModel>>(HttpContext.Session.GetString("SessionCartList"));
            }
            CartItemModel searchedCartItem = cartItems.FirstOrDefault(c => c.Id == id);
            // check if list or cartitem exists in the list
            if (cartItems == null || searchedCartItem == null)
            {
                ModelState.AddModelError("", "Er liep iets fout met het opzoeken in de winkelkar.");
            }
            CartDeleteViewModel cartDeleteViewModel = new CartDeleteViewModel { Id = id };
            if (!ModelState.IsValid)
            {
                return View(cartDeleteViewModel);
            }
            cartDeleteViewModel.Title = searchedCartItem.Title;
            return View(cartDeleteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(CartDeleteViewModel cartDeleteViewModel)
        {
            // Get list of all cartItems
            List<CartItemModel> cartItems = new List<CartItemModel>();
            if (HttpContext.Session.Keys.Contains("SessionCartList"))
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItemModel>>(HttpContext.Session.GetString("SessionCartList"));
            }
            CartItemModel searchedCartItem = cartItems.FirstOrDefault(c => c.Id == cartDeleteViewModel.Id);
            // check if list or cartitem exists in the list
            if (cartItems == null || searchedCartItem == null)
            {
                ModelState.AddModelError("", "Er liep iets fout met het opzoeken in de winkelkar.");
            }
            if (!ModelState.IsValid)
            {
                return View(cartDeleteViewModel);
            }
            cartItems.Remove(searchedCartItem);
            // put list back in and dimnush quantity
            HttpContext.Session.SetString("SessionCartList", JsonConvert.SerializeObject(cartItems));
            int newQuantity = cartItems.Sum(q => q.Quantity) - searchedCartItem.Quantity;
            HttpContext.Session.SetInt32("NumberOfItems", newQuantity);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Order()
        {
            return View();
        }
    }
}
