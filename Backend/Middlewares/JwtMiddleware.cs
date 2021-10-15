using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PMMC.Configs;
using PMMC.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using PMMC.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace PMMC.Middlewares
{
    /// <summary>
    /// The jwt auth middleware
    /// </summary>
    public class JwtMiddleware
    {
        /// <summary>
        /// The request delegate
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The app settings
        /// </summary>
        private readonly AppSettings _appSettings;

        /// <summary>
        /// The app settings
        /// </summary>
        private readonly ILogger<JwtMiddleware> _logger;

        /// <summary>
        /// Constructor with request delegate and app settings
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="next">The request delegate</param>
        /// <param name="appSettings">the app settings</param>
        public JwtMiddleware(ILogger<JwtMiddleware>logger,  RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Invoke function and attach jwt user if found valid jwt token in context
        /// </summary>
        /// <param name="context"></param>
        public async Task Invoke(HttpContext context)
        {
            var tokenSplits = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");
            if (tokenSplits != null && string.Equals(tokenSplits.First(), "bearer", StringComparison.OrdinalIgnoreCase))
            {
                AttachUserToContext(context, tokenSplits.Last());
            }

            await _next(context);
        }

        /// <summary>
        /// Attachment user to context 
        /// </summary>
        /// <param name="context">the context</param>
        /// <param name="token">the token to check</param>
        /// <exception cref="ForbiddenException">throws if user role is invalid</exception>
        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var user = JwtHelper.ParseJwtToken(token, _appSettings.Jwt);
                var userRole = user.Role;
                if (string.IsNullOrEmpty(userRole) || !_appSettings.Roles.Contains(userRole))
                {
                    throw new ForbiddenException($"User with role `{userRole}` don't have permission to access");
                }

                // attach user to context on successful jwt validation
                context.Items[Helper.UserPropertyName] = user;
            }
            catch (AppException error)
            {
                throw error;
            }
            catch(SecurityTokenExpiredException)
            {
                // process jwt token expired error
                throw new UnauthorizedException("Jwt token expired");
            }
            catch (Exception e)
            {
                // user is not attached to context so request won't have access to secure routes
                _logger.LogError(e.Message);
            }
        }
    }
}