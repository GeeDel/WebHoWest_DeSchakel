using Microsoft.EntityFrameworkCore;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services
{
    public class LocationsService : ILocationsService
    {
        private readonly ApplicationDbContext _applicationDbcontext;

        public LocationsService(ApplicationDbContext applicationDbcontext)
        {
            _applicationDbcontext = applicationDbcontext;
        }

        public async Task<bool> DoesLocationIdExistAsync(int id)
        {
            bool r =  await _applicationDbcontext.Locations.AnyAsync(e => e.Id.Equals(id));
            return r;
        }

        public async Task<bool> DoesLocationNameExistsAsync(Location entity)
        {
            return await _applicationDbcontext.Locations
                      .Where(e => !e.Id.Equals(entity.Id))
                      .AnyAsync(e => e.Name.Equals(entity.Name));
        }

        public IQueryable<Location> GetAll()
        {
            return _applicationDbcontext.Locations; 
        }

        public async Task<ResultModel<Location>> GetByIdAsync(int id)
        {
            if (!await DoesLocationIdExistAsync(id))
            {
                return new ResultModel<Location> { Errors = new List<string> { $"Geen locatie gevonden met id {id}" } };  
            }
            var location = await _applicationDbcontext.Locations
                .FirstOrDefaultAsync(e => e.Id == id);
            return new ResultModel<Location> { Data = location };
        }

        public async Task<ResultModel<IEnumerable<Location>>> ListAllAsync()
        {
            var locations = await _applicationDbcontext.Locations.ToListAsync();
            var resultModel = new ResultModel<IEnumerable<Location>>()
            {
                Data = locations
            };
            return resultModel;
        }

        public async Task<ResultModel<IEnumerable<Location>>> SearchAsync(string search)
        {
            search = search ?? string.Empty;
            var locations = await _applicationDbcontext.Locations
                .Where(e => e.Name.Contains(search.Trim()))
                        .ToListAsync();
            if (locations.Count() != 0)
            {
                return new ResultModel<IEnumerable<Location>> { Data = locations };
            }
            return new ResultModel<IEnumerable<Location>> { Errors = new List<string> { $"Geen locaties gevonden met ${search}" } };
        }

        public async Task<ResultModel<Location>> AddAsync(Location entity)
        {
            var resultModel = new ResultModel<Location>();

            if (await DoesLocationNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"We vonden al een locatie met de titel {entity.Name}");

                return resultModel;
            }

            _applicationDbcontext.Locations.Add(entity);
            try
            {
                await _applicationDbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }

            resultModel = new ResultModel<Location> { Data = entity };

            return resultModel;
        }



        public async Task<ResultModel<Location>> UpdateAsync(Location entity)
        {
            var resultModel = new ResultModel<Location>();

            if (!await DoesLocationIdExistAsync(entity.Id))
            {
                resultModel.Errors.Add($"De locatie met ID {entity.Id} is niet gevonden.");

                return resultModel;
            }

            if (await DoesLocationNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"We vonden al een locatie met de naam  {entity.Name}");

                return resultModel;
            }

            _applicationDbcontext.Update(entity);   //  _applicationDbcontext.Events.Update(entity)
            try
            {

                await _applicationDbcontext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }

            resultModel = new ResultModel<Location> { Data = entity };
            return resultModel;
        }
        
        public async Task<ResultModel<Location>> DeleteAsync(Location entity)
        {
            var resultModel = new ResultModel<Location>();

            _applicationDbcontext.Locations.Remove(entity);
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
    }
}
