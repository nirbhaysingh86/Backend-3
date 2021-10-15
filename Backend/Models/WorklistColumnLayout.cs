using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The worklist column layout
    /// </summary>
    public class WorklistColumnLayout
    {
        /// <summary>
        /// The worklist column layout id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The worklist column layout field name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FieldName { get; set; }

        /// <summary>
        /// The worklist column layout location
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        /// The worklist column layout width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The worklist column layout visible flag
        /// </summary>
        public bool IsVisible { get; set; }
    }
}
