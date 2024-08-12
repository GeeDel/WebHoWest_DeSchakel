using DeSchakel.Client.Mvc.Areas.Staff.ViewModels;
using DeSchakel.Client.Mvc.Areas.User.Viewmodels;
using DeSchakel.Client.Mvc.Services;
using DeSchakel.Client.Mvc.Services.Interfaces;
using DeSchakelApi.Consumer.Models.Accounts;
using DeSchakelApi.Consumer.Users;
using DeSchakelApi.Consumer.Users.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Helpers;
using System.Runtime.Intrinsics.Arm;

namespace DeSchakel.Client.Mvc.Areas.User.Controllers
{

    [Area("User")]
    public class AuthenticationController : Controller
    {
        private readonly IUserLoginApiService _userApiService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountsApiService _accountsService;

        public AuthenticationController(IUserLoginApiService userApiService, IAuthenticationService authenticationService, 
            IAccountsApiService accountsService, IHttpClientFactory httpClientFactory, HttpClient deSchakelhttpClient)
        {
            _userApiService = userApiService;
            _authenticationService = authenticationService;
            _accountsService = accountsService;
        }

        public IActionResult Login(string returnUrl)
        {
            LoginViewModel vm = new LoginViewModel { ReturnUrl = returnUrl};
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult>Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var userToLogin = new UserLoginRequestApiModel { 
                Email = loginViewModel.Email, 
                Password= loginViewModel.Password };
            var result = await _userApiService.LoginAsync(userToLogin);
            //
            if (result == null)
            {
                ModelState.AddModelError("", "Geen toegang met deze combinatie van gegevens.");
                return View(loginViewModel);
            }
            var token = new JwtSecurityTokenHandler().ReadJwtToken(result.Token);
            // get cookie
            var identity = new ClaimsPrincipal(new ClaimsIdentity(token.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme));
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, authProperties);
            HttpContext.Session.SetString("Token", result.Token);
            if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl))
            {
                return Redirect(loginViewModel.ReturnUrl);
            }
            return RedirectToAction ("Index", "Home", new {Area = "Home"}) ;
        }

        public IActionResult AccessDenied() { return View(); }

        public async Task<IActionResult> Logout()
        {

            if (HttpContext.Session.Keys.Contains("SessionCartList"))
            {
                HttpContext.Session.SetString("SessionCartList", "");
            }
            if (HttpContext.Session.Keys.Contains("NumberOfItems"))
            {
                HttpContext.Session.SetInt32("NumberOfItems", 0);
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Home", new { Area = "Home" });
        }


        public IActionResult Register()
        {
            UserRegisterViewmodel userRegisterViewmodel = new UserRegisterViewmodel();
            return View(userRegisterViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewmodel userRegisterViewmodel)
        {
            var result = await _accountsService.GetByEmailAsync(userRegisterViewmodel.Username);


            //  if (user != null)
            if(result.Success)
            {
                ModelState.AddModelError("", "Gebruikerprofiel is al aangemaakt.");
            }
            if (!ModelState.IsValid)
            {
                return View(userRegisterViewmodel);
            }
            var userToRegister = new AccountRegisterResponseApiModel
            {
                Email = userRegisterViewmodel.Username,
                Password = userRegisterViewmodel.Password,
                Firstname = userRegisterViewmodel.FirstName,
                Lastname = userRegisterViewmodel.LastName,
                DateOfBirth = userRegisterViewmodel.DateOfBirth,
                City = userRegisterViewmodel.City,
                Zipcode = userRegisterViewmodel.Zipcode,
                Roles = new List<string>() { "Bezoeker" }
            };
            try
            {
                await _userApiService.Register(userToRegister);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Onverwachte fout: {ex.Message}");
                return View(userRegisterViewmodel);
            }
            return RedirectToAction("Index", "Home", new { Area = "Home" });
        }

        public async Task<IActionResult> Update ()
        {
            string email =  User.Claims.SingleOrDefault(e => e.Type == "email")?.Value;
            var result = await _accountsService.GetByEmailAsync(email);
            if (!result.Success)
            {
                return NotFound(result.Errors.First());
            }
            var searchedUser = result.Data;
            //
            UserAccountUpdateViewmodel staffAccountUpdateViewmodel = new UserAccountUpdateViewmodel
            {
                Id = searchedUser.Id,
                Firstname = searchedUser.Firstname,
                Lastname = searchedUser.Lastname,
                DateOfBirth = searchedUser.DateOfBirth,
                Zipcode = searchedUser.Zipcode,
                City = searchedUser.City,
                Email = searchedUser.Email,
            };

            return View(staffAccountUpdateViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserAccountUpdateViewmodel userAccountUpdateViewmodel)
        {
            var resultAccount = await _accountsService.GetByIdAsync(userAccountUpdateViewmodel.Id);
            if (!resultAccount.Success)
            {
                ModelState.AddModelError("", resultAccount.Errors.First());
            }
            if (!ModelState.IsValid)
            {
                return View(userAccountUpdateViewmodel);
            }

            var accountToUpdate = new AccountsResponseApiModel
            {
                Id = resultAccount.Data.Id,
                Email = userAccountUpdateViewmodel.Email,
                Firstname = userAccountUpdateViewmodel.Firstname,
                Lastname = userAccountUpdateViewmodel.Lastname,
                DateOfBirth = userAccountUpdateViewmodel.DateOfBirth,
                Zipcode = userAccountUpdateViewmodel.Zipcode,
                City = userAccountUpdateViewmodel.City,
                Roles = new List<string>() { "Bezoeker" }
            };

            var result = await _accountsService.UpdateByUser(accountToUpdate);   //, Request.Cookies["jwtToken"].ToString());
            if (!result.Success)
            {
                ModelState.AddModelError("", $"Volgende fout deed zich voor bij het wegschrijven in de database:" +
                    $" \n {result.Errors}.");

            }
            // cookie can only be read by javascript and only https (=secure)
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1),
            };
            if ( userAccountUpdateViewmodel.Email.Equals(resultAccount.Data.Email))
            {
            }
           return RedirectToAction("Index", "Home", new { Area = "Home" });
        }
    }
}


