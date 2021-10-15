using PMMC.Models;

namespace PMMC.UnitTests
{
    /// <summary>
    /// The test settings
    /// </summary>
    public class TestSettings
    {
        /// <summary>
        /// The database server name used in test
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The database name used in test
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// The site admin user information
        /// </summary>
        public User SiteAdmin { get; set; }

        /// <summary>
        /// The account management user information
        /// </summary>
        public User AccountManagement { get; set; }

        /// <summary>
        /// The normal user information
        /// </summary>
        public User NormalUser { get; set; }

        /// <summary>
        /// The null role user information
        /// </summary>
        public User NullRoleUser { get; set; }
    }
}