using DeSchakel.Client.Mvc.Models;
using DeSchakelApi.Consumer.Events;
using DeSchakelApi.Consumer.Genres;
using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Events;
using DeSchakel.Client.Mvc.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Diagnostics;
using DeSchakelApi.Consumer.Locations;
using DeSchakelApi.Consumer.Users;
using DeSchakelApi.Consumer.Users.Models;
using DeSchakelApi.Consumer.Companies;
using Microsoft.VisualBasic;



namespace DeSchakel.Client.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventApiService _eventApiService;
        private readonly IUserLoginApiService _userService;
        private readonly IAccountsApiService _accountsService;
        private readonly IGenreApiService _genreApiService;
        private readonly IHttpClientFactory _httpClientFactory;


        public HomeController(ILogger<HomeController> logger, IEventApiService eventApiService,
            IUserLoginApiService userApiService,IAccountsApiService accountsService,
            IHttpClientFactory httpClientFactory, IGenreApiService genreApiService
            )
        {
            _logger = logger;
            _eventApiService = eventApiService;
            _userService = userApiService;
            _httpClientFactory = httpClientFactory;
            _genreApiService = genreApiService;
            _accountsService = accountsService;
        }



        public async Task<IActionResult> Index()
        {
            var eventsFromApi = await _eventApiService.GetAsync();
            var eventsViewModel = new EventListViewModel
            { Events = new List<EventItemViewModel>() };
            eventsViewModel.Events = eventsFromApi.Select(e => new EventItemViewModel

            {
                Id = e.Id,
                Title = e.Title,
                EventDate = e.EventDate,
                Description = e.Description,
                Imagestring = e.Imagestring,
                Audiostring = e.Audiostring,
                Videostring = e.Videostring,
                LocationName = e.Location.Name,
                CompanyName = e.Company.Name,
                Genres = e.Genres,
            }
            )
            .OrderBy(d => d.EventDate);
            if (User.Identity.IsAuthenticated)
            {
                eventsViewModel.LoggedInUser = User.Identity.Name;
            }
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

        [Authorize(Policy = "NewAbos")]
        public IActionResult NewAbo()
        {
            return Ok("Hier zien jong-aboneess hun voordelen");
        }

        [Authorize(Policy = "FromWaregem")]
        public IActionResult Voucher()
        {
            return Ok("Hier kunnen Waregemnaars een voucher voor één voorstelling bestellen.");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
