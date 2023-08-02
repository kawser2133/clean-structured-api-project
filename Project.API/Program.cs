using Microsoft.EntityFrameworkCore;
using Project.API.Middlewares;
using Project.Infrastructure.Data;
using Project.UI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration
                .GetConnectionString("PrimaryDbConnection")));

builder.Services.RegisterService();
builder.Services.AddControllers();

// Register ILogger service
builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Clean Structured API Project", Version = "v1" });
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
#region Custom Middleware

app.UseRequestResponseLogging();
#endregion

app.Run();
