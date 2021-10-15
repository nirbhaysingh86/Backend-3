using Microsoft.Data.SqlClient;
using PMMC.Configs;
using System;

namespace PMMC.Helpers
{
    /// <summary>
    /// The helper class used in this application
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Represents the context property name for current user.
        /// </summary>
        internal const string UserPropertyName = "JwtUser";

        /// <summary>
        /// Validates that <paramref name="param"/> is not <c>null</c>.
        /// </summary>
        ///
        /// <typeparam name="T">The type of the parameter, must be reference type.</typeparam>
        ///
        /// <param name="param">The parameter to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        ///
        /// <exception cref="ArgumentNullException">If <paramref name="param"/> is <c>null</c>.</exception>
        internal static void ValidateArgumentNotNull<T>(T param, string paramName)
            where T : class
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
            }
        }

        /// <summary>
        /// Validates that <paramref name="param"/> is not <c>null</c> or empty.
        /// </summary>
        ///
        /// <param name="param">The parameter to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        ///
        /// <exception cref="ArgumentNullException">If <paramref name="param"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="param"/> is empty.</exception>
        internal static void ValidateArgumentNotNullOrEmpty(string param, string paramName)
        {
            ValidateArgumentNotNull(param, paramName);
            if (string.IsNullOrWhiteSpace(param))
            {
                throw new ArgumentException($"{paramName} cannot be empty.", paramName);
            }
        }

        /// <summary>
        /// Validates that <paramref name="param"/> is positive number.
        /// </summary>
        ///
        /// <param name="param">The parameter to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        ///
        /// <exception cref="ArgumentException">If <paramref name="param"/> is not positive number.</exception>
        internal static void ValidateArgumentPositive(long param, string paramName)
        {
            if (param <= 0)
            {
                throw new ArgumentException($"{paramName} should be positive.", paramName);
            }
        }

        /// <summary>
        /// Get connection string with custom database server name/database name
        /// </summary>
        /// <param name="appSettings">the app settings</param>
        /// <param name="server">the server name</param>
        /// <param name="dbName">the database name</param>
        /// <returns>The connection after replaced with server name/databae name</returns>
        internal static string GetConnectionString(AppSettings appSettings, string server, string dbName)
        {
            var connStringTemplate = appSettings.ConnectionString;
            var connString = connStringTemplate.Replace("{server}", server).Replace("{dbName}", dbName);
            var builder = new SqlConnectionStringBuilder(connString);
            var dbUser = Environment.GetEnvironmentVariable("AZURE_DATABASE_USERID");
            var dbPassword = Environment.GetEnvironmentVariable("AZURE_DATABASE_PASSWORD");
            if (!string.IsNullOrWhiteSpace(dbUser) && !string.IsNullOrWhiteSpace(dbPassword))
            {
                builder.UserID = dbUser;
                builder.Password = dbPassword;
            }
            return builder.ConnectionString;
        }
    }
}