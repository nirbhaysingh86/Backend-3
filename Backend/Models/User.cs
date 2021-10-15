namespace PMMC.Models
{
    /// <summary>
    /// The user
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The user name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user password
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// The user role
        /// </summary>
        public string Role { get; set; }
    }
}