using PMMC.Entities;

namespace PMMC.Interfaces
{
    /// <summary>
    /// The user service interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Login with auth request
        /// response 400 if error to connect to database
        /// response 401 if not found valid user
        /// response 403 if invalid role
        /// </summary>
        /// <param name="authRequest">the auth request</param>
        /// <returns>auth response with jwt token</returns>
        AuthResponse Login(AuthRequest authRequest);
    }
}