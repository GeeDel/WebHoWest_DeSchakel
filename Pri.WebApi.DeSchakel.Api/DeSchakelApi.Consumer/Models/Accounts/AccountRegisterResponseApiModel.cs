using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Models.Accounts
{
    public class AccountRegisterResponseApiModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public List<string> Roles { get; set; }
    }
}
