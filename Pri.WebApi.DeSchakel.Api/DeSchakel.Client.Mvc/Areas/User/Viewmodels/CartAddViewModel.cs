using DeSchakel.Client.Mvc.Areas.User.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Areas.User.Viewmodels
{
    public class CartAddViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public CartItemModel CartItem { get; set; }
    }
}
