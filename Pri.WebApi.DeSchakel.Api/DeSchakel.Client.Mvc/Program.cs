using DeSchakel.Client.Mvc.Services;
using DeSchakel.Client.Mvc.Services.Interfaces;
using DeSchakelApi.Consumer.Companies;
using DeSchakelApi.Consumer.Events;
using DeSchakelApi.Consumer.Genres;
using DeSchakelApi.Consumer.Locations;
using DeSchakelApi.Consumer.Navigations;
using DeSchakelApi.Consumer.Roles;
using DeSchakelApi.Consumer.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Globalization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IEventApiService, EventApiService>();
builder.Services.AddScoped<ICompanyApiService, CompanyApiService>();
builder.Services.AddScoped<ILocationApiService, LocationApiService>();
builder.Services.AddScoped<IGenreApiService, GenreApiService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<IUserLoginApiService, UserLoginApiService>();
builder.Services.AddScoped<IAccountsApiService, AccountsApiService>();
builder.Services.AddScoped<IRoleApiService, RoleApiService>();
builder.Services.AddScoped<IFileService,FileService>();
builder.Services.AddScoped<IFormBuilder,FormBuilder>();
// services - increase max size for upload -  to increase the maximum request size
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});
// session
builder.Services.AddSession(options =>
{
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
});
// policies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(35);
    options.SlidingExpiration = true;
    options.LoginPath = "/User/Authentication/Login";
    options.AccessDeniedPath = "/User/Authentication/AccessDenied";
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MemberOfStaff", policy =>
        policy.RequireAssertion(context =>
        {
            return context.User.IsInRole("Programmator") || context.User.IsInRole("Onthaal")
                        || context.User.IsInRole("Admin");
        }
     ));
    options.AddPolicy("MemberOfManagement", policy =>
    policy.RequireAssertion(context =>
    {
        return context.User.IsInRole("Programmator") || context.User.IsInRole("Admin");
    }
    ));
    options.AddPolicy("NewAbos", policy =>
        policy.RequireAssertion(context =>
        {
            var newAboClaimValue = context.User.Claims
                            .SingleOrDefault(c => c.Type == "registration-date")?.Value;
            if (DateTime.TryParseExact(newAboClaimValue, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var registrationDate))
            {
                return registrationDate.AddYears(+1) > DateTime.Now && DateTime.Now < registrationDate.AddYears(+2);

            }
            return false;
    }
    ));
    options.AddPolicy("FromWaregem", policy =>
        policy.RequireAssertion(context =>
        {
        const string ZipcodesFromWaregem = "B8790B8791B8792B8793";
        var zipCodeValue = context.User.Claims
                        .SingleOrDefault(c => c.Type == "zipcode")?.Value;
        if (String.IsNullOrEmpty(zipCodeValue))
            {
                return false;
            }
        else
            {
                return ZipcodesFromWaregem.Contains(zipCodeValue);
            }
        }
    ));

});
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "staff",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Staff" }
);

app.MapControllerRoute(
    name: "user",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "User" }
);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
