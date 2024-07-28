using DeSchakel.Client.Mvc.Areas.User.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffAccountCreateViewmodel : UserRegisterViewmodel
    {
/*
        [Display(Name = "Emailadres")]
        [DataType(DataType.EmailAddress,       ErrorMessage = "Graag een geldig emailadres gebruiken.")]
        [Required(ErrorMessage = "Gebruikersnaam mag niet leeg zijn.")]
        public string Email { get; set; }
        [Display(Name = "Voornaam")]
        // hoeft niet bij string 
        [Required(ErrorMessage = "Voornaam mag niet leeg zijn.")]
        public string Firstname { get; set; }
        [Display(Name = "Familienaam")]
        [Required(ErrorMessage = "Naam mag niet leeg zijn.")]
        public string Lastname { get; set; }
        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "Postcode mag niet leeg zijn.")]
        public string Zipcode { get; set; }
        [Display(Name = "Gemeente")]
        [Required(ErrorMessage = "Gemeente mag niet leeg zijn.")]
        public string City { get; set; }
        [Display(Name = "Wachtwoord")]
        [DataType(DataType.Password)]  // no errormessage
        [Required(ErrorMessage = "Wachtwoord mag niet leeg zijn.")]
        [MinLength(8, ErrorMessage = "Wachtwoord moet 8 karakters lang zijn")]
        public string Password { get; set; }
        [Display(Name = "Herhaal wachtwoord")]
        [DataType(DataType.Password)]  // no errormessage
        [Required(ErrorMessage = "Herhaalwachtwoord mag niet leeg zijn.")]
        // Nieuw: gebruik compare
        [Compare("Password", ErrorMessage = "Wachtwoorden moeten gelijk zijn.")]
        public string CheckPassword { get; set; }
        [Display(Name = "Geboortedatum")]
        [Required(ErrorMessage = "Geef een geboortedatum.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        public DateTime DateOfBirth { get; set; }
 */
        [HiddenInput]
        public IEnumerable<string> RoleIds { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
