using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Models.Events;
using DeSchakelApi.Consumer.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Users
{
    public interface IAccountsApiService
    {
        Task<IEnumerable<AccountsResponseApiModel>> GetAsync();
        Task<ResultModel<AccountsResponseApiModel>> GetByIdAsync(string id);
        Task<ResultModel<AccountsResponseApiModel>> GetByEmailAsync(string email);
            Task<AccountsResponseApiModel> GetByLastnameAsync(string lastname);
        Task<IEnumerable<AccountsResponseApiModel>> GetByRoles(string roleName,string token);
        Task<ResultModel<AccountRegisterResponseApiModel>> Register(AccountRegisterResponseApiModel userToRegister);
        Task<ResultModel<AccountsResponseApiModel>> Update(AccountsResponseApiModel userToUpdate, string token);
        Task<ResultModel<AccountsResponseApiModel>> UpdateByUser(AccountsResponseApiModel userToUpdate);
        Task<ResultModel<List<string>>> Delete(string id, string token);
    }
}
