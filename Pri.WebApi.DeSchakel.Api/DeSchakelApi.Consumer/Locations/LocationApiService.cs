using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Models.Companies;
using DeSchakelApi.Consumer.Models.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

 

        public async Task<ResultModel<BaseResponseApiModel>> GetByName(string name, string token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization =
                      new AuthenticationHeaderValue("Bearer", token);

            string zoekString = ($"Name/{name}");
            ResultModel<BaseResponseApiModel> baseResponseApiModel = new ResultModel<BaseResponseApiModel>();
            try
            {
                var response = await _DeSchakelhttpClient.GetFromJsonAsync<BaseResponseApiModel>(zoekString);
                if (response != null)
                {
                    baseResponseApiModel.Data = response;
                }
                else
                {
                    baseResponseApiModel.Errors = new List<string> { $"Geen locatie gevonden met naam {name}" };
                }
            }
            catch (Exception ex)
            {
                // inform the user
                baseResponseApiModel.Errors = new List<string>
                {
                    $"Fout-code: er deed zich een fout voor bij  het opzoeken op naam." +
                   $"{_DeSchakelhttpClient.BaseAddress } \n {_DeSchakelhttpClient.DefaultRequestVersion}"
                   };
            }
            return baseResponseApiModel;

        }

        public async Task<LocationResponseApiModel> GetByIdAsync(int id, string token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _DeSchakelhttpClient.GetFromJsonAsync<LocationResponseApiModel>($"{id}");
                return response;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;

            }
        }

        public async Task<ResultModel<string>> UpdateAsyn(LocationRequestApiModel locationToUpdate, string Token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);
            ResultModel<string> resultModel = new ResultModel<string>();
            var response = await _DeSchakelhttpClient.PutAsJsonAsync("", locationToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> {$"Fout-code: {response.StatusCode}" };
            }
            return resultModel;
        }

        public async Task<ResultModel<string>> CreateAsyn(LocationRequestApiModel locationToCreate, string Token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);
            ResultModel<string> resultModel = new ResultModel<string>();
            var response = await _DeSchakelhttpClient.PostAsJsonAsync("", locationToCreate);
            if (!response.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {response.StatusCode}" };
            }
            return resultModel;
        }

        public async Task<ResultModel<string>> DeleteAsync(int id, string Token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue("Bearer", Token);
            ResultModel<string> resultModel = new ResultModel<string>();
            var response = await _DeSchakelhttpClient.DeleteAsync($"{id}");
            if (!response.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {response.StatusCode}" };
            }
            return resultModel;
        }


    }
}
