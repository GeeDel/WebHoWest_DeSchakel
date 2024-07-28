using DeSchakelApi.Consumer.Models.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Models.Events
{
    public class EventRequestApiModel
    {
   //     public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int SuccesRate { get; set; }
        public string Imagestring { get; set; }
        // one Company
        public int CompanyId { get; set; }
        // one location
        public int LocationId { get; set; }
        // many genres
        public IEnumerable<BaseRequestApiModel> Genres { get; set; }
        // progammator
        public IEnumerable<AccountsRequestsApiModel> Programmators { get; set; }
    }
}
