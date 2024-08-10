using Pri.WebApi.DeSchakel.Api.Dtos.ApplicationUser;
using Pri.WebApi.DeSchakel.Api.Dtos.Genre;

namespace Pri.WebApi.DeSchakel.Api.Dtos.Event
{
    public class AddEventRequestMultipartDto
    {

        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int SuccesRate { get; set; }
        // one Company
        public int CompanyId { get; set; }
        // one location
        public int LocationId { get; set; }
        // many genres
       // public IEnumerable<GenreResponseDto> Genres { get; set; }
        public List<int> GenreIds { get; set; }
        // progammator
      //  public IEnumerable<ApplicationUserResponseDto> Programmators { get; set; }
        public List<string> ProgrammatorIds { get; set; }
        public IEnumerable<IFormFile> filesToUpload { get; set; }
    }
}
