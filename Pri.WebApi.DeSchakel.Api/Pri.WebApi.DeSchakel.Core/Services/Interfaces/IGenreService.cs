using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface IGenreService
    {
        IQueryable<Genre> GetAll();
        Task<ResultModel<IEnumerable<Genre>>> ListAllAsync();
        Task<ResultModel<Genre>> GetByIdAsync(int id);
        Task<ResultModel<Genre>> UpdateAsync(Genre entity);
        Task<ResultModel<Genre>> AddAsync(Genre entity);
        Task<ResultModel<Genre>> DeleteAsync(Genre entity);
        Task<ResultModel<IEnumerable<Genre>>> SearchAsync(string search);
        Task<bool> DoesGenreIdExistAsync(int id);
        Task<bool> DoesGenreNameExistsAsync(Genre entity);
    }
}

