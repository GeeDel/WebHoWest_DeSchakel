using Pri.WebApi.DeSchakel.Api.Dtos.ApplicationUser;
using Pri.WebApi.DeSchakel.Api.Dtos.Company;
using Pri.WebApi.DeSchakel.Api.Dtos.Genre;
using Pri.WebApi.DeSchakel.Api.Dtos.Location;
using Pri.WebApi.DeSchakel.Core.Entities;
using System.ComponentModel.DataAnnotations;


namespace Pri.WebApi.DeSchakel.Api.Dtos.Event
{
    public class EventRequestDto
    {
//        public int Id { get; set; }
//        [Required(ErrorMessage = "Invoer voor {0} is vereist")]
 //       [StringLength(150)]
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
 //       [Required(ErrorMessage = "Invoer voor {0} is vereist")]
 //       [StringLength(750)]
        public string Description { get; set; }
//        [Required(ErrorMessage = "Invoer voor {0} is vereist")]
        public double Price { get; set; }
        public int SuccesRate { get; set; }
        public string Imagestring { get; set; }
        // one Company
//        [Required(ErrorMessage = "Invoer voor {0} is vereist")]
        public int CompanyId { get; set; }
        // one location
//        [Required(ErrorMessage = "Invoer voor {0} is vereist")]
        public int LocationId { get; set; }
        // many genres
         public IEnumerable<GenreResponseDto> Genres { get; set; }
        public IEnumerable<int> GenreIds { get; set; }
        // progammator
       public IEnumerable<ApplicationUserResponseDto> Programmators {get; set;}
        public IEnumerable<string> ProgrammatorIds { get; set; }
    }
    }
