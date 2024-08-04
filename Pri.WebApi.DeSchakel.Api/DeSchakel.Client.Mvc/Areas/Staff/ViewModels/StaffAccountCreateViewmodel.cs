using DeSchakel.Client.Mvc.Areas.User.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DeSchakel.Client.Mvc.Areas.Staff.ViewModels
{
    public class StaffAccountCreateViewmodel : UserRegisterViewmodel
    {
        [HiddenInput]
        public IEnumerable<string> RoleIds { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
