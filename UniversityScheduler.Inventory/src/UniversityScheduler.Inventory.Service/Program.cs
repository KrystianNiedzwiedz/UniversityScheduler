using MassTransit;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Polly;
using Polly.Timeout;
using UniversityScheduler.Common.MassTransit;
using UniversityScheduler.Common.MongoDB;
using UniversityScheduler.Inventory.Service.Clients;
using UniversityScheduler.Inventory.Service.Entities;

namespace UniversityScheduler.Inventory.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            const string AllowedOriginSetting = "AllowedOrigin";
            // Add services to the container.

            builder.Services.AddMongo()
                .AddMongoRepository<InventoryCourse>("inventorycourses")
                .AddMongoRepository<CatalogCourse>("catalogcourses")
                .AddMassTransitWithRabbitMq();

            AddCatalogClient(builder);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors(policy =>
                {
                    policy.WithOrigins(builder.Configuration[AllowedOriginSetting])
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void AddCatalogClient(WebApplicationBuilder builder)
        {
            Random jitter = new Random();

            builder.Services.AddHttpClient<CatalogClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5090");
            });
            // .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
            //     5,
            //     retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            //                               + TimeSpan.FromMilliseconds(jitter.Next(0,1000)),
            //     onRetry: (outcome, timespan, retryAttempt) =>
            //     {
            //         var serviceProvider = builder.Services.BuildServiceProvider();
            //         serviceProvider.GetService<ILogger<CatalogClient>>()?
            //             .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}");
            //     }
            // ))
            // .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
            //     3,
            //     TimeSpan.FromSeconds(15),
            //     onBreak: (outcome, timespan) =>
            //     {
            //         var serviceProvider = builder.Services.BuildServiceProvider();
            //         serviceProvider.GetService<ILogger<CatalogClient>>()?
            //         .LogWarning($"Openinig the circuit for {timespan.TotalSeconds} seconds...");
            //     },
            //     onReset: () =>
            //     {
            //         var serviceProvider = builder.Services.BuildServiceProvider();
            //         serviceProvider.GetService<ILogger<CatalogClient>>()?
            //         .LogWarning($"Closing the circuit...");
            //     }
            // ))
            // .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
        }
    }
}
