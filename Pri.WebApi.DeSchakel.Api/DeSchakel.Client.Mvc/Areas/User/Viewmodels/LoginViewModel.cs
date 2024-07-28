using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeSchakel.Client.Mvc.Areas.User.Viewmodels
{
    public class LoginViewModel
    {
        [HiddenInput]
        public string ReturnUrl { get; set; }
        [Required(ErrorMessage = "Je moet een emailadresingeven.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Je moet een paswoord ingeven.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
