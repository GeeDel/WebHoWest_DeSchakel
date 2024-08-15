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
    public class CompaniesService : ICompanyService
    {


        private readonly ApplicationDbContext _applicationDbcontext;

        public CompaniesService(ApplicationDbContext applicationDbcontext)
        {
            _applicationDbcontext = applicationDbcontext;
        }

        public async Task<bool> DoesCompanyIdExistAsync(int id)
        {
            bool r = await _applicationDbcontext.Companies.AnyAsync(e => e.Id.Equals(id));
            return r;
        }

        public async Task<bool> DoesCompanyNameExistsAsync(Company entity)
        {
            return await _applicationDbcontext.Companies
                      .Where(e => !e.Id.Equals(entity.Id))
                      .AnyAsync(e => e.Name.Equals(entity.Name));
        }

        public IQueryable<Company> GetAll()
        {
            return _applicationDbcontext.Companies;
        }

        public async Task<ResultModel<IEnumerable<Event>>> GetEventsByIdAsync(int id)
        {
            var events = await _applicationDbcontext.Events
                .Include(g => g.Genres)
                .Include(c => c.Company)
                .Include(p => p.ActionUsers)
                .Where(e => e.LocationId.Equals(id))
                .ToListAsync();
            if (events.Count() == 0)
            {
                return new ResultModel<IEnumerable<Event>>
                {
                    Errors = new List<string> { "Geen voorstelling voor deze locatie." }
                };
            }
            return new ResultModel<IEnumerable<Event>> { Data = events };
        }

        public async Task<ResultModel<Company>> GetByIdAsync(int id)
        {
            if (!await DoesCompanyIdExistAsync(id))
            {
                return new ResultModel<Company> { Errors = new List<string> { $"Geen locatie gevonden met id {id}" } };
            }
            var company = await _applicationDbcontext.Companies
                .FirstOrDefaultAsync(e => e.Id == id);
            return new ResultModel<Company> { Data = company };
        }

        public async Task<ResultModel<IEnumerable<Company>>> ListAllAsync()
        {
            var Companies = await _applicationDbcontext.Companies
                .ToListAsync();
            var resultModel = new ResultModel<IEnumerable<Company>>()
            {
                Data = Companies
            };
            return resultModel;
        }

        public async Task<ResultModel<Company>> SearchAsync(string search)
        {
            search = search ?? string.Empty;
            var company = await _applicationDbcontext.Companies
                .FirstOrDefaultAsync(e => e.Name.Trim().Equals(search.Trim()));
            if (company != null)
            {
                return new ResultModel<Company>
                {
                    Data = company
                };
            }
            return new ResultModel<Company> { Errors = new List<string> { $"Geen voorstellingen gevonden met ${search}" } };
        }

        public async Task<ResultModel<Company>> AddAsync(Company entity)
        {
            var resultModel = new ResultModel<Company>();

            if (await DoesCompanyNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"We vonden al een voorstelling met de titel {entity.Name}");

                return resultModel;
            }

            _applicationDbcontext.Companies.Add(entity);
            try
            {
                await _applicationDbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resultModel.Errors.Add($"Er deed zich een onverwachte fout voor {ex.Message}.");
                return resultModel;

            }

            resultModel = new ResultModel<Company> { Data = entity };

            return resultModel;
        }



        public async Task<ResultModel<Company>> UpdateAsync(Company entity)
        {
            var resultModel = new ResultModel<Company>();

            if (!await DoesCompanyIdExistAsync(entity.Id))
            {
                resultModel.Errors.Add($"De voorstelling met ID {entity.Id} is niet gevonden.");

                return resultModel;
            }

            if (await DoesCompanyNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"We vonden al een voorstelling met de titel  {entity.Name}");

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

            resultModel = new ResultModel<Company> { Data = entity };
            return resultModel;
        }

        public async Task<ResultModel<Company>> DeleteAsync(Company entity)
        {
            var resultModel = new ResultModel<Company>();

            _applicationDbcontext.Companies.Remove(entity);
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
