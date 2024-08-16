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
                Locations = locationsFromApi.Select(c => new StaffLocationViewModel
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

        [HttpGet]
        public async Task<IActionResult> UpdateLocation(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            var result = await _locationApiService.GetByIdAsync(id, token);
            StaffLocationUpdateViewmodel locationViewModel = new StaffLocationUpdateViewmodel
            {
                Id = result.Id,
                Name = result.Name,
                Capacity = result.Capacity
            };
            return View(locationViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateLocation(StaffLocationUpdateViewmodel staffLocationUpdateViewModel)
        {
            var token = HttpContext.Session.GetString("Token");
            var result = _locationApiService.GetByName(staffLocationUpdateViewModel.Name, token);
            if (result.Result.Success)
            {
                if (result.Result.Data.Id != staffLocationUpdateViewModel.Id)
                {
                    ModelState.AddModelError("", $"Het gezelschap {staffLocationUpdateViewModel.Name} bestaat al in ons bestand.");
                }
            }
            if (!ModelState.IsValid)
            {
                return View(staffLocationUpdateViewModel);
            }
            var locationToUpdate = new LocationRequestApiModel
            {
                Id = staffLocationUpdateViewModel.Id,
                Name = staffLocationUpdateViewModel.Name,
                Capacity = staffLocationUpdateViewModel.Capacity
            };
            await _locationApiService.UpdateAsyn(locationToUpdate, token);
            return RedirectToAction("Index", "Location", new { Area = "Staff" });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDeleteLocation(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            var result = await _locationApiService.GetByIdAsync(id, token);
            if (result == null)
            {
                return NotFound($"Onverwachte fout: gezelschap met id {id} is niet gevonden");
            }

            StaffLocationDeleteViewmodel staffDeleteViewModel = new StaffLocationDeleteViewmodel
            {
                Id = id,
                Name = result.Name,
            };
            return View(staffDeleteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            string token = HttpContext.Session.GetString("Token");

            var result = await _locationApiService.GetByIdAsync(id, token);

            if (result == null)
            {
                ModelState.AddModelError("", $"Het gezelschap met {id} is niet gevonden in ons bestand.");
            }
            // todo  controle op voorstellingen voor de locatie
            var resultEventsOnLocation = await _eventApiService.GetByLocation(id);
            if (resultEventsOnLocation.Length != 0)
            {
                ModelState.AddModelError("", ($"De locatie {result.Name} kan niet worden verwijderd omdat er nog voorstellingen zijn gekoppeld."));
            }
            else
            {
                try
                {
                    await _locationApiService.DeleteAsync(id, token);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Er liep iets mis. Probeer het later opnieuw");
                    Console.WriteLine(ex.Message);

                }
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("FoundErrorOnLocation", "Location", new { Area = "Staff" }, 
                    ModelState.Root.Errors.ToString());

            }
            return RedirectToAction("Index", "Location", new { Area = "Staff" });

        }

        public IActionResult FoundErrorOnLocation()
        {
            return View();
        }

    }
}
