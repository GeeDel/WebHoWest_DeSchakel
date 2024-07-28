using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface IAccountsService
    {
        IQueryable<ApplicationUser> GetAll();
        Task<ResultModel<IEnumerable<ApplicationUser>>> ListAllAsync();
 //       Task<ResultModel<ApplicationUser>> GetByIdAsync(string id);
        Task<ResultModel<ApplicationUser>> UpdateAsync(ApplicationUser entity);
        Task<ResultModel<IEnumerable<ApplicationUser>>> SearchAsync(string search);
        Task<bool> DoesApplicationUserIdExistAsync(string id);
        Task<bool> DoesApplicationUserNameExistsAsync(ApplicationUser entity);
        //
        public Task<ResultModel<List<String>>> GetUserToken(LoginUserRequestModel login);
    }
}
