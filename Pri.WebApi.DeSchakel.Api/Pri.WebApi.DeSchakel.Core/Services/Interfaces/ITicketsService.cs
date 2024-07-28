using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface ITicketsService
    {
        IQueryable<Ticket> GetAll();
        Task<ResultModel<IEnumerable<Ticket>>> ListAllAsync();
        Task<ResultModel<Ticket>> GetByIdAsync(Guid id);
        Task<ResultModel<Ticket>> UpdateAsync(Ticket entity);
        Task<ResultModel<Ticket>> AddAsync(Ticket entity);
        Task<ResultModel<Ticket>> DeleteAsync(Ticket entity);
        Task<ResultModel<IEnumerable<Ticket>>> GetByGenreIdAsync(Guid id);
        Task<ResultModel<IEnumerable<Ticket>>> SearchAsync(string search);
        Task<bool> DoesTicketIdExistAsync(Guid id);
        Task<bool> DoesTicketNameExistsAsync(Ticket entity);
    }
}
