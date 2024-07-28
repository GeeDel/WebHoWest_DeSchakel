using Pri.WebApi.DeSchakel.Api.Dtos.Role;

namespace Pri.WebApi.DeSchakel.Api.Dtos.ApplicationUser
{
    public class ApplicationUserResponseDto
    {
        
        public string Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        //
        public IList<string> Roles { get; set;}
        
    }
}
