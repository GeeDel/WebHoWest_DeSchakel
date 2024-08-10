using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pri.WebApi.DeSchakel.Api.Dtos.ApplicationUser;
using Pri.WebApi.DeSchakel.Api.Dtos.Event;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pri.WebApi.DeSchakel.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IAccountsService _accountsService;
        private readonly IRolesService _rolesService;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IAccountsService usersService,
            IRolesService rolesService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _accountsService = usersService;
            _rolesService = rolesService;
        }

        [AllowAnonymous]
        [HttpPost("/api/auth/login")]
        public async Task<ActionResult> Login([FromBody] LoginUserRequestDto login)
        {
            var applicationUser = await _userManager.FindByEmailAsync(login.Email);
            var result = await _signInManager.PasswordSignInAsync(applicationUser, login.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            JwtSecurityToken token = await GenerateTokenAsync(applicationUser);
            //defined
            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token); //serialize the token
            return Ok(new LoginUserResponseDto()
            {
                Token = serializedToken
            });

        }


        [AllowAnonymous]
        [HttpPost("/api/auth/register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequestDto registerUserRequestDto)
        {
            var result = new ResultModel<List<string>>();
            if (registerUserRequestDto == null)
            {
                result.Errors = new List<string> { $"Geen gegevens doorgegeven voor de gebruiker." };
                return BadRequest(result.Errors);
            }
            ApplicationUser applicationUser = new ApplicationUser
            {
                Firstname = registerUserRequestDto.Firstname,
                Lastname = registerUserRequestDto.Lastname,
                DateOfBirth = registerUserRequestDto.DateOfBirth,
                Zipcode = registerUserRequestDto.Zipcode,
                City = registerUserRequestDto.City,
                UserName = registerUserRequestDto.Email,
                Email = registerUserRequestDto.Email,
                EmailConfirmed = true,
            };
            IdentityResult identityResult;
            try
            {
                identityResult = await _userManager.CreateAsync(applicationUser, registerUserRequestDto.Password);

            }
            catch (Exception ex)
            {
                return BadRequest(result.Errors = new List<string> { $"{ex.Message}" });
            }
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    result.Errors.Add($"{error.Code} - {error.Description}");
                }
                return BadRequest(result);
            }
            applicationUser = await _userManager.FindByEmailAsync(registerUserRequestDto.Email);
            foreach (string role in registerUserRequestDto.Roles)
            {
                var managerResult = await _userManager.AddToRoleAsync(applicationUser, role);
            }
            
            //
            await _userManager.AddClaimAsync(applicationUser,
                new Claim("Name", $"{applicationUser.Firstname} {applicationUser.Lastname}"));
            await _userManager.AddClaimAsync(applicationUser, new Claim("email", applicationUser.Email));
            await _userManager.AddClaimAsync(applicationUser, new Claim("registration-date", DateTime.UtcNow.ToString("yyyy-MM-dd")));
            await _userManager.AddClaimAsync(applicationUser, new Claim("zipcode", registerUserRequestDto.Zipcode));

            return CreatedAtAction(nameof(GetById), new { id = applicationUser.Id }, registerUserRequestDto);
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

        [HttpGet]
        public async Task<IActionResult> ListAll()
        {
            var result = await _accountsService.ListAllAsync();
            var users = result.Data;
            var applicationUserResponseDto = users.Select(u => new ApplicationUserResponseDto
            {
                Id = u.Id,
                Email = u.UserName,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                DateOfBirth = u.DateOfBirth,
                ZipCode = u.Zipcode,
                City = u.City,
         //      Roles =  await _userManager.GetRolesAsync(u).Result
            });
            return Ok(applicationUserResponseDto);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = new ResultModel<ApplicationUser>();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                result.Errors = new List<string> { $"Gebruiker met id {id} werd niet gevonden" };
                return BadRequest(result.Errors);
            }
            var roles = await _userManager.GetRolesAsync(user);
            ApplicationUserResponseDto applicationUserResponseDto = new ApplicationUserResponseDto
            {
                Id = id,
                Email = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                DateOfBirth = user.DateOfBirth,
                ZipCode = user.Zipcode,
                City = user.City,
                Roles = roles,
            };
            return Ok(applicationUserResponseDto);
        }

        [AllowAnonymous]
        [HttpGet("ByEmail")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var result = new ResultModel<ApplicationUser>();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                result.Errors = new List<string> { $"Gebruiker met emailadres {email} werd niet gevonden" };
                return BadRequest(result.Errors);
            }
            var roles = _userManager.GetRolesAsync(user);
            ApplicationUserResponseDto applicationUserResponseDto = new ApplicationUserResponseDto
            {
                Id = user.Id,
                Email = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                DateOfBirth = user.DateOfBirth,
                City = user.City,
                ZipCode = user.Zipcode,
                Roles = roles.Result,
            };
            return Ok(applicationUserResponseDto);
        }


        [HttpGet("LastName/{lastName}")]
        public async Task<IActionResult> SearchByName( string lastName)
        {
            var result = await _accountsService.SearchAsync(lastName);
            if (result.Success)
            {
                var applicationUserResponseDto = result.Data.Select(u => new ApplicationUserResponseDto
                {
                    Id = u.Id,
                    Email = u.UserName,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    DateOfBirth = u.DateOfBirth,
                    Roles = _userManager.GetRolesAsync(u).Result
                });
                return Ok(applicationUserResponseDto);
            }
            result.Errors = new List<string> { $"Geen gebruikers gevonden met de familienaam {lastName}." };
            return BadRequest(result.Errors);
        }

        [Authorize(Policy =("MemberOfStaff"))]
        [HttpGet("{rolename}/Roles")]
        public async Task<IActionResult> GetRole( string rolename)
        {
            var result = new ResultModel<ApplicationUser>();
            var users = await _userManager.GetUsersInRoleAsync(rolename);
            if (users.Count() == 0)
            {
                result.Errors = new List<string> { $"Geen gebruikers vor de rol {rolename}" };
                return BadRequest(result.Errors);
            }

            var applicationUserResponseDto = users.Select(u => new ApplicationUserResponseDto
            {
                Id = u.Id,
                Email = u.UserName,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                DateOfBirth = u.DateOfBirth,
                ZipCode = u.Zipcode,
                City = u.City,
                Roles =  _userManager.GetRolesAsync(u).Result
            });
            return Ok(applicationUserResponseDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ApplicationUserResponseDto applicationUserResponseDto)
        {
            var searchedUser = await _userManager.FindByIdAsync(applicationUserResponseDto.Id);
            if (searchedUser == null)
            {
                ResultModel<ApplicationUser> ResultModel = new ResultModel<ApplicationUser> { 
                    Errors = new List<string> { $"Gebruiker met id {applicationUserResponseDto.Id} is niet gevonden" } };
                return BadRequest(ResultModel);
            }
     
            var roles =  _userManager.GetRolesAsync(searchedUser).Result;
            foreach(var role in roles)
            {
            await _userManager.RemoveFromRoleAsync(searchedUser, role);
            }
            //
            searchedUser.Firstname = applicationUserResponseDto.Firstname;
            searchedUser.Lastname = applicationUserResponseDto.Lastname;
            searchedUser.DateOfBirth = applicationUserResponseDto.DateOfBirth;
            searchedUser.Zipcode = applicationUserResponseDto.ZipCode;
            searchedUser.City = applicationUserResponseDto.City;
    //        searchedUser.Email = applicationUserResponseDto.Email;
    //        searchedUser.UserName = applicationUserResponseDto.Email;
            //
            IdentityResult identityResult = new IdentityResult();
            try
            {
                identityResult = await _userManager.UpdateAsync(searchedUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"{identityResult.Errors}");
            }
            if (!identityResult.Succeeded)
            {
                return BadRequest($"{identityResult.Errors}");
            }
            foreach (string role in applicationUserResponseDto.Roles)
            {
                var managerResult = await _userManager.AddToRoleAsync(searchedUser, role);
            }
            return Ok();
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(string id)
        {
            var result = new ResultModel<ApplicationUser>();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                result.Errors = new List<string> { $"Gebruiker met id {id} werd niet gevonden" };
                return BadRequest(result.Errors);
            }
            try
            {
                   await _userManager.DeleteAsync(user);
            }
            catch   (Exception ex)
            {
                return BadRequest(result.Errors = new List<string> { $"{ex.Message}"});
            }
            return Ok();
        }

    }

}
