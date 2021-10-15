using PMMC.Entities;
using PMMC.Models;
using System.Collections.Generic;

namespace PMMC.Interfaces
{
    /// <summary>
    /// The worklist account service interface
    /// </summary>
    public interface IWorklistAccountService
    {
        /// <summary>
        /// Get all revcpt codes for the patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all revcpt codes for the patient id</returns>
        IEnumerable<RevCptCode> GetRevCptCodes(int patientId, JwtUser user);

        /// <summary>
        /// Get all charge codes for the patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all charge codes for the patient id</returns>
        IEnumerable<ChargeCodeDetail> GetChargeCodes(int patientId, JwtUser user);

        /// <summary>
        /// Create charge code for the patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCode">the charge code</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the new created charge code with id</returns>
        ChargeCodeDetail CreateChargeCode(int patientId, ChargeCodeDetail chargeCode, JwtUser user);

        /// <summary>
        /// Update charge code
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCodeId">the charge code id to update</param>
        /// <param name="chargeCode">the charge code to update</param>
        /// <param name="user">the jwt user</param>
        void UpdateChargeCode(int patientId, int chargeCodeId, ChargeCodeDetail chargeCode, JwtUser user);

        /// <summary>
        /// Delete charge code by id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCodeId">the charge code id</param>
        /// <param name="user">the jwt user</param>
        void DeleteChargeCode(int patientId, int chargeCodeId, JwtUser user);

        /// <summary>
        /// Get claims history by patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match claims history by patient id</returns>
        ClaimAndRemitHistory GetClaimsHistory(int patientId, JwtUser user);

        /// <summary>
        /// Get professional claims by patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match professional claims by patient id</returns>
        IEnumerable<ClaimHistoryItem> GetProfessionalClaims(int patientId, JwtUser user);

        /// <summary>
        /// Search worklist accounts
        /// </summary>
        /// <param name="query">the search query</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match worklist accounts</returns>
        WorklistAccount SearchWorklistAccounts(WorklistAccountSearchQuery query, JwtUser user);

        /// <summary>
        /// Get patient's contact information
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match contact info by patient id</returns>
        ContactInfo GetContactInfo(int patientId, JwtUser user);

        /// <summary>
        /// Update patient's contact information
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="contactInfo">the contact info</param>
        /// <param name="user">the jwt user</param>
        void UpdateContactInfo(int patientId, ContactInfo contactInfo, JwtUser user);

        /// <summary>
        /// Get patient's eor
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match eor by patient id</returns>
        IEnumerable<string> GetEor(int patientId, JwtUser user);

        /// <summary>
        /// Get patient's detailed reimbursement items
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match detailed reimbursement items by patient id</returns>
        IEnumerable<DetailReimb> GetDetailReimb(int patientId, JwtUser user);

        /// <summary>
        /// Get patient's audit status history
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match audit status history by patient id</returns>
        IEnumerable<AuditStatusHistory> GetAuditStatusHistory(int patientId, JwtUser user);

        /// <summary>
        /// Get patient's payment summary
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match payment summary by patient id</returns>
        IEnumerable<PaymentSummary> GetPayments(int patientId, JwtUser user);

        /// <summary>
        /// Creates new payment detail for the given patient.
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="paymentDetail">the payment detail to be created</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the created payment detaild with id</returns>
        PaymentDetail CreatePayment(int patientId, PaymentDetail paymentDetail, JwtUser user);

        /// <summary>
        /// Get patient's payment details
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match payment details by patient id and payer number</returns>
        IEnumerable<PaymentDetail> GetPaymentDetails(int patientId, int payerNumber, JwtUser user);

        /// <summary>
        /// Delete patient's payment details
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        /// <param name="user">the jwt user</param>
        void DeletePaymentDetails(int patientId, int payerNumber, JwtUser user);

        /// <summary>
        /// Get patient's other payments
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match other payments by patient id</returns>
        IEnumerable<OtherPayment> GetOtherPayments(int patientId, JwtUser user);

        /// <summary>
        /// Update patient's other payments
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="otherPaymentUpdate">the other payment data</param>
        /// <param name="user">the jwt user</param>
        void UpdateOtherPayment(int patientId, OtherPaymentUpdate otherPaymentUpdate, JwtUser user);

        /// <summary>
        /// Commits patient's given payment
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        /// <param name="user">the jwt user</param>
        void CommitPayment(int patientId, int payerNumber, JwtUser user);

        /// <summary>
        /// Get patient's icd codes
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match icd codes by patient id</returns>
        IEnumerable<ICDCode> GetIcdCodes(int patientId, JwtUser user);
    }
}
