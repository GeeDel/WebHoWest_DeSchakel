using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Models.Companies;
using DeSchakelApi.Consumer.Models.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Locations
{
    public interface ILocationApiService
    {
        public Task<IEnumerable<LocationResponseApiModel>> GetAsync();
        public  Task<LocationResponseApiModel> GetByIdAsync(int id, string token);
        public Task<ResultModel<BaseResponseApiModel>> GetByName(string name, string token);
        public Task<ResultModel<string>> CreateAsyn(LocationRequestApiModel locationToCreate, string Token);
        public Task<ResultModel<string>> UpdateAsyn(LocationRequestApiModel locationToUpdate, string Token);
        public Task<ResultModel<string>> DeleteAsync(int id, string Token);
    }
}
