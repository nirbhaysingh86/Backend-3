using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PMMC
{
    /// <summary>
    /// The main program
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        /// <summary>
        /// The main method 
        /// </summary>
        /// <param name="args">the method arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create host builder
        /// </summary>
        /// <param name="args">the method arguments</param>
        /// <returns>the custom host builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).ConfigureLogging(
                    (builder) =>
                    {
                        builder.AddApplicationInsights();
                    })
                    .ConfigureAppConfiguration(configurationBuilder =>
                    {
                        configurationBuilder.Sources.Remove(
                        configurationBuilder.Sources.First(source =>
                            source.GetType() == typeof(EnvironmentVariablesConfigurationSource))); //remove the default one first
                        configurationBuilder.AddEnvironmentVariables();
                    });
    }
}