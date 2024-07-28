using Pri.WebApi.DeSchakel.Api.Dtos.ApplicationUser;
using Pri.WebApi.DeSchakel.Api.Dtos.Company;
using Pri.WebApi.DeSchakel.Api.Dtos.Genre;
using Pri.WebApi.DeSchakel.Api.Dtos.Location;
using Pri.WebApi.DeSchakel.Core.Entities;

namespace Pri.WebApi.DeSchakel.Api.Dtos.Event
{
    public class EventResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Imagestring { get; set; }
        // Genre
        public IEnumerable<GenreResponseDto> Genres { get; set; }
        //  Company
        public CompanyResponseDto Company { get; set; }
        // Location
        public LocationResponseDto Location { get; set; }
        // Programators
        public IEnumerable<ApplicationUserResponseDto> Programmators { get; set; }
    }
}
