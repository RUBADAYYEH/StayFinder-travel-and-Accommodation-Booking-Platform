using Application.Abstraction;
using Application.Extensions;
using Application.Services;
using Application.Services.Identity;
using Domain.Abstractions;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Presentation.Controllers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddApplicationPart(typeof(HotelController).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<StayFinderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IOwnerService,OwnerService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITrendingRepository, TrendingRepository>();
builder.Services.AddScoped<ITrendingService,TrendingService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IdentityService, IdentityService>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.RegisterAuthentication();
builder.Services.AddSwagger();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRewriter(new Microsoft.AspNetCore.Rewrite.RewriteOptions()
       .AddRedirect("^$", "swagger"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();