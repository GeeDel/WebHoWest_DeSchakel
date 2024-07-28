using DeSchakel.Client.Mvc.Areas.Staff.Models;
using System.Web.Mvc;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffGenreListViewmodel
    {
        public IEnumerable<BaseModel> Genres { get; set; }
        public string SetController { get; set; }
        public string SetArea { get; set; }

    }
}
