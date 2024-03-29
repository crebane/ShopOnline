using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Must configure connection before running migrations
builder.Services.AddDbContext<ShopOnlineDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection")));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.WithOrigins("http://localhost:7076", "https://localhost:7076")
    .AllowAnyHeader()
    .WithHeaders(HeaderNames.ContentType)); // undviker ett exception n�r applikationen startas

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
