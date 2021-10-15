using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The view
    /// </summary>
    public class View
    {
        /// <summary>
        /// The view id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The view name and must be unique in database
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// The view description
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// The view owner
        /// </summary>
        [JsonIgnore]
        public int ViewOwner { get; set; }

        /// <summary>
        /// The default view limits
        /// </summary>
        [Required]
        public DefaultViewLimits Limits { get; set; }

        /// <summary>
        /// The default view flag
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
