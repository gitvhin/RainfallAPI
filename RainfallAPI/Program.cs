using Microsoft.OpenApi.Models;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Validate the base URL at service registration time
var configuration = builder.Configuration;
var baseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");

if (string.IsNullOrEmpty(baseUrl))
{
    throw new InvalidOperationException("API base URL is not configured.");
}

// Configure named HttpClient with base URL
builder.Services.AddHttpClient("RainfallAPI", client =>
{
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0",
        Title = "Rainfall Api",
        Description = "An API which provides rainfall reading data",
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com")
        }
    });

    c.AddServer(new OpenApiServer { Url = "https://localhost:3000", Description = "Rainfall Api" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


// Register application services
builder.Services.AddScoped<IRainfallService, RainfallService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseStaticFiles();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
