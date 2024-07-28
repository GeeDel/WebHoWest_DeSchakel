using System.ComponentModel.DataAnnotations;

namespace Pri.WebApi.DeSchakel.Api.Dtos.ApplicationUser
{
    public class RegisterUserRequestDto : LoginUserRequestDto
    {


        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<string> Roles { get; set; }
    }
}
