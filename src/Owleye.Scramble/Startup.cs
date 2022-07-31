using EasyCaching.Core.Configurations;
using LiteX.Email;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Owleye.Application.Handlers;
using Owleye.Application.Sensors.Queries.GetSensorsList;
using Owleye.Infrastructure.Cache;
using Owleye.Infrastructure.Data;
using Owleye.Infrastructure.MappingConfiguration;
using Owleye.Shared.Base;
using Owleye.Shared.Cache;
using Owleye.Shared.Data;

namespace Owleye.Scramble
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddMvcOptions(option => option.EnableEndpointRouting = false);

            services.AddTransient<IRedisCache, RedisCache>();
            services.AddMediatR(typeof(DoPingNotificationHandler).Assembly);
            services.AddMediatR(typeof(GetSensorsListQueryHandler).Assembly);
            services.AddOptions();
            services.AddEasyCaching(options =>
            {
                options.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new
                        ServerEndPoint(Configuration["General:RedisAddress"],
                        int.Parse(Configuration["General:RedisPort"])));
                }, Configuration["General:RedisInstanceName"]) //redis provider name
                .WithMessagePack()
                .UseRedisLock(); //with distributed lock, prevent problem in aysnc.
            });
            services.AddLiteXSmtpEmail();
            services.AddScoped<IAppSession, AppSession>();
            services.AddDbContext<OwleyeDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString(nameof(OwleyeDbContext))), ServiceLifetime.Transient);
            services.AddAutoMapper(typeof(SensorAutoMapProfile));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

          
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
