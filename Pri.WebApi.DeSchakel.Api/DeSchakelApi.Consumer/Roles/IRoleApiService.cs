using DeSchakelApi.Consumer.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Roles
{
    public interface IRoleApiService
    {
        Task<ResultModel<IEnumerable<RoleResponseApiModel>>> GetAsync(string token);
    }
}
