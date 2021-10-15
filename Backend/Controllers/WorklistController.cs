using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMMC.Attributes;
using PMMC.Helpers;
using PMMC.Interfaces;
using PMMC.Models;
using System.Collections.Generic;

namespace PMMC.Controllers
{
    /// <summary>
    /// The worklist controller
    /// </summary>
    [Authorize]
    [ApiController]
    [DefaultRouting]
    public class WorklistController : BaseController
    {
        /// <summary>
        /// The logger used in controller
        /// </summary>
        private readonly ILogger<WorklistController> _logger;

        /// <summary>
        /// The worklist layout service used in controller
        /// </summary>
        private readonly IWorklistLayoutService _worklistLayoutService;

        /// <summary>
        /// The worklist account service used in controller
        /// </summary>
        private readonly IWorklistAccountService _worklistAccountService;

        /// <summary>
        /// Constructor with logger, worklist layout service, http context accessor
        /// </summary>
        /// <param name="logger">the logger </param>
        /// <param name="worklistLayoutService">the worklist layout service</param>
        /// <param name="httpContextAccessor">the http context accessor</param>
        public WorklistController(ILogger<WorklistController> logger, IWorklistLayoutService worklistLayoutService, IWorklistAccountService worklistAccountService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _worklistLayoutService = worklistLayoutService;
            _worklistAccountService = worklistAccountService;
        }

        /// <summary>
        /// Get all worklist layouts for current user
        /// </summary>
        /// <returns>all worklist layouts for current user</returns>
        [HttpGet]
        [Route("layouts")]
        public IEnumerable<WorklistLayout> GetAllWorklistLayouts()
        {
            return _logger.Process(() => _worklistLayoutService.GetAllWorklistLayouts(CurrentUser), "get all current user's worklist layouts");
        }

        /// <summary>
        /// Get worklist layout by id
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <returns>match worklist layout by id</returns>
        [HttpGet]
        [Route("layouts/{layoutId}")]
        public WorklistLayout GetWorklistLayoutById([FromRoute] int layoutId)
        {
            return _logger.Process(() => _worklistLayoutService.GetWorklistLayoutById(layoutId, CurrentUser), "get worklist layout with the given Id",
                parameters: layoutId);
        }

        /// <summary>
        /// Create worklist layout
        /// </summary>
        /// <param name="worklistLayout">the worklist layout to create</param>
        /// <returns>the new created worklist layout with id</returns>
        [HttpPost]
        [Route("layouts")]
        public WorklistLayout CreateWorklistLayout([FromBody] WorklistLayout worklistLayout)
        {
            return _logger.Process(() => _worklistLayoutService.CreateWorklistLayout(worklistLayout, CurrentUser), "creates new worklist layout",
                parameters: worklistLayout);
        }

        /// <summary>
        /// Update worklist layout
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <param name="worklistLayout">the worklist layout to update</param>
        [HttpPut]
        [Route("layouts/{layoutId}")]
        public WorklistLayout UpdateWorklistLayout([FromRoute] int layoutId, [FromBody] WorklistLayout worklistLayout)
        {
            return _logger.Process(() => _worklistLayoutService.UpdateWorklistLayout(layoutId, worklistLayout, CurrentUser), "updates worklist layout with the given Id",
                parameters: new object[] { layoutId, worklistLayout });
        }

        /// <summary>
        /// Delete worklist layout by id
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        [HttpDelete]
        [Route("layouts/{layoutId}")]
        public void DeleteWorklistLayout([FromRoute] int layoutId)
        {
            _logger.Process(() => _worklistLayoutService.DeleteWorklistLayout(layoutId, CurrentUser), "deletes worklist layout with the given Id",
                parameters: layoutId);
        }

        /// <summary>
        /// Get all revcpt codes for the patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match revcpt codes for the patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/revCpt")]
        public IEnumerable<RevCptCode> GetRevCptCodes([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetRevCptCodes(patientId, CurrentUser), "get revcpt codes with the given patient id",
                parameters: patientId);
        }

