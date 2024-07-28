using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface IRolesService
    {
        IQueryable<RoleResponseModel> GetAll();
        Task<ResultModel<IEnumerable<RoleResponseModel>>> ListAllAsync();
        Task<ResultModel<RoleResponseModel>> GetByIdAsync(string id);
        Task<ResultModel<RoleResponseModel>> UpdateAsync(RoleRequestModel entity);
              /*
             Task<ResultModel<RoleResponseModel>> AddAsync(RoleResponseModel entity);
        */
        Task<ResultModel<RoleResponseModel>> DeleteAsync(RoleRequestModel entity);
        Task<ResultModel<IEnumerable<RoleResponseModel>>> SearchAsync(string search);
        Task<bool> DoesLRoleIdExistAsync(string id);
        Task<bool> DoesRoleNameExistsAsync(RoleRequestModel entity);
    }
}
