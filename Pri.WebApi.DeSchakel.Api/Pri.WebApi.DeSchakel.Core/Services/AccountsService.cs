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
            return  _applicationDbcontext.Users;
        }

 
        public async Task<ResultModel<IEnumerable<ApplicationUser>>> ListAllAsync()
        {
            return new ResultModel<IEnumerable<ApplicationUser>>
            { Data = await _applicationDbcontext.Users.ToListAsync() };
                
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


        
        public async Task<ResultModel<List<string>>> GetUserToken(LoginUserRequestModel login)
        {

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: false, lockoutOnFailure: false);


            if (!result.Succeeded)
            {
                return new ResultModel<List<string>>
                {
                    Errors = new List<string> { "Gebruiker is niet gevondent " }
                };
            }

            var applicationUser = await _userManager.FindByEmailAsync(login.Username);
            JwtSecurityToken token = await GenerateTokenAsync(applicationUser);
            //defined
            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token); //serialize the token
     
            var ResponseDtoToken = new ResultModel<List<string>>()
            {
                Data = new List<string> { serializedToken }
            };
            return ResponseDtoToken;  
        }
        

        private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>();

            // Loading the user Claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);

            // Loading the rolenamess and put them in a claim of a Role ClaimType
            var roleClaims = await _userManager.GetRolesAsync(user);
            foreach (var roleClaim in roleClaims)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleClaim));
            }

            var expirationDays = _configuration.GetValue<int>("JWTConfiguration:TokenExpirationDays");
            var siginingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTConfiguration:SigningKey"));
            var token = new JwtSecurityToken
            (
                issuer: _configuration.GetValue<string>("JWTConfiguration:Issuer"),
                audience: _configuration.GetValue<string>("JWTConfiguration:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(expirationDays)),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(siginingKey), SecurityAlgorithms.HmacSha256)
            );

            return token;
        }


    }


    }
