using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pri.WebApi.DeSchakel.Core.Services
{
    public  class AccountsService : IAccountsService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbcontext;


        public AccountsService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext applicationDbcontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _applicationDbcontext = applicationDbcontext;
        }

        // add, delet via _userManager

        public async Task<bool> DoesApplicationUserIdExistAsync(string id)
        {
            return await _applicationDbcontext.Users.AnyAsync(p => p.Id.Equals(id));
        }

        public async Task<bool> DoesApplicationUserNameExistsAsync(ApplicationUser entity)
        {
            return await _applicationDbcontext.Users
           .Where(p => p.Id != entity.Id)
           .AnyAsync(p => p.UserName.Equals(entity.UserName));
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return  _applicationDbcontext.Users
                .OrderBy(f=>f.Lastname);
        }

 
        public async Task<ResultModel<IEnumerable<ApplicationUser>>> ListAllAsync()
        {
            return new ResultModel<IEnumerable<ApplicationUser>>
            { Data = await _applicationDbcontext.Users
            .OrderBy(f => f.Lastname)
            .ToListAsync() };
                
        }

        public async Task<ResultModel<IEnumerable<ApplicationUser>>> SearchAsync(string search)
        {
            search = search ?? string.Empty;
            var users = await _applicationDbcontext.Users
                                .Where(u => u.Lastname
                        .Contains(search.Trim()))
                      .ToListAsync();
            if (users.Count() == 0)
            {
                return new ResultModel<IEnumerable<ApplicationUser>>
                {
                    Errors = new List<string> { $"Geen gebruiker gevonden met {search}" }
                };
            }
            return new ResultModel<IEnumerable<ApplicationUser>> { Data = users };
        }

        public Task<ResultModel<ApplicationUser>> UpdateAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }


    }


    }
