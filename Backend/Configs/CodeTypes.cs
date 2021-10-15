using System.ComponentModel.DataAnnotations;

namespace PMMC.Configs
{
    /// <summary>
    /// The valid code type mapping.
    /// </summary>
    public class CodeTypes
    {
        /// <summary>
        /// The auditor code type
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Auditor { get; set; }

        /// <summary>
        /// The follow up code type
        /// </summary>
        [Required]
        [StringLength(255)]
        public string FollowUp { get; set; }


        /// <summary>
        /// The status code type
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Status { get; set; }


        /// <summary>
        /// The account age code type
        /// </summary>
        [Required]
        [StringLength(255)]
        public string AccountAge { get; set; }

        /// <summary>
        /// The hidden records code type
        /// </summary>
        [Required]
        [StringLength(255)]
        public string HiddenRecords { get; set; }

        public string PaymentStatus { get; set; }

        /// <summary>
        /// Check value is valid code type
        /// </summary>
        /// <param name="value">the value to check</param>
        /// <returns>true if is valid code type otherwise return false</returns>
        public bool ContainsValue(string value)
        {
            return Auditor.Equals(value) || FollowUp.Equals(value) || Status.Equals(value) ||
                   AccountAge.Equals(value) || HiddenRecords.Equals(value)|| PaymentStatus.Equals(value);
        }

        /// <summary>
        /// The all possible valid code types
        /// </summary>
        /// <returns>all possible valid code types</returns>
        public string[] Values()
        {
            return new string[] {Auditor, FollowUp, Status, AccountAge, HiddenRecords, PaymentStatus };
        }
    }
}