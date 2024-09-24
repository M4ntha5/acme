using Acme.Core;
using Acme.Data.Contexts;
using Acme.Utilities.BackgroundJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "full-date",
        Example = new OpenApiString("2024-01-01"),
    });
});


var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Default connection string is null");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(defaultConnectionString);
});

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ServiceCollectionExtension).Assembly);

builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddBackgroundJobs(typeof(Program).Assembly);

builder.Services.AddQuartz();
builder.Services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });

var app = builder.Build();
{
    using var scope = app.Services.CreateScope();
    
    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    var pendingMigrations = applicationDbContext.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        applicationDbContext.Database.Migrate();
    }
    
    var backgroundJobScheduler = scope.ServiceProvider.GetRequiredService<BackgroundJobScheduler>();
    await backgroundJobScheduler.ScheduleBackgroundJobs();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(configurePolicy =>
{
    configurePolicy.AllowAnyOrigin();
    configurePolicy.AllowAnyMethod();
    configurePolicy.AllowAnyHeader();
});


app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();



app.Run();