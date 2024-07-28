using System.ComponentModel.DataAnnotations;

namespace DeSchakel.Client.Mvc.Areas.User.Models.Account
{
    public class CartItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Range(1, 999, ErrorMessage = "Aantal tussen 1 en 999")]
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ItemsPrice { get; set; }
        public string ImageString { get; set; }
    }
}
