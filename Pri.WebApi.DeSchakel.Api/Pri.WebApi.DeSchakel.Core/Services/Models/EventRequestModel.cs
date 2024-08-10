using Microsoft.AspNetCore.Http;
using Pri.WebApi.DeSchakel.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Models
{
    public class EventRequestModel
    {

        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int SuccesRate { get; set; }
        public IFormFile Image { get; set; }
        public string Imagestring { get; set; }
        public string Audiostring { get; set; }
        public string Videostring { get; set; }
        public int CompanyId { get; set; }
        public int LocationId { get; set; }
        // many genres
        public IEnumerable<int>GenreIds { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        // progammator
        public IEnumerable<string>ProgrammatorIds { get; set; }
        public IEnumerable<ApplicationUser> Programmators { get; set; }
    }
}
