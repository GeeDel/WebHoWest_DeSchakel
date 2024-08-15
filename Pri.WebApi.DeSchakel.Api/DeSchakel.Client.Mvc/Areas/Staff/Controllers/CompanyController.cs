using DeSchakel.Client.Mvc.Areas.Staff.ViewModels;
using DeSchakel.Client.Mvc.Viewmodels;
using DeSchakelApi.Consumer.Companies;
using DeSchakelApi.Consumer.Events;
using DeSchakelApi.Consumer.Models.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Policy = "MemberOfStaff")]
    public class CompanyController : Controller
    {

        private readonly IEventApiService _eventApiService;
        private readonly ICompanyApiService _companyApiService;

        public CompanyController(ICompanyApiService companyApiService, IEventApiService eventApiService)
        {
            _companyApiService = companyApiService;
            _eventApiService = eventApiService;
        }


        public async Task<IActionResult> Index()
        {
            var companiesFromApi = await _companyApiService.GetAsync();
            if (companiesFromApi == null)
            {
                return NotFound();
            }
            StaffCompaniesListViewModel companiesViewModel = new StaffCompaniesListViewModel
            {
                Companies = companiesFromApi.Select(c => new BaseViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
            };

            return View(companiesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCompany()
        {

            StaffCompanyCreateViewModel companyCreateViewModel = new StaffCompanyCreateViewModel();
            return View(companyCreateViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateCompany(StaffCompanyCreateViewModel staffCompanyCreateViewModel)
        {
            var token = HttpContext.Session.GetString("Token");
            var result = _companyApiService.GetByName(staffCompanyCreateViewModel.Name, token);
            if (result.Result.Success)
            {
                ModelState.AddModelError("", "De naam van de locatie bestaat al.");
            }
            if (!ModelState.IsValid)
            {
                return View(staffCompanyCreateViewModel);
            }

            var companyToCreate = new CompanyCreateRequestApiModel
            {
                Name = staffCompanyCreateViewModel.Name,
            };

            await _companyApiService.CreateAsyn(companyToCreate, token);
            return RedirectToAction("Index", "Company", new { Area = "Staff" });
        }


        [HttpGet]
        public async Task<IActionResult> UpdateCompany(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            var result = await _companyApiService.GetByIdAsync(id, token);
            StaffCompanyUpdateViewModel companyViewModel = new StaffCompanyUpdateViewModel
            {
                Id = result.Id,
                Name = result.Name
            };
            return View(companyViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateCompany(StaffCompanyUpdateViewModel staffCompanyUpdateViewModel)
        {
            var token = HttpContext.Session.GetString("Token");
            var result = _companyApiService.GetByName(staffCompanyUpdateViewModel.Name, token);
            if (result.Result.Success)
            {
                if (result.Result.Data.Id != staffCompanyUpdateViewModel.Id)
                {
                    ModelState.AddModelError("", $"Het gezelschap {staffCompanyUpdateViewModel.Name} bestaat al in ons bestand.");
                }
            }
            if (!ModelState.IsValid)
            {
                return View(staffCompanyUpdateViewModel);
            }
            var companyToUpdate = new CompanyUpdateRequestApiModel
            {
                Id = staffCompanyUpdateViewModel.Id,
                Name = staffCompanyUpdateViewModel.Name
            };
            await _companyApiService.UpdateAsyn(companyToUpdate, token);
            return RedirectToAction("Index", "Company", new { Area = "Staff" });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDeleteCompany(int id)
        {
            string token = HttpContext.Session.GetString("Token");
            var result = await _companyApiService.GetByIdAsync(id, token);
            if (result == null)
            {
                return NotFound($"Onverwachte fout: gezelschap met id {id} is niet gevonden");
            }

            StaffDeleteCompanyViewModel staffDeleteViewModel = new StaffDeleteCompanyViewModel
            {
                Id = id,
                Name = result.Name,
            };
            return View(staffDeleteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            string token = HttpContext.Session.GetString("Token");

            var result = await _companyApiService.GetByIdAsync(id, token);

            if (result == null)
            {
                return NotFound($"Het gezelschap met {id} is niet gevonden in ons bestand.");
            }
            // todo  controle op voorstellingen voor het gezelschap
            var resultPerformances = await _eventApiService.GetByCompany(id);
            if (!resultPerformances.Count().Equals(0))
            {
                return NotFound($"Het gezelschap met {id} is niet worden verwijderd omdat er nog voorstellingen zijn gekoppeld.");
            }
            try
            {
                await _companyApiService.DeleteAsyn(id, token);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Er liep iets mis. Probeer het later opnieuw");
                Console.WriteLine(ex.Message);

            }
            return RedirectToAction("Index", "Company", new { Area = "Staff" });

        }

    }
}
