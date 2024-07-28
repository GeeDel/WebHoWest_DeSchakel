using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pri.WebApi.DeSchakel.Core.Data;
using Pri.WebApi.DeSchakel.Core.Entities;
using Pri.WebApi.DeSchakel.Core.Services.Interfaces;
using Pri.WebApi.DeSchakel.Core.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
// own services
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<ICompanyService, CompaniesService>();
builder.Services.AddScoped<ILocationsService, LocationsService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<ITicketsService, TicketsService>();
builder.Services.AddScoped<IActionLinkService, ActionLinkService>();
// Identity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
// AddIdentity because UI package is not needed, if UI is needed then AddDefaultIdentity<ApplicationUser>
{
    options.SignIn.RequireConfirmedEmail = false;
    //configure options for testing purposes
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 2;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

//
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWTConfiguration:Issuer"],
        ValidAudience = builder.Configuration["JWTConfiguration:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfiguration:SigningKey"]))

    };
});
//
builder.Services.AddControllers();


// policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, new List<string> { "admin", "Admin" });
    });
    options.AddPolicy("Programmator", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, new List<string> { "programmator", "Programmator" });
    });
    options.AddPolicy("Reception", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, new List<string> { "onthaal", "Onthaal" });
    });
    options.AddPolicy("Visitor", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, new List<string> { "bezoeker", "Bezoeker" });
    });
    options.AddPolicy("MemberOfStaff", policy =>
        policy.RequireAssertion(context =>
        {
            bool isStaff = context.User.IsInRole("Programmator") || context.User.IsInRole("Onthaal")
                        || context.User.IsInRole("Admin");
            return isStaff;
        }
      ));
    options.AddPolicy("MemberOfManagement", policy =>
        policy.RequireAssertion(context =>
        {
            bool isManager = context.User.IsInRole("Programmator") || context.User.IsInRole("Admin");
            return isManager;
        }
      ));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
// volgende code  implementeert authetication bij Swagger
// volgens https://www.infoworld.com/article/3650668/implement-authorization-for-swagger-in-aspnet-core.html

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "De Schakel Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//
app.UseSwagger();
app.UseSwaggerUI();
//
app.MapControllers();
app.UseStaticFiles();

app.Run();
