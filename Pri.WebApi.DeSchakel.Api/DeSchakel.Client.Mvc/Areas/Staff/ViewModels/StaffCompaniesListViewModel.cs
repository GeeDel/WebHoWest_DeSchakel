using DeSchakel.Client.Mvc.Areas.Staff.Models;
using DeSchakel.Client.Mvc.Viewmodels;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffCompaniesListViewModel
    {
        public IEnumerable<BaseViewModel> Companies { get; set; }
    }
}
