using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartRefund.Application.Interfaces;
using SmartRefund.Application.Services;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
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

            //Controllers
            builder.Services.AddControllers(options =>
            {
                //Custom Exception Filter
                options.Filters.Add<ExceptionFilter>();
            }
            );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1" });
                c.EnableAnnotations();
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("SmartRefundSqlite"));
            });

            builder.Services.AddScoped<IFileValidatorService, FileValidatorService>();
            builder.Services.AddScoped<IInternalAnalyzerService, InternalAnalyzerService>();
            builder.Services.AddScoped<IRepositoryTeste, RepositoryTeste>();
            builder.Services.AddScoped<ITranslatedVisionReceiptRepository, TranslatedVisionReceiptRepository>();
            builder.Services.AddScoped<IInternalAnalyzerService, InternalAnalyzerService>();
            builder.Services.AddScoped<IRawVisionReceiptRepository, RawVisionReceiptRepository>();

            var app = builder.Build();
           /* 
            * Teste Manual limpar posteriormente
            * using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();


                SeedInitialData(dbContext);
            }*/
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

       /*
        * Adicionar dados iniciais para teste manual limpar posteriormente
        *private static void SeedInitialData(AppDbContext dbContext)
        {
            // Check if there is any existing data
            if (!dbContext.TranslatedVisionReceipt.Any())
            {
                // Seed initial TranslatedVisionReceipt data
                var receipt1 = new TranslatedVisionReceipt(new RawVisionReceipt(), true, TranslatedVisionReceiptCategoryEnum.ALIMENTACAO, TranslatedVisionReceiptStatusEnum.SUBMETIDO, 100, "Receipt description 1");
                var receipt2 = new TranslatedVisionReceipt(new RawVisionReceipt(), true, TranslatedVisionReceiptCategoryEnum.HOSPEDAGEM, TranslatedVisionReceiptStatusEnum.PAGA, 200, "Receipt description 2");
                var receipt3 = new TranslatedVisionReceipt(new RawVisionReceipt(), true, TranslatedVisionReceiptCategoryEnum.TRANSPORTE, TranslatedVisionReceiptStatusEnum.SUBMETIDO, 150, "Receipt description 3");

                dbContext.TranslatedVisionReceipt.AddRange(receipt1, receipt2, receipt3);
                dbContext.SaveChanges();
            }
        */

    }
}
