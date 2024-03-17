using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.Infra.Context;
using SmartRefund.Infra.Interfaces;
using SmartRefund.Infra.Repositories;
using SmartRefund.WebAPI.Middlewares;
using System.Text;

namespace SmartRefund.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var apiName = "SmartRefund Web API";
            var builder = WebApplication.CreateBuilder(args);

            // Add Logging
            builder.Services.AddLogging();
            builder.Services.AddMemoryCache();

            // Controllers
            builder.Services.AddControllers(options =>
            {
                // Custom Exception Filter
                options.Filters.Add<ExceptionFilter>();
            }
            );

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "ABCXYZ",
                    ValidAudience = "http://localhost:5029",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecretkey@12345678901234567890"))
                };
            });

            // Remove os provedores de log padr?o**
            builder.Logging.ClearProviders();

            // Adiciona os log no console**
            builder.Logging.AddConsole();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1" });
                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("SmartRefundSqlite"));
            });

            // Add OpenAIKey EnvVar
            builder.Configuration.AddEnvironmentVariables(
                builder.Configuration.GetSection("OpenAIVisionConfig:EnvVariable").Value
                );

            // Services
            builder.Services.AddScoped<IFileValidatorService, FileValidatorService>();
            builder.Services.AddScoped<IVisionExecutorServiceConfiguration, VisionExecutorServiceConfiguration>();
            builder.Services.AddScoped<IVisionExecutorService, VisionExecutorService>();
            builder.Services.AddScoped<IVisionTranslatorService, VisionTranslatorService>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<IInternalAnalyzerService, InternalAnalyzerService>();


            // Repositories
            builder.Services.AddScoped<ITranslatedVisionReceiptRepository, TranslatedVisionReceiptRepository>();
            builder.Services.AddScoped<IRawVisionReceiptRepository, RawVisionReceiptRepository>();
            builder.Services.AddScoped<IInternalReceiptRepository, InternalReceiptRepository>();
            // Add CacheService



            //builder.Services.AddHostedService<VisionProcessingWorker>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Custom Logging Middleware
            app.UseMiddleware<LoggingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}