using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The default view limits
    /// </summary>
    public class DefaultViewLimits
    {
        /// <summary>
        /// The auditor
        /// </summary>
        [Required]
        public int Auditor { get; set; }
        
        /// <summary>
        /// The follow up
        /// </summary>
        [Required]
        public int FollowUp { get; set; }
        
        /// <summary>
        /// The status
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// The account age
        /// </summary>
        [Required]
        public int AccountAge { get; set; }

        /// <summary>
        /// The hidden records
        /// </summary>
        [Required]
        public int HiddenRecords { get; set; }

    }
}
