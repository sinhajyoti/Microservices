using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CoreMicroService.Models;
using CoreMicroService.Cache;
using Swashbuckle.Swagger.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace CoreMicroService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                
            if (env.IsEnvironment("Development"))
            {
                
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddOptions();
            services.AddMvc();
            

            services.AddLogging();
            services.AddCors(options =>
            {
                options.AddPolicy("MySpecificOriginPolicy",
                    builder => builder.WithOrigins("http://localhost:63238"));
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("MySpecificOriginPolicy"));
            });


            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddSingleton<IApiRepository, ApiRepository>();
            //services.AddTransient<IApiRepository, ApiRepository>();
            services.AddTransient<ICacheClient, CacheClient>();
            var testSettings = Configuration.GetSection("TestSettings");
            services.Configure<TestSetting>(testSettings);

            var pathToDoc = Configuration["Swagger:Path"];


            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "My CORE API",
                    Description = "A simple api to create Items",
                    TermsOfService = "None"
                });
                //options.IncludeXmlComments(pathToDoc);
                options.DescribeAllEnumsAsStrings();
            });

            ///adding distributed cache
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "CoreMicroServiceRedisInstance";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDistributedCache cache)
        {
            app.UseStaticFiles();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors("MySpecificOriginPolicy");
            //app.UseCors();
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            app.UseStatusCodePages();//enable using status code 404, 500 etc. pages
            if (env.EnvironmentName == "Development")
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    //app.UseStatusCodePagesWithRedirects("/Error/{0}");
            //    app.UseExceptionHandler("/Error");
            //}
            //app.UseMvc();
            app.UseMvcWithDefaultRoute();

            app.UseStartTimeHeader();


            //var options = new RewriteOptions()
            //    .AddRedirectToHttpsPermanent();
            //app.UseRewriter(options);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUi();

        }

        /// <summary>
        /// This is an override method that will come in play instead of Configure(...) method, provided ASPNETCORE_Environment is set to Development(Remember method name should be in same case as environment)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configuredevelopment(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            if (env.EnvironmentName == "Development")
            {
                app.UseBrowserLink();
            }
            app.UseMvc();
        }
    }
}
