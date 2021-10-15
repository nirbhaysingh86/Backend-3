using System.ComponentModel.DataAnnotations;

namespace PMMC.Configs
{
    /// <summary>
    /// The App settings used in this application
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// The connection string template and must exist valid database username/password and server/db name template
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Valid jwt user roles,and default to use Site Admin/Account Management
        /// </summary>
        [Required]
        [MinLength(1)]
        public string[] Roles { get; set; }

        /// <summary>
        /// The valid code types and default is AMAuditorType/AMFollowUpType/AMStatusType/AMRecordAgeType/AMRecordHiddenType
        /// </summary>
        [Required]
        public CodeTypes CodeTypes { get; set; }

        /// <summary>
        /// The case insensitive values selection type
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ValuesSelectionType { get; set; }

        /// <summary>
        /// Valid values SelectionType field names used in this project
        /// </summary>
        [Required]
        [MinLength(1)]
        public string[] ValuesFieldNames { get; set; }

        /// <summary>
        /// Valid values SelectionType field operands("include"/"exclude") used in this project
        /// </summary>
        [Required]
        [MinLength(1)]
        public string[] ValuesFieldOperands { get; set; }

        /// <summary>
        /// The DateRange selection type
        /// </summary>
        [Required]
        public string DateRangeSelectionType { get; set; }

        /// <summary>
        /// The DateTimeRange selection type
        /// </summary>
        [Required]
        public string DateTimeRangeSelectionType { get; set; }

        /// <summary>
        /// The Percent selection type
        /// </summary>
        [Required]
        public string PercentSelectionType { get; set; }

        /// <summary>
        /// The Number selection type
        /// </summary>
        [Required]
        public string NumberSelectionType { get; set; }

        /// <summary>
        /// The Text selection type
        /// </summary>
        [Required]
        public string TextSelectionType { get; set; }

        /// <summary>
        /// The date range format for rule
        /// </summary>
        [Required]
        [MinLength(1)]
        public string[] DateRangeFormats { get; set; }

        /// <summary>
        /// The date range time format for rule
        /// </summary>
        [Required]
        [MinLength(1)]
        public string[] DateTimeRangeFormats { get; set; }

        /// <summary>
        /// The closed status value
        /// </summary>
        [Required]
        public int ClosedStatusValue { get; set; }

        /// <summary>
        /// The actual payment type that will be count in payment summary screen
        /// </summary>
        [Required]
        public string ActualPaymentType { get; set; }

        /// <summary>
        /// The jwt configuration
        /// </summary>
        [Required]
        public JwtConfig Jwt { get; set; }

        /// <summary>
        /// The valid rule operands
        /// </summary>
        [Required]
        public RuleOperands RuleOperands { get; set; }

        /// <summary>
        /// The worklist accounts default sort by value
        /// </summary>
        [Required]
        public string WorklistAccountsDefaultSortBy { get; set; }

        /// <summary>
        /// The worklist accounts default sort order value
        /// </summary>
        [Required]
        public string WorklistAccountsDefaultSortOrder { get; set; }

        // <summary>
        /// The patient label
        /// </summary>
        [Required]
        public string Patient { get; set; }

        /// <summary>
        /// The payer 1 label
        /// </summary>
        [Required]
        public string Payer1 { get; set; }

        /// <summary>
        /// The payer 2 label
        /// </summary>
        [Required]
        public string Payer2 { get; set; }

        /// <summary>
        /// The payer 3 label
        /// </summary>
        [Required]
        public string Payer3 { get; set; }

        /// <summary>
        /// The payer 4 label
        /// </summary>
        [Required]
        public string Payer4 { get; set; }

        /// <summary>
        /// The other payer label
        /// </summary>
        [Required]
        public string OtherPayer { get; set; }

        /// <summary>
        /// The payment type for other payer
        /// </summary>
        [Required]
        public string PaymentTypeForOtherPayer { get; set; }
    }
}