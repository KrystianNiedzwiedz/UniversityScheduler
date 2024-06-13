using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using UniversityScheduler.Catalog.Service.Entities;
using UniversityScheduler.Common.MongoDB;
using UniversityScheduler.Common.Settings;
using MassTransit;
using RabbitMQ.Client;
using UniversityScheduler.Common.MassTransit;

namespace UniversityScheduler.Catalog.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            const string AllowedOriginSetting = "AllowedOrigin";
            var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            //builder.Services.AddMongo().AddMongoRepository<Course>("courses").AddMassTransitWithRabbitMq();
            // builder.Services.AddMassTransit(x => 
            // {
            //     x.UsingRabbitMq((context, configurator) =>
            //     {
            //         var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
            //         configurator.Host(rabbitMQSettings.Host);
            //         configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
            //     });
            // });

            //builder.Services.AddMassTransitHostedService();

            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
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
    }
}
