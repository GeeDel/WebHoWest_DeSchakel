using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        // programmators originze many events
        public ICollection<Event> Events { get; set; }
        //
        public ICollection<Ticket> Tickets { get; set; }
    }
}
