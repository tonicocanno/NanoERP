using MongoDB.Driver;
using NanoERP.API.Configurations;
using NanoERP.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using dotenv.net;
using NanoERP.API.Services;
using NanoERP.API.Utilities;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? builder.Configuration["Jwt:Key"] ?? "";
string connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PartnerService>();

var emailService = new EmailServiceFactory(builder.Configuration).Create();
builder.Services.AddSingleton(emailService);

var mongoClient = new MongoClient(connectionString);
var database = mongoClient.GetDatabase("NanoCluster");
builder.Services.AddSingleton(DataContext.Create(database));

builder.Services.Configure(CustomApiBehaviorOptions.ConfigureInvalidModelStateResponse());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.MapControllers();

app.Run();

public partial class Program {}