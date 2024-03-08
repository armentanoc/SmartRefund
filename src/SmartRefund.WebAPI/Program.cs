
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using SmartRefund.Infra.Repositories;
using SmartRefund.WebAPI.Middlewares;

namespace SmartRefund.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var apiName = "SmartRefund Web API";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Add Logging
            builder.Services.AddLogging();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1" });
                c.EnableAnnotations();
            });

            builder.Services.AddScoped<IFileValidatorService, FileValidatorService>();
            builder.Services.AddScoped<IRepositoryTeste, RepositoryTeste>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("SmartRefundDbContext"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Custom Logging Middleware
            app.UseMiddleware<LoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
