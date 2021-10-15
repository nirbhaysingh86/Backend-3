using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The Contact Info
    /// </summary>
    public class ContactInfo
    {
        /// <summary>
        /// The patient id
        /// </summary>
        [JsonIgnore]
        public int PatientId { get; set; }

        /// <summary>
        /// The actual patient id
        /// </summary>
        public string ActualPatientId { get; set; }

        /// <summary>
        /// The facility name
        /// </summary>
        [StringLength(50)]
        public string FacilityName { get; set; }

        /// <summary>
        /// The tax id
        /// </summary>
        [StringLength(15)]
        public string TaxId { get; set; }

        /// <summary>
        /// The npi
        /// </summary>
        [StringLength(100)]
        public string Npi { get; set; }

        /// <summary>
        /// The payee name
        /// </summary>
        [StringLength(150)]
        public string PayeeName { get; set; }

        /// <summary>
        /// The payer phone
        /// </summary>
        public string PayerPhone { get; set; }

        /// <summary>
        /// The subscriber id
        /// </summary>
        [StringLength(50)]
        public string SubscriberId { get; set; }

        /// <summary>
        /// The insured ssn
        /// </summary>
        public string InsuredSsn { get; set; }

        /// <summary>
        /// The insured group name
        /// </summary>
        [StringLength(100)]
        public string InsuredGroupName { get; set; }

        /// <summary>
        /// The patient name
        /// </summary>
        [StringLength(100)]
        public string PatientName { get; set; }

        /// <summary>
        /// The date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// The account number
        /// </summary>
        [StringLength(30)]
        public string AccountNumber { get; set; }

        /// <summary>
        /// The total charges
        /// </summary>
        public double? TotalCharges { get; set; }

        /// <summary>
        /// The insured name
        /// </summary>
        [StringLength(100)]
        public string InsuredName { get; set; }

        /// <summary>
        /// The admit date
        /// </summary>
        public DateTime? AdmitDate { get; set; }

        /// <summary>
        /// The discharge date
        /// </summary>
        public DateTime? DischargeDate { get; set; }

        /// <summary>
        /// The patient type
        /// </summary>
        public int? PatientType { get; set; }

        /// <summary>
        /// The medical record no
        /// </summary>
        [StringLength(30)]
        public string MedicalRecordNo { get; set; }

        /// <summary>
        /// The social security no
        /// </summary>
        [StringLength(11)]
        public string SocialSecurityNo { get; set; }

        /// <summary>
        /// The audit status
        /// </summary>
        [StringLength(30)]
        public string AuditStatus { get; set; }

        /// <summary>
        /// The review category
        /// </summary>
        [StringLength(30)]
        public string ReviewCategory { get; set; }

        /// <summary>
        /// The variance category
        /// </summary>
        [StringLength(30)]
        public string VarianceCategory { get; set; }

        /// <summary>
        /// The closed result
        /// </summary>
        [StringLength(30)]
        public string ClosedResult { get; set; }

        /// <summary>
        /// The comitted amount
        /// </summary>
        public double? CommittedAmount { get; set; }

        /// <summary>
        /// The assigned reviewer
        /// </summary>
        public int? AssignedReviewer { get; set; }

        /// <summary>
        /// The argument
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// The auditor
        /// </summary>
        [StringLength(255)]
        public string Auditor { get; set; }

        /// <summary>
        /// The review reason
        /// </summary>
        [StringLength(30)]
        public string ReviewReason { get; set; }

        /// <summary>
        /// The var cat suggestion
        /// </summary>
        public string VarCatSuggestion { get; set; }

        /// <summary>
        /// The closed reason
        /// </summary>
        [StringLength(30)]
        public string ClosedReason { get; set; }

        /// <summary>
        /// The pursuing reason
        /// </summary>
        public string PursuingReason { get; set; }

        /// <summary>
        /// The follow up date
        /// </summary>
        public DateTime? FollowUpDate { get; set; }

        /// <summary>
        /// The event date
        /// </summary>
        public DateTime? EventDate { get; set; }

        /// <summary>
        /// The start date
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The agency
        /// </summary>
        [StringLength(255)]
        public string Agency { get; set; }

        /// <summary>
        /// The review stage
        /// </summary>
        [StringLength(255)]
        public string ReviewStage { get; set; }

        /// <summary>
        /// The responsibility
        /// </summary>
        public string Responsibility { get; set; }

        /// <summary>
        /// The type
        /// </summary>
        [StringLength(255)]
        public string Type { get; set; }

        /// <summary>
        /// The duration
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// The note
        /// </summary>
        [StringLength(30)]
        public string Note { get; set; }
    }
}
