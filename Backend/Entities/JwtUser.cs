namespace PMMC.Entities
{
    /// <summary>
    /// The jwt user
    /// </summary>
    public class JwtUser
    {
        /// <summary>
        /// The user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The user name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// The database server name
        /// </summary>
        public string DbServer { get; set; }

        /// <summary>
        /// The database name
        /// </summary>
        public string DbName { get; set; }
    }
}