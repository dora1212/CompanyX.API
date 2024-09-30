using CompanyX.API.BusinessLogic.Interfaces;
using CompanyX.API.BusinessLogic.MappingProfiles;
using CompanyX.API.BusinessLogic.Services;
using CompanyX.API.DataAccess.DatabaseContext;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Interfaces;
using CompanyX.API.DataAccess.Repositories;
using CompanyX.API.DataAccess.Validators;
using CompanyX.API.Services;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<CompanyXDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")), ServiceLifetime.Scoped);
builder.Services.AddScoped<IDataSeedingService, DataSeedingService>();
builder.Services.AddScoped<IDataSeedingRepository, DataSeedingRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IValidator<Employee>, EmployeeValidator>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddAutoMapper(typeof(CustomerMappingProfile).Assembly);
builder.Services.AddSingleton(new SemaphoreSlim(5));
builder.Services.AddSingleton<IConcurrencyService, ConcurrencyService>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Company X Management System",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataSeedingService>();
        await dataInitializer.SeedInitialDataAsync();
    }
    catch (SqlException ex)
    {
        Console.WriteLine("SQL server error occurred: " + ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Server error occurred: " + ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompanyX.API");
        c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
