using System.ComponentModel.DataAnnotations;

namespace PMMC.Configs
{
    /// <summary>
    /// The jwt configuration 
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// The security key
        /// </summary>
        [Required]
        public string SecurityKey { get; set; }

        /// <summary>
        /// The issuer
        /// </summary>
        [Required]
        public string Issuer { get; set; }

        /// <summary>
        /// The audience
        /// </summary>
        [Required]
        public string Audience { get; set; }

        /// <summary>
        /// The expiration time in minutes
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ExpirationTimeInMinutes { get; set; }
    }
}