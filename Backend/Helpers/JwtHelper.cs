using Microsoft.IdentityModel.Tokens;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace PMMC.Helpers
{
    /// <summary>
    /// The jwt helper class used in application
    /// </summary>
    internal static class JwtHelper
    {
        /// <summary>
        /// The user id claim type
        /// </summary>
        private const string UserID = "UserId";

        /// <summary>
        /// The username claim type
        /// </summary>
        private const string Username = "Username";

        /// <summary>
        /// The role claim type
        /// </summary>
        private const string Role = "Role";

        /// <summary>
        /// The database server claim type
        /// </summary>
        private const string DbServer = "DbServer";

        /// <summary>
        /// The database name claim type
        /// </summary>
        private const string Dbname = "DbName";

        /// <summary>
        /// Generate jwt token
        /// </summary>
        /// <param name="config">the jwt configuration</param>
        /// <param name="user">the user</param>
        /// <param name="authRequest">the auth request</param>
        /// <returns>jwt token</returns>
        internal static string GenerateJwtToken(JwtConfig config, User user, AuthRequest authRequest)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config.Issuer,
                audience: config.Audience,
                claims: new[]
                {
                    new Claim(UserID, user.UserID.ToString()),
                    new Claim(Username, user.Username),
                    new Claim(Role, user.Role),
                    new Claim(DbServer, authRequest.Server),
                    new Claim(Dbname, authRequest.DbName),
                },
                expires: DateTime.UtcNow.AddMinutes(config.ExpirationTimeInMinutes),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Parse jwt token
        /// </summary>
        /// <param name="token">the jwt token</param>
        /// <param name="config">the jwt configuration</param>
        /// <returns>the parsed jwt user</returns>
        internal static JwtUser ParseJwtToken(string token, JwtConfig config)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config.SecurityKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;

            return new JwtUser
            {
                UserId = int.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == UserID)?.Value),
                Username = jwtToken.Claims.FirstOrDefault(x => x.Type == Username)?.Value,
                Role = jwtToken.Claims.FirstOrDefault(x => x.Type == Role)?.Value,
                DbServer = jwtToken.Claims.FirstOrDefault(x => x.Type == DbServer)?.Value,
                DbName = jwtToken.Claims.FirstOrDefault(x => x.Type == Dbname)?.Value,
            };
        }
    }
}