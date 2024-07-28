using DeSchakelApi.Consumer.Models;
using DeSchakelApi.Consumer.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer.Navigations
{
    public interface INavigationService
    {
        Task<IEnumerable<NavigationResponseApiModel>> GetAsync(string area);
    }
}