        /// <summary>
        /// Get all charge codes for the patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match charge codes for the patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/chargeCodes")]
        public IEnumerable<ChargeCodeDetail> GetChargeCodes([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetChargeCodes(patientId, CurrentUser), "get worklist layout with the given Id",
                parameters: patientId);
        }

        /// <summary>
        /// Create charge code
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCode">the charge code to create</param>
        /// <returns>the new created charge code with id</returns>
        [HttpPost]
        [Route("accounts/{patientId}/chargeCodes")]
        public ChargeCodeDetail CreateChargeCode([FromRoute] int patientId, [FromBody] ChargeCodeDetail chargeCode)
        {
            return _logger.Process(() => _worklistAccountService.CreateChargeCode(patientId, chargeCode, CurrentUser), "creates new charge code",
                parameters: new object[] { patientId, chargeCode });
        }

        /// <summary>
        /// Update charge code
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCodeId">the charge code id to update</param>
        /// <param name="chargeCode">the charge code to update</param>
        [HttpPut]
        [Route("accounts/{patientId}/chargeCodes/{chargeCodeId}")]
        public void UpdateChargeCode([FromRoute] int patientId, [FromRoute] int chargeCodeId, [FromBody] ChargeCodeDetail chargeCode)
        {
            _logger.Process(() => _worklistAccountService.UpdateChargeCode(patientId, chargeCodeId, chargeCode, CurrentUser), "updates charge code",
                parameters: new object[] { patientId, chargeCodeId, chargeCode });
        }

        /// <summary>
        /// Delete charge code by id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCodeId">the charge code id</param>
        [HttpDelete]
        [Route("accounts/{patientId}/chargeCodes/{chargeCodeId}")]
        public void DeleteChargeCode([FromRoute] int patientId, [FromRoute] int chargeCodeId)
        {
            _logger.Process(() => _worklistAccountService.DeleteChargeCode(patientId, chargeCodeId, CurrentUser), "deletes charge code with the given Id",
                parameters: new object[] { patientId, chargeCodeId });
        }

        /// <summary>
        /// Get claim history by patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match claim history by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/claimsHistory")]
        public ClaimAndRemitHistory GetClaimsHistory([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetClaimsHistory(patientId, CurrentUser), "get claim history",
                parameters: new object[] { patientId });
        }

        /// <summary>
        /// Get professional claims by patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match professional claim by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/professionalClaims")]
        public IEnumerable<ClaimHistoryItem> GetProfessionalClaims([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetProfessionalClaims(patientId, CurrentUser), "get professional claims",
                parameters: new object[] { patientId });
        }

        /// <summary>
        /// Search worklist accounts
        /// </summary>
        /// <param name="query">the search query</param>
        /// <returns>match worklist accounts</returns>
        [HttpGet]
        [Route("accounts")]
        public WorklistAccount SearchWorklistAccounts([FromQuery] WorklistAccountSearchQuery query)
        {
            return _logger.Process(() => _worklistAccountService.SearchWorklistAccounts(query, CurrentUser), "get worklist accounts",
                parameters: new object[] { query });
        }

        /// <summary>
        /// Get patient's contact information
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match contact info by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/contactInfo")]
        public ContactInfo GetContactInfo([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetContactInfo(patientId, CurrentUser), "get contact info",
                parameters: new object[] { patientId });
        }

        /// <summary>
        /// Update patient's contact information
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="contactInfo">the contact info</param>
        [HttpPatch]
        [Route("accounts/{patientId}/contactInfo")]
        public void UpdateContactInfo([FromRoute] int patientId, [FromBody] ContactInfo contactInfo)
        {
            _logger.Process(() => _worklistAccountService.UpdateContactInfo(patientId, contactInfo, CurrentUser), "update contact info",
                parameters: new object[] { patientId, contactInfo });
        }

        /// <summary>
        /// Get patient's eor
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match eor by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/eor")]
        public IEnumerable<string> GetEor([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetEor(patientId, CurrentUser), "get eor",
                parameters: new object[] { patientId });
        }

