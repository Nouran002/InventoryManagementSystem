
using Hangfire;
using Inventory.BL.Entities;
using Inventory.BL.Interfaces;
using Inventory.BL.Services;
using Inventory.DAL.Persistence;
using Inventory.DAL.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var ConnectionString = builder.Configuration.GetConnectionString("CS") ??
    throw new InvalidOperationException("No connection string was found");

builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseSqlServer(ConnectionString)
);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<ILowStockChecker, LowStockChecker>();
builder.Services.AddSingleton<ReportPdfGeneratorService>();
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("CS")));

builder.Services.AddHangfireServer();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "ReportPolicy", options =>
    {
        options.PermitLimit = 3;
        options.Window = TimeSpan.FromMinutes(1);
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;
    });

    // Add custom rate limit response
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        var response = new
        {
            message = "Too many requests. Please try again later.",
        };

        await context.HttpContext.Response.WriteAsJsonAsync(response, token);
    };
});



var app = builder.Build();
app.UseRateLimiter();
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<ILowStockChecker>(
    "low-stock-checker-job",
    checker => checker.CheckLowStockAsync(),
    Cron.Daily
);

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
