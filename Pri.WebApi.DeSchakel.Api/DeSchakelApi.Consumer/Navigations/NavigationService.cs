using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Navigations
{
    public  class NavigationService : INavigationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _DeSchakelhttpClient;

        public NavigationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.ActionLinks);
        }


        public async Task<IEnumerable<NavigationResponseApiModel>> GetAsync(string area)
        {

            var navigationItems = await _DeSchakelhttpClient.GetFromJsonAsync<IEnumerable<NavigationResponseApiModel>>($"{area}");
            return navigationItems;
        }
    }
}
