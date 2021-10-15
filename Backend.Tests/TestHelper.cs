using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PMMC.Configs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using PMMC.Helpers;
using Dapper;
using System.Linq;

namespace PMMC.UnitTests
{
    /// <summary>
    /// The test helper
    /// </summary>
    internal static class TestHelper
    {
        /// <summary>
        /// The test environment
        /// </summary>
        internal const string TestEnvironment = "Test";

        /// <summary>
        /// Represents the JSON serializer settings.
        /// </summary>
        internal static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "MM/dd/yyyy HH:mm:ss",
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Formatting.Indented
        };

        /// <summary>
        /// Get environment used in test 
        /// and will pass CI in ASPNETCORE_ENVIRONMENT environment for gitlab CI
        /// and default is using Test environment
        /// </summary>
        /// <returns></returns>
        internal static string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? TestEnvironment;
        }

        /// <summary>
        /// Get configuration using environment related file `appsettings.{env}.json`
        /// </summary>
        /// <returns>the configurations match environment</returns>
        private static IConfiguration GetConfiguration()
        {
            var env = GetEnvironment();
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{env}.json", true, false);
            return builder.Build();
        }

        /// <summary>
        /// Get app settings
        /// </summary>
        /// <returns>the app settings</returns>
        internal static AppSettings GetAppSettings()
        {
            var appSettingsSection = GetConfiguration().GetSection("AppSettings");
            return appSettingsSection.Get<AppSettings>();
        }

        /// <summary>
        /// Get test settings
        /// </summary>
        /// <returns>the test settings</returns>
        internal static TestSettings GetTestSettings()
        {
            var testSettingsSection = GetConfiguration().GetSection("TestSettings");
            return testSettingsSection.Get<TestSettings>();
        }

        /// <summary>
        /// Get connection string and if pass exist database=false will generate connection string without database
        /// </summary>
        /// <param name="existDatabase">the exist database flag</param>
        /// <returns>connection string with server/database name replaced and will not exist database information if pass exist database=false</returns>
        private static string GetConnectionString(bool existDatabase = true)
        {
            var appSettings = GetAppSettings();
            var testSettings = GetTestSettings();
            var connectionString = Helper.GetConnectionString(appSettings, testSettings.ServerName, testSettings.DbName);
            if (existDatabase)
            {
                return connectionString;
            }

            var builder = new SqlConnectionStringBuilder(GetConnectionString());
            builder.Remove("InitialCatalog");
            builder.Remove("Database");
            return builder.ConnectionString;
        }


        /// <summary>
        /// Gets <see cref="IDbConnection"/> instance to access the persistence.
        /// </summary>
        ///<param name="existDatabase">the exist database flag</param>
        /// <returns>The created connection.</returns>
        internal static IDbConnection GetConnection(bool existDatabase = true)
        {
            IDbConnection connection = null;
            try
            {
                // Create the connection
                connection = new SqlConnection(GetConnectionString(existDatabase));

                // Open the connection
                connection.Open();

                // Return the opened connection
                return connection;
            }
            catch
            {
                // Be sure to dispose of connection object if it isn't null
                if (connection != null)
                {
                    connection.Dispose();
                }

                throw;
            }
        }


        /// <summary>
        /// Executes the given SQL statement.
        /// </summary>
        ///
        /// <param name="sql">The SQL statement to execute.</param>
        internal static void Execute(string sql)
        {
            using (var conn = GetConnection())
            {
                conn.Query(sql);
            }
        }

        /// <summary>
        /// Executes the given SQL statement.
        /// </summary>
        ///
        /// <param name="sql">The SQL statement to execute.</param>
        /// <param name="param">The SQL param to execute.</param>
        internal static IEnumerable<T> Query<T>(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql,param);
            }
        }


        /// <summary>
        /// Create test tables and adds test data to the database.
        /// </summary>
        internal static void PrepareDatabase()
        {
            using (var conn = GetConnection(false))
            {
                var testSettings = GetTestSettings();
                var checkDB =
                    conn.Query<string>(
                        $"SELECT name FROM master.dbo.sysdatabases WHERE name = '{testSettings.DbName}'");
                // only create test database if not exist
                if (!checkDB.Any())
                {
                    conn.Execute($"CREATE DATABASE [{testSettings.DbName}]");
                }
            }
            Execute(File.ReadAllText(Path.Combine("..", "..", "..", "TestFiles", "TestData.sql")));
        }


        /// <summary>
        /// Create web host builder
        /// </summary>
        /// <param name="env">the environment</param>
        /// <returns>the web host builder with environment from input</returns>
        internal static IWebHostBuilder GetWebHostBuilder(string env)
        {
            return new WebHostBuilder()
                .UseEnvironment(env)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, false);
                })
                .UseStartup<Startup>();
        }
    }
}