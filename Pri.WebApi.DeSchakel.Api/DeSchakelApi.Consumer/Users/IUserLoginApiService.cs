using DeSchakelApi.Consumer.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Users
{
    public interface IUserLoginApiService
    {
        Task<UserLoginResponseApiModel>LoginAsync(UserLoginRequestApiModel userLoginRequestApiModel);
           Task<UserResponseApiModel> GetByIdAsync(string id);
        Task<UserResponseApiModel> GetByEmailAsync(string email);

    }
}
