using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PMMC.Entities;
using PMMC.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PMMC.Middlewares
{
    /// <summary>
    /// The global error handle middleware
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        /// <summary>
        /// Represents the JSON serializer settings.
        /// </summary>
        internal static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// The request delegate
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The constructor with request delegate
        /// </summary>
        /// <param name="next">The request delegate</param>
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke function and handle error
        /// and will response json with proper status code and error message
        /// </summary>
        /// <param name="context">the http context</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ArgumentException e:
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        break;
                    case AppException e:
                        response.StatusCode = e.StatusCode;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonConvert.SerializeObject(new ApiErrorModel  { Message = error?.Message}, SerializerSettings);
                await response.WriteAsync(result);
            }
        }
    }
}