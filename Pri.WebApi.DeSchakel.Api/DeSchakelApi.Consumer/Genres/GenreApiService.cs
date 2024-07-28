using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Genres
{
    public class GenreApiService : IGenreApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _deSchakelhttpClient;

        public GenreApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _deSchakelhttpClient = _httpClientFactory.CreateClient("DeSchakelApiClient");
            _deSchakelhttpClient.BaseAddress = new Uri(ApiRoutes.Genres);

        }


        public async Task<IEnumerable<BaseResponseApiModel>> GetAsync()
        {
            try
            {
                var genres = await _deSchakelhttpClient.GetFromJsonAsync<IEnumerable<BaseResponseApiModel>>("");
                return genres;
            }
            catch
            {
                return new List<BaseResponseApiModel>();
            }

        }

        public async Task<BaseResponseApiModel> GetByIdAsync(int id)
        {
            try
            {
                var searchedGenre = await _deSchakelhttpClient.GetFromJsonAsync<BaseResponseApiModel>($"{id}");
                if (searchedGenre == null)
                {
                    return null;
                }
                return searchedGenre;
            }
            catch
            {
                return null;

            }
        }
    }
}
