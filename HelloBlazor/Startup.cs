using BlazorBasics.Areas.Identity;
using HelloBlazor.Data;
using HelloBlazor.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;

using Microsoft.AspNetCore.Authentication;

namespace HelloBlazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<AppDbContext>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddSingleton<WeatherForecastService>();
            
            services.AddScoped(s =>
            {
                var client = new HttpClient { BaseAddress = new Uri("https://localhost:44352/") };
                return client;
            });

            services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
            services.AddScoped<IWeatherForecastService1, WeatherForecastService1>();

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader());
            });
            services.AddControllers()
              .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
              });

            services.Configure<AzureFileLoggerOptions>(Configuration.GetSection("AzureLogging"));

            //services.AddCors(policy =>
            //{
            //    policy.AddPolicy("CorsPolicy", opt => opt
            //        .WithOrigins("https://localhost:5001")
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials());
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            loggerFactory.AddFile("Logs/mylog-{Date}.txt");
        }
    }
}
