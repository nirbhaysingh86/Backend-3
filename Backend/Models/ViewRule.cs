using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The view rule
    /// </summary>
    public class ViewRule
    {
        /// <summary>
        /// The view rule id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The field id
        /// </summary>
        [Range(1, int.MaxValue)]
        public int FieldId { get; set; }

        /// <summary>
        /// The view field 
        /// </summary>
        [JsonIgnore]
        public ViewField viewField { get; set; }

        /// <summary>
        /// view filed name
        /// </summary>
        public string fieldName { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The value id 
        /// </summary>
        public string ValueId { get; set; }

        /// <summary>
        /// The begin range
        /// </summary>
        [StringLength(30)]
        public string BeginRange { get; set; }

        /// <summary>
        /// The end range
        /// </summary>
        [StringLength(30)]
        public string EndRange { get; set; }

        /// <summary>
        /// The operand
        /// </summary>
        [StringLength(20)]
        public string Operand { get; set; }
    }
}
