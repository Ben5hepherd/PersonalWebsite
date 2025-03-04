using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalWebsiteBFF.Core.Interfaces;
using PersonalWebsiteBFF.Core.Services;
using PersonalWebsiteBFF.Infrastructure.Data;
using Scalar.AspNetCore;
using System.Text;

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

var app = builder.Build();

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
