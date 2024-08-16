using DeSchakel.Client.Mvc.Areas.Staff.ViewModels;
using DeSchakel.Client.Mvc.Areas.User.Viewmodels;
using DeSchakel.Client.Mvc.Services;
using DeSchakel.Client.Mvc.Services.Interfaces;
using DeSchakel.Client.Mvc.Viewmodels;
using DeSchakelApi.Consumer;
using DeSchakelApi.Consumer.Companies;
using DeSchakelApi.Consumer.Events;
using DeSchakelApi.Consumer.Genres;
using DeSchakelApi.Consumer.Locations;
using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Models.Companies;
using DeSchakelApi.Consumer.Models.Events;
using DeSchakelApi.Consumer.Models.Roles;
using DeSchakelApi.Consumer.Roles;
using DeSchakelApi.Consumer.Users;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;   //  temporary for ContentDispositionHeaderValue

namespace DeSchakel.Client.Mvc.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Policy = "MemberOfStaff")] 
    public class StaffController : Controller
    {

        private readonly IEventApiService _eventApiService;
        private readonly ILocationApiService _locationApiService;
        private readonly ICompanyApiService _companyApiService;
        private readonly IGenreApiService _genreApiService;
        private readonly IRoleApiService _roleApiService;
        private readonly IUserLoginApiService _userApiService;
        private readonly IAccountsApiService _accountsService;
        private readonly IFormBuilder _formBuilder;
        private readonly IFileService _fileService;


        public StaffController(IEventApiService eventApiService, IGenreApiService genreApiService, ILocationApiService locationApiService,
            ICompanyApiService companyApiService, IUserLoginApiService userApiService, IAccountsApiService accountsService,
            IFormBuilder formBuilder, IFileService fileService, IRoleApiService roleApiService)
        {
            _eventApiService = eventApiService;
            _locationApiService = locationApiService;
            _companyApiService = companyApiService;
            _userApiService = userApiService;
            _formBuilder = formBuilder;
            _genreApiService = genreApiService;
            _accountsService = accountsService;
            _fileService = fileService;
            _accountsService = accountsService;
            _roleApiService = roleApiService;
        }

        public async Task<IActionResult> Index()
        {
            var eventFromApi = await _eventApiService.GetAsync();
            if (eventFromApi == null)
            {
                return BadRequest();
            }
            var eventsViewModel = new EventListViewModel();
            eventsViewModel.Events = eventFromApi.Select(e => new EventItemViewModel
            {
                Id = e.Id,
                Title = e.Title.Length >30 ? e.Title.Substring(0,30) : $"{e.Title} {new String(' ', 30 - e.Title.Length)}",
                EventDate = e.EventDate,
                Description = e.Description,
                Imagestring = e.Imagestring,

            });
            return View(eventsViewModel);
        }

        public async Task<IActionResult> SetGenre(int id)
        {
            var eventsFromApi = await _eventApiService.GetByGenres(id);

            var eventsViewModel = new EventListViewModel
            { Events = new List<EventItemViewModel>() };
            eventsViewModel.Events = eventsFromApi.Select(e => new EventItemViewModel

            {
                Id = e.Id,
                Title = e.Title,
                EventDate = e.EventDate,
                Description = e.Description,
                Imagestring = e.Imagestring,
                LocationName = e.Location.Name,
                CompanyName = e.Company.Name,
                Genres = e.Genres,
            }
            );

            return View(eventsViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _eventApiService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response.Errors.First());
            }
            var searchedEvent = response.Data;
            EventItemViewModel eventItemViewModel = new EventItemViewModel
            {
                Id = searchedEvent.Id,
                Title = searchedEvent.Title,
                EventDate = searchedEvent.EventDate,
                Description = searchedEvent.Description,
                Imagestring = searchedEvent.Imagestring,
                Audiostring = searchedEvent.Audiostring,
                Videostring = searchedEvent.Videostring,
                LocationName = searchedEvent.Location.Name,
                CompanyName = searchedEvent.Company.Name,
                Genres = searchedEvent.Genres,
                Programmators = searchedEvent.Programmators
            };
            return View(eventItemViewModel);
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpGet]
        public async Task<IActionResult> CreateEvent()
        {
            var token = HttpContext.Session.GetString("Token");
            StaffEventCreateViewmodel staffEventCreateViewmodel = new StaffEventCreateViewmodel
            {

                EventDate = DateTime.Now.AddDays(1).AtNoon(),
                Companies = await _formBuilder.GetCompaniesSelectListItems(),
                Locations = await _formBuilder.GetLocationsSelectListItems(),
                Programmators = await _formBuilder.GetProgrammatorsSelectList(token),
                Genres = await _formBuilder.GetGenresCheckBoxes()
            };
            return View(staffEventCreateViewmodel);
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(StaffEventCreateViewmodel staffEventCreateViewmodel)
        {
            string token = HttpContext.Session.GetString("Token");

            var resultTitle = await _eventApiService.GetByTitleAsync(staffEventCreateViewmodel.Title);
            // exist in database
            if (resultTitle.Success)  // title exists
            {
                ModelState.AddModelError("", "Een voorstelling met deze titel is al geregistreerd.");
            }
            if(staffEventCreateViewmodel.ProgrammatorIds == null)
            {
                ModelState.AddModelError("", "Selecteer minstens één programmator.");
            }
            if (!staffEventCreateViewmodel.Genres.Any(g => g.IsSelected))
            {
                ModelState.AddModelError("", "Je moet minstens één genre aangeven.");
            }
            if (staffEventCreateViewmodel.Images == null)
            {
                ModelState.AddModelError("", "Je moet een afbeelding of een video kiezen.");
            }
            else
            {
                bool hasImageOpVideo = staffEventCreateViewmodel.Images.Any(f => f.ContentType.Contains("image/")) ||
                 staffEventCreateViewmodel.Images.Any(f => f.ContentType.Contains("video/"));
                if (staffEventCreateViewmodel.Images == null
                      || !hasImageOpVideo)
                {
                    ModelState.AddModelError("", "Je moet een afbeelding of een video kiezen.");
                }
                var numberOfImageFiles = staffEventCreateViewmodel.Images.Where(i => i.ContentType.Contains("image/")).Count();
                var numberOfAudioFiles = staffEventCreateViewmodel.Images.Where(i => i.ContentType.Contains("audio/")).Count();
                var numberOfVideoFiles = staffEventCreateViewmodel.Images.Where(i => i.ContentType.Contains("video/")).Count();
                if (numberOfImageFiles > 1 || numberOfAudioFiles > 1 || numberOfVideoFiles > 1)
                {
                    ModelState.AddModelError("", "Maximum 1 afbeelding, video- of audiobestand.");
                }
                if (staffEventCreateViewmodel.Images.Count() > (numberOfImageFiles + numberOfAudioFiles + numberOfVideoFiles))
                {
                    ModelState.AddModelError("", "Alleen afbeeldings-, video- of audiobestand.");
                }
        }

            if (!ModelState.IsValid)
            {
                staffEventCreateViewmodel.Companies = await _formBuilder.GetCompaniesSelectListItems();
                staffEventCreateViewmodel.Locations = await _formBuilder.GetLocationsSelectListItems();
                staffEventCreateViewmodel.Programmators = await _formBuilder.GetProgrammatorsSelectList(token);
                staffEventCreateViewmodel.Genres = await _formBuilder.GetGenresCheckBoxes();
                return View(staffEventCreateViewmodel);
            }
            var genreIds = staffEventCreateViewmodel.Genres
                .Where(d => d.IsSelected)
                .Select(d => d.Value).ToList();
            var allGenres = await _genreApiService.GetAsync();
            var d = allGenres.Select(g => new BaseResponseApiModel
            {
                Id = g.Id,
                Name = g.Name
            });
            var allPprogrammators = _accountsService.GetByRoles("Programmator", token).Result
                .Select(p => new AccountsResponseApiModel
                {
                    Id = p.Id,
                    Firstname = p.Firstname,
                    Lastname = p.Lastname,
                    DateOfBirth = p.DateOfBirth,
                    Zipcode = p.Zipcode,
                    City = p.City,
                });
            // make multipart
            MultipartFormDataContent performanceMp = new MultipartFormDataContent 
            {    
                { new StringContent(staffEventCreateViewmodel.Title), "Title" },
                { new StringContent(staffEventCreateViewmodel.EventDate.ToString()), "EventDate" },
                { new StringContent(staffEventCreateViewmodel.Description), "Description" },
                { new StringContent(staffEventCreateViewmodel.CompanyId.ToString()),"CompanyId" },
                { new StringContent(staffEventCreateViewmodel.LocationId.ToString()),"LocationId" },
                { new StringContent(staffEventCreateViewmodel.SuccesRate.ToString()), "SuccesRate" },
                { new StringContent(staffEventCreateViewmodel.Price.ToString()), "Price" },
            };
            // the lists
            foreach(var programmator in staffEventCreateViewmodel.ProgrammatorIds)
            {
                performanceMp.Add(new StringContent(programmator), "ProgrammatorIds");
            }
            foreach(var genreId in genreIds)
            {
                performanceMp.Add(new StringContent(genreId.ToString()), "GenreIds");
            }

            // the Images
            Stream fileStream = null;
            ResultModel<List<string>> resultFileModel = new ResultModel<List<String>>();
            List<string> temporyFiles = new List<string>();
            const string AcceptedMediatypes = "image/audio/video/";
            var fileFolder = await _fileService.GetPathToImages();
            foreach (IFormFile formFile in staffEventCreateViewmodel.Images)
            {
                string fileMediatype = formFile.ContentType.Substring(0, 6);

                if (!String.IsNullOrEmpty(fileMediatype) && AcceptedMediatypes.Contains(fileMediatype))
                {
                    var tmpFileName = await _fileService.Store(formFile);
                    string pathToformFile = Path.Combine(fileFolder, tmpFileName);
                    fileStream = new FileStream(pathToformFile, FileMode.Open);
                    var streamContent = new StreamContent(fileStream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                    performanceMp.Add(streamContent, "filesToUpload", formFile.FileName);
                    temporyFiles.Add(tmpFileName);
                }
                else
                {
                    resultFileModel.Errors.Add($"Het bestand {formFile.FileName} wordt niet aanvaard.");
                }
            }
            //

            var result = await _eventApiService.CreateMultipart(performanceMp, token);
            //
            fileStream.Dispose();
            performanceMp.Dispose();
            //
            foreach (var tmp in temporyFiles)
            {
                _fileService.Delete(tmp);
            }
            //     
            if (!result.Success)
            {
                ModelState.AddModelError("Create", $"Volgende fout deed zich voor bij het wegschrijven in de database: \n {result.Errors}.");
                staffEventCreateViewmodel.Companies = await _formBuilder.GetCompaniesSelectListItems();
                staffEventCreateViewmodel.Locations = await _formBuilder.GetLocationsSelectListItems();
                staffEventCreateViewmodel.Programmators = await _formBuilder.GetProgrammatorsSelectList(token);
                staffEventCreateViewmodel.Genres = await _formBuilder.GetGenresCheckBoxes();

                return View(staffEventCreateViewmodel);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpGet]
        public async Task<IActionResult> UpdateEvent(int id)
        {
            var result = await _eventApiService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Errors.First());
            }
            string token = HttpContext.Session.GetString("Token");
            var searchedEvent = result.Data;
            StaffEventUpdateViewModel staffUserUpdateViewmodel = new StaffEventUpdateViewModel
            {
                Id = id,
                Title = searchedEvent.Title,
                EventDate = searchedEvent.EventDate,
                LocationId = searchedEvent.LocationId,
                CompanyId = searchedEvent.CompanyId,
                Imagestring = searchedEvent.Imagestring,
                Price = searchedEvent.Price,
                SuccesRate = searchedEvent.SuccesRate,
                Description = searchedEvent.Description,
                Companies = await _formBuilder.GetCompaniesSelectListItems(),
                Locations = await _formBuilder.GetLocationsSelectListItems(),
                Programmators = await _formBuilder.GetProgrammatorsSelectList(token),
                ProgrammatorIds = searchedEvent.Programmators
                    .Select(p => p.Id).ToList(),
                Genres = await _formBuilder.GetGenresCheckBoxes()
            };

            // set genres
            foreach (var checkBox in staffUserUpdateViewmodel.Genres)
            {
                if (searchedEvent.Genres.Any(g => g.Id.Equals(checkBox.Value)))
                {
                    checkBox.IsSelected = true;
                }
            }
            return View(staffUserUpdateViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEvent(StaffEventUpdateViewModel staffEventUpdateViewModel)
        {
            string token = HttpContext.Session.GetString("Token");
            var result = await _eventApiService.GetByIdAsync(staffEventUpdateViewModel.Id);
            if (!result.Success)
            {
                ModelState.AddModelError("",result.Errors.First());
            }
            var resultTitle = await _eventApiService.GetByTitleAsync(staffEventUpdateViewModel.Title);
            // exist in database
            if (resultTitle.Success && resultTitle.Data.Id != staffEventUpdateViewModel.Id)  // title exists
            {
                ModelState.AddModelError("", "Een voorstelling met deze titel is al geregistreerd.");
            }

            if (staffEventUpdateViewModel.ProgrammatorIds == null)
            {
                ModelState.AddModelError("", "Selecteer minstens één programmator.");
            }
            if (!staffEventUpdateViewModel.Genres.Any(g => g.IsSelected))
            {
                ModelState.AddModelError("", "Je moet minstens één genre aangeven.");
            }
            if (staffEventUpdateViewModel.Images == null ) 
            {
                ModelState.AddModelError("", "Je moet een afbeelding of een video kiezen.");
            }
            else
            {
                bool hasImageOpVideo = staffEventUpdateViewModel.Images.Any(f => f.ContentType.Contains("image/")) ||
                 staffEventUpdateViewModel.Images.Any(f => f.ContentType.Contains("video/"));
                if (staffEventUpdateViewModel.Images == null
                      || !hasImageOpVideo)
                {
                    ModelState.AddModelError("", "Je moet een afbeelding of een video kiezen.");
                }
                var numberOfImageFiles = staffEventUpdateViewModel.Images.Where(i => i.ContentType.Contains("image/")).Count();
                var numberOfAudioFiles = staffEventUpdateViewModel.Images.Where(i => i.ContentType.Contains("audio/")).Count();
                var numberOfVideoFiles = staffEventUpdateViewModel.Images.Where(i => i.ContentType.Contains("video/")).Count();
                if(numberOfImageFiles > 1 || numberOfAudioFiles > 1 || numberOfVideoFiles > 1 )
                {
                    ModelState.AddModelError("", "Maximum 1 afbeelding, video- of audiobestand.");
                }
                if (staffEventUpdateViewModel.Images.Count() > (numberOfImageFiles + numberOfAudioFiles + numberOfVideoFiles)){
                    ModelState.AddModelError("", "Alleen afbeelding, video- of audiobestand.");
                }
            }
            if (!ModelState.IsValid)
            {
                staffEventUpdateViewModel.Companies = await _formBuilder.GetCompaniesSelectListItems();
                staffEventUpdateViewModel.Locations = await _formBuilder.GetLocationsSelectListItems();
                staffEventUpdateViewModel.Programmators = await _formBuilder.GetProgrammatorsSelectList(token);
                staffEventUpdateViewModel.Genres = await _formBuilder.GetGenresCheckBoxes();
                // set genres
                foreach (var checkBox in staffEventUpdateViewModel.Genres)
                {
                    if (staffEventUpdateViewModel.Genres.Any(g => g.Value.Equals(checkBox.Value.ToString())))
                    {
                        checkBox.IsSelected = true;
                    }
                }
                return View(staffEventUpdateViewModel);
            }
            // lists of ids and entities
            var genreIds = staffEventUpdateViewModel.Genres
               .Where(d => d.IsSelected)
               .Select(d => d.Value);
            var genres = await _genreApiService.GetAsync();
            var allGenres = genres.Select(g => new BaseResponseApiModel
            {
                Id = g.Id,
                Name = g.Name
            });
            var allPprogrammators = _accountsService.GetByRoles("Programmator", token).Result
                .Select(p => new AccountsResponseApiModel
                {
                    Id = p.Id,
                    Firstname = p.Firstname,
                    Lastname = p.Lastname,
                    DateOfBirth = p.DateOfBirth,
                    Zipcode = p.Zipcode,
                    City = p.City,
                });
            // make multipart
            MultipartFormDataContent performanceToUpdateMp = new MultipartFormDataContent {

                { new  StringContent(staffEventUpdateViewModel.Id.ToString()),"Id"},
                { new StringContent(staffEventUpdateViewModel.Title), "Title" },
                { new StringContent(staffEventUpdateViewModel.EventDate.ToString()), "EventDate" },
                { new StringContent(staffEventUpdateViewModel.Description), "Description" },
                { new StringContent(staffEventUpdateViewModel.CompanyId.ToString()),"CompanyId" },
                { new StringContent(staffEventUpdateViewModel.LocationId.ToString()),"LocationId" },
                { new StringContent(staffEventUpdateViewModel.SuccesRate.ToString()), "SuccesRate" },
                { new StringContent(staffEventUpdateViewModel.Price.ToString()), "Price" },
            };
            // the lists
            foreach (var programmator in staffEventUpdateViewModel.ProgrammatorIds)
            {
                performanceToUpdateMp.Add(new StringContent(programmator), "ProgrammatorIds");
            }
            foreach (var genreId in genreIds)
            {
                performanceToUpdateMp.Add(new StringContent(genreId.ToString()), "GenreIds");
            }
            // the Images
            Stream fileStream = null;
            ResultModel<List<string>> resultFileModel = new ResultModel<List<String>>();
            List<string> temporyFiles = new List<string>();
            const string AcceptedMediatypes = "image/audio/video/";
            var fileFolder = await _fileService.GetPathToImages();
            foreach (IFormFile formFile in staffEventUpdateViewModel.Images)
            {
                if(formFile.Length > int.MaxValue)
                {
                    resultFileModel.Errors.Add($"Het bestand {formFile.FileName} is te groot");
                    continue;
                }
                string fileMediatype = formFile.ContentType.Substring(0, 6);

                if (!String.IsNullOrEmpty(fileMediatype) && AcceptedMediatypes.Contains(fileMediatype))
                { 
                    var tmpFileName = await _fileService.Store(formFile);
                    string pathToformFile = Path.Combine(fileFolder, tmpFileName);
                    fileStream = new FileStream(pathToformFile, FileMode.Open);
                    var streamContent = new StreamContent(fileStream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                    streamContent.Headers.ContentLength = formFile.Length;
                    performanceToUpdateMp.Add(streamContent, "filesToUpload", formFile.FileName);
                    temporyFiles.Add(tmpFileName);
                }
                else
                {
                    resultFileModel.Errors.Add($"Het bestand {formFile.FileName} wordt niet aanvaard.");
                }
            }
            //
            result = await _eventApiService.Update(performanceToUpdateMp, token);
            fileStream.Dispose();
            performanceToUpdateMp.Dispose();
            foreach(var tmp in temporyFiles)
            {
                _fileService.Delete(tmp);
            }

            if (!result.Success)
            {
                ModelState.AddModelError("", $"Volgende fout deed zich voor bij het wegschrijven in de database: \n {result.Errors.First()}.");
                staffEventUpdateViewModel.Companies = await _formBuilder.GetCompaniesSelectListItems();
                staffEventUpdateViewModel.Locations = await _formBuilder.GetLocationsSelectListItems();
                staffEventUpdateViewModel.Programmators = await _formBuilder.GetProgrammatorsSelectList(token);
                staffEventUpdateViewModel.Genres = await _formBuilder.GetGenresCheckBoxes();
                return View(staffEventUpdateViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteEvent(int id)
        {
            var result = await _eventApiService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Onverwachte fout: voorstelling met id {id} is niet gevonden");
            }

            StaffDeleteEventViewmodel staffDeleteViewModel = new StaffDeleteEventViewmodel
            {
                Id = id,
                Title = result.Data.Title,
            };
            return View(staffDeleteViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var result = await _eventApiService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            string token = HttpContext.Session.GetString("Token");
            try
            {
                await _eventApiService.DeleteAsync(id, token);
            }
            catch (Exception ex)
            {
                return BadRequest($"Er liep iets mis. Probeer het later opnieuw\n {ex.Message}");

            }

            return RedirectToAction("Index", "Staff", new { Area = "Staff" });

        }

        public async Task<IActionResult> Locations()
        {
            return View();
        }

        public async Task<IActionResult> Companies()
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
            if (!ModelState.IsValid)
            {
                return View(staffCompanyCreateViewModel);
            }
            var token = HttpContext.Session.GetString("Token");

            var companyToCreate = new CompanyCreateRequestApiModel
            {
                Name = staffCompanyCreateViewModel.Name,
            };

            await _companyApiService.CreateAsyn(companyToCreate, token);
            return RedirectToAction("Companies", "Staff", new { Area = "Staff" });
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
            if (!ModelState.IsValid)
            {
                return View(staffCompanyUpdateViewModel);
            }
            string token = HttpContext.Session.GetString("Token");
            var result = _companyApiService.GetByIdAsync(staffCompanyUpdateViewModel.Id, token);
            if (result == null)
            {
                ModelState.AddModelError("", $"Het gezelschap {result.Result.Name} is niet gevonden in ons bestand.");
                return View(staffCompanyUpdateViewModel);
            }
            var companyToUpdate = new CompanyUpdateRequestApiModel
            {
                Id = staffCompanyUpdateViewModel.Id,
                Name = staffCompanyUpdateViewModel.Name
            };
            await _companyApiService.UpdateAsyn(companyToUpdate, token);
            return RedirectToAction("Companies", "Staff", new { Area = "Staff" });
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
            return RedirectToAction("Companies", "Staff", new { Area = "Staff" });

        }

        public async Task<IActionResult> Genres()
        {
            return View();
        }

        [Authorize(Roles=("Admin"))]
        public async Task<IActionResult> Accounts()
        {
            var usersFromApi = await _accountsService.GetAsync();
            if (usersFromApi == null)
            {
                return BadRequest();
            }
            var usersViewModel = new StaffAccountsListViewModel
            {
                Accounts = usersFromApi.Select(u => new StaffAccountItemViewmodel
                {
                    Id = u.Id,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Zipcode = u.Zipcode,
                    City = u.City,
                    Email = u.Email,
                })
            };
            return View(usersViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {
            string token = HttpContext.Session.GetString("Token");

            StaffAccountCreateViewmodel staffAccountCreateViewmodel = new StaffAccountCreateViewmodel
            {
                Roles = _formBuilder.GetRolesSelectList(token).Result,

            };
            return View(staffAccountCreateViewmodel);
        }


        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(StaffAccountCreateViewmodel staffAccountCreateViewmodel)
        {
            string token = HttpContext.Session.GetString("Token");
            var result = await _accountsService.GetByEmailAsync(staffAccountCreateViewmodel.Username);
         if (result.Success)
            {
                ModelState.AddModelError("", "Gebruikerprofiel is al aangemaakt.");
            }
            if (!ModelState.IsValid)
            {
                staffAccountCreateViewmodel.Roles = _formBuilder.GetRolesSelectList(token).Result;
                return View(staffAccountCreateViewmodel);
            }
            var resultRoles = await _roleApiService.GetAsync(token);
            if (!resultRoles.Success)
            {
                ModelState.AddModelError("", resultRoles.Errors.First());
                staffAccountCreateViewmodel.Roles = _formBuilder.GetRolesSelectList(token).Result;
                return View(staffAccountCreateViewmodel);
            }
            var rolesOfAccountToUpdate = resultRoles.Data
                   .Where(r => staffAccountCreateViewmodel.RoleIds.Contains(r.Id))
                   .Select(r => r.Name).ToList();
            var accountToRegister = new AccountRegisterResponseApiModel
            {
                Email = staffAccountCreateViewmodel.Username,
                Password = staffAccountCreateViewmodel.Password,
                Firstname = staffAccountCreateViewmodel.FirstName,
                Lastname = staffAccountCreateViewmodel.LastName,
                DateOfBirth = staffAccountCreateViewmodel.DateOfBirth,
                City = staffAccountCreateViewmodel.City,
                Zipcode = staffAccountCreateViewmodel.Zipcode,
                Roles = rolesOfAccountToUpdate
            };
            try
            {
                await _accountsService.Register(accountToRegister);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Onverwachte fout: {ex.Message}");
                return View(staffAccountCreateViewmodel);
            }
            return RedirectToAction("Accounts", "Staff", new { Area = "Staff" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateAccount(string id)
        {
            var result = await _accountsService.GetByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Errors.First());
            }
            string token = HttpContext.Session.GetString("Token");
            var searchedUser = result.Data;
            //
            var resultRoles = await _roleApiService.GetAsync(token);
            if (! resultRoles.Success)
            {
                return BadRequest(resultRoles.Errors.First());
            }
            var roles = resultRoles.Data;
            var listRole = new List<string>();
            foreach( var r in roles)
            {
                var bevat = searchedUser.Roles.Contains(r.Name);
                if(bevat) listRole.Add(r.Name);

            }
            var roleIds = roles.Where(r => searchedUser.Roles.Contains(r.Name))
                .Select(r => r.Id)
                .ToList();
            //
            StaffAccountUpdateViewmodel staffAccountUpdateViewmodel = new StaffAccountUpdateViewmodel
            {
                Id = id,
                Firstname = searchedUser.Firstname,
                Lastname = searchedUser.Lastname,
                DateOfBirth = searchedUser.DateOfBirth,
                Zipcode = searchedUser.Zipcode,
                City = searchedUser.City,
                Email = searchedUser.Email,
                Roles = _formBuilder.GetRolesSelectList(token).Result,
               RoleIds = roleIds
            };


            return View(staffAccountUpdateViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(StaffAccountUpdateViewmodel staffAccountUpdateViewmodel)
        {
            string token = HttpContext.Session.GetString("Token");
            var resultAccount = await _accountsService.GetByIdAsync(staffAccountUpdateViewmodel.Id);
            if (!resultAccount.Success)
            {
                ModelState.AddModelError("", resultAccount.Errors.First());
            }
            if (!ModelState.IsValid)
            {
                staffAccountUpdateViewmodel.Roles = _formBuilder.GetRolesSelectList(token).Result;
                return View(staffAccountUpdateViewmodel);
            }
            var searchedAccount = resultAccount.Data;
            var resultRoles = await _roleApiService.GetAsync(token);
            if (!resultRoles.Success)
            {
                ModelState.AddModelError("", resultRoles.Errors.First());
                staffAccountUpdateViewmodel.Roles = _formBuilder.GetRolesSelectList(token).Result;
                return View(staffAccountUpdateViewmodel);
            }
            var rolesOfAccountToUpdate = resultRoles.Data
                .Where(r => staffAccountUpdateViewmodel.RoleIds.Contains(r.Id))
               .Select(r => r.Name).ToList();

            var accountToUpdate = new AccountsResponseApiModel
            {
                Id = staffAccountUpdateViewmodel.Id,
                Email = staffAccountUpdateViewmodel.Email,
                Firstname = staffAccountUpdateViewmodel.Firstname,
                Lastname = staffAccountUpdateViewmodel.Lastname,
                DateOfBirth = staffAccountUpdateViewmodel.DateOfBirth,
                Zipcode = staffAccountUpdateViewmodel.Zipcode,
                City = staffAccountUpdateViewmodel.City,
                Roles = rolesOfAccountToUpdate
            };
            var result = await _accountsService.Update(accountToUpdate, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", $"Volgende fout deed zich voor bij het wegschrijven in de database:" +
                    $" \n {result.Errors}.");
                staffAccountUpdateViewmodel.Roles = _formBuilder.GetRolesSelectList(token).Result;
                return View(staffAccountUpdateViewmodel);
            }
            return RedirectToAction("Accounts", "Staff", new { Area = "Staff" });

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var result = await _accountsService.GetByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Errors.First());
            }
            //           string token = HttpContext.Session.GetString("Token");
            string identityBasedToken = User.Claims.FirstOrDefault(t => t.Type.Equals("Token")).Value;

            var searchedUser = result.Data;
            var resultDelete = await _accountsService.Delete(id, identityBasedToken);
                
                //, Request.Cookies["jwtToken"].ToString());
            if (resultDelete == null)   // temporary
            {
                ModelState.AddModelError("", $"Volgende fout deed zich voor bij het wegschrijven in de database:" +
                    $" \n {result.Errors}.");
            }
            return RedirectToAction("Accounts", "Staff", new { Area = "Staff" });

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdatePassword(string id)
        {
            var result = await _accountsService.GetByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Errors.First());
            }
            var searchedUser = result.Data;
            StaffAccountUpdatePasswordViewmodelToBeDeleted staffUpdatePasswordViewmodel = new StaffAccountUpdatePasswordViewmodelToBeDeleted
            {
                Firstname = searchedUser.Firstname,
                Lastname = searchedUser.Lastname,
            };

            return View(staffUpdatePasswordViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(StaffAccountUpdatePasswordViewmodelToBeDeleted staffUpdatePasswordViewmodel)
        {
            var resultAccount = await _accountsService.GetByIdAsync(staffUpdatePasswordViewmodel.Id);
            if (!resultAccount.Success)
            {
                ModelState.AddModelError("", resultAccount.Errors.First());
            }
            if (!ModelState.IsValid)
            {
                return View(staffUpdatePasswordViewmodel);
            }
            var searchedAccount = resultAccount.Data;
            // temporary
            ModelState.AddModelError("", "Aan deze procedure wordt verder gewerkt. \n\n Gebruik <Terug>");
            return View(staffUpdatePasswordViewmodel);
            // till here
            return RedirectToAction("Accounts", "Staff", new { Area = "Staff" });
        }

    }

}