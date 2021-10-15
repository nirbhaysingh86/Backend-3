using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Exceptions;
using PMMC.Helpers;
using PMMC.Interfaces;
using PMMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PMMC.Services
{
    /// <summary>
    /// The worklist account service
    /// </summary>
    public class WorklistAccountService : BaseService, IWorklistAccountService
    {
        /// <summary>
        /// The get all revcpt codes for the patient id sql
        /// </summary>
        internal const string GetRevCptCodesSql =
            "SELECT [RevCodeId] AS Id,[RevenueCode] AS RevenueCode,[RevCodeDescription] AS RevenueCodeDescription,[CPTTransId] AS TransactionId,[CPTCode] AS CptCode,[CPTDescr] AS CptCodeDescription,[ServiceDate] AS ServiceDate,[Mod1] AS Modifier1,[Mod2] AS Modifier2,[ServiceType] AS ServiceType,[PhysicianID] AS PhysicianId,[ServiceLocation] AS PointOfService,[Units] AS Units,[Charges] AS Charges,[DeniedCharges] AS DeniedCharges,[ExcludedCharges] AS NonCoveredCharges,[NonBilledCharges] AS NonBilledCharges,[Cost] AS Cost FROM [dbo].[RevCPTDetail] WHERE [patientid] = @patientId";

        /// <summary>
        /// The get all charge codes for the patient id sql
        /// </summary>
        internal const string GetChargeCodesSql =
            "SELECT [Id] AS Id,[Code] AS ChargeCode,[RevenueCode] AS RevenueCode,[DeniedCharges] AS DeniedCharges,[Description] AS Description,[CPT] AS CptCode,[ExcludedCharges] AS NonCoveredCharges,[TransactionID] AS TransactionId,[Units] AS Units,[NonBillCharges] AS NonBilledCharges,[ServiceDate] AS ServiceDate,[charges] AS Charges,[Cost] AS Cost FROM [dbo].[ChargeCodeDetails] WHERE [ActualPatientID]=@patientId";

        /// <summary>
        /// The get charge code by id sql
        /// </summary>
        internal const string GetChargeCodeByIdSql =
            "SELECT [Id] AS Id,[Code] AS ChargeCode,[RevenueCode] AS RevenueCode,[DeniedCharges] AS DeniedCharges,[Description] AS Description,[CPT] AS CptCode,[ExcludedCharges] AS NonCoveredCharges,[TransactionID] AS TransactionId,[Units] AS Units,[NonBillCharges] AS NonBilledCharges,[ServiceDate] AS ServiceDate,[charges] AS Charges,[Cost] AS Cost FROM [dbo].[ChargeCodeDetails] WHERE [ActualPatientID]=@patientId AND [Id]=@chargeCodeId";

        /// <summary>
        /// The create charge code sql
        /// </summary>
        internal const string CreateChargeCodeSql =
            "INSERT INTO [dbo].[ChargeCodeDetails]([Code],[RevenueCode],[DeniedCharges],[Description],[CPT],[ExcludedCharges],[TransactionID],[Units],[NonBillCharges],[ServiceDate],[charges],[Cost],[ActualPatientID]) OUTPUT INSERTED.[Id] VALUES(@ChargeCode,@RevenueCode,@DeniedCharges,@Description,@CptCode,@NonCoveredCharges,@TransactionId,@Units,@NonBilledCharges,@ServiceDate,@Charges,@Cost,@patientId)";

        /// <summary>
        /// The update charge code sql
        /// </summary>
        internal const string UpdateChargeCodeSql =
            "UPDATE [dbo].[ChargeCodeDetails] SET [Code]=@ChargeCode,[RevenueCode]=@RevenueCode,[DeniedCharges]=@DeniedCharges,[Description]=@Description,[CPT]=@CptCode,[ExcludedCharges]=@NonCoveredCharges,[TransactionID]=@TransactionId,[Units]=@Units,[NonBillCharges]=@NonBilledCharges,[ServiceDate]=@ServiceDate,[charges]=@Charges,[Cost]=@Cost WHERE [ActualPatientID]=@patientId AND Id=@chargeCodeId";

        /// <summary>
        /// The delete charge code sql
        /// </summary>
        internal const string DeleteChargeCodeSql =
            "DELETE [dbo].[ChargeCodeDetails] WHERE [ActualPatientID]=@patientId AND Id=@chargeCodeId";

        /// <summary>
        /// The get claims history sql
        /// </summary>
        internal const string GetClaimsHistorySql =
            "SELECT [ch].ID AS Id,[ch].[ClaimNumber] AS PatientControlNumber,[ch].[PrimaryPayer] AS PrimaryPayer,[ch].[BillType] AS BillType,[ch].[ClaimFreqType] AS ClaimFrequencyType,[ch].[BillingDate] AS BillingDate,[ch].[ClaimNumber] AS ClaimNumber,[ch].[StatementFromDate] AS StatementFromDate,[ch].[StatementToDate] AS StatementToDate,[ch].[TotalCharges] AS TotalCharges,[ch].[TotalNonCoveredCharges] AS TotalNonCoveredCharges,[ch].[PatientEstAmtDue] AS PatientEstAmtDue,LineCount=0,[ch].[DestinationPayer] AS DestinationPayer,[ch].[DestinationPayerResp] AS DestinationPayerResponsibility,[cd].[ID] AS Id,[cd].[LineNumber] AS LineNumber,[cd].[ServiceDate] AS [Date],[cd].[RevenueCode] AS RevenueCode,[cd].[CPTCode] AS HcpcsCptCode,[cd].[Modifier] AS Modifier,[cd].[Quantity] AS Quantity,[cd].[TotalCharges] AS Charges,[cd].[NonCoveredCharges] AS NonCoveredCharges FROM [dbo].[ClaimHistory] [ch] LEFT JOIN [dbo].[ClaimDetail] [cd] ON [cd].[iClaimId]=[ch].[ID] WHERE [ActualPatientID]=@patientId";

        /// <summary>
        /// The get claims history sql
        /// </summary>
        internal const string GetRemitsHistorySql =
            "SELECT [rh].[ID] AS Id,[rh].[Name] AS [Provider],[rh].[PayeeID] AS Payer,[rh].[BillType] AS BillType,[rh].[ClaimFrequencyType] AS ClaimFrequencyType,[rh].[EffectiveDate] AS [EffectiveDate],[rh].[CheckNumber] AS CheckNumber,[rh].[StatementFromDate] AS StatementFromDate,[rh].[StatementToDate] AS StatementToDate,[rh].[ClaimStatusCode] AS ClaimStatus,(SELECT SUM([rds].[Charges]) FROM [dbo].[RemitDetail] rds WHERE [rds].[RemitID]=[rh].[ID]) AS ClaimBilledAmount,[rh].[CoverageAmt] AS ClaimPaidAmount,[rh].[AdjAmt] AS ClaimLevelAdjustAmount,[rh].[DenialAmt] AS ClaimLevelDenialAmount,(SELECT SUM([rds].[Charges])-SUM([rds].[ActualAllowedAmt]) FROM [dbo].[RemitDetail] rds WHERE [rds].[RemitID]=[rh].[ID]) AS LineLevelAdjustAmount,(SELECT SUM([rds].[DenialAmt]) FROM [dbo].[RemitDetail] rds WHERE [rds].[RemitID]=[rh].[ID]) AS LineLevelDenialAmount,(SELECT COUNT([rds].[ID]) FROM [dbo].[RemitDetail] rds WHERE [rds].[RemitID]=[rh].[ID]) AS LineCount,[rh].[AdjRsnCodes] AS ClaimLevelAdjustReasonCodes,[rh].[RemarkCodes] AS ClaimLevelRemarkCodes,[rh].[ClaimNumber] AS ClaimNumber,[rd].[ID] AS Id,[rd].[AdjRsnCode] AS AdjustmentReasonCodes,[rd].[RemarkCode] AS RemarkCodes FROM [dbo].[RemitHistory] [rh] LEFT JOIN [dbo].[RemitDetail] [rd] ON [rd].[RemitID]=[rh].[ID] WHERE [rh].[ActualPatientID]=@patientId";

        /// <summary>
        /// The validate patient id sql
        /// </summary>
        internal const string ValidatePatientIdSql =
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM [dbo].[tblWorklistData] WHERE [ActualPatientID]=@patientId) THEN 1 ELSE 0 END";

        /// <summary>
        /// The validate patient id 2 sql
        /// Checks if patient exists by PatientId
        /// </summary>
        internal const string ValidatePatientId2Sql =
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM [dbo].[tblWorklistData] WHERE [PatientID]=@patientId) THEN 1 ELSE 0 END";

        /// <summary>
        /// The filter logic
        /// </summary>
        internal const string GetFilterLogic =
            "SELECT [Description] FROM [dbo].[tblComboBoxesSystemValues] WHERE [CodeType]='{0}' AND [Code]={1}";

        /// <summary>
        /// The validate sort by sql
        /// </summary>
        internal const string ValidateSortBy =
            "SELECT CASE WHEN EXISTS(SELECT 1 FROM [dbo].[tblColumns] WHERE [DataField]=@SortBy) THEN 1 ELSE 0 END";

        /// <summary>
        /// The get worklist layout by name sql
        /// </summary>
        internal const string GetDefaultWorklistLayoutColumnsSql =
            "SELECT [cd].[CustomDetailID] AS Id,[cd].[FieldName] AS FieldName,[cd].[FieldLocation] AS Location,[cd].[FieldWidth] AS Width, [cd].[FieldVisible] AS IsVisible  FROM [dbo].[tblColumnsCustom] [c] LEFT JOIN [dbo].[tblColumnsCustomDetail] [cd] ON [cd].[CustomID] = [c].[CustomID]  WHERE [c].[UserID]=@UserId AND [c].[DefaultView]=1";

        /// <summary>
        /// The get worklist layout by name sql
        /// </summary>
        internal const string GetDefaultColumnsSql =
            "SELECT [cd].[CustomDetailID] AS Id,[cd].[FieldName] AS FieldName,[cd].[FieldLocation] AS Location,[cd].[FieldWidth] AS Width, [cd].[FieldVisible] AS IsVisible  FROM [dbo].[tblColumnsCustomDetail] [cd] WHERE [cd].[CustomID]=1";

        /// <summary>
        /// The get contact info sql
        /// </summary>
        internal const string GetContactInfoSql =
            "SELECT [ActualPatientID] AS [ActualPatientId],[FacilityName] AS [FacilityName],[FederalTaxID] AS [TaxId],[AttendingPhysicianNPI] AS [Npi],[Name_PatientName] AS [PayeeName],NULL AS [PayerPhone],[SubscriberID] AS [SubscriberID],NULL AS [InsuredSsn],[InsuredGroupName] AS [InsuredGroupName],[Name_PatientName] AS [PatientName],[BirthDate] AS [DateOfBirth],[ActualPatientID] AS [AccountNumber],[TotalCharges] AS [TotalCharges],[InsuredName] AS [InsuredName],[AdmissionDate] AS [AdmitDate],[DischargeDate] AS [DischargeDate],[PatTypeCodeLink] AS [PatientType],[MedicalRecordID] AS [MedicalRecordNo],[SSN] AS [SocialSecurityNo],(SELECT [cb].[Description] FROM [dbo].[tblComboBoxesSystemValues] [cb] WHERE [cb].[CodeType]='AuditFlagAll' AND [cb].[Code]=CONVERT(VARCHAR(30),[Payer1AuditFlag])) AS [AuditStatus],(SELECT [cb].[Description] FROM [dbo].[tblComboBoxesSystemValues] [cb] WHERE [cb].[CodeType]='ReviewCategory' AND [cb].[Code]=CONVERT(VARCHAR(30),[Payer1ReviewCategory])) AS [ReviewCateogry],(SELECT [cb].[Description] FROM [dbo].[tblComboBoxesSystemValues] [cb] WHERE [cb].[CodeType]='VarianceCategory' AND [cb].[Code]=CONVERT(VARCHAR(30),[VarianceCategoryId])) AS [VarianceCategory],(SELECT [cb].[Description] FROM [dbo].[tblComboBoxesSystemValues] [cb] WHERE [cb].[CodeType]='acAuditResultFlag' AND [cb].[Code]=CONVERT(VARCHAR(30),[Payer1ResultFlag])) AS [ClosedResult],[Payer1RecoveryCommittedAmount] AS [CommittedAmount],[ReviewerUserID] AS [AssignedReviewer],[RecoveryArgument] AS [Argument],[HISAuditor] AS [Auditor],(SELECT [cb].[Description] FROM [dbo].[tblComboBoxesSystemValues] [cb] WHERE [cb].[CodeType]='ReviewReason' AND [cb].[Code]=CONVERT(VARCHAR(30),[Payer1ReviewReason])) AS [ReviewReason],[PrimaryProjectedVariance] AS [VarCatSuggestion],(SELECT [cb].[Description] FROM [dbo].[tblComboBoxesSystemValues] [cb] WHERE [cb].[CodeType]='AuditReason' AND [cb].[Code]=CONVERT(VARCHAR(30),[Payer1AuditReason])) AS [ClosedReason],NULL AS [PursuingReason],[RACFollowupDate] AS [FollowUpDate],GETDATE() AS [EventDate],[Payer1StartDate] AS [StartDate],[CollectionAgency] AS [Agency],[rcyP2_ReviewStage] AS [ReviewStage],'Payer 1' AS [Responsibility],[CommunicationType] AS [Type],[RecoveryTotalDuration] AS [Duration],[RegistrationNote] AS [Note] FROM [dbo].[tblWorklistData] WHERE [PatientID]=@patientId";

        /// <summary>
        /// The update contact info sql
        /// </summary>
        internal const string UpdateContactInfoSql =
            "UPDATE [dbo].[tblWorklistData] SET [FacilityName]=@FacilityName,[FederalTaxID]=@TaxId,[AttendingPhysicianNPI]=@Npi,[Name_PayerID]=@PayeeName,[SubscriberID]=@SubscriberID,[InsuredGroupName]=@InsuredGroupName,[Name_PatientName]=@PatientName,[BirthDate]=@DateOfBirth,[ActualPatientID]=@AccountNumber,[TotalCharges]=@TotalCharges,[InsuredName]=@InsuredName,[AdmissionDate]=@AdmitDate,[DischargeDate]=@DischargeDate,[PatTypeCodeLink]=@PatientType,[MedicalRecordID]=@MedicalRecordNo,[SSN]=@SocialSecurityNo,[Payer1AuditFlag]=(SELECT CONVERT(INTEGER,[Code]) FROM [dbo].[tblComboBoxesSystemValues] WHERE [CodeType]='AuditFlagAll' AND [Description]=@AuditStatus),[Payer1RecoveryCommittedAmount]=@CommittedAmount,[RecoveryArgument]=@Argument,[HISAuditor]=@Auditor,[PrimaryProjectedVariance]=@VarCatSuggestion,[RACFollowupDate]=@FollowUpDate,[Payer1StartDate]=@StartDate,[CollectionAgency]=@Agency,[CommunicationType]=@Type,[RecoveryTotalDuration]=@Duration,[RegistrationNote]=@Note WHERE [PatientID]=@PatientId";

        /// <summary>
        /// The get eor sql
        /// </summary>
        internal const string GetEorSql =
            "SELECT [EOB] FROM [dbo].[tblEOR] WHERE [PatientID]=@patientId";

        /// <summary>
        /// The get detail reimb sql
        /// </summary>
        internal const string GetDetailReimbSql =
            "SELECT [CodeReimId] AS [Id],[CodeType] AS [CodeType],[ServiceType] AS [ServiceType],NULL AS [Item],[Service] AS [ServicePayOrGroup],[ServiceDate] AS [ServiceDate],[AdjTotalCharges] AS [AdjustedCharges],[Units] AS [Units],[ReimbursementMethod] AS [ReimbMethod],[PPC] AS [Ppc],[Rate] AS [Rate],[ExpectedReimburse] AS [ExpectedReimburse],[TermsDiff] AS [TermsDiff] FROM [dbo].[tblDetailReimb] WHERE [PatientID]=@patientId";

        /// <summary>
        /// The get audit status history sql
        /// </summary>
        internal const string GetAuditStatusHistorySql =
            "SELECT [PatientID] AS [Id],[Name_Payer1AuditFlag] AS [AuditStatus],[ImportDate] AS [DateSet],[Name_AuditorID] AS [ChangedBy],[AssignedAuditor] AS [CurrentAuditor],[EventDate] AS [EventDate],[DaysElapsed] AS [DaysElapsed] FROM [dbo].[AuditHistory] WHERE [PatientID]=@patientId";

        /// <summary>
        /// The get audit status history sql
        /// </summary>
        internal const string GetPaymentsSql =
            "SELECT [PatientID] AS [Id],[PayerNumberLabel] AS [PaidBy],[PayerNumber] AS [PayerId],[PayerName] AS [PayerName],[ContractName] AS [ContractName],[PaymentDate] AS [PaymentDate],[EstPayAllowable] AS [EstPayAllowable],[ActualPayment] AS [ActualPayment],[Writeoff] AS [Writeoff],[BalanceDue] AS [BalanceDue] FROM [dbo].[tblPaymentSummary] WHERE [ActualPatientID]=@patientId";

        /// <summary>
        /// The create charge code sql
        /// </summary>
        internal const string CreatePaymentSql =
            "INSERT INTO [dbo].[tblPaymentDetail]([PatientId],[Entity],[IncrementName],[DateAdded],[PostingDate],[Amount],[ExcludedAmount],[AdjustCode],[AdjustCodeDesc],[PayerCodeLink]) OUTPUT INSERTED.[IncrementId] VALUES(@PatientId,@PaidBy,@PaymentType,@ImportDate,@PostingDate,@Amount,@ExcludedAmount,@AdjustCode,@AdjustCodeDescription,CASE WHEN @IsOtherPayment=0 THEN (SELECT PayerCodeLink FROM [dbo].[tblWorklistData] WHERE PatientID=@PatientId) ELSE 0 END)";

        /// <summary>
        /// The update payment summary sql
        /// </summary>
        internal const string UpdatePaymentSummarySql =
            @"WITH [pd] AS (SELECT [pd].[PatientID],[wd].[ActualPatientID]
            ,[PayerNumber]=CASE WHEN [pd].[Entity]=@patient THEN 0 WHEN [pd].[Entity]=@payer1 THEN 1 WHEN [pd].[Entity]=@payer2 THEN 2 WHEN [pd].[Entity]=@payer3 THEN 3 WHEN [pd].[Entity]=@payer4 THEN 4 WHEN [pd].[Entity]=@otherPayer THEN 5 END
            ,[PayerNumberLabel]=[pd].[Entity]
            ,[PayerID]=CASE WHEN [pd].[Entity]=@patient THEN [wd].[PatientID] WHEN [pd].[Entity]=@payer1 THEN [wd].[PayerID] WHEN [pd].[Entity]=@payer2 THEN [wd].[Payer2ID] WHEN [pd].[Entity]=@payer3 THEN [wd].[Payer3ID] WHEN [pd].[Entity]=@payer4 THEN [wd].[Payer4ID] END
            ,[PayerName]=CASE WHEN [pd].[Entity]=@patient THEN [wd].[Name_PatientName] WHEN [pd].[Entity]=@payer1 THEN [wd].[Name_PayerID] WHEN [pd].[Entity]=@payer2 THEN [wd].[Name_Payer2ID] WHEN [pd].[Entity]=@payer3 THEN [wd].[Name_Payer3ID] WHEN [pd].[Entity]=@payer4 THEN [wd].[Name_Payer4ID] END
            ,[ContractName]=[wd].[Name_ContractID],[PaymentDate]=MAX([pd].[PostingDate]),[EstPayAllowable]=SUM([pd].[Amount]),[ActualPayment]=SUM([pd].[Amount]),[WriteOff]=0,[BalanceDue]=0
            FROM [dbo].[tblPaymentDetail] [pd]
            LEFT JOIN [dbo].[tblWorklistData] [wd] ON [wd].[PatientID]=[pd].[PatientID]
            WHERE [pd].PatientID=@patientId AND [pd].[Entity]=@paidBy AND [pd].[IncrementName] IN (@actualPayment,@otherPayment)
            GROUP BY [pd].[PatientId],[wd].[ActualPatientID],[pd].[Entity],[wd].[PatientID],[wd].[PayerID],[wd].[Payer2ID],[wd].[Payer3ID],[wd].[Payer4ID],[wd].[Name_PatientName],[wd].[Name_PayerID],[wd].[Name_Payer2ID],[wd].[Name_Payer3ID],[wd].[Name_Payer4ID],[wd].[Name_ContractID])
                        
            MERGE [dbo].[tblPaymentSummary] [ps]
            USING [pd] ON [ps].[PatientID]=@patientId AND [ps].[PayerNumberLabel]=@paidBy
            WHEN MATCHED THEN UPDATE SET [ps].[EstPayAllowable]=[pd].[EstPayAllowable],[ps].[ActualPayment]=[pd].[ActualPayment],[ps].[PaymentDate]=[pd].[PaymentDate]
            WHEN NOT MATCHED THEN INSERT ([PatientID],[ActualPatientID],[PayerNumber],[PayerNumberLabel],[PayerID],[PayerName],[ContractName],[PaymentDate],[EstPayAllowable],[ActualPayment],[WriteOff],[BalanceDue])
            VALUES ([pd].[PatientID],[pd].[ActualPatientID],[pd].[PayerNumber],[pd].[PayerNumberLabel],[pd].[PayerID],[pd].[PayerName],[pd].[ContractName],[pd].[PaymentDate],[pd].[EstPayAllowable],[pd].[ActualPayment],[pd].[WriteOff],[pd].[BalanceDue])
            WHEN NOT MATCHED BY SOURCE AND [ps].[PatientID]=@patientId AND [ps].[PayerNumberLabel]=@paidBy THEN DELETE;";

        /// <summary>
        /// The get payment details sql
        /// </summary>
        internal const string GetPaymentDetailsSql =
            "SELECT [pd].[IncrementId] AS [Id],[pd].[Entity] AS [PaidBy],[pd].[IncrementName] AS [PaymentType],[pd].[DateAdded] AS [ImportDate],[pd].[PostingDate] AS [PostingDate],[pd].[Amount] AS [Amount],[pd].[ExcludedAmount] AS [ExcludedAmount],[pd].[AdjustCode] AS [AdjustCode],[pd].[AdjustCodeDesc] AS [AdjustCodeDescription] FROM [dbo].[tblPaymentSummary] [ps] LEFT JOIN [dbo].[tblPaymentDetail] [pd] ON [pd].[PatientID]=[ps].[PatientID] AND [pd].[Entity]=[ps].[PayerNumberLabel] LEFT JOIN [dbo].[tblWorklistData] [wd] ON [wd].[PatientID]=[pd].[PatientID] WHERE [ps].[PatientID]=@patientId AND [ps].[PayerNumber]=@payerNumber AND [pd].[PayerCodeLink]=[wd].[PayerCodeLink]";

        /// <summary>
        /// The validate other payment sql
        /// </summary>
        internal const string ValidateOtherPaymentSql =
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM [dbo].[tblPaymentSummary] [ps] LEFT JOIN [dbo].[tblPaymentDetail] [pd] ON [pd].[PatientID]=[ps].[PatientID] AND [pd].[Entity]=[ps].[PayerNumberLabel] LEFT JOIN [dbo].[tblWorklistData] [wd] ON [wd].[PatientID]=[pd].[PatientID] WHERE [ps].[PatientID]=@patientId AND [ps].[PayerNumber]=@payerNumber AND [pd].[PayerCodeLink]!=[wd].[PayerCodeLink]) THEN 1 ELSE 0 END";

        /// <summary>
        /// The delete payment details sql
        /// </summary>
        internal const string DeletePaymentDetailsSql =
            "DELETE [pd] FROM [dbo].[tblPaymentDetail] [pd] LEFT JOIN [dbo].[tblWorklistData] [wd] ON [wd].[PatientID]=[pd].[PatientID] WHERE [pd].[PatientID]=@patientId AND ISNULL([pd].[PayerCodeLink],0)!=[wd].[PayerCodeLink]";

        /// <summary>
        /// The get payment details sql
        /// </summary>
        internal const string GetOtherPaymentsSql =
            "SELECT [pd].[Entity] AS [PaidBy],[wd].[ActualPatientID] AS [AccountNumber],[pd].[AdjustCode] AS [AdjustCode],[pd].[AdjustCodeDesc] AS [AdjustCodeDescription],[pd].[AdjustType] AS [AdjustmentType],[pd].[AdjustCode] AS [PostingCode],[wd].[PayerPlanCode] AS [PayerId],[pd].[Amount] AS [Amount],[pd].[DateAdded] AS [ImportDate] FROM [dbo].[tblWorklistData] [wd] LEFT JOIN [dbo].[tblPaymentDetail] [pd] ON [pd].[PatientID]=[wd].[PatientID] WHERE [wd].[PatientID]=@patientId AND [wd].[PayerCodeLink]<>[pd].[PayerCodeLink]";

        /// <summary>
        /// The get payment details sql
        /// </summary>
        internal const string GetOtherPaymentsByPayerNumberSql =
            "SELECT [pd].[Entity] AS [PaidBy],[wd].[ActualPatientID] AS [AccountNumber],[pd].[AdjustCode] AS [AdjustCode],[pd].[AdjustCodeDesc] AS [AdjustCodeDescription],[pd].[AdjustType] AS [AdjustmentType],[pd].[AdjustCode] AS [PostingCode],[wd].[PayerPlanCode] AS [PayerId],[pd].[Amount] AS [Amount],[pd].[DateAdded] AS [ImportDate] FROM [dbo].[tblPaymentSummary] [ps] LEFT JOIN [dbo].[tblPaymentDetail] [pd] ON [pd].[PatientID]=[ps].[PatientID] AND [pd].[Entity]=[ps].[PayerNumberLabel] LEFT JOIN [dbo].[tblWorklistData] [wd] ON [wd].[PatientID]=[pd].[PatientID] WHERE [wd].[PatientID]=@patientId AND [ps].[PayerNumber]=@payerNumber AND [wd].[PayerCodeLink]<>[pd].[PayerCodeLink]";

        /// <summary>
        /// The get payment for update sql
        /// </summary>
        internal const string GetOtherPaymentForUpdateSql =
            "SELECT [pd].[Entity] AS [PaidBy],[wd].[ActualPatientID] AS [AccountNumber],[pd].[AdjustCode] AS [AdjustCode],[pd].[AdjustCodeDesc] AS [AdjustCodeDescription],[pd].[AdjustType] AS [AdjustmentType],[pd].[AdjustCode] AS [PostingCode],[wd].[PayerPlanCode] AS [PayerId],[pd].[Amount] AS [Amount],[pd].[DateAdded] AS [ImportDate] FROM [dbo].[tblWorklistData] [wd] LEFT JOIN [dbo].[tblPaymentDetail] [pd] ON [pd].[PatientID]=[wd].[PatientID] WHERE [wd].[PatientID]=@patientId AND [wd].[PayerCodeLink]<>[pd].[PayerCodeLink] AND [pd].[Entity]=@Entity AND [pd].[IncrementName]=@IncrementName";

        /// <summary>
        /// The update other payment sql
        /// </summary>
        internal const string UpdateOtherPaymentSql =
            "UPDATE [pd] SET [pd].AdjustCode=@AdjustCode, [pd].PayerCodeLink=@payerId FROM [dbo].[tblWorklistData] [wd] LEFT JOIN [dbo].[tblPaymentDetail] [pd] ON [pd].[PatientID]=[wd].[PatientID] WHERE [wd].[PatientID]=@patientId AND [wd].[PayerCodeLink]<>[pd].[PayerCodeLink] AND [pd].[Entity]=@Entity AND [pd].[IncrementName]=@IncrementName";

        /// <summary>
        /// The update other payment sql
        /// </summary>
        internal const string CommitPaymentSql =
            "UPDATE [pd] SET [pd].[PayerCodeLink]=[wd].[PayerCodeLink] FROM [dbo].[tblPaymentSummary] [ps] LEFT JOIN [dbo].[tblPaymentDetail] [pd] ON [pd].[PatientID]=[ps].[PatientID] AND [pd].[Entity]=[ps].[PayerNumberLabel] LEFT JOIN [dbo].[tblWorklistData] [wd] ON [wd].[PatientID]=[pd].[PatientID] WHERE [wd].[PatientID]=@patientId AND [ps].[PayerNumber]=@payerNumber AND [wd].[PayerCodeLink]<>[pd].[PayerCodeLink]";

        /// <summary>
        /// The get detail reimb sql
        /// </summary>
        internal const string GetIcdCodesSql =
            "SELECT [Id] AS [Id],[Code] AS [IcdCode],[ICDRevisionNumber] AS [IcdVersion],[Description] AS [Description],[ServiceDate] AS [ServiceDate],[Rank] AS [Rank],[TransactionID] AS [TransactionId],[POA] AS [Poa] FROM [dbo].[tblICDCodeDetail] WHERE [PatientID]=@patientId";

        private readonly IViewService _viewService;
        private readonly IWorklistLayoutService _worklistLayoutService;

        /// <summary>
        /// Constructor with logger and app settings
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="appSettings">the app settings</param>
        public WorklistAccountService(ILogger<WorklistAccountService> logger, IOptions<AppSettings> appSettings, IViewService viewService, IWorklistLayoutService worklistLayoutService) : base(logger, appSettings)
        {
            _viewService = viewService;
            _worklistLayoutService = worklistLayoutService;
        }

        /// <summary>
        /// Get all revcpt codes for the patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all revcpt codes for the patient id</returns>
        public IEnumerable<RevCptCode> GetRevCptCodes(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                ValidatePatientId(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<RevCptCode>(GetRevCptCodesSql, new { patientId });
                }, user);
            }, "get all revcpt codes for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Get all charge codes for the patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all charge codes for the patient id</returns>
        public IEnumerable<ChargeCodeDetail> GetChargeCodes(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                ValidatePatientId(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<ChargeCodeDetail>(GetChargeCodesSql, new { patientId });
                }, user);
            }, "get all charge codes for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Create charge code for the patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCode">the charge code</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the new created charge code with id</returns>
        public ChargeCodeDetail CreateChargeCode(int patientId, ChargeCodeDetail chargeCode, JwtUser user)
        {
            return _logger.Process(() =>
            {
                ValidateChargeCode(chargeCode, user, patientId);
                return ProcessWithDbTransaction((conn) =>
                {
                    var newId = conn.QuerySingle<int>(CreateChargeCodeSql,
                        new
                        {
                            chargeCode.ChargeCode,
                            chargeCode.RevenueCode,
                            chargeCode.DeniedCharges,
                            chargeCode.Description,
                            chargeCode.CptCode,
                            chargeCode.NonCoveredCharges,
                            chargeCode.TransactionId,
                            chargeCode.Units,
                            chargeCode.NonBilledCharges,
                            chargeCode.ServiceDate,
                            chargeCode.Charges,
                            chargeCode.Cost,
                            patientId
                        });
                    chargeCode.Id = newId;
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = null,
                        NewValue = LoggerHelper.GetObjectDescription(chargeCode),
                        OperationType = OperationType.Create,
                        ObjectType = nameof(ChargeCodeDetail),
                        Timestamp = DateTime.Now
                    });
                    return chargeCode;
                }, user);
            }, "creates new charge code",
                parameters: new object[] { patientId, chargeCode, user });
        }

        /// <summary>
        /// Update charge code
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCodeId">the charge code id to update</param>
        /// <param name="chargeCode">the charge code to update</param>
        /// <param name="user">the jwt user</param>
        public void UpdateChargeCode(int patientId, int chargeCodeId, ChargeCodeDetail chargeCode, JwtUser user)
        {
            _logger.Process(() =>
            {
                ValidateChargeCode(chargeCode, user, patientId, chargeCodeId);
                var oldChargeCode = FindValidChargeCode(patientId, chargeCodeId, user);
                ProcessWithDbTransaction((conn) =>
                {
                    conn.Execute(UpdateChargeCodeSql,
                        new
                        {
                            chargeCode.ChargeCode,
                            chargeCode.RevenueCode,
                            chargeCode.DeniedCharges,
                            chargeCode.Description,
                            chargeCode.CptCode,
                            chargeCode.NonCoveredCharges,
                            chargeCode.TransactionId,
                            chargeCode.Units,
                            chargeCode.NonBilledCharges,
                            chargeCode.ServiceDate,
                            chargeCode.Charges,
                            chargeCode.Cost,
                            patientId,
                            chargeCodeId
                        });
                    chargeCode.Id = chargeCodeId;
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = LoggerHelper.GetObjectDescription(oldChargeCode),
                        NewValue = LoggerHelper.GetObjectDescription(chargeCode),
                        OperationType = OperationType.Update,
                        ObjectType = nameof(ChargeCodeDetail),
                        Timestamp = DateTime.Now
                    });
                }, user);
            }, "updates charge code with the given patient id and charge code id",
                parameters: new object[] { patientId, chargeCodeId, chargeCode, user });
        }

        /// <summary>
        /// Delete charge code by id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCodeId">the charge code id</param>
        /// <param name="user">the jwt user</param>
        public void DeleteChargeCode(int patientId, int chargeCodeId, JwtUser user)
        {
            _logger.Process(() =>
            {
                ValidatePatientId(patientId, user);
                var oldChargeCode = FindValidChargeCode(patientId, chargeCodeId, user);
                ProcessWithDbTransaction((conn) =>
                {
                    conn.Execute(DeleteChargeCodeSql, new { patientId, chargeCodeId });
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = LoggerHelper.GetObjectDescription(oldChargeCode),
                        NewValue = null,
                        OperationType = OperationType.Delete,
                        ObjectType = nameof(ChargeCodeDetail),
                        Timestamp = DateTime.Now
                    });
                }, user);
            }, "deletes charge code with the given Id",
                parameters: new object[] { patientId, chargeCodeId, user });
        }

        /// <summary>
        /// Get claims history by patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match claims history by patient id</returns>
        public ClaimAndRemitHistory GetClaimsHistory(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                ValidatePatientId(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    var Claims = GetClaims(patientId, conn);
                    var Remits = GetRemits(patientId, conn);
                    return new ClaimAndRemitHistory() { Claims = Claims, Remits = Remits };
                }, user);
            }, "get claims history by patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Get professional claims by patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match professional claims by patient id</returns>
        public IEnumerable<ClaimHistoryItem> GetProfessionalClaims(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                ValidatePatientId(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return GetClaims(patientId, conn);
                }, user);
            }, "get professional claims by patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Search worklist accounts
        /// </summary>
        /// <param name="query">the search query</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match worklist accounts</returns>
        public WorklistAccount SearchWorklistAccounts(WorklistAccountSearchQuery query, JwtUser user)
        {
            return _logger.Process(() =>
            {
                return ProcessWithDb((conn) =>
                {
                    query.SortBy = ValidateSearchAccountsSortBy(query.SortBy, conn);
                    query.SortOrder = ValidateSearchAccountsSortOrder(query.SortOrder);
                    // get the columns to use in SELECT statement.
                    IList<WorklistColumnLayout> selectedLayoutColumns = GetSelectedLayoutColumns(query.LayoutId, conn, user);
                    if (!query.Offset.HasValue) query.Offset = 0;
                    if (!query.Limit.HasValue) query.Limit = 20;
                    // get view rule by view id
                    var rules = _viewService.GetAllViewRules(query.ViewId.Value, user);
                    StringBuilder viewFilter = new StringBuilder();
                    Dictionary<int, string> valueTypes = new Dictionary<int, string>();
                    foreach (ViewRule rule in rules)
                    {
                        // create filters according to field selection type.
                        var field = rule.viewField;
                        if (IsSelectionType(_appSettings.DateRangeSelectionType, field.SelectionType))
                        {
                            viewFilter.Append($" AND [{field.Name}] BETWEEN '{rule.BeginRange}' AND '{rule.EndRange}'");
                        }
                        else if (IsSelectionType(_appSettings.DateTimeRangeSelectionType, field.SelectionType))
                        {
                            viewFilter.Append($" AND [{field.Name}] BETWEEN '{rule.BeginRange}' AND '{rule.EndRange}'");
                        }
                        else if (IsSelectionType(_appSettings.NumberSelectionType, field.SelectionType) || IsSelectionType(_appSettings.PercentSelectionType, field.SelectionType))
                        {
                            if (rule.Operand == _appSettings.RuleOperands.EqualTo)
                                viewFilter.Append($" AND [{field.Name}] = {rule.Value}");
                            else if (rule.Operand == _appSettings.RuleOperands.GreaterThan)
                                viewFilter.Append($" AND [{field.Name}] > {rule.Value}");
                            else if (rule.Operand == _appSettings.RuleOperands.LessThan)
                                viewFilter.Append($" AND [{field.Name}] < {rule.Value}");
                            else if (rule.Operand == _appSettings.RuleOperands.Between)
                                viewFilter.Append($" AND [{field.Name}] BETWEEN {rule.BeginRange} AND {rule.EndRange}");
                            else
                                throw new InternalServerErrorException($"Rule Operand: {rule.Operand} is not defined.");
                        }
                        else if (IsSelectionType(_appSettings.TextSelectionType, field.SelectionType))
                        {
                            if (rule.Operand == _appSettings.RuleOperands.StartsWith)
                                viewFilter.Append($" AND [{field.Name}] LIKE '{rule.Value}%'");
                            else if (rule.Operand == _appSettings.RuleOperands.EndsWith)
                                viewFilter.Append($" AND [{field.Name}] LIKE '%{rule.Value}'");
                            else if (rule.Operand == _appSettings.RuleOperands.Contains)
                                viewFilter.Append($" AND [{field.Name}] LIKE '%{rule.Value}%'");
                            else
                                throw new InternalServerErrorException($"Rule Operand: {rule.Operand} is not defined.");
                        }
                        else if (IsSelectionType(_appSettings.ValuesSelectionType, field.SelectionType))
                        {
                            var correctValue = field.IntegerFieldLink ? rule.ValueId : rule.Value;
                            if (valueTypes.ContainsKey(rule.FieldId))
                            {
                                valueTypes[rule.FieldId] = $"{valueTypes[rule.FieldId]},'{correctValue}'";
                            }
                            else
                            {
                                if (rule.Operand == _appSettings.RuleOperands.In)
                                    valueTypes[rule.FieldId] = $"[{field.Name}] IN ('{correctValue}'";
                                else if (rule.Operand == _appSettings.RuleOperands.NotIn)
                                    valueTypes[rule.FieldId] = $"[{field.Name}] NOT IN ('{correctValue}'";
                                else
                                    throw new InternalServerErrorException($"Rule Operand: {rule.Operand} is not defined.");
                            }
                        }
                    }
                    foreach (var statement in valueTypes.Values)
                    {
                        viewFilter.Append($" AND {statement})");
                    }

                    var queryAuditor = string.Format(GetFilterLogic, _appSettings.CodeTypes.Auditor, query.Auditor.Value);
                    var queryFollowUp = string.Format(GetFilterLogic, _appSettings.CodeTypes.FollowUp, query.FollowUp.Value);
                    var queryStatus = string.Format(GetFilterLogic, _appSettings.CodeTypes.Status, query.Status.Value);
                    var queryAccountAge = string.Format(GetFilterLogic, _appSettings.CodeTypes.AccountAge, query.AccountAge.Value);
                    var queryHiddenRecords = string.Format(GetFilterLogic, _appSettings.CodeTypes.HiddenRecords, query.HiddenRecords.Value);
                    var multiSQL = $"{queryAuditor};{queryFollowUp};{queryStatus};{queryAccountAge};{queryHiddenRecords}";
                    using (var multi = conn.QueryMultiple(multiSQL))
                    {
                        var auditor = multi.ReadSingleOrDefault<string>();
                        var followUp = multi.ReadSingleOrDefault<string>();
                        var status = multi.ReadSingleOrDefault<string>();
                        var accountAge = multi.ReadSingleOrDefault<string>();
                        var hiddenRecord = multi.ReadSingleOrDefault<string>();

                        switch (auditor)
                        {
                            case "All Records": break;
                            case "Assigned To Me": viewFilter.Append($" AND [AuditorId]={user.UserId}"); break;
                            case "Unassigned": viewFilter.Append($" AND ([AuditorId] IS NULL OR [AuditorId]=0)"); break;
                            default: throw new BadRequestException($"Auditor='{query.Auditor.Value}' is not valid");
                        }
                        switch (followUp)
                        {
                            case "All Records": break;
                            case "Past Due": viewFilter.Append($" AND [ActivityDate] < CONVERT(DATE, GETDATE())"); break;
                            case "Due Today or Past Due": viewFilter.Append($" AND [ActivityDate] <= CONVERT(DATE, GETDATE())"); break;
                            case "Due Within 1 Week": viewFilter.Append($" AND [ActivityDate] <= CONVERT(DATE,DATEADD(DAY,7,GETDATE()))"); break;
                            case "Due Within 1 Month": viewFilter.Append($" AND [ActivityDate] <= CONVERT(DATE,DATEADD(MONTH,1,GETDATE()))"); break;
                            default: throw new BadRequestException($"FollowUp='{query.FollowUp.Value}' is not valid");
                        }
                        switch (status)
                        {
                            case "Include All": break;
                            case "Exclude Closed": viewFilter.Append($" AND ([Status] != {_appSettings.ClosedStatusValue} OR [Status] IS NULL)"); break;
                            default: throw new BadRequestException($"Status='{query.Status.Value}' is not valid");
                        }
                        switch (accountAge)
                        {
                            case "All Records": break;
                            case "Within 1 Year": viewFilter.Append($" AND [Age] - [AgeOfClaim] <= 1"); break;
                            case "Within 2 Years": viewFilter.Append($" AND [Age] - [AgeOfClaim] <= 2"); break;
                            case "Within 3 Years": viewFilter.Append($" AND [Age] - [AgeOfClaim] <= 3"); break;
                            case "Within 4 Years": viewFilter.Append($" AND [Age] - [AgeOfClaim] <= 4"); break;
                            case "Within 5 Years": viewFilter.Append($" AND [Age] - [AgeOfClaim] <= 5"); break;
                            default: throw new BadRequestException($"AccountAge='{query.AccountAge.Value}' is not valid");
                        }
                        switch (hiddenRecord)
                        {
                            case "Include": break;
                            case "Exclude": viewFilter.Append($" AND [HiddenByUser] IS NOT NULL"); break;
                            default: throw new BadRequestException($"HiddenRecords='{query.HiddenRecords.Value}' is not valid");
                        }
                    }
                    if (viewFilter.ToString().StartsWith(" AND "))
                    {
                        viewFilter.Remove(0, 5);
                        viewFilter.Insert(0, "WHERE ");
                    }
                    var columns = selectedLayoutColumns.Where(x => x.IsVisible == true && x.FieldName != "PatientID" && x.FieldName != "ActualPatientID").OrderBy(o => o.Location).Select(x => x.FieldName).ToArray();
                    string finalQuery = $"SELECT [PatientId],[ActualPatientId]{(columns.Any() ? ",[" + string.Join("],[", columns) + "]" : "")} FROM [dbo].[tblWorklistData] {viewFilter} ORDER BY {query.SortBy} {query.SortOrder} OFFSET {query.Offset} ROWS FETCH NEXT {query.Limit} ROWS ONLY";
                    string countQuery = $"SELECT [Count]=COUNT(1) FROM [dbo].[tblWorklistData] {viewFilter}";
                    var result = conn.Query(finalQuery);
                    var totalCount = conn.QuerySingle<int>(countQuery);
                    var items = new List<WorklistAccountItem>();
                    foreach (IDictionary<string, object> row in result)
                    {
                        var values = new List<string>();
                        foreach (var col in columns)
                        {
                            values.Add(row[col]?.ToString());
                        }
                        items.Add(new WorklistAccountItem
                        {
                            PatientId = int.Parse(row["PatientId"].ToString()),
                            ActualPatientId = row["ActualPatientId"].ToString(),
                            Values = values.ToArray()
                        });
                    }
                    return new WorklistAccount
                    {
                        TotalCount = totalCount,
                        Items = items
                    };
                }, user);
            }, "search worklist accounts",
                parameters: new object[] { query, user });
        }

        /// <summary>
        /// Get patient's contact information
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match contact info by patient id</returns>
        public ContactInfo GetContactInfo(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                return FindValidContactInfo(patientId, user);
            }, "get contact info for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Update patient's contact information
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="contactInfo">the contact info</param>
        /// <param name="user">the jwt user</param>
        public void UpdateContactInfo(int patientId, ContactInfo contactInfo, JwtUser user)
        {
            _logger.Process(() =>
            {
                // valiadate patient exists and get it's existing contact information
                var oldContactInfo = FindValidContactInfo(patientId, user);
                ProcessWithDbTransaction((conn) =>
                {
                    contactInfo.PatientId = patientId;
                    contactInfo.ActualPatientId = oldContactInfo.ActualPatientId;
                    // execute update sql
                    conn.Execute(UpdateContactInfoSql, contactInfo);
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = LoggerHelper.GetObjectDescription(oldContactInfo),
                        NewValue = LoggerHelper.GetObjectDescription(contactInfo),
                        OperationType = OperationType.Update,
                        ObjectType = nameof(ContactInfo),
                        Timestamp = DateTime.Now
                    });
                }, user);
            }, "updates contact info with the given patient id",
                parameters: new object[] { patientId, contactInfo, user });
        }

        /// <summary>
        /// Get patient's eor
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match eor by patient id</returns>
        public IEnumerable<string> GetEor(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<string>(GetEorSql, new { patientId });
                }, user);
            }, "get eor for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Get patient's detailed reimbursement items
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match detailed reimbursement items by patient id</returns>
        public IEnumerable<DetailReimb> GetDetailReimb(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<DetailReimb>(GetDetailReimbSql, new { patientId });
                }, user);
            }, "get detailed reimbursement items for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Get patient's audit status history
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match audit status history by patient id</returns>
        public IEnumerable<AuditStatusHistory> GetAuditStatusHistory(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<AuditStatusHistory>(GetAuditStatusHistorySql, new { patientId });
                }, user);
            }, "get audit status history for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Get patient's payment summary
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match payment summary by patient id</returns>
        public IEnumerable<PaymentSummary> GetPayments(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<PaymentSummary>(GetPaymentsSql, new { patientId });
                }, user);
            }, "get payments summary for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Creates new payment detail for the given patient.
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="paymentDetail">the payment detail to be created</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the created payment detaild with id</returns>
        public PaymentDetail CreatePayment(int patientId, PaymentDetail paymentDetail, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                ValidatePaymentDetail(paymentDetail);
                return ProcessWithDbTransaction((conn) =>
                {
                    paymentDetail.PatientId = patientId;
                    // create the payment detail and get new id
                    var newId = conn.QuerySingle<int>(CreatePaymentSql, paymentDetail);
                    paymentDetail.Id = newId;
                    // if payment type is actual payment, update the payment summary record.
                    if (paymentDetail.PaymentType == _appSettings.ActualPaymentType || paymentDetail.PaymentType == _appSettings.PaymentTypeForOtherPayer)
                    {
                        UpdatePaymentSummary(conn, patientId, paymentDetail.PaidBy);
                    }
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = null,
                        NewValue = LoggerHelper.GetObjectDescription(paymentDetail),
                        OperationType = OperationType.Create,
                        ObjectType = nameof(PaymentDetail),
                        Timestamp = DateTime.Now
                    });
                    return paymentDetail;
                }, user);
            }, "create new payment detail for the patient id",
                parameters: new object[] { patientId, paymentDetail, user });
        }

        /// <summary>
        /// Get patient's payment details
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match payment details by patient id and payer number</returns>
        public IEnumerable<PaymentDetail> GetPaymentDetails(int patientId, int payerNumber, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<PaymentDetail>(GetPaymentDetailsSql, new { patientId, payerNumber });
                }, user);
            }, "get payments details for the patient id and payer number",
                parameters: new object[] { patientId, payerNumber, user });
        }

        /// <summary>
        /// Delete patient's payment details
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        /// <param name="user">the jwt user</param>
        public void DeletePaymentDetails(int patientId, int payerNumber, JwtUser user)
        {
            _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                // validate payment is other payment
                ValidateOtherPayment(patientId, payerNumber, user);
                ProcessWithDbTransaction((conn) =>
                {
                    // get existent payment detail
                    var oldPaymentDetail = conn.Query<OtherPayment>(GetOtherPaymentsByPayerNumberSql, new { patientId, payerNumber });
                    // delete the payment
                    conn.Execute(DeletePaymentDetailsSql, new { patientId, payerNumber });
                    // update summary info after deleting payment
                    UpdatePaymentSummary(conn, patientId, oldPaymentDetail.ElementAt(0).PaidBy);
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = LoggerHelper.GetObjectDescription(oldPaymentDetail),
                        NewValue = null,
                        OperationType = OperationType.Delete,
                        ObjectType = nameof(PaymentDetail),
                        Timestamp = DateTime.Now
                    });
                }, user);
            }, "delete payments details for the patient id and payer number",
                parameters: new object[] { patientId, payerNumber, user });
        }

        /// <summary>
        /// Get patient's other payments
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match other payments by patient id</returns>
        public IEnumerable<OtherPayment> GetOtherPayments(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<OtherPayment>(GetOtherPaymentsSql, new { patientId });
                }, user);
            }, "get other payments for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Update patient's other payments
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="otherPaymentUpdate">the other payment data</param>
        /// <param name="user">the jwt user</param>
        public void UpdateOtherPayment(int patientId, OtherPaymentUpdate otherPaymentUpdate, JwtUser user)
        {
            _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                ProcessWithDbTransaction((conn) =>
                {
                    otherPaymentUpdate.PatientId = patientId;
                    // get existent payment detail
                    var oldPaymentDetail = conn.Query<OtherPayment>(GetOtherPaymentForUpdateSql, otherPaymentUpdate);
                    // validate given payment is other payment
                    ValidateOtherPayment(oldPaymentDetail, otherPaymentUpdate);
                    // execute update sql
                    conn.Execute(UpdateOtherPaymentSql, otherPaymentUpdate);
                    var newPaymentDetail = oldPaymentDetail.ToList();
                    newPaymentDetail.ForEach(pd =>
                    {
                        pd.AdjustCode = otherPaymentUpdate.AdjustCode;
                        pd.PayerId = otherPaymentUpdate.PayerId;
                    });
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = LoggerHelper.GetObjectDescription(oldPaymentDetail),
                        NewValue = LoggerHelper.GetObjectDescription(newPaymentDetail),
                        OperationType = OperationType.Update,
                        ObjectType = nameof(OtherPayment),
                        Timestamp = DateTime.Now
                    });
                }, user);
            }, "updates other payment with the given patient id",
                parameters: new object[] { patientId, otherPaymentUpdate, user });
        }

        /// <summary>
        /// Commits patient's given payment
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="payerNumber">the payer number</param>
        /// <param name="user">the jwt user</param>
        public void CommitPayment(int patientId, int payerNumber, JwtUser user)
        {
            _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                // validate payment is other payment
                ValidateOtherPayment(patientId, payerNumber, user);
                ProcessWithDbTransaction((conn) =>
                {
                    // get existent payment
                    var oldPaymentDetail = conn.Query<PaymentDetail>(GetOtherPaymentsByPayerNumberSql, new { patientId, payerNumber });
                    // execute commit sql, and it won't be other payment anymore
                    conn.Execute(CommitPaymentSql, new { patientId, payerNumber });
                    var newPaymentDetail = oldPaymentDetail.ToList();

                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = LoggerHelper.GetObjectDescription(oldPaymentDetail),
                        NewValue = LoggerHelper.GetObjectDescription(newPaymentDetail),
                        OperationType = OperationType.Update,
                        ObjectType = nameof(PaymentDetail),
                        Timestamp = DateTime.Now
                    });
                }, user);
            }, "commit payment details for the patient id and payer number",
                parameters: new object[] { patientId, payerNumber, user });
        }

        /// <summary>
        /// Get patient's icd codes
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match icd codes by patient id</returns>
        public IEnumerable<ICDCode> GetIcdCodes(int patientId, JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                // validate patient exists
                ValidatePatientId2(patientId, user);
                return ProcessWithDb((conn) =>
                {
                    return conn.Query<ICDCode>(GetIcdCodesSql, new { patientId });
                }, user);
            }, "get icd codes for the patient id",
                parameters: new object[] { patientId, user });
        }

        /// <summary>
        /// Validates patient id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <exception cref="NotFoundException">throws if patient is not found</exception>
        private void ValidatePatientId(int patientId, JwtUser user)
        {
            Helper.ValidateArgumentPositive(patientId, nameof(patientId));
            ProcessWithDb((conn) =>
            {
                var record = conn.QuerySingle<int>(ValidatePatientIdSql, new { patientId });
                if (record == 0)
                {
                    throw new NotFoundException($"Patient with patientId='{patientId}' not found");
                }
            }, user);
        }

        /// <summary>
        /// Validates patient id by using patientId
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <exception cref="NotFoundException">throws if patient is not found</exception>
        private void ValidatePatientId2(int patientId, JwtUser user)
        {
            Helper.ValidateArgumentPositive(patientId, nameof(patientId));
            ProcessWithDb((conn) =>
            {
                var record = conn.QuerySingle<int>(ValidatePatientId2Sql, new { patientId });
                if (record == 0)
                {
                    throw new NotFoundException($"Patient with patientId='{patientId}' not found");
                }
            }, user);
        }

        /// <summary>
        /// Get worklist layout by id
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCodeId">the charge code id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match worklist layout by id</returns>
        /// <exception cref="NotFoundException">throws if charge code is not found</exception>
        private ChargeCodeDetail FindValidChargeCode(int patientId, int chargeCodeId, JwtUser user)
        {
            Helper.ValidateArgumentPositive(patientId, nameof(patientId));
            Helper.ValidateArgumentPositive(chargeCodeId, nameof(chargeCodeId));
            Helper.ValidateArgumentNotNull(user, nameof(user));
            return ProcessWithDb((conn) =>
            {
                var record = conn.Query<ChargeCodeDetail>(GetChargeCodeByIdSql, new { patientId, chargeCodeId });
                if (!record.Any())
                {
                    throw new NotFoundException($"Charge code by patientId='{patientId}' and id='{chargeCodeId}' not found");
                }
                return record.First();
            }, user);
        }

        /// <summary>
        /// Validate charge code to ensure valid inputs
        /// </summary>
        /// <param name="chargeCode">the charge code to validate</param>
        /// <param name="user">the jwt user</param>
        /// <param name="patientId">the patient id</param>
        /// <param name="chargeCodeId">the updated charge code id</param>
        /// <exception cref="BadRequestException">throws if worklist layout is invalid</exception>
        private void ValidateChargeCode(ChargeCodeDetail chargeCode, JwtUser user, int patientId, int? chargeCodeId = null)
        {
            Helper.ValidateArgumentNotNull(chargeCode, nameof(chargeCode));
            Helper.ValidateArgumentNotNull(user, nameof(user));
            ValidatePatientId(patientId, user);
            if (chargeCodeId.HasValue)
            {
                Helper.ValidateArgumentPositive(chargeCodeId.Value, nameof(chargeCodeId));
            }
        }

        /// <summary>
        /// Check whether selection type is valid
        /// </summary>
        /// <param name="expectedSelectionType">the expected selection type</param>
        /// <param name="selectionType">the selection type to check</param>
        /// <returns>true if selection type match otherwise false</returns>
        private bool IsSelectionType(string expectedSelectionType, string selectionType)
        {
            return string.Equals(expectedSelectionType, selectionType, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validates the sortBy parameter
        /// </summary>
        /// <param name="sortBy">the sortBy value</param>
        /// <param name="conn">the connection to be used for query</param>
        /// <returns>the sortBy value if it's valid or default value if it's empty</returns>
        /// <exception cref="BadRequestException">throws if sortBy parameter is invalid</exception>
        private string ValidateSearchAccountsSortBy(string sortBy, IDbConnection conn)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortByRecord = conn.QuerySingle<int>(ValidateSortBy, new { sortBy });
                if (sortByRecord == 0)
                {
                    throw new BadRequestException($"Sort By='{sortBy}' is not valid");
                }
                return sortBy;
            }
            else
            {
                return _appSettings.WorklistAccountsDefaultSortBy;
            }
        }

        /// <summary>
        /// Validates the sort order parameter
        /// </summary>
        /// <param name="sortOrder">the sortOrder value</param>
        /// <returns>the sortOrder value if it's valid or default value if it's empty</returns>
        /// <exception cref="BadRequestException">throws if sortOrder parameter is invalid</exception>
        private string ValidateSearchAccountsSortOrder(string sortOrder)
        {
            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder.ToLower() != "asc" && sortOrder.ToLower() != "desc")
                {
                    throw new BadRequestException($"Sort Order='{sortOrder}' is not valid");
                }
                return sortOrder;
            }
            else
            {
                return _appSettings.WorklistAccountsDefaultSortOrder;
            }
        }

        /// <summary>
        /// Gets the worklist column layouts of the given layout id
        /// </summary>
        /// <param name="layoutId">the sortOrder value</param>
        /// <param name="conn">the connection to be used for query</param>
        /// <param name="user">the current user</param>
        /// <returns>the list of worklist column layouts</returns>
        private IList<WorklistColumnLayout> GetSelectedLayoutColumns(int? layoutId, IDbConnection conn, JwtUser user)
        {
            if (layoutId.HasValue)
            {
                return _worklistLayoutService.GetWorklistLayoutById(layoutId.Value, user).Columns;
            }
            else
            {
                var layoutColumns = conn.Query<WorklistColumnLayout>(GetDefaultWorklistLayoutColumnsSql, new { user.UserId });
                if (layoutColumns.Any())
                {
                    return layoutColumns.ToList();
                }
                else
                {
                    return conn.Query<WorklistColumnLayout>(GetDefaultColumnsSql).ToList();
                }
            }
        }

        /// <summary>
        /// Gets the claim history items with details
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="conn">the connection to be used for query</param>
        /// <returns>the list of claim history items</returns>
        private List<ClaimHistoryItem> GetClaims(int patientId, IDbConnection conn)
        {
            var claims = new Dictionary<int, ClaimHistoryItem>();
            conn.Query<ClaimHistoryItem, ClaimDetailItem, ClaimHistoryItem>(GetClaimsHistorySql, (claimItems, claimDetailItems) =>
            {
                if (!claims.TryGetValue(claimItems.Id, out ClaimHistoryItem claimHistoryItem))
                    claims.Add(claimItems.Id, claimHistoryItem = claimItems);
                if (claimHistoryItem.ClaimDetails == null)
                    claimHistoryItem.ClaimDetails = new List<ClaimDetailItem>();
                if (claimDetailItems != null)
                    claimHistoryItem.ClaimDetails.Add(claimDetailItems);
                return claimHistoryItem;
            }, new { patientId }, splitOn: "Id");

            var Claims = claims.Values.ToList();
            Claims.ForEach(x => x.LineCount = x.ClaimDetails.Count);
            return Claims;
        }

        /// <summary>
        /// Gets the remit history items with details
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="conn">the connection to be used for query</param>
        /// <returns>the list of remit history items</returns>
        private List<RemitHistoryItem> GetRemits(int patientId, IDbConnection conn)
        {
            var remits = new Dictionary<int, RemitHistoryItem>();
            conn.Query<RemitHistoryItem, RemitDetailItem, RemitHistoryItem>(GetRemitsHistorySql, (remitItems, remitDetailItems) =>
            {
                if (!remits.TryGetValue(remitItems.Id, out RemitHistoryItem remitHistoryItem))
                    remits.Add(remitItems.Id, remitHistoryItem = remitItems);
                if (remitHistoryItem.RemitDetails == null)
                    remitHistoryItem.RemitDetails = new List<RemitDetailItem>();
                if (remitDetailItems != null)
                    remitHistoryItem.RemitDetails.Add(remitDetailItems);
                return remitHistoryItem;
            }, new { patientId }, splitOn: "Id");

            var Remits = remits.Values.ToList();
            return Remits;
        }

        /// <summary>
        /// Get patient's contact information
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match contact info by patient id</returns>
        private ContactInfo FindValidContactInfo(int patientId, JwtUser user)
        {
            Helper.ValidateArgumentNotNull(user, nameof(user));
            // validate patient exists
            ValidatePatientId2(patientId, user);
            return ProcessWithDb((conn) =>
            {
                return conn.QueryFirst<ContactInfo>(GetContactInfoSql, new { patientId });
            }, user);
        }

        /// <summary>
        /// Update payment summary information
        /// </summary>
        /// <param name="conn">the connection to be used for query</param>
        /// <param name="patientId">the patient id</param>
        /// <param name="paidBy">the paid by</param>
        private void UpdatePaymentSummary(IDbConnection conn, int patientId, string paidBy)
        {
            // summarizes the payment details by patient id and entity name. Only Actual payments and Miscellaneous payments are calculated.
            // if record exists in payment summary table then updates its value.
            // if record doesn't exist in payment summary table then inserts new record with summarized data.
            conn.Execute(UpdatePaymentSummarySql, new { patientId, paidBy, actualPayment = _appSettings.ActualPaymentType, patient = _appSettings.Patient, payer1 = _appSettings.Payer1, payer2 = _appSettings.Payer2, payer3 = _appSettings.Payer3, payer4 = _appSettings.Payer4, otherPayer = _appSettings.OtherPayer, otherPayment = _appSettings.PaymentTypeForOtherPayer });
        }

        /// <summary>
        /// Validates if the payment detail is other payment
        /// </summary>
        /// <param name="patientId">the patient id</param>
        /// <param name="patientId">the payer number</param>
        /// <param name="user">the jwt user</param>
        /// <exception cref="NotFoundException">throws if payment is not other payment</exception>
        private void ValidateOtherPayment(int patientId, int payerNumber, JwtUser user)
        {
            Helper.ValidateArgumentPositive(payerNumber, nameof(payerNumber));
            ProcessWithDb((conn) =>
            {
                var record = conn.QuerySingle<int>(ValidateOtherPaymentSql, new { patientId, payerNumber });
                if (record == 0)
                {
                    throw new NotFoundException($"Other payment with patientId='{patientId}' and payerNumber='{payerNumber}' not found");
                }
            }, user);
        }

        /// <summary>
        /// Validates if the payment detail is other payment
        /// </summary>
        /// <param name="otherPayment">the other payment</param>
        /// <exception cref="NotFoundException">throws if payment is not other payment</exception>
        private void ValidateOtherPayment(IEnumerable<OtherPayment> otherPayment, OtherPaymentUpdate otherPaymentUpdate)
        {
            if (!otherPayment.Any())
            {
                throw new NotFoundException($"Other payment with patientId='{otherPaymentUpdate.PatientId}', entity='{otherPaymentUpdate.Entity}' and incrementName='{otherPaymentUpdate.IncrementName}' not found");
            }
        }

        /// <summary>
        /// Validates if the payment detail model for creation is valid
        /// </summary>
        /// <param name="paymentDetail">the payment detail</param>
        /// <exception cref="BadRequestException">throws if payment detail is not valid</exception>
        private void ValidatePaymentDetail(PaymentDetail paymentDetail)
        {
            string[] payerLabels = { _appSettings.Patient, _appSettings.Payer1, _appSettings.Payer2, _appSettings.Payer3, _appSettings.Payer4, _appSettings.OtherPayer };
            if (!payerLabels.Contains(paymentDetail.PaidBy))
            {
                throw new BadRequestException($"PaidBy: '{paymentDetail.PaidBy}' is invalid. Value must be one of '{String.Join(',', payerLabels)}'");
            }
            if (paymentDetail.PaidBy == _appSettings.OtherPayer && paymentDetail.PaymentType != _appSettings.PaymentTypeForOtherPayer)
            {
                throw new BadRequestException($"Payment type must be '{_appSettings.PaymentTypeForOtherPayer}' for '{_appSettings.OtherPayer}'");
            }
            if (paymentDetail.PaidBy == _appSettings.OtherPayer)
            {
                paymentDetail.IsOtherPayment = 1;
            }
            else
            {
                paymentDetail.IsOtherPayment = 0;
            }
        }
    }
}
