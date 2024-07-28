using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface ILocationsService
    {
        IQueryable<Location> GetAll();
        Task<ResultModel<IEnumerable<Location>>> ListAllAsync();
        Task<ResultModel<Location>> GetByIdAsync(int id);
        Task<ResultModel<Location>> UpdateAsync(Location entity);
        Task<ResultModel<Location>> AddAsync(Location entity);
        Task<ResultModel<Location>> DeleteAsync(Location entity);
        Task<ResultModel<IEnumerable<Location>>> SearchAsync(string search);
        Task<bool> DoesLocationIdExistAsync(int id);
        Task<bool> DoesLocationNameExistsAsync(Location entity);
    }
}
