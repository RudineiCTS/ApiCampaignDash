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
builder.Services.AddScoped<ICampaignResultReportRepository, CampaignResultReportRepository>();
builder.Services.AddScoped<ICampaignResultReportService, CampaignResultReportService>();
builder.Services.AddScoped<ICampaignSummaryRepository, CampaignSummaryRepository>();
builder.Services.AddScoped<ICampaignSummaryService, CampaignSummaryService>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductLineRepository, ProductLineRepository>();
builder.Services.AddScoped<IProductLineService, ProductLineService>();
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowElectronApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://192.168.3.208:5173") // origem do seu Electron/Vite
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();
app.UseCors("AllowElectronApp");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// ─── DbContext ───────────────────────────────────────────

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
