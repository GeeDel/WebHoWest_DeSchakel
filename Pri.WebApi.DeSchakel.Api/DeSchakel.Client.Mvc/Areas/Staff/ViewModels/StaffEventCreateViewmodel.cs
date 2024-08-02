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
        /*
         https://stackoverflow.com/questions/17321948/is-there-a-rangeattribute-for-datetime/17322252#17322252
        */
        [CustomEventDateAttribute(ErrorMessage = "Een datum vanaf morgen en maximum één jaar in de toekomst")]
        [Display(Name = "Datum van de voorstelling")] 
        public DateTime EventDate { get; set; }
        [StringLength(450)]
        [Required(ErrorMessage = "De beschrijving moet een waarde hebben.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "De prijs is verplicht.")]
        [Range(0,999, ErrorMessage = "Prijs boven 0 en onder 1000 euro")]
        public double Price { get; set; }
        public int SuccesRate { get; set; }
        [Display(Name = "Laad  een afbeelding op (max 1), optioneel één video en één geluidsfragment")]
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

    /*
     * https://stackoverflow.com/questions/17321948/is-there-a-rangeattribute-for-datetime/17322252#17322252
    */
    public class CustomEventDateAttribute : RangeAttribute
    {
        public CustomEventDateAttribute()
          : base(typeof(DateTime),
                  DateTime.Now.AddDays(1).ToString(),
                  DateTime.Now.AddDays(365).ToString())
        { }
    }
}
