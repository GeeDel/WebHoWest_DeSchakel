using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Models.Events;
using DeSchakelApi.Consumer.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DeSchakelApi.Consumer.Users
{
    public class AccountsApiService : IAccountsApiService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _DeSchakelhttpClient;

        public AccountsApiService(IHttpClientFactory httpClientFactory, HttpClient deSchakelhttpClient)
        {
            _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.Accounts);
        }

        public async Task<IEnumerable<AccountsResponseApiModel>> GetAsync()
        {
            try
            {
               var allAccounts = await _DeSchakelhttpClient.GetFromJsonAsync<AccountsResponseApiModel[]>("");
                return allAccounts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Array.Empty<AccountsResponseApiModel>();
            }

        }

        public async Task<ResultModel<AccountsResponseApiModel>> GetByEmailAsync(string email)
        {

            ResultModel<AccountsResponseApiModel> resultModel = new ResultModel<AccountsResponseApiModel>();
            string zoekString = ($"ByEmail?email={email}");

            try
            {
                var searchedUser = await _DeSchakelhttpClient.GetFromJsonAsync<AccountsResponseApiModel>($"{zoekString}");
                if (searchedUser != null)
                {
                    resultModel.Data = searchedUser;
                }
                else
                {
                    resultModel.Errors = new List<string> { $"Geen account gevonden met emailadres {email}" };
                }

            }
            catch (Exception exception)
            {
                // inform the user
                resultModel.Errors = new List<string>
                {
                    $"Fout-code: er deed zich een fout voor bij  het opzeken." +
                   $"{_DeSchakelhttpClient.BaseAddress } \n {_DeSchakelhttpClient.DefaultRequestVersion}" +
                   $"\n\n {exception.Message}"

                   };
            }

            return resultModel;

        }


        public async Task<ResultModel<AccountsResponseApiModel>> GetByIdAsync(string id)
        {
            ResultModel<AccountsResponseApiModel> resultModel = new ResultModel<AccountsResponseApiModel>();
            try
            {
                var searchedUser = await _DeSchakelhttpClient.GetFromJsonAsync<AccountsResponseApiModel>($"{id}");
                if (searchedUser != null)
                {
                    resultModel.Data = searchedUser;
                }
                else
                {
                    resultModel.Errors = new List<string> { $"Geen account gevonden met id {id}" };
                }
            }
            catch(Exception exception)
            {
               // inform the user
                resultModel.Errors = new List<string>
                {
                    $"Fout-code: er deed zich een fout voor bij  het opzeken." +
                   $"{_DeSchakelhttpClient.BaseAddress } \n {_DeSchakelhttpClient.DefaultRequestVersion}" +
                   $"\n\n {exception.Message}"
                    
                   };
                }
            return resultModel;
        }

        public async  Task<IEnumerable<AccountsResponseApiModel>> GetByRoles(string roleName, string token)
        {
            string zoekString = ($"{roleName}/Roles");
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {

                var searchedAccount = await _DeSchakelhttpClient.GetFromJsonAsync<IEnumerable<AccountsResponseApiModel>>(zoekString);

                if (searchedAccount is null)
                {
                    return null;
                }
                return searchedAccount;
            }
            catch
            {
                return null;
            }
        }

        public async Task<AccountsResponseApiModel> GetByLastnameAsync(string lastname)
        {
            
            string zoekString = ($"LastName/{lastname}");
            try
            {
                var searchedUser = await _DeSchakelhttpClient.GetFromJsonAsync<AccountsResponseApiModel>($"{zoekString}");

                return searchedUser;
            }
            catch
            {
                return null;
            }

        }

        public async Task<ResultModel<AccountRegisterResponseApiModel>> Register(AccountRegisterResponseApiModel userToRegister)
        {

            ResultModel<AccountRegisterResponseApiModel> resultModel = new ResultModel<AccountRegisterResponseApiModel>();
            
            var result = await _DeSchakelhttpClient.PostAsJsonAsync<AccountRegisterResponseApiModel>("", userToRegister);
            if (!result.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {result.StatusCode}" };
            }
            return resultModel;
        }

        public async Task<ResultModel<AccountsResponseApiModel>> Update(AccountsResponseApiModel userToUpdate, string token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            ResultModel<AccountsResponseApiModel> resultModel = new ResultModel<AccountsResponseApiModel>();

            var result = await _DeSchakelhttpClient.PutAsJsonAsync<AccountsResponseApiModel>("", userToUpdate);
            if (!result.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {result.StatusCode}" };
            }
            return resultModel;
        }

        public async Task<ResultModel<AccountsResponseApiModel>> UpdateByUser(AccountsResponseApiModel userToUpdate)
        {
           // _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            ResultModel<AccountsResponseApiModel> resultModel = new ResultModel<AccountsResponseApiModel>();

            var result = await _DeSchakelhttpClient.PutAsJsonAsync<AccountsResponseApiModel>("", userToUpdate);
            if (!result.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {result.StatusCode}" };
            }
            return resultModel;
        }

        public async Task<ResultModel<List<string>>> Delete(string id, string token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            ResultModel<List<string>> resultModel = new ResultModel<List<string>>();

            var result = await _DeSchakelhttpClient.DeleteAsync(id);
            if (!result.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {result.StatusCode}" };
            }
            resultModel.Data = null;
            return resultModel;
        }
    }
}
