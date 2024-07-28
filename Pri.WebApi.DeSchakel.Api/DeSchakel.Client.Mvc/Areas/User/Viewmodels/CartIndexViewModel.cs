using DeSchakel.Client.Mvc.Areas.User.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Areas.User.Viewmodels
{
    public class CartIndexViewModel
    {
        public List<CartItemModel> CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
