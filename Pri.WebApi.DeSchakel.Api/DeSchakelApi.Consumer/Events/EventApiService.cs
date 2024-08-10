using DeSchakelApi.Consumer.Locations;
using DeSchakelApi.Consumer.Models.Companies;
using DeSchakelApi.Consumer.Models.Events;
using DeSchakelApi.Consumer.Models.Navigation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DeSchakelApi.Consumer.Events
{
    public class EventApiService : IEventApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _DeSchakelhttpClient;


        public EventApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _DeSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _DeSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.Events);
        }

        public async Task<EventResponseApiModel[]> GetAsync()
        {
             var searchedEvents = await _DeSchakelhttpClient.GetFromJsonAsync<EventResponseApiModel[]>("");
           try
            {
            return searchedEvents;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Array.Empty<EventResponseApiModel>();

            }
        }




        public async Task<ResultModel<EventResponseApiModel>> GetByIdAsync(int id)
        {
            ResultModel<EventResponseApiModel> resultModel = new ResultModel<EventResponseApiModel>();
            try
            {
                var searchedEvent =   await _DeSchakelhttpClient.GetFromJsonAsync<EventResponseApiModel>($"{id}");
                if (searchedEvent != null)
                {
                 resultModel.Data = searchedEvent; 
                }
                else
                {
                    resultModel.Errors = new List<string> { $"Geen voorstelling gevonden met id {id}"};
                }
            }
            catch(Exception exception)
            {
               // inform the user
                resultModel.Errors = new List<string>
                {
                    $"Fout-code: er deed zich een fout voor bij  het opzeken." +
                   $"{_DeSchakelhttpClient.BaseAddress } \n {_DeSchakelhttpClient.DefaultRequestVersion}"
                   };
            }
            return resultModel;
        }


        public async Task<ResultModel<MultipartFormDataContent>> CreateMultipart(MultipartFormDataContent mpPerformance, string token)
        {

            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _DeSchakelhttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ResultModel<MultipartFormDataContent> resultModel = new ResultModel<MultipartFormDataContent>();
            // end multipart
            var response = await _DeSchakelhttpClient.PostAsync("", mpPerformance);
            if (!response.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {response.StatusCode}" };
            }
            return resultModel;
        }


        public async Task<ResultModel<List<string>>> Add(EventResponseApiModel performance , string token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ResultModel <List<string>> resultModel = new ResultModel<List<string>>();
            var response = await _DeSchakelhttpClient.PostAsJsonAsync("", performance);
            if (!response.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {response.StatusCode}" };
            }
            return resultModel;
        }


        public async Task<ResultModel<EventResponseApiModel>> Update(MultipartFormDataContent performanceToUpdate,
            string token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _DeSchakelhttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ResultModel<EventResponseApiModel> resultModel = new ResultModel<EventResponseApiModel>();
            //   var response = await _DeSchakelhttpClient.PutAsJsonAsync("", performanceToUpdate);
            var response = await _DeSchakelhttpClient.PutAsync("", performanceToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                // inform the user
                resultModel.Errors = new List<string> { $"Fout-code: {response.StatusCode}" };
            }
            return resultModel;
        }

        public async Task<ResultModel<EventResponseApiModel>> GetByNameAsync(string title)
                {
                    string zoekString = ($"SearchTitle/{title}");
                    ResultModel< EventResponseApiModel> resultModel = new ResultModel<EventResponseApiModel>();
            try
            {
                var searchedEvent = await _DeSchakelhttpClient.GetFromJsonAsync<EventResponseApiModel>(zoekString);

                if (searchedEvent != null)
                {
                    resultModel.Data = searchedEvent;
                }
                else
                {
                    resultModel.Errors = new List<string> { $"Geen voorstelling gevonden met titel {title}" };
                }
            }
            catch (Exception exception)
            {
                // inform the user
                resultModel.Errors = new List<string>
                {
                    $"Fout-code: er deed zich een fout voor bij  het opzeken." +
                   $"{_DeSchakelhttpClient.BaseAddress } \n {_DeSchakelhttpClient.DefaultRequestVersion}"
                   };
            }
            return resultModel;

        }


        public async Task<ResultModel<EventResponseApiModel>> GetByTitleAsync(string title)
        {
            string zoekString = ($"Title/{title}");

            ResultModel<EventResponseApiModel> resultModel = new ResultModel<EventResponseApiModel>();


            try
            {
                var searchedEvent = await _DeSchakelhttpClient.GetFromJsonAsync<EventResponseApiModel>(zoekString);


                if (searchedEvent != null)
                {
                    resultModel.Data = searchedEvent;
                }
                else
                {
                    resultModel.Errors = new List<string> { $"Geen voorstelling gevonden met titel {title}" };
                }
            }
            catch (Exception exception)
            {
                // inform the user
                resultModel.Errors = new List<string>
                {
                    $"Fout-code: er deed zich een fout voor bij  het opzeken." +
                   $"{exception.Message}"
                   };
            }
            return resultModel;

        }



        public async Task DeleteAsync(int id, string token)
        {
            _DeSchakelhttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await _DeSchakelhttpClient.DeleteAsync($"{id}");
        }

        public async Task<IEnumerable<EventResponseApiModel>> GetByCompany(int id)
        {
            string zoekString = ($"{id}/company");

            try
            {

                var searchedEvents = await _DeSchakelhttpClient.GetFromJsonAsync<IEnumerable<EventResponseApiModel>>(zoekString);

                if (searchedEvents is null)
                {
                    return Array.Empty<EventResponseApiModel>();
                }
                return searchedEvents;
            }
            catch
            {
                return Array.Empty<EventResponseApiModel>();
            }
        }

        public async Task<EventResponseApiModel[]> GetByLocation(int id)
        {
            string zoekString = ($"{id}/location");

            try
            {

                var searchedEvents = await _DeSchakelhttpClient.GetFromJsonAsync<EventResponseApiModel[]>(zoekString);

                if (searchedEvents is null)
                {
                    return Array.Empty<EventResponseApiModel>();
                }
                return searchedEvents;
            }
            catch
            {
                return Array.Empty<EventResponseApiModel>();
            }
        }


        public async Task<IEnumerable<EventResponseApiModel>> GetByGenres(int id)
        {

            string zoekString = ($"{id}/genre");

            try
            {

                var searchedEvents = await _DeSchakelhttpClient.GetFromJsonAsync<IEnumerable<EventResponseApiModel>>(zoekString);

                if (searchedEvents is null)
                {
                    return Array.Empty<EventResponseApiModel>();
                }
                return searchedEvents;
            }
            catch
            {
                return Array.Empty<EventResponseApiModel>();
            }
        }
    }
}
