using System.ComponentModel.DataAnnotations;

namespace Pri.WebApi.DeSchakel.Api.Dtos.ApplicationUser
{
    public class CreateUserResquestDto : LoginUserRequestDto
    {
        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Geef minstens 2 karakters in")]
        public string City { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Geef minstens 4 karakters in")]
        public string Zipcode { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public List<string> Roles { get; set; }
    }
}
