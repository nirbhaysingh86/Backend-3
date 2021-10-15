using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMMC.Attributes;
using PMMC.Entities;
using PMMC.Helpers;
using PMMC.Interfaces;

namespace PMMC.Controllers
{
    /// <summary>
    /// The auth controller
    /// </summary>
    [ApiController]
    [DefaultRouting]
    public class AuthController
    {
        /// <summary>
        /// The logger used in controller
        /// </summary>
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// The user service used in controller
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor with logger and user service
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="userService">the user service</param>
        public AuthController(ILogger<AuthController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Login with auth request
        /// response 400 if error to connect to database
        /// response 401 if not found valid user
        /// response 403 if invalid role
        /// </summary>
        /// <param name="authRequest">the auth request</param>
        /// <returns>auth response with jwt token</returns>
        [HttpPost]
        [Route("[action]")]
        public AuthResponse Login([FromBody] AuthRequest authRequest)
        {
            return _logger.Process(() => _userService.Login(authRequest), "login by given credentials",
                parameters: authRequest);
        }
    }
}