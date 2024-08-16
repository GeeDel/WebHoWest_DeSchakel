using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffAccountUpdatePasswordViewmodelToBeDeleted
    {
        [HiddenInput]
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
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

    }
}
