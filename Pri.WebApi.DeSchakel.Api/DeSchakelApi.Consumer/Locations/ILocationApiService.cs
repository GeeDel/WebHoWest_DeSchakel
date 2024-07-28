using DeSchakelApi.Consumer.Models;
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
        public Task<LocationResponseApiModel> GetByIdAsync(int id);
    }
}
