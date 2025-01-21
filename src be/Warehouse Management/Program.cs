using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Warehouse_Management.Data;
using Warehouse_Management.Mappings;
using Warehouse_Management.Middlewares;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Repositories.Repository;
using Warehouse_Management.Services.IService;
using Warehouse_Management.Services.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//DB
builder.Services.AddDbContext<WareHouseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<WareHouseDbContext>()
    .AddDefaultTokenProviders();

// Password
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
});

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    });

//Repo and Service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

//Handler
builder.Services.AddScoped<IExceptionHandler, BadRequestExceptionHandler>();
builder.Services.AddScoped<IExceptionHandler, NotFoundExceptionHandler>();
builder.Services.AddScoped<IExceptionHandler, InternalServerErrorHandler>();
builder.Services.AddScoped<IExceptionHandler, UnauthorizedExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
