using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The worklist account
    /// </summary>
    public class WorklistAccount
    {
        /// <summary>
        /// The total count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The items
        /// </summary>
        public IList<WorklistAccountItem> Items { get; set; }

    }

    public class WorklistAccountItem
    {
        /// <summary>
        /// The patient id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The actual patient id
        /// </summary>
        public string ActualPatientId { get; set; }

        /// <summary>
        /// The values
        /// </summary>
        public string[] Values { get; set; }
    }

    public class WorklistAccountSearchQuery
    {
        /// <summary>
        /// The view id
        /// </summary>
        [Required]
        public int? ViewId { get; set; }

        /// <summary>
        /// The auditor
        /// </summary>
        [Required]
        public int? Auditor { get; set; }

        /// <summary>
        /// The follow up
        /// </summary>
        [Required]
        public int? FollowUp { get; set; }

        /// <summary>
        /// The status
        /// </summary>
        [Required]
        public int? Status { get; set; }

        /// <summary>
        /// The account age
        /// </summary>
        [Required]
        public int? AccountAge { get; set; }

        /// <summary>
        /// The hidden records
        /// </summary>
        [Required]
        public int? HiddenRecords { get; set; }

        /// <summary>
        /// The sort by
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// The sort order
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// The offset
        /// </summary>
        [Range(0, int.MaxValue)]
        public int? Offset { get; set; }

        /// <summary>
        /// The limit
        /// </summary>
        [Range(1, 100)]
        public int? Limit { get; set; }

        /// <summary>
        /// The layoutId
        /// </summary>
        public int? LayoutId { get; set; }
    }
}
