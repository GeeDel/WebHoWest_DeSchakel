using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffLocationDeleteViewmodel
    {
        [HiddenInput]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
