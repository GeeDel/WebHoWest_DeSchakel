using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeSchakel.Client.Mvc.Areas.User.Viewmodels
{
    public class UserAccountUpdateViewmodel
    {
        [HiddenInput]
        public string Id { get; set; }
        [HiddenInput]
        [Display(Name = "Emailadres")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Graag een geldig emailadres gebruiken.")]
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
        [Display(Name = "Geboortedatum")]
        [Required(ErrorMessage = "Geef een geboortedatum.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}")]
        public DateTime DateOfBirth { get; set; }
    }
    }