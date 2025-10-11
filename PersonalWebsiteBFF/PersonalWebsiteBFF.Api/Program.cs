using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalWebsiteBFF.Common.DTOs;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Core.Services;
using PersonalWebsiteBFF.Domain.Entities;
using PersonalWebsiteBFF.Infrastructure.Data;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<AppDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetValue<string>("ConnectionString") ??
                               Environment.GetEnvironmentVariable("CONNECTION_STRING");

        options.UseNpgsql(connectionString);
    });

var validIssuer = builder.Configuration["AuthSettings:Issuer"] ?? Environment.GetEnvironmentVariable("AUTH_ISSUER");
var validAudience = builder.Configuration["AuthSettings:Audience"] ?? Environment.GetEnvironmentVariable("AUTH_AUDIENCE");
var token = builder.Configuration["AuthSettings:Token"] ?? Environment.GetEnvironmentVariable("AUTH_TOKEN");

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var error = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .FirstOrDefault()?.ErrorMessage ?? "Invalid input";
        return new OkObjectResult(new ResultDto { Success = false, ErrorMessage = error });
    };
});

// Add authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token ?? ""))
        };
    });

// Add authorization
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Configure CORS based on the environment (Development or Production)
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: myAllowSpecificOrigins,
        policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                // Allow only localhost during development
                policy.WithOrigins("http://localhost:4200", "http://localhost")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }
            else
            {
                // Allow your live domain in production
                policy.WithOrigins("https://www.ben-shepherd.co.uk")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials(); // If using authentication cookies or tokens
            }
        });
});

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(60);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors(myAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
