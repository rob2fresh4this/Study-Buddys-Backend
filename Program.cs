using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Study_Buddys_Backend.Context;
using Study_Buddys_Backend.Hubs;
using Study_Buddys_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<CommunityServices>();

// Database connection
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

// CORS configuration
builder.Services.AddCors(Options =>
{
    Options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// JWT authentication setup
var secretKey = builder.Configuration["Jwt:Key"];
var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

string serverUrl = "https://studybuddies-g9bmedddeah6aqe7.westus-01.azurewebsites.net/";
string localHostUrl = "https://localhost:5233/";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuers = new List<string> { serverUrl, localHostUrl },
        ValidAudiences = new List<string> { serverUrl, localHostUrl },
        IssuerSigningKey = signingCredentials
    };
});

// Add SignalR service
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS, Authentication, and Authorization
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Map SignalR hub after Authentication/Authorization
app.MapHub<ChatHub>("/chathub");

// Map API controllers
app.MapControllers();

app.Run();
