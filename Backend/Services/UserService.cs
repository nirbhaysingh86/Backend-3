using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Exceptions;
using PMMC.Helpers;
using PMMC.Interfaces;
using PMMC.Models;
using System.Linq;

namespace PMMC.Services
{
    /// <summary>
    /// The user service.
    /// </summary>
    public class UserService :BaseService,IUserService
    {
        /// <summary>
        /// The login sql and will find top one match user with username/password
        /// </summary>
        internal const string LoginSql = "SELECT TOP(1) [UserID],[UserName], [Role] FROM [dbo].[tblUsers] WHERE [UserName]=@Username AND [UserPassword]=@Password";
        
        /// <summary>
        /// Constructor with logger and app settings
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="appSettings">the app settings</param>
        public UserService(ILogger<UserService> logger, IOptions<AppSettings> appSettings) : base(logger,  appSettings)
        {
        }
        
        /// <summary>
        /// Login with auth request
        /// response 400 if error to connect to database
        /// response 401 if not found valid user
        /// response 403 if invalid role
        /// </summary>
        /// <param name="authRequest">the auth request</param>
        /// <returns>auth response with jwt token</returns>
        public AuthResponse Login(AuthRequest authRequest)
        {
            return  _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(authRequest, nameof(authRequest));
                return ProcessWithDb((conn) =>
                {
                    var users = conn.Query<User>(LoginSql, new
                    {
                        authRequest.Username,
                        authRequest.Password
                    });
                    if (!users.Any())
                    {
                        throw new UnauthorizedException($"User with name `{authRequest.Username}` not found with given password");
                    }
                    var user = users.First();
                    if (string.IsNullOrWhiteSpace(user.Role) || !_appSettings.Roles.Contains(user.Role))
                    {
                        throw new ForbiddenException($"User with role `{user.Role}` don't have permission to access");
                    }
                    return new AuthResponse { Token = JwtHelper.GenerateJwtToken(_appSettings.Jwt, user, authRequest) };

                }, authRequest);
            }, "login by given credentials",
            parameters: authRequest);

        }
    }
}
