using System.ComponentModel.DataAnnotations;

namespace PMMC.Configs
{
    /// <summary>
    /// The valid rule operand mapping.
    /// </summary>
    public class RuleOperands
    {
        /// <summary>
        /// The equal to operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string EqualTo { get; set; }

        /// <summary>
        /// The greater than operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string GreaterThan { get; set; }


        /// <summary>
        /// The less than operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string LessThan { get; set; }


        /// <summary>
        /// The between operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Between { get; set; }

        /// <summary>
        /// The starts with operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string StartsWith { get; set; }

        /// <summary>
        /// The end with operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string EndsWith { get; set; }

        /// <summary>
        /// The contains operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Contains { get; set; }

        /// <summary>
        /// The in operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string In { get; set; }

        /// <summary>
        /// The not in operand
        /// </summary>
        [Required]
        [StringLength(255)]
        public string NotIn { get; set; }
    }
}