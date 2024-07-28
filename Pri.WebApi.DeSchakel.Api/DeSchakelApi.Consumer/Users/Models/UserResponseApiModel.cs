using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Users.Models
{
    public class UserResponseApiModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
