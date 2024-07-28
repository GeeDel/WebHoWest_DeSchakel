using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Areas.User.Viewmodels
{
    public class CartDeleteViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
