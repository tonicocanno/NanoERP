using NanoERP.API.Configurations;
using dotenv.net;
using NanoERP.API.Services;
using NanoERP.API.Utilities;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? builder.Configuration["Jwt:Key"] ?? "";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PartnerService>();
builder.Services.AddScoped<ProductService>();

var emailService = new EmailServiceFactory(builder.Configuration).Create();
builder.Services.AddSingleton(emailService);

builder.Services.AddMongoDb(builder.Configuration);

builder.Services.Configure(CustomApiBehaviorOptions.ConfigureInvalidModelStateResponse());

builder.Services.AddJwtAuthentication(builder.Configuration);

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

public partial class Program { }