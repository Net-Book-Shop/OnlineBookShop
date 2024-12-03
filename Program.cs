using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineBookShop.Data;
using OnlineBookShop.Security;
using OnlineBookShop.Service.Impl;
using OnlineBookShop.Service;
using System.Text;
using OnlineBookShop.Repository;
using Microsoft.AspNetCore.Identity;
using OnlineBookShop.Model;
using ProductMiniApi.Repository.Implementation;
using Microsoft.Extensions.FileProviders;
using OnlineBookShop.Other;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    // Add CORS configuration
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()                   
              .AllowAnyMethod()                    
              .AllowCredentials();              
    });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, // Corrected to SecuritySchemeType.Http
        Scheme = "bearer", // Added the scheme to specify the type of HTTP scheme used
        Description = "Enter your JWT Access Token", // Corrected the spelling of "Access"
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        }
    };

    // Add the security definition for Swagger
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    // Add the security requirement for Swagger
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});


builder.Services.
    AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))));

builder.Services.AddScoped<JwtService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);



// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]))
    };
});



//service add
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddAuthentication();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPrivilageService, PrivilageService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICustomerService,CustomerService>();
//repository
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PrivilegeRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<PrivilegeDetailsRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<CustomerRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseCors("AllowAngularApp");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
