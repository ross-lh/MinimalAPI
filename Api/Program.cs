using MinimalAPI.DTO;
using MinimalAPI.Data;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Interfaces;
using MinimalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.ModelViews;
using MinimalAPI.Entities;
using System.Runtime.InteropServices;
using MinimalAPI.Enums;
using System.Windows.Markup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key)) key = "1234";

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insert the JWT token:"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql")));
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Admins
string GenerateJwtToken(Admin admin)
{
    if (string.IsNullOrEmpty(key)) return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Username", admin.UserName),
        new Claim("Profile", admin.Profile),
        new Claim(ClaimTypes.Role, admin.Profile)
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/admins/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
{
    var admin = adminService.Login(loginDTO);
    if (admin != null)
    {
        string token = GenerateJwtToken(admin);
        return Results.Ok(new LoggedAdmin
        {
            UserName = admin.UserName,
            Profile = admin.Profile,
            Token = token
        });
    }
    else
        return Results.Unauthorized();

}).AllowAnonymous().WithTags("Admins");

app.MapGet("/admins", ([FromQuery] int? page, IAdminService adminService) =>
{
    var adms = new List<AdminModelView>();
    var admins = adminService.All(page);

    foreach (var admin in admins)
    {
        adms.Add(new AdminModelView
        {
            Id = admin.Id,
            UserName = admin.UserName,
            Profile = admin.Profile
        });
    }

    return Results.Ok(adms);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Admins");

app.MapGet("/admins/{id}", ([FromRoute] int id, IAdminService adminService) =>
{
    var admin = adminService.SearchById(id);
    if (admin == null) return Results.NotFound();

    var adm = new AdminModelView
    {
        Id = admin.Id,
        UserName = admin.UserName,
        Profile = admin.Profile
    };

    return Results.Ok(adm);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Admins");

app.MapPost("/admins", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
{
    var validation = new ValidationError
    {
        Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(adminDTO.UserName))
        validation.Messages.Add("Username is required");
    if (string.IsNullOrEmpty(adminDTO.Password))
        validation.Messages.Add("Password is required");
    if (adminDTO.Profile == null)
        validation.Messages.Add("Profile is required");

    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);

    var admin = new Admin
    {
        UserName = adminDTO.UserName,
        Password = adminDTO.Password,
        Profile = adminDTO.Profile.ToString() ?? Profile.Editor.ToString()
    };

    adminService.Insert(admin);

    var adminView = new AdminModelView
    {
        Id = admin.Id,
        UserName = admin.UserName,
        Profile = admin.Profile
    };

    return Results.Created($"/admins/{admin.Id}", adminView);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Admins");
#endregion

#region Vehicles
ValidationError validationDTO(VehicleDTO vehicleDTO)
{
    var validation = new ValidationError
    {
        Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(vehicleDTO.Model))
        validation.Messages.Add("Model is required");

    if (string.IsNullOrEmpty(vehicleDTO.Model))
        validation.Messages.Add("Model is required");

    if (vehicleDTO.Year < 1886 || vehicleDTO.Year > DateTime.Now.Year + 1)
        validation.Messages.Add("Vehicle is either too old or from the future");

    return validation;
}

app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var validation = validationDTO(vehicleDTO);

    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);

    var vehicle = new Vehicle
    {
        Model = vehicleDTO.Model,
        Make = vehicleDTO.Make,
        Year = vehicleDTO.Year
    };

    vehicleService.Insert(vehicle);

    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
.WithTags("Vehicles");

app.MapGet("/vehicles", ([FromQuery] int? page, IVehicleService vehicleService) =>
{
    var vehicles = vehicleService.All(page);
    return Results.Ok(vehicles);
}).RequireAuthorization().WithTags("Vehicles");

app.MapGet("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.SearchById(id);
    if (vehicle == null) return Results.NotFound();
    return Results.Ok(vehicle);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
.WithTags("Vehicles");

app.MapPut("/vehicles/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.SearchById(id);
    if (vehicle == null) return Results.NotFound();

    var validation = validationDTO(vehicleDTO);
    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);

    vehicle.Model = vehicleDTO.Model;
    vehicle.Make = vehicleDTO.Make;
    vehicle.Year = vehicleDTO.Year;

    vehicleService.Update(vehicle);

    return Results.Ok(vehicle);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Vehicles");

app.MapDelete("/vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.SearchById(id);
    if (vehicle == null) return Results.NotFound();

    vehicleService.Delete(vehicle);

    return Results.NoContent();
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Vehicles");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

// ALWAYS AuthENTICATION before AuthORIZATION
app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion