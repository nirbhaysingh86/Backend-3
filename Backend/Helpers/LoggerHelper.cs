using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PMMC.Entities;
using PMMC.Exceptions;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace PMMC.Helpers
{
    /// <summary>
    /// The logger helper used in application
    /// </summary>
    internal static class LoggerHelper
    {
        /// <summary>
        /// Represents the password mask.
        /// </summary>
        internal const string PasswordMask = "***";

        /// <summary>
        /// Represents the JSON serializer settings.
        /// </summary>
        internal static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "MM/dd/yyyy HH:mm:ss",
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        /// <summary>
        /// Gets JSON description of the object.
        /// </summary>
        ///
        /// <param name="obj">The object to describe.</param>
        /// <returns>The JSON description of the object.</returns>
        internal static string GetObjectDescription(object obj)
        {
            try
            {
                var request = obj as AuthRequest;
                if (request != null)
                {
                    obj = new AuthRequest
                    {
                        Username = request.Username,
                        Password = PasswordMask,
                        Server = request.Server,
                        DbName = request.DbName
                    };
                }

                return JsonConvert.SerializeObject(obj, SerializerSettings);
            }
            catch
            {
                return "[Can't express this value]";
            }
        }

        /// <summary>
        /// Logs method entrance and input parameters with DEBUG level.
        /// </summary>
        /// <remarks>The internal exception may be thrown directly.</remarks>
        /// <param name="logger">The logger.</param>
        /// <param name="method">The method where the logging occurs.</param>
        /// <param name="parameters">The parameters used to format the message.</param>
        internal static void LogMethodEntry(ILogger logger, MethodBase method, params object[] parameters)
        {
            // Create a string format to display parameters
            var logFormat = new StringBuilder();
            var pis = method.GetParameters();
            string methodName = $"{method.DeclaringType.Name}.{method.Name}";
            if (pis.Length != parameters.Length)
            {
                throw new ArgumentException(
                    $"The number of provided parameters for method '{methodName}' is wrong.", nameof(parameters));
            }

            logFormat.AppendFormat("Entering method {0}", methodName).AppendLine();
            logFormat.AppendLine("Argument Values:");
            for (int i = 0; i < pis.Length; i++)
            {
                logFormat.Append("\t").Append(pis[i].Name).Append(": ");
                logFormat.AppendLine(GetObjectDescription(parameters[i]));
            }

            // log method entry and input parameters
            logger.LogDebug(logFormat.ToString());
        }

        /// <summary>
        /// Logs method exit with DEBUG level.
        /// </summary>
        ///
        /// <param name="logger">The logger.</param>
        /// <param name="methodName">The full method name.</param>
        internal static void LogMethodExit(ILogger logger, string methodName)
        {
            logger.LogDebug("[Exiting method {0}]", methodName);
        }

        /// <summary>
        /// Logs method exit and output parameter with DEBUG level.
        /// </summary>
        ///
        /// <param name="logger">The logger.</param>
        /// <param name="methodName">The full method name.</param>
        /// <param name="returnValue">The method return value.</param>
        internal static void LogMethodExit(ILogger logger, string methodName, object returnValue)
        {
            LogMethodExit(logger, methodName);

            // log return value
            logger.LogDebug("[Output parameter: {0}]", GetObjectDescription(returnValue));
        }

        /// <summary>
        /// Logs the given exception with ERROR level.
        /// </summary>
        ///
        /// <param name="exception">The exception to log.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="methodName">The full method name.</param>
        internal static void LogException(ILogger logger, string methodName, Exception exception)
        {
            // log exception
            logger.LogError(string.Format("Error in method {0}.{1}Details:{1}{2}",
                methodName, Environment.NewLine, exception));
        }

        /// <summary>
        /// Processes the specified action and wraps it with common error handling logic.
        /// </summary>
        /// <remarks>
        /// If any exception is thrown, the <see cref="ArgumentException"/>, <see cref="AppException"/> exceptions will be simply re-thrown.
        /// All other exceptions will be wrapped in <see cref="AppException"/> and thrown.
        /// </remarks>
        /// <param name="action">The action to process.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="methodDescription">The short description of what the source method does.</param>
        /// <param name="methodName">The full method name.</param>
        internal static void Process(Action action, ILogger logger, string methodDescription, string methodName)
        {
            Exception thrownException = null;
            try
            {
                action();
            }
            catch (ArgumentException ex)
            {
                thrownException = ex;
                throw;
            }
            catch (AppException ex)
            {
                thrownException = ex;
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error occurred while {methodDescription}.";
                thrownException = new AppException(errorMessage, ex);
                throw thrownException;
            }
            finally
            {
                if (thrownException != null)
                {
                    LogException(logger, methodName, thrownException);
                }
            }
        }


        /// <summary>
        /// Processes the specified action and wraps it with common logging and error handling logic.
        /// </summary>
        /// <remarks>
        /// If any exception is thrown, the <see cref="ArgumentException"/>, <see cref="AppException"/> exceptions will be simply re-thrown.
        /// All other exceptions will be wrapped in <see cref="AppException"/> and thrown.
        /// </remarks>
        /// <param name="logger">The logger.</param>
        /// <param name="action">The action to process.</param>
        /// <param name="methodDescription">The short description of what the source method does.</param>
        /// <param name="callingMethod">The source method information.</param>
        /// <param name="parameters">The parameters for the source method.</param>
        internal static void Process(this ILogger logger, Action action, string methodDescription,
            MethodBase callingMethod = null, params object[] parameters)
        {
            callingMethod = callingMethod ?? new StackTrace().GetFrame(1).GetMethod();
            string methodName = $"{callingMethod.DeclaringType.Name}.{callingMethod.Name}";

            Process(() =>
                {
                    LogMethodEntry(logger, callingMethod, parameters);
                    action();
                    LogMethodExit(logger, methodName);
                },
                logger,
                methodDescription,
                methodName);
        }

        /// <summary>
        /// Processes the specified function and wraps it with common logging and error handling logic.
        /// </summary>
        /// <remarks>
        /// If any exception is thrown, the <see cref="ArgumentException"/>, <see cref="AppException"/> exceptions will be simply re-thrown.
        /// All other exceptions will be wrapped in <see cref="AppException"/> and thrown.
        /// </remarks>
        /// <typeparam name="T">The type of the function return value.</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="function">The function to process.</param>
        /// <param name="methodDescription">The short description of what the source method does.</param>
        /// <param name="callingMethod">The source method information.</param>
        /// <param name="parameters">The parameters for the source method.</param>
        /// <returns>The function result.</returns>
        internal static T Process<T>(this ILogger logger, Func<T> function, string methodDescription,
            MethodBase callingMethod = null, params object[] parameters)
        {
            callingMethod = callingMethod ?? new StackTrace().GetFrame(1).GetMethod();
            string methodName = $"{callingMethod.DeclaringType.Name}.{callingMethod.Name}";
            T result = default(T);
            Process(() =>
                {
                    LogMethodEntry(logger, callingMethod, parameters);
                    result = function();
                    LogMethodExit(logger, methodName, result);
                },
                logger,
                methodDescription,
                methodName);

            return result;
        }
    }
}