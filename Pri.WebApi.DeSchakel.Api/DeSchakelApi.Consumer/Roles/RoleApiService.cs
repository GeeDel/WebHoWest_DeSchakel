using DeSchakelApi.Consumer.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Roles
{
    public class RoleApiService : IRoleApiService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _DeSchakelhttpClient;

        public RoleApiService(IHttpClientFactory httpClientFactory) 
      {
            _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.Roles);
        }  

        public async  Task<ResultModel<IEnumerable<RoleResponseApiModel>>> GetAsync(string token)
        {
            ResultModel<IEnumerable<RoleResponseApiModel>> resultModel = new ResultModel<IEnumerable<RoleResponseApiModel>>();
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var result = await _DeSchakelhttpClient.GetFromJsonAsync<IEnumerable<RoleResponseApiModel>>("");
                if (result == null)
                {
                    resultModel.Errors = new List<string> { "Geen rollen  gevonden" };
                }
                resultModel.Data = result;
            }
            catch (Exception ex) 
            {
                resultModel.Errors = new List<string> { $"Onverwachte fout: {ex.Message}" };
            }
            return resultModel;
        }
    }
}
