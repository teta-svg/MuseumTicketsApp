using Microsoft.EntityFrameworkCore;
using Museum.Application.Interfaces;
using Museum.Application.Services;
using Museum.Persistence;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


//бд
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MuseumTicketsDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddOpenApi();

builder.Services.AddScoped<IExhibitionService, ExhibitionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
