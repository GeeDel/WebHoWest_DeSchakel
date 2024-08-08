using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _applicationDbcontext;
        private readonly IGenreService _genreService;
        private readonly IAccountsService _accountsService;
        private readonly ILocationsService _locationsService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventService(ApplicationDbContext applicationDbcontext, IAccountsService accountsService, IGenreService genreService,
            ILocationsService locationsService, ICompanyService companyService, UserManager<ApplicationUser> userManager)
        {
            _applicationDbcontext = applicationDbcontext;
            _accountsService = accountsService;
            _genreService = genreService;
            _locationsService = locationsService;
            _companyService = companyService;
            _userManager = userManager;
        }



        public async Task<bool> DoesEventIdExistAsync(int id)
        {
            bool r = await _applicationDbcontext.Events.AsNoTracking().AnyAsync(e => e.Id.Equals(id));
            return r;
        }

        public async Task<bool> DoesEventNameExistsAsync(string title)
        {
            return await _applicationDbcontext.Events
                      .AnyAsync(e => e.Title.Equals(title));
        }

        public async Task<bool> DoesAllGenresExists(IEnumerable<int> eventGenreIds)
        {

            var genreIds = _applicationDbcontext.Genres
                .Select(g => g.Id);
            return eventGenreIds.All(g => genreIds.Contains(g));
        }

        public async Task<bool> DoesAllProgrammatorIdsExists(IEnumerable<string> eventProgrammatorIds)
        {

            var programmatorsIds =  _userManager.GetUsersInRoleAsync("Programmator").Result
                .Select(a => a.Id);
            return eventProgrammatorIds.All(a => programmatorsIds.Contains(a));
        }

        public IQueryable<Event> GetAll()
        {
            return _applicationDbcontext.Events
                .Include(l => l.Location)
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers);
        }

        public async Task<ResultModel<IEnumerable<Event>>> GetByGenreIdAsync(int id)
        {
            
            var events =  await _applicationDbcontext.Events
                .Include(l => l.Location)
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers)
                .Where(e => e.Genres.Any(g => g.Id == id))
                .ToListAsync();
            if (events.Count  == 0)
            {
                return new ResultModel<IEnumerable<Event>>
                { 
                    Errors = new List<string> { "Geen voorstellingen voor dit genre" } 
                };
            }
            return new ResultModel<IEnumerable<Event>> { Data = events };
        }

        public async Task<ResultModel<IEnumerable<Event>>> GetByIdAsync(int id)
        {
             if ( !await DoesEventIdExistAsync(id))
                {
                return new ResultModel<IEnumerable<Event>>
                {
                    Errors = new List<string> { $"Geen voorstelling gevonden met id {id}" }
                };
            }
            var events = new List<Event>
            { await _applicationDbcontext.Events
                .Include(l => l.Location)
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers)
                .FirstOrDefaultAsync(e => e.Id ==id)
            };
            return new ResultModel<IEnumerable<Event>> { Data = events };

        }

        public async Task<ResultModel<IEnumerable<Event>>> ListAllAsync()
        {

            var events = await _applicationDbcontext.Events
                .Include(l => l.Location)
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers)
                .ToListAsync();
            var resultModel = new ResultModel<IEnumerable<Event>>()
            {
                Data = events
            };
            return resultModel;
        }

        public async Task<ResultModel<IEnumerable<Event>>> GetByTitleAsync(string title)
        {
            title = title ?? string.Empty;
            var performance = new List<Event>
            {
                await _applicationDbcontext.Events
                .Include(l => l.Location)
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers)
                .FirstOrDefaultAsync(e => e.Title.Equals(title.Trim()))
            };
            if (performance.Count() > 0)
            {
                return new ResultModel<IEnumerable<Event>> { Data = performance };
            }
            return new ResultModel<IEnumerable<Event>> { Errors = new List<string> { $"Geen voorstellingen gevonden met de naam ${title}" } };
        }

        public async Task<ResultModel<IEnumerable<Event>>> SearchAsync(string search)
        {
            search = search ?? string.Empty;
            var events = await _applicationDbcontext.Events
                .Include(l => l.Location)
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers)
                .Where(e => e.Title.Contains(search.Trim()))
                        .ToListAsync();
            if (events.Count() != 0)
            {
                return new ResultModel<IEnumerable<Event>> { Data = events };
            }
            return new ResultModel<IEnumerable<Event>> { Errors = new List<string> { $"Geen voorstellingen gevonden met ${search}" } };
        }

        public async Task<ResultModel<EventRequestModel>> AddAsync(EventRequestModel eventRequestModel)
        {
            // requestmodelklasse
            var resultModel = new ResultModel<EventRequestModel>( );

            if (await DoesEventNameExistsAsync(eventRequestModel.Title))
            {
                resultModel.Errors.Add($"Er bestaat al een voorstelling met de titel {eventRequestModel.Title}");

                return resultModel;
            }
            // Controles
            if (!await _companyService.DoesCompanyIdExistAsync(eventRequestModel.CompanyId))
            {
               resultModel.Errors.Add($"Kan de voorstelling niet toevoegen: id van het gezelschap {eventRequestModel.CompanyId} bestaat niet.");
            }
            if (!await _locationsService.DoesLocationIdExistAsync(eventRequestModel.LocationId))
            {
                resultModel.Errors.Add($"Kan de voorstelling niet toevoegen: id van het gezelschap {eventRequestModel.LocationId} bestaat niet.");
            }

             bool allGenresExist = await DoesAllGenresExists(eventRequestModel.GenreIds);
                   if (!allGenresExist)
                   {
                     resultModel.Errors.Add($"Kan de voorstelling niet toevoegen: een id van minstens een  genre bestaat niet.");
                   }
  
            if (! await DoesAllProgrammatorIdsExists(eventRequestModel.ProgrammatorIds))
            {
                resultModel.Errors.Add($"Kan de voorstelling niet toevoegen: een id van minstens een programmator bestaat niet.");
            }
            //
            if (!resultModel.Success)
            {
                return resultModel;
            }
            // hier dto laten binnenkomen: alle genres en dan where = die binnenkomen van 
            var resultGenres = await _genreService.ListAllAsync();
            var allPogrammators = await _accountsService.ListAllAsync();
            var newPerformance = new Event
            {
                Title = eventRequestModel?.Title,
                EventDate = eventRequestModel.EventDate,
                LocationId = eventRequestModel.LocationId,
                CompanyId = eventRequestModel.CompanyId,
                Description = eventRequestModel?.Description,
                Price = eventRequestModel.Price,
                Imagestring = eventRequestModel?.Imagestring,
                SuccesRate = eventRequestModel.SuccesRate,
                Genres = resultGenres.Data.
                          Where(g => eventRequestModel.GenreIds.Contains(g.Id)).ToList(),
               ActionUsers = allPogrammators.Data
                          .Where(p => eventRequestModel.ProgrammatorIds.Contains(p.Id)).ToList()
            };
                
            _applicationDbcontext.Events.Add(newPerformance);
            try
            {
                await _applicationDbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }


            resultModel = new ResultModel<EventRequestModel> { Data = eventRequestModel };

            return resultModel;
        }

        public async  Task<ResultModel<EventUpdateRequestModel>> UpdateAsync(EventUpdateRequestModel eventUpdateRequestModel)
        {
            var resultModel = new ResultModel<EventUpdateRequestModel>();

            if (! await DoesEventIdExistAsync(eventUpdateRequestModel.Id))
            {
                resultModel.Errors.Add($"De voorstelling met ID {eventUpdateRequestModel.Id} is niet gevonden.");

                return resultModel;
            }
            // Controles
            if (await DoesEventNameExistsAsync(eventUpdateRequestModel.Title))
            {
                resultModel.Errors.Add($"Er bestaat een andere voorstelling met de titel {eventUpdateRequestModel.Title}");

                return resultModel;
            }
            if (!await _companyService.DoesCompanyIdExistAsync(eventUpdateRequestModel.CompanyId))
            {
                resultModel.Errors.Add($"Kan de voorstelling niet toevoegen: id van het gezelschap {eventUpdateRequestModel.CompanyId} bestaat niet.");
            }
            if (!await _locationsService.DoesLocationIdExistAsync(eventUpdateRequestModel.LocationId))
            {
                resultModel.Errors.Add($"Kan de voorstelling niet toevoegen: id van het gezelschap {eventUpdateRequestModel.LocationId} bestaat niet.");
            }
            bool allGenresExist = await DoesAllGenresExists(eventUpdateRequestModel.GenreIds);
            if (!allGenresExist)
            {
                resultModel.Errors.Add($"Kan de voorstelling niet toevoegen: een id van minstens een  genre bestaat niet.");
            }
            if (!await DoesAllProgrammatorIdsExists(eventUpdateRequestModel.ProgrammatorIds))
            {
                resultModel.Errors.Add($"Kan de voorstelling niet toevoegen: een id van minstens een  genre bestaat niet.");
            }
            // make entities from database
            var resultGenres = await _genreService.ListAllAsync();
            var allPogrammators = await _accountsService.ListAllAsync();
            var result = await GetByIdAsync(eventUpdateRequestModel.Id);
            var performanceToUpdate = result.Data.First();
            performanceToUpdate.Id = eventUpdateRequestModel.Id;
            performanceToUpdate.Title = eventUpdateRequestModel?.Title;
            performanceToUpdate.EventDate = eventUpdateRequestModel.EventDate;
            performanceToUpdate.LocationId = eventUpdateRequestModel.LocationId;
            performanceToUpdate.CompanyId = eventUpdateRequestModel.CompanyId;
            performanceToUpdate.Description = eventUpdateRequestModel?.Description;
            performanceToUpdate.Price = eventUpdateRequestModel.Price;
            performanceToUpdate.Imagestring = eventUpdateRequestModel?.Imagestring;
            performanceToUpdate.SuccesRate = eventUpdateRequestModel.SuccesRate;
            performanceToUpdate.Genres = resultGenres.Data.
                Where(g => eventUpdateRequestModel.GenreIds.Contains(g.Id)).ToList();
            performanceToUpdate.ActionUsers = allPogrammators.Data
             .Where(p => eventUpdateRequestModel.ProgrammatorIds.Contains(p.Id)).ToList();
            _applicationDbcontext.Update(performanceToUpdate);
            try
            {

                await _applicationDbcontext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }

            resultModel = new ResultModel<EventUpdateRequestModel> { Data = eventUpdateRequestModel };
            return resultModel;
        }

        public async Task<ResultModel<Event>> DeleteAsync(Event entity)
        {
            var resultModel = new ResultModel<Event>();

            _applicationDbcontext.Events.Remove(entity);
            try
            {
                await _applicationDbcontext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }


            resultModel.Data = entity;

            return resultModel;
        }

        public async Task<ResultModel<IEnumerable<Event>>> GetByLocationIdAsync(int id)
        {
            var events = await _applicationDbcontext.Events
                .Include(l =>l.Location)
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers)
                .Where(e => e.LocationId.Equals(id))
                .ToListAsync();
            if (events.Count == 0)
            {
                return new ResultModel<IEnumerable<Event>>
                {
                    Errors = new List<string> { "Geen voorstelling voor deze locatie." }
                };
            }
            return new ResultModel<IEnumerable<Event>> { Data = events };
        }


        public async Task<ResultModel<IEnumerable<Event>>> GetByCompanyIdAsync(int id)
        {
            var events = await _applicationDbcontext.Events
                .Include(l => l.Location)
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers)
                .Where(e => e.CompanyId.Equals(id))
                .ToListAsync();
            if (events.Count == 0)
            {
                return new ResultModel<IEnumerable<Event>>
                {
                    Errors = new List<string> { "Geen voorstelling voor dit gezelschap." }
                };
            }
            return new ResultModel<IEnumerable<Event>> { Data = events };
        }


    }
}
