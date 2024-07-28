using DeSchakel.Client.Mvc.Services.Interfaces;
using DeSchakelApi.Consumer.Companies;
using DeSchakelApi.Consumer.Events;
using DeSchakelApi.Consumer.Genres;
using DeSchakelApi.Consumer.Locations;
using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Roles;
using DeSchakelApi.Consumer.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeSchakel.Client.Mvc.Services
{
    public class FormBuilder : IFormBuilder
    {
        private readonly IEventApiService _eventApiService;
        private readonly IGenreApiService _genreApiService;
        private readonly ILocationApiService _locationApiService;
        private readonly ICompanyApiService _companyApiService;
        private readonly IRoleApiService _roleApiService;
        private readonly IUserLoginApiService _userApiService;
        private readonly IAccountsApiService _accountsService;

        public FormBuilder(IEventApiService eventApiService, IGenreApiService genreApiService,
            ILocationApiService locationApiService, ICompanyApiService companyApiService,
            IUserLoginApiService userApiService, IAccountsApiService accountsService, IRoleApiService roleApiService)
        {
            _eventApiService = eventApiService;
            _genreApiService = genreApiService;
            _locationApiService = locationApiService;
            _companyApiService = companyApiService;
            _userApiService = userApiService;
            _accountsService = accountsService;
            _roleApiService = roleApiService;
        }



        // Selectlistitems

        // Locations
        public async Task<IEnumerable<SelectListItem>> GetLocationsSelectListItems()
        {
            var allLocations = await _locationApiService.GetAsync();
            var locations = allLocations.OrderBy(g => g.Name)
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }
                ).ToList();
            return locations;
        }
        //companies
        public async Task<IEnumerable<SelectListItem>> GetCompaniesSelectListItems()
        {
            var allCompanies = await _companyApiService.GetAsync();
            var companies = allCompanies.OrderBy(g => g.Name)
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }
                ).ToList();
            return companies;
        }
        // programators
        // programmators
        public async Task<IEnumerable<SelectListItem>> GetProgrammatorsSelectList(string token)
        {
            var allPprogrammators = await _accountsService.GetByRoles("Programmator",token );
            var programmators = allPprogrammators.OrderBy(g => g.Lastname)
        .Select(g => new SelectListItem
        {
            Value = g.Id,
            Text = $"{g.Firstname} {g.Lastname}"
        }
    ).ToList();
            return programmators;
        }
        // roles
        public async Task<IEnumerable<SelectListItem>> GetRolesSelectList(string token)
        {
            var allRoles = await _roleApiService.GetAsync(token);
            var roles = allRoles.Data.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }
           )

        .ToList();
            return roles;
        }
        // Checkboxitems
        // genres
        public async Task<List<CheckBoxItem>> GetGenresCheckBoxes()
        {
            var allGenres = await _genreApiService.GetAsync();
            return allGenres
                .OrderBy(n => n.Name)
                .Select(n => new CheckBoxItem
                {
                    Value = n.Id,
                    Text = n.Name,
                }).ToList();
        }
    }
}
