using System.Web.Mvc;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffDeleteEventViewmodel
    {
        [HiddenInput]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
