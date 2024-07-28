using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeSchakelApi.Consumer.Models.Events;

namespace DeSchakelApi.Consumer.Events
{
    public interface IEventApiService
    {
        Task<EventResponseApiModel[]> GetAsync();
        Task<ResultModel<EventResponseApiModel>> GetByIdAsync(int id);
        Task<IEnumerable<EventResponseApiModel>> GetByGenres(int id);
        Task<ResultModel<EventResponseApiModel>> GetByTitleAsync(string title);
        Task<IEnumerable<EventResponseApiModel>> GetByCompany(int id);
        Task<EventResponseApiModel[]> GetByLocation(int id);
        Task<ResultModel<List<string>>> Add(EventResponseApiModel performance, string token);
        Task<ResultModel<MultipartFormDataContent>> CreateMultipart(MultipartFormDataContent mpPerformance, string token);
        Task<ResultModel<EventResponseApiModel>> Update(MultipartFormDataContent performanceToUpdateMp,
                    string token);
         Task DeleteAsync(int id, string token);
    }
}
