using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Models.Companies;
using DeSchakelApi.Consumer.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Companies
{
    public class CompanyApiService : ICompanyApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _DeSchakelhttpClient;

        public CompanyApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.Companies);
        }


        public async Task<IEnumerable<BaseResponseApiModel>> GetAsync()
        {
            try
            {
                return await _DeSchakelhttpClient.GetFromJsonAsync<IEnumerable<BaseResponseApiModel>>("");

            }
            catch
            {
                return new List<BaseResponseApiModel>();

            }
        }

        public async Task<BaseResponseApiModel> GetByIdAsync(int id, string token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _DeSchakelhttpClient.GetFromJsonAsync<BaseResponseApiModel>($"{id}");
                return response;
            }
           catch (Exception ex) 
            {

                Console.WriteLine(ex.Message);
                return null;

            }
        }

        public async Task<ResultModel<string>> UpdateAsyn(CompanyUpdateRequestApiModel companyToUpdate, string Token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);
            ResultModel <string> resultModel = new ResultModel<string>();
            var response = await _DeSchakelhttpClient.PutAsJsonAsync("", companyToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                // inform the user
                ResultModel<string> errors = new ResultModel<string> { Errors = new List<string> { $"Fout-code: {response.StatusCode}" } };
            }
            return resultModel;
        }

        public async Task<ResultModel<string>> CreateAsyn(CompanyCreateRequestApiModel companyToCreate, string Token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Token);
            ResultModel<string> resultModel = new ResultModel<string>();
             var response = await _DeSchakelhttpClient.PostAsJsonAsync("", companyToCreate);
           if (!response.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {response.StatusCode}" };
            }
            return resultModel;
        }

        public async Task<ResultModel<string>> DeleteAsyn(int id, string Token)
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
            return resultModel ;
        }

    }
}
