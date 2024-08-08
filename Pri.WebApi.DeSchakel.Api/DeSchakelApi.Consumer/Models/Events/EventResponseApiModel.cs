using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Models.Events
{
    public class EventResponseApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
        public string Imagestring { get; set; }
        public string Audiostring { get; set; }
        public string Videostring { get; set; }
        public int SuccesRate { get; set; }

        // one Company
        public int CompanyId { get; set; }
        public BaseResponseApiModel Company { get; set; }
        public int LocationId { get; set; }
        public BaseResponseApiModel Location { get; set; }
        // many genres
        public IEnumerable<BaseResponseApiModel> Genres { get; set; }
        public IEnumerable<AccountsResponseApiModel> Programmators { get; set; }
        public IEnumerable<int> GenreIds { get; set; }
        // progammator
        public IEnumerable<string> ProgrammatorIds { get; set; }
    }
}