        /// <summary>
        /// Get patient's detailed reimbursement items
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match detailed reimbursement items by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/detailReimb")]
        public IEnumerable<DetailReimb> GetDetailReimb([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetDetailReimb(patientId, CurrentUser), "get detailed reimbursement",
                parameters: new object[] { patientId });
        }

        /// <summary>
        /// Get patient's audit status history
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match audit status history by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/auditStatusHistory")]
        public IEnumerable<AuditStatusHistory> GetAuditStatusHistory([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetAuditStatusHistory(patientId, CurrentUser), "get audit status history",
                parameters: new object[] { patientId });
        }

        /// <summary>
        /// Get patient's payment summary
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match payment summary by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/payments")]
        public IEnumerable<PaymentSummary> GetPayments([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetPayments(patientId, CurrentUser), "get payment summary",
                parameters: new object[] { patientId });
        }

        /// <summary>
        /// Creates new payment detail for the given patient.
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="paymentDetail">the payment detail to be created</param>
        /// <returns>the created payment detaild with id</returns>
        [HttpPost]
        [Route("accounts/{patientId}/payments")]
        public PaymentDetail CreatePayment([FromRoute] int patientId, [FromBody] PaymentDetail paymentDetail)
        {
            return _logger.Process(() => _worklistAccountService.CreatePayment(patientId, paymentDetail, CurrentUser), "create payment detail",
                parameters: new object[] { patientId, paymentDetail });
        }

        /// <summary>
        /// Get patient's payment details
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        /// <returns>match payment details by patient id and payer number</returns>
        [HttpGet]
        [Route("accounts/{patientId}/payments/{payerNumber}")]
        public IEnumerable<PaymentDetail> GetPaymentDetails([FromRoute] int patientId, [FromRoute] int payerNumber)
        {
            return _logger.Process(() => _worklistAccountService.GetPaymentDetails(patientId, payerNumber, CurrentUser), "get payment details",
                parameters: new object[] { patientId, payerNumber });
        }

        /// <summary>
        /// Delete patient's payment details
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        [HttpDelete]
        [Route("accounts/{patientId}/payments/{payerNumber}")]
        public void DeletePaymentDetails([FromRoute] int patientId, [FromRoute] int payerNumber)
        {
            _logger.Process(() => _worklistAccountService.DeletePaymentDetails(patientId, payerNumber, CurrentUser), "delete payment details",
                parameters: new object[] { patientId, payerNumber });
        }

        /// <summary>
        /// Get patient's other payments
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match other payments by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/payments/other")]
        public IEnumerable<OtherPayment> GetOtherPayments([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetOtherPayments(patientId, CurrentUser), "get other payments",
                parameters: new object[] { patientId });
        }

        /// <summary>
        /// Update patient's other payments
        /// </summary>
        /// <param name="patientId">the patient id</param>
        [HttpPatch]
        [Route("accounts/{patientId}/payments/other")]
        public void UpdateOtherPayment([FromRoute] int patientId, [FromBody] OtherPaymentUpdate otherPaymentUpdate)
        {
            _logger.Process(() => _worklistAccountService.UpdateOtherPayment(patientId, otherPaymentUpdate, CurrentUser), "update other payments",
                parameters: new object[] { patientId, otherPaymentUpdate });
        }

        /// <summary>
        /// Commits patient's given payment
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        [HttpPost]
        [Route("accounts/{patientId}/payments/{payerNumber}/commit")]
        public void CommitPayment([FromRoute] int patientId, [FromRoute] int payerNumber)
        {
            _logger.Process(() => _worklistAccountService.CommitPayment(patientId, payerNumber, CurrentUser), "commit payment",
                parameters: new object[] { patientId, payerNumber });
        }

        /// <summary>
        /// Get patient's icd codes
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <returns>match icd codes by patient id</returns>
        [HttpGet]
        [Route("accounts/{patientId}/icdCodes")]
        public IEnumerable<ICDCode> GetIcdCodes([FromRoute] int patientId)
        {
            return _logger.Process(() => _worklistAccountService.GetIcdCodes(patientId, CurrentUser), "get icd codes",
                parameters: new object[] { patientId });
        }
    }
}
