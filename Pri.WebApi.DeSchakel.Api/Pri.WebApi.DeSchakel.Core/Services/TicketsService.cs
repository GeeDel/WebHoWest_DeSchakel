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
    public class TicketsService : ITicketsService
    {
        public Task<ResultModel<Ticket>> AddAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<Ticket>> DeleteAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesTicketIdExistAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesTicketNameExistsAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Ticket> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<IEnumerable<Ticket>>> GetByGenreIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<Ticket>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<IEnumerable<Ticket>>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<IEnumerable<Ticket>>> SearchAsync(string search)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<Ticket>> UpdateAsync(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
