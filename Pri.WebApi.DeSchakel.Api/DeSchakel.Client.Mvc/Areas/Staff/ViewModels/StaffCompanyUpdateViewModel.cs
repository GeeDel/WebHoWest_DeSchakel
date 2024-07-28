using System.Web.Mvc;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffCompanyUpdateViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
