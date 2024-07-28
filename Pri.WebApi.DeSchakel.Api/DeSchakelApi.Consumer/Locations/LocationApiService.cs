using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Locations
{
    public class LocationApiService : ILocationApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _DeSchakelhttpClient;

        public LocationApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.Locations);

        }

        public async Task<IEnumerable<LocationResponseApiModel>> GetAsync()
        {
            try
            {
                return await _DeSchakelhttpClient.GetFromJsonAsync<IEnumerable<LocationResponseApiModel>>("");
            }

            catch
            {
                return new List<LocationResponseApiModel>();
            }

        }

        public async Task<LocationResponseApiModel> GetByIdAsync(int id)
        {
            try
            {
                var location = await _DeSchakelhttpClient.GetFromJsonAsync<LocationResponseApiModel>($"{id}");
                return location;
            }
            catch
            {
                return new LocationResponseApiModel();
            }
        }
    }
}
