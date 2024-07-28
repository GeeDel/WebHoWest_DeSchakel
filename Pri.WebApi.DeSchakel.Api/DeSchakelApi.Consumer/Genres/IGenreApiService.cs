using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Genres
{
    public  interface IGenreApiService
    {
        public Task<IEnumerable<BaseResponseApiModel>> GetAsync();
        public Task<BaseResponseApiModel> GetByIdAsync(int id);
    }
}
