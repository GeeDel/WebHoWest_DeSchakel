using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Models.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Companies
{
    public interface ICompanyApiService
    {
        public Task<IEnumerable<BaseResponseApiModel>> GetAsync();
        public Task<BaseResponseApiModel> GetByIdAsync(int id, string token);
        public Task<ResultModel<string>> CreateAsyn(CompanyCreateRequestApiModel companyToCreate, string Token);
        public Task<ResultModel<string>> UpdateAsyn(CompanyUpdateRequestApiModel companyToUpdate, string Token);
        public Task<ResultModel<string>> DeleteAsyn(int id, string Token);

    }
}
