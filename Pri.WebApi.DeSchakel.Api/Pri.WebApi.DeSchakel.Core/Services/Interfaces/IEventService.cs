using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface IEventService
    {
        IQueryable<Event> GetAll();
        Task<ResultModel<IEnumerable<Event>>> ListAllAsync();
        Task<ResultModel<IEnumerable<Event>>> GetByIdAsync(int id);
        Task<ResultModel<EventUpdateRequestModel>> UpdateAsync(EventUpdateRequestModel entity);
        Task<ResultModel<EventRequestModel>> AddAsync(EventRequestModel eventRequestModel);
        Task<ResultModel<Event>> DeleteAsync(Event entity);
        Task<ResultModel<IEnumerable<Event>>> GetByGenreIdAsync(int id);
        Task<ResultModel<IEnumerable<Event>>> GetByLocationIdAsync(int id);
        Task<ResultModel<IEnumerable<Event>>> GetByCompanyIdAsync(int id);
        Task<ResultModel<IEnumerable<Event>>> GetByTitleAsync(string title);
        Task<ResultModel<IEnumerable<Event>>> SearchAsync(string search);
        Task<bool> DoesEventIdExistAsync(int id);
        Task<bool> DoesEventNameExistsAsync(string title);
        Task<bool> DoesAllGenresExists(IEnumerable<int> eventGenreIds);
    }
}
