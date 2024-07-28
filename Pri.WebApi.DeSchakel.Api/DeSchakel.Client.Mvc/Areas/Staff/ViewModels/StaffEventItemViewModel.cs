using DeSchakel.Client.Mvc.Areas.Staff.Models;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffEventItemViewModel
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
        public IEnumerable<BaseModel> Genres { get; set; }
    }
}
