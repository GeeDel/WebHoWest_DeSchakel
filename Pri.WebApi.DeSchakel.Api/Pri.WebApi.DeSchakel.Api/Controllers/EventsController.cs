using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Pri.WebApi.DeSchakel.Api.Dtos;
using Pri.WebApi.DeSchakel.Api.Dtos.ApplicationUser;
using Pri.WebApi.DeSchakel.Api.Dtos.Company;
using Pri.WebApi.DeSchakel.Api.Dtos.Event;
using Pri.WebApi.DeSchakel.Api.Dtos.Genre;
using Pri.WebApi.DeSchakel.Api.Dtos.Location;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System.ComponentModel.Design;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pri.WebApi.DeSchakel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IGenreService _genreService;
        private readonly IFileService _fileService;
        private readonly ILocationsService _locationsService;
        private readonly ICompanyService _companiesService;
        private readonly IAccountsService _accountsService;
        private readonly ApplicationDbContext _applicationDbcontext;
   //     private readonly HttpContext _httpContext;

        public EventsController(IEventService eventService, IGenreService genreService,
            IFileService fileService, ApplicationDbContext applicationDbcontext,
            ILocationsService locationsService, ICompanyService companiesService,
            IAccountsService accountsService)
        {
            _eventService = eventService;
            _genreService = genreService;
            _fileService = fileService;
            _applicationDbcontext = applicationDbcontext;
            _locationsService = locationsService;
            _companiesService = companiesService;
            _accountsService = accountsService;
     //       _httpContext = httpContext;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        
        {

            var result = await _eventService.ListAllAsync();
            if (result.Success)
            {
                var eventDtos = SetResponseDto(result);
                return Ok(eventDtos);
            }
            return BadRequest(result.Errors); 
        
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _eventService.GetByIdAsync(id);
            if (result.Success)
            {
                var eventResponseDto = SetResponseDto(result);
                return Ok(eventResponseDto.First());  // only first of list
            }
            return BadRequest(result.Errors);
        }


        [HttpGet("Title/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var result = await _eventService.GetByTitleAsync(title);
            if (result.Data.First() != null)
            {
                var eventResponseDto = SetResponseDto(result);
                return Ok(eventResponseDto.First());  //  // only first of list
            }
            else
            {
                return null;
                   
            }
        }

        [HttpGet("SearchTitle/{search}")]
        public async Task<IActionResult> SearchByTitle( string search)
        {
            var result = await _eventService.SearchAsync(search);
            if (result.Success)
            {
                var eventResponseDto = SetResponseDto(result);
                return Ok(eventResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpGet("{id}/genre")]
        public async Task<IActionResult> GetEventsByGenre(int id)
        {
            var result = await _eventService.GetByGenreIdAsync(id);
            if (result.Success)
            {
                var eventResponseDto = SetResponseDto(result);
                return Ok(eventResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("{id}/location")]
        public async Task<IActionResult> GetEventsByLocation(int id)
        {
            var result = await _eventService.GetByLocationIdAsync(id);
            if (result.Success)
            {
                var eventResponseDto = SetResponseDto(result);
                return Ok(eventResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpGet("{id}/Company")]
        public async Task<IActionResult> GetEventsByCompany(int id)
        {
            var result = await _eventService.GetByCompanyIdAsync(id);
            if (result.Success)
            {
                var eventResponseDto = SetResponseDto(result);
                return Ok(eventResponseDto);
            }
            return BadRequest(result.Errors);
        }

        [Authorize(Policy = "MemberOfManagement")] 
        [HttpPost ]
        public async Task<IActionResult> Add([FromForm] AddEventRequestMultipartDto mpRequest)    
        {
            if (mpRequest == null)
            {
                return BadRequest("Ongeldige aanvraag");
            }
            // the data ======================================================================
            var performance = new EventRequestModel
            {

                Title = mpRequest.Title,
                CompanyId = mpRequest.CompanyId,
                LocationId = mpRequest.LocationId,
                EventDate = mpRequest.EventDate,
                Description = mpRequest.Description,
                Price = mpRequest.Price,
                SuccesRate = mpRequest.SuccesRate,
                ProgrammatorIds = mpRequest.ProgrammatorIds,
                GenreIds = mpRequest.GenreIds
            };
            // the files
            foreach (IFormFile update in mpRequest.filesToUpload)
            {
                var resultModelImage = await _fileService.AddOrUpdateImageAsync(update, update.FileName);

                performance.Imagestring = resultModelImage.Data;
                if (!resultModelImage.Success)
                {
                    return BadRequest($"Fout bij het wegschrijven van de afbeelding {performance.Imagestring}.");
                }
            }

            var resultAddAsync = await _eventService.AddAsync(performance);
            if (resultAddAsync.Success)
            {
                var dto = new EventResponseDto
                {
                    Title = performance.Title,
                    Imagestring = $"{Request.Scheme}://{Request.Host}/images/events/{performance.Imagestring}"
                };
                return CreatedAtAction(nameof(Get), new { performance.Title }, dto);
            }
            return BadRequest(resultAddAsync.Errors);

        }


 
        [Authorize(Policy = "MemberOfManagement")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] MultipartFormDataContent mpRequest)
        {

            //  
            var result = Request.ReadFormAsync();
            if (!result.IsCompletedSuccessfully)
            {
                return BadRequest("Ongeldige aanvraag");
            }
            var body = result.Result;
            int id;
            bool existId = int.TryParse(body["Id"], out id);
            if (!existId)
            {
                return BadRequest($"Voorstelling met id {id} niet gevonden.");
            }
            var resultPerformanceById = await _eventService.GetByIdAsync(id);
            if (resultPerformanceById.Success == false)
            {
                return BadRequest(resultPerformanceById.Errors);
            }
            var existingEntity = resultPerformanceById.Data.First();   // only the first of the list
            // set variables
            int companyId;
            int locationId;
            int succesRate;
            double price;
            DateTime eventDate;
            List<int> genreIds = new List<int>();
            List<string> progIds = new List<string>();
            //
            bool existsC_Id = int.TryParse(body["CompanyId"], out companyId);
            if (!existsC_Id)
            {
                return BadRequest("Ongeldige uitvoerder.");
            }
            bool existsL_LId = int.TryParse(body["LocationId"], out locationId);
            if (!existsL_LId)
            {
                return BadRequest("Ongeldige locatie.");
            }
            bool existsSR_Id = int.TryParse(body["SuccesRate"], out succesRate);
            if (!existsSR_Id)
            {
                return BadRequest("Ongeldige succesgraad.");
            }
            bool existsPrice = double.TryParse(body["Price"], out price);
            if (!existsPrice)
            {
                return BadRequest("Ongeldige prijs.");
            }
            if (body["Eventdate"].First() == null)
            {
                return BadRequest("Ongeldige datum van de voorstelling");
            }
            eventDate = DateTime.Parse(body["Eventdate"]);
            StringValues progIdsString;
            bool existsProgIds = body.TryGetValue("ProgrammatorIds", out progIdsString);
            if (!existsProgIds)
            {
                return BadRequest("Ongeldige lijst van genres");
            }
            foreach (string progId in progIdsString)
            {
                progIds.Add(progId);
            }

            StringValues genreIdsStringValues;
            bool exitsGenreIds = body.TryGetValue("GenreIds", out genreIdsStringValues);
            if (!exitsGenreIds)
            {
                return BadRequest("Ongeldige lijst van genres");
            }

            foreach (string genreId in genreIdsStringValues)
            {
                genreIds.Add(int.Parse(genreId));
            }
            //  the image
            IFormFile image = body.Files.FirstOrDefault();
            var resultModelImage = await _fileService.AddOrUpdateImageAsync(image, image.FileName);
            string imageString = resultModelImage.Data;

            var performanceToUpdate = new EventUpdateRequestModel
            {
                Id = id,
                Title = body["Title"],
                CompanyId = companyId,
                LocationId = locationId,
                EventDate = eventDate,
                Description = body["Description"],
                Price = price,
                ProgrammatorIds = progIds,
                Imagestring = imageString,
                SuccesRate = succesRate,
                GenreIds = genreIds,
            };
            
            //update
            var resultUpdate = await _eventService.UpdateAsync(performanceToUpdate);
            if (resultUpdate.Success)
            {
                var dto = new EventResponseDto
                {
                    Title = performanceToUpdate.Title,
                    Imagestring = $"{Request.Scheme}://{Request.Host}/images/events/{performanceToUpdate.Imagestring}",
                    Price = performanceToUpdate.Price,
                    //    Genres = product.GenreResponsDto.Genres.ToList(),

                };
                return CreatedAtAction(nameof(Get), new { performanceToUpdate.Title }, dto);
            }
            return BadRequest(resultUpdate.Errors);
        }

        [Authorize(Policy = "MemberOfManagement")]  //  [Authorize(Roles = "Admin,Programmator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _eventService.DoesEventIdExistAsync(id) == false)
            {
                return NotFound($"De voorssteling met  id {id} is niet gevonden.");
            }
            var existingProductResult = await _eventService.GetByIdAsync(id);
            if (existingProductResult.Success == false)
            {
                return BadRequest(existingProductResult.Errors);
            }
            var result = await _eventService.DeleteAsync(existingProductResult.Data.First());  // only the first entity of the list
            return Ok($"Voorstelling verwijderd:  {existingProductResult.Data.First().Title}");
        }

        private IEnumerable<EventResponseDto> SetResponseDto(ResultModel<IEnumerable<Event>> result)
        {
            var data = result.Data.Select(e =>

            new EventResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                EventDate = e.EventDate,
                Price = e.Price,
                Imagestring = $"{Request.Scheme}://{Request.Host}/images/events/{e.Imagestring}",
                Genres = e.Genres.Select(g => new GenreResponseDto
                {
                    Id = g.Id,
                    Name = g.Name
                }),
                Company = new CompanyResponseDto
                {
                    Id = e.Company.Id,
                    Name = e.Company.Name,
                },
                Location = new LocationResponseDto
                {
                    Id = e.Location.Id,
                    Name = e.Location.Name,
                },
                Programmators = e.ActionUsers.Select(p => new ApplicationUserResponseDto
                {
                    Id = p.Id,
                    Firstname = p.Firstname,
                    Lastname = p.Lastname,
                    ZipCode = p.Zipcode,
                    City = p.City,
                    DateOfBirth = p.DateOfBirth,
                    Email = p.UserName
                })
              
            }).ToList();
            return data;
        }
    }
    }
