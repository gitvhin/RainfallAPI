using Microsoft.OpenApi.Models;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Services;
using RainfallAPI.Infrastracture.ExternalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
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

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "RainfallAPI.xml");
    c.IncludeXmlComments(filePath);

    
});

// Register application services
builder.Services.AddScoped<IRainfallService, RainfallService>();
builder.Services.AddScoped<IExternalAPIService, RestClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
