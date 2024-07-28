using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int SuccesRate { get; set; }
        public string Imagestring { get; set; }
        // one Company
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        // one location
        public Location Location { get; set; }
        public int LocationId { get; set; }
        // many genres
        public ICollection<Genre> Genres { get; set; }
        // progammator
        public ICollection<ApplicationUser> ActionUsers { get; set; }
        // many tickets
        public ICollection<Ticket> Tickets { get; set; }

    }
}
