using Microsoft.EntityFrameworkCore;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Services
{
    public class ActionLinkService : IActionLinkService
    {
        private readonly ApplicationDbContext _applicationDbcontext;

        public ActionLinkService(ApplicationDbContext applicationDbcontext)
        {
            _applicationDbcontext = applicationDbcontext;
        }

        public async Task<ResultModel<IEnumerable<NavigationItem>>> GetNavigationLinks(string areaName)
        {
            var navigationLinks =  await   _applicationDbcontext.Navigations
                .Where(n => n.Area == areaName)
                .OrderBy(n => n.Position)
                .ToListAsync();
            if (navigationLinks.Count() == 0)
            {
                return new ResultModel<IEnumerable<NavigationItem>> 
                { Errors = new List<string> {"Fout: geen navigatie-items gevonden." } };
            }
            return new ResultModel<IEnumerable<NavigationItem>> { Data = navigationLinks };
        }
    }
}
