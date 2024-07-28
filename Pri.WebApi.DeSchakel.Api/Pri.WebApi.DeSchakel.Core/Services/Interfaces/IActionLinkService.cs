using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services.Interfaces
{
    public interface IActionLinkService
    {
        public Task<ResultModel<IEnumerable<NavigationItem>>> GetNavigationLinks(string areaName);
    }
}
