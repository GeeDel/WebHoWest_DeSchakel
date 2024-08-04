using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Models.Events;
using DeSchakelApi.Consumer.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Users
{
    public class UserLoginApiService : IUserLoginApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _DeSchakelhttpClient;

        public UserLoginApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.Auth);
        }

        public async Task<UserLoginResponseApiModel> LoginAsync(UserLoginRequestApiModel userToLogin)
        {
            try
            { 
                var response = await _DeSchakelhttpClient.PostAsJsonAsync("login/", userToLogin);
                if (response.IsSuccessStatusCode)
                {
                UserLoginResponseApiModel userToken = await response.Content.ReadFromJsonAsync<UserLoginResponseApiModel>();
                return userToken;
                 }
                return null;

            }
            catch
            {
                    return null;               
            }


        }


        public async Task<ResultModel<AccountRegisterResponseApiModel>> Register(AccountRegisterResponseApiModel userToRegister)
        {

            ResultModel<AccountRegisterResponseApiModel> resultModel = new ResultModel<AccountRegisterResponseApiModel>();

            var result = await _DeSchakelhttpClient.PostAsJsonAsync<AccountRegisterResponseApiModel>("register/", userToRegister);
            if (!result.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {result.StatusCode}" };
            }
            return resultModel;
        }


        public async Task<UserResponseApiModel> GetByEmailAsync(string email)
        {
            string zoekString = ($"ByEmail?email={email}");
            try
            {
            var searchedUser = await _DeSchakelhttpClient.GetFromJsonAsync<UserResponseApiModel>($"ByEmail{zoekString}");

            return searchedUser;
            }
            catch
            {
                return null;
            }

        }

        public async Task<UserResponseApiModel> GetByIdAsync(string id)
        {
            try
            {
            var searchedUser = await _DeSchakelhttpClient.GetFromJsonAsync<UserResponseApiModel>($"{id}");
            return searchedUser;

            }
            catch
            {
                return null;
            }
        }
    }
}
