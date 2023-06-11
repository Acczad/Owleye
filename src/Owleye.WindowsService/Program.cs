using EasyCaching.Core.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Owleye.Application;
using Owleye.Application.Dto.Messages;
using Owleye.Application.Handlers;
using Owleye.Application.Monitoring.NotifyToUser;
using Owleye.Application.Sensors.Queries.GetSensorsList;
using Owleye.Infrastructure.Cache;
using Owleye.Infrastructure.Data;
using Owleye.Infrastructure.MappingConfiguration;
using Owleye.Infrastructure.MicrosoftTeams;
using Owleye.Infrastructure.Quartz;
using Owleye.Infrastructure.Service;
using Owleye.Shared.Base;
using Owleye.Shared.Cache;
using Owleye.Shared.Data;
using Owleye.Shared.MicrosoftTeams;
using System;

namespace Owleye.WindowsService
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Owleye Window Service!");
            Console.WriteLine("Install Service using this Command:");
            Console.WriteLine("SC CREATE \"OwleyeSerive\" binpath= \"Owleye.WindowsService.exe\"");
            Console.WriteLine();
            var host = CreateHostBuilder(args).Build();
            ServiceLocator.Init(host.Services);
            QuartzBootStrap.Boot();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configs = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<OwleyeDbContext>(options =>
                   options.UseSqlServer(configs.GetConnectionString(nameof(OwleyeDbContext))), ServiceLifetime.Transient);

                    services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                    services.AddTransient<INotifyDispatcherService, NotifyDispatcherService>();
                    services.AddTransient<IRedisCache, RedisCache>();
                    services.AddTransient<IQrtzSchedule, QrtzSchedule>();
                    services.AddTransient<IMicrosoftTeamsService, MicrosoftTeamsService>();
                    services.AddMediatR(typeof(DoPingNotificationHandler).Assembly);
                    services.AddMediatR(typeof(GetSensorsListQueryHandler).Assembly);

                    services.AddEasyCaching(options =>
                    {
                        options.UseRedis(config =>
                        {
                            config.DBConfig.Endpoints.Add(new
                                ServerEndPoint(configs["General:RedisAddress"],
                                int.Parse(configs["General:RedisPort"])));
                        }, configs["General:RedisInstanceName"]) //redis provider name
                        .WithMessagePack()
                        .UseRedisLock(); //with distributed lock, prevent problem in aysnc.
                    });

                    services.AddScoped<IAppSession, AppSession>();
                   
                    services.AddAutoMapper(typeof(SensorAutoMapProfile));
                    services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

                }).UseWindowsService();
        }
    }
}
