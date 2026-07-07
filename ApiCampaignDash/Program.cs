using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Application.Mappings;
using ApiCampaignDash.Application.Services;
using ApiCampaignDash.Domain.Interfaces;
using ApiCampaignDash.Infrastructure.Data;
using ApiCampaignDash.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(cfg => { }, typeof(CampaignProfile));
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// ─── DbContext ───────────────────────────────────────────

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
