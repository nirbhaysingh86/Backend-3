using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The worklist layout
    /// </summary>
    public class WorklistLayout
    {
        /// <summary>
        /// The worklist layout id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The worklist layout name and must be unique in database
        /// </summary>
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// The worklist layout description
        /// </summary>
        [StringLength(275)]
        public string Description { get; set; }

        /// <summary>
        /// The default worklist layout flag
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The worklist column layouts
        /// </summary>
        [Required]
        public IList<WorklistColumnLayout> Columns { get; set; }
    }
}
