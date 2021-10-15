using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Helpers;
using PMMC.Interfaces;
using PMMC.Middlewares;
using PMMC.Services;
using System.Configuration;
using System.Linq;

namespace PMMC
{
    /// <summary>
    /// The start up of this application
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor with configuration
        /// </summary>
        /// <param name="configuration">the configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// The configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services">the services collection</param>
        /// <exception cref="ConfigurationErrorsException">throws if app settings is invalid</exception>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddHttpContextAccessor();
            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry(Configuration);

            // throw custom error message for model validation errors
            services.AddMvc().ConfigureApiBehaviorOptions(opt => opt.InvalidModelStateResponseFactory =
                (context => new BadRequestObjectResult(new ApiErrorModel
                {
                    Message = string.Join("", context.ModelState
                        .Where(modelError => modelError.Value.Errors.Count > 0)
                        .Select(modelError =>
                            string.Join("", modelError.Value.Errors.Select(error => error.ErrorMessage))))
                })));

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IViewService, ViewService>();
            services.AddScoped<IWorklistLayoutService, WorklistLayoutService>();
            services.AddScoped<IWorklistAccountService, WorklistAccountService>();

            // ensure app settings is valid
            services.PostConfigure<AppSettings>(settings =>
            {
                var configErrors = settings.ValidationErrors().ToArray();
                if (configErrors.Any())
                {
                    throw new ConfigurationErrorsException(
                        $"Found {configErrors.Length} configuration error(s) in {nameof(AppSettings)}: {string.Join(",", configErrors)}");
                }
            });
        }

        /// <summary>
        /// Configure application with web host environment 
        /// </summary>
        /// <param name="app">the application builder</param>
        /// <param name="env">the web host environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();
            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}