using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi;
using WebApi.core.IConfigurations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy",
                      build =>
                      {
                          build
                            .WithOrigins("http://localhost:3000") 
                            .AllowAnyMethod()
                            .AllowAnyHeader(); 
                      });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding the unit of work to the DI container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<EmployeeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//middlewares
app.UseHttpsRedirection();

app.UseCors("corspolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
