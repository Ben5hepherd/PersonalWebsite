using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Api.Data;
using PersonalWebsite.Api.Services;
using Scalar.AspNetCore;

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

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
