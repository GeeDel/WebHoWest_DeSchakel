using DeSchakelApi.Consumer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffEventCreateViewmodel
    {
        [Required(ErrorMessage = "Titel moet een waarde hebben")]
        [MaxLength(100, ErrorMessage = "Invoer is te lang")]
        public string Title { get; set; }
        [Required(ErrorMessage = "De datum moet een waarde hebben.")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyyTHH:mm}")]
        [Display(Name = "Datum van de voorstelling")] 
        public DateTime EventDate { get; set; }
        [StringLength(450)]
        [Required(ErrorMessage = "De beschrijving moet een waarde hebben.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "De prijs is verplicht.")]
        public double Price { get; set; }
        public int SuccesRate { get; set; }
        [Display(Name = "Laad een afbeelding op")]
        public IEnumerable<IFormFile> Images { get; set; }
        public string Imagestring { get; set; }
        // Company
        public IEnumerable<SelectListItem> Companies { get; set; }
        [HiddenInput]
        public int CompanyId { get; set; }
        // one location
        public IEnumerable<SelectListItem> Locations { get; set; }
        [Display(Name = "Locatie")] 
        [HiddenInput]
        public int LocationId { get; set; }
        // many genres
        public List<CheckBoxItem> Genres { get; set; }
        [HiddenInput]
        public List<int> GenreIds { get; set; }
        // progammator
        public IEnumerable<SelectListItem> Programmators { get; set; }
        public List<string> ProgrammatorIds { get; set; }

    }
}
