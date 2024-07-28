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
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _applicationDbcontext;

        public GenreService(ApplicationDbContext applicationDbcontext)
        {
            _applicationDbcontext = applicationDbcontext;
        }



        public Task<bool> DoesGenreIdExistAsync(int id)
        {
           return _applicationDbcontext.Genres.AnyAsync(e => e.Id.Equals(id));
        }

        public async Task<bool> DoesGenreNameExistsAsync(Genre entity)
        {
            return await _applicationDbcontext.Genres
                      .Where(e => !e.Id.Equals(entity.Id))
                      .AnyAsync(e => e.Name.Equals(entity.Name));
        }

        public IQueryable<Genre> GetAll()
        {
            return _applicationDbcontext.Genres;
        }


        public async Task<ResultModel<IEnumerable<Genre>>> ListAllAsync()
        {
            var genres = await _applicationDbcontext.Genres.ToListAsync();
            return new ResultModel<IEnumerable<Genre>> 
            {
                Data = genres 
            };
        }

        public async Task<ResultModel<Genre>> GetByIdAsync(int id)
        {
            if (!await DoesGenreIdExistAsync(id))
            {
                return new ResultModel<Genre> { Errors = new List<string> { $"Geen locatie gevonden met id {id}" } };
            }
            var genre = await _applicationDbcontext.Genres
                .FirstOrDefaultAsync(e => e.Id == id);
            return new ResultModel<Genre> { Data = genre };
        }

        public async Task<ResultModel<IEnumerable<Genre>>> SearchAsync(string search)
        {
            search = search ?? string.Empty;
            var locations = await _applicationDbcontext.Genres
                .Where(e => e.Name.Contains(search.Trim()))
                        .ToListAsync();
            if (locations.Count() != 0)
            {
                return new ResultModel<IEnumerable<Genre>> { Data = locations };
            }
            return new ResultModel<IEnumerable<Genre>> { Errors = new List<string> { $"Geen locaties gevonden met ${search}" } };
        }

        public async Task<ResultModel<Genre>> AddAsync(Genre entity)
        {
            var resultModel = new ResultModel<Genre>();

            if (await DoesGenreNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"We vonden al een genre met de naam {entity.Name}");

                return resultModel;
            }

            _applicationDbcontext.Genres.Add(entity);
            try
            {
                await _applicationDbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }

            resultModel = new ResultModel<Genre> { Data = entity };

            return resultModel;
        }

        public async Task<ResultModel<Genre>> UpdateAsync(Genre entity)
        {
            var resultModel = new ResultModel<Genre>();

            if (!await DoesGenreIdExistAsync(entity.Id))
            {
                resultModel.Errors.Add($"Het genre met ID {entity.Id} is niet gevonden.");

                return resultModel;
            }

            if (await DoesGenreNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"We vonden al een genre met de naam  {entity.Name}");

                return resultModel;
            }

            _applicationDbcontext.Update(entity);   
            try
            {

                await _applicationDbcontext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }

            resultModel = new ResultModel<Genre> { Data = entity };
            return resultModel;
        }

        public async Task<ResultModel<Genre>> DeleteAsync(Genre entity)
        {
            var resultModel = new ResultModel<Genre>();

            _applicationDbcontext.Genres.Remove(entity);
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
