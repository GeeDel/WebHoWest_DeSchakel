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
            // chech for files
            if (mpRequest.filesToUpload.Count() == 0)
            {
                ModelState.AddModelError("", "Je moet een afbeelding of een video kiezen.");
            }
            else
            {
                bool hasImageOpVideo = mpRequest.filesToUpload.Any(f => f.ContentType.Contains("image/")) ||
                 mpRequest.filesToUpload.Any(f => f.ContentType.Contains("video/"));
                if (mpRequest.filesToUpload == null
                      || !hasImageOpVideo)
                {
                    ModelState.AddModelError("", "Je moet een afbeelding of een video kiezen.");
                }
                var numberOfImageFiles = mpRequest.filesToUpload.Where(i => i.ContentType.Contains("image/")).Count();
                var numberOfAudioFiles = mpRequest.filesToUpload.Where(i => i.ContentType.Contains("audio/")).Count();
                var numberOfVideoFiles = mpRequest.filesToUpload.Where(i => i.ContentType.Contains("video/")).Count();
                if (numberOfImageFiles > 1 || numberOfAudioFiles > 1 || numberOfVideoFiles > 1)
                {
                    ModelState.AddModelError("", "Maximum 1 afbeelding, video- of audiobestand.");
                }
                if (mpRequest.filesToUpload.Count() > (numberOfImageFiles + numberOfAudioFiles + numberOfVideoFiles))
                {
                    ModelState.AddModelError("", "Alleen afbeelding, video- of audiobestand.");
                }
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
            const string AcceptedContetenttype = "image/audio/video/";
            ResultModel<string> resultModelImage = new();

           foreach (IFormFile file in mpRequest.filesToUpload)
            {
                string fileContenttype = file.ContentType.Substring(0, 6);
                if (!String.IsNullOrEmpty(fileContenttype) && AcceptedContetenttype.Contains(fileContenttype))
                {
                  resultModelImage = await _fileService.AddOrUpdateImageAsync(file, file.FileName);
                  performance.Imagestring = resultModelImage.Data;
                }
                else
                {
                    resultModelImage.Errors.Add($"Het type bestand {file.FileName} wordt niet aanvaard.");
                }

                if (!resultModelImage.Success)
                {
                    resultModelImage.Errors.Add($"Fout bij het wegschrijven van de afbeelding {performance.Imagestring}.");
                }
            }

            var resultAddAsync = await _eventService.AddAsync(performance);
            if (resultAddAsync.Success)
            {
                var dto = new EventResponseDto
                {
                    Title = performance.Title,
                    Errors = resultModelImage.Errors
                };
                return CreatedAtAction(nameof(Get), new { performance.Title }, dto);
            }
            return BadRequest(resultAddAsync.Errors);

        }

        [Authorize(Policy = "MemberOfManagement")]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateEventRequestMultipartDto mpRequest)
        {

            //  
            if (mpRequest == null)
            {
                return BadRequest("Ongeldige aanvraag");
            }
            // the data ======================================================================
            var performanceToUpdate = new EventUpdateRequestModel
            {
                Id = mpRequest.Id,
                Title = mpRequest.Title,
                CompanyId = mpRequest.CompanyId,
                LocationId = mpRequest.LocationId,
                EventDate = mpRequest.EventDate,
                Description = mpRequest.Description,
                Price = mpRequest.Price,
                SuccesRate = mpRequest.SuccesRate,
                ProgrammatorIds = mpRequest.ProgrammatorIds,
                GenreIds = mpRequest.GenreIds,
            };
            // the files
            const string AcceptedContetenttype = "image/audio/video/";
            ResultModel<string> resultModelImage = new();

            foreach (IFormFile file in mpRequest.filesToUpload)
            {

                string fileContenttype = file.ContentType.Substring(0, 6);
                if (!String.IsNullOrEmpty(fileContenttype) && AcceptedContetenttype.Contains(fileContenttype))
                {
                    resultModelImage = await _fileService.AddOrUpdateImageAsync(file, file.FileName);
                    performanceToUpdate.Imagestring = resultModelImage.Data;
                }
                else
                {
                    resultModelImage.Errors.Add($"Het type bestand {file.FileName} wordt niet aanvaard.");
                }
                if (!resultModelImage.Success)
                {
                    resultModelImage.Errors.Add($"Fout bij het wegschrijven van de afbeelding {performanceToUpdate.Imagestring}.");
                }
            }
            //updateservice
            var resultUpdate = await _eventService.UpdateAsync(performanceToUpdate);
            if (resultUpdate.Success)
            {
                var dto = new EventResponseDto
                {
                    Title = performanceToUpdate.Title,
                    Errors = resultModelImage.Errors

                };
                return CreatedAtAction(nameof(Get), new { performanceToUpdate.Title }, dto);
            }
            return BadRequest(resultUpdate.Errors);
        }

        [Authorize(Policy = "MemberOfManagement")]
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
                Imagestring = e.Imagestring == null ? $"{Request.Scheme}://{Request.Host}/media/performances/default.jpg"
                                : $"{Request.Scheme}://{Request.Host}/media/performances/{e.Imagestring}",
                Audiostring = e.Audiostring == null ? null : $"{Request.Scheme}://{Request.Host}/media/performances/{e.Audiostring}",
                Videostring = e.Videostring == null ? null : $"{Request.Scheme}://{Request.Host}/media/performances/{e.Videostring}",
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
