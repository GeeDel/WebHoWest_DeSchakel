using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface ICompanyService
    {
        IQueryable<Company> GetAll();
        Task<ResultModel<IEnumerable<Company>>> ListAllAsync();
        Task<ResultModel<Company>> GetByIdAsync(int id);
        Task<ResultModel<Company>> UpdateAsync(Company entity);
        Task<ResultModel<Company>> AddAsync(Company entity);
        Task<ResultModel<Company>> DeleteAsync(Company entity);
        Task<ResultModel<IEnumerable<Event>>> GetEventsByIdAsync(int id);
        Task<ResultModel<IEnumerable<Company>>> SearchAsync(string search);
        Task<bool> DoesCompanyIdExistAsync(int id);
        Task<bool> DoesCompanyNameExistsAsync(Company entity);
    }
}
