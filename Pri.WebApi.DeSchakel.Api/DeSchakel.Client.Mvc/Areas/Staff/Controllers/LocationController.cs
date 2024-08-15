using DeSchakel.Client.Mvc.Areas.Staff.ViewModels;
using DeSchakelApi.Consumer.Events;
using DeSchakelApi.Consumer.Locations;
using DeSchakelApi.Consumer.Models.Companies;
using DeSchakelApi.Consumer.Models.Locations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Policy = "MemberOfStaff")]
    public class LocationController : Controller
    {
        private readonly IEventApiService _eventApiService;
        private readonly ILocationApiService _locationApiService;



        public LocationController(ILocationApiService locationApiService, IEventApiService eventApiService)
        {
            _locationApiService = locationApiService;
            _eventApiService = eventApiService;
        }

        public async Task<IActionResult> Index()
        {
            var locationsFromApi = await _locationApiService.GetAsync();
            if (locationsFromApi == null)
            {
                return NotFound();
            }
            StaffLocationsListViewmodel companiesViewModel = new StaffLocationsListViewmodel
            {
                Locations = locationsFromApi.Select(c => new StaffLocationViewMmodel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Capacity = c.Capacity,
                })
            };

            return View(companiesViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> CreateLocation()
        {

            StaffLocationCreateViewmodel locationCreateViewModel = new StaffLocationCreateViewmodel();
            return View(locationCreateViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateLocation(StaffLocationCreateViewmodel staffLocationCreateViewmodel)
        {
            var token = HttpContext.Session.GetString("Token");
            var result = _locationApiService.GetByName(staffLocationCreateViewmodel.Name, token);
            if (result.Result.Success)
            {
                ModelState.AddModelError("", "De naam van de locatie bestaat al.");
            }
            if (staffLocationCreateViewmodel.Capacity < 2)
            {
                ModelState.AddModelError("", "De locatie moet een capaciteit hebben van >1");
            }
            if (!ModelState.IsValid)
            {
                return View(staffLocationCreateViewmodel);
            }

            var locationToCreate = new LocationRequestApiModel
            {
                Name = staffLocationCreateViewmodel.Name,
                Capacity = staffLocationCreateViewmodel.Capacity,
            };

           await _locationApiService.CreateAsyn(locationToCreate, token);
            return RedirectToAction("Index", "Location", new { Area = "Staff" });
        }


 

    }
}
