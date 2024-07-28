using DeSchakelApi.Consumer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeSchakel.Client.Mvc.Services.Interfaces
{
    public interface IFormBuilder
    {
        public Task<IEnumerable<SelectListItem>> GetProgrammatorsSelectList(string token);
        public Task<IEnumerable<SelectListItem>> GetCompaniesSelectListItems();
        public Task<IEnumerable<SelectListItem>> GetLocationsSelectListItems();
        public Task<IEnumerable<SelectListItem>> GetRolesSelectList(string token);
        public  Task<List<CheckBoxItem>> GetGenresCheckBoxes();
    }
}
