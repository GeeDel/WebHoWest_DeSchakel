using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Models
{
    public class LoginUserRequestModel
    {

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
