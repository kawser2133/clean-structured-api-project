using Microsoft.EntityFrameworkCore;
using Project.API.Middlewares;
using Project.Infrastructure.Data;
using Project.API.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BenchmarkDotNet.Running;
using Project.API.Controllers;
using BenchmarkDotNet.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration
                .GetConnectionString("PrimaryDbConnection")));

// Register ILogger service
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
});

//Register Services
builder.Services.RegisterService();
builder.Services.AddControllers();

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

app.UseRouting(); // Add this line to configure routing

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Map your regular API controllers

    // Add a custom endpoint for triggering benchmarks
    endpoints.MapGet("/runbenchmarks", async context =>
    {
        // You can run the benchmarks here
        var summary = BenchmarkRunner.Run<OrderControllerBenchmark>();

        await context.Response.WriteAsync("Benchmarks completed. Check console for results.");
    });
});

#region Custom Middleware

app.UseRequestResponseLogging();
#endregion



app.Run();
