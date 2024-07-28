using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;

namespace DeSchakel.Client.Mvc.Viewmodels
{
    public class EventItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string Imagestring { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<BaseResponseApiModel> Genres { get; set; }
        public IEnumerable<AccountsResponseApiModel> Programmators { get; set; }

    }
}
