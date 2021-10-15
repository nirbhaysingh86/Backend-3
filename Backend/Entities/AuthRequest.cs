using System.ComponentModel.DataAnnotations;

namespace PMMC.Entities
{
    /// <summary>
    /// The auth request
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// The user name to auth
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        
        /// <summary>
        /// The password to auth
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        /// <summary>
        /// The database server name
        /// </summary>
        [Required]
        public string Server { get; set; }

        /// <summary>
        /// The database name
        /// </summary>
        [Required]
        public string DbName { get; set; }
    }
}
