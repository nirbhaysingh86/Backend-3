UPDATE [dbo].[tblEOR] SET [PatientID]=5279364 WHERE [PatientID]=1
GO
UPDATE [dbo].[tblDetailReimb] SET [PatientID]=5279364 WHERE [PatientID]=1
GO
UPDATE [dbo].[tblICDCodeDetail] SET [PatientID]=5279364 WHERE [PatientId]=1
GO
UPDATE [dbo].[tblPaymentSummary] SET [PatientID]=5279364 WHERE [PatientId]=1
GO

DROP TABLE IF EXISTS [dbo].[AuditHistory]
GO
CREATE TABLE [dbo].[AuditHistory]
(
	[PatientID] [INT],
	[Name_Payer1AuditFlag] [VARCHAR](100),
	[AssignedAuditor] [VARCHAR](200),
	[ImportDate] [DATETIME],
	[Name_AuditorID] [VARCHAR](200),
	[EventDate] [DATE],
	[DaysElapsed] [INT],
	[ResponsbilityType] [VARCHAR](7),
	[VarianceCategory] [VARCHAR](100),
	[AssignedReviewer] [VARCHAR](200)
) ON [PRIMARY]
GO
INSERT [dbo].[AuditHistory] ([PatientID], [Name_Payer1AuditFlag], [AssignedAuditor], [ImportDate], [Name_AuditorID], [EventDate], [DaysElapsed], [ResponsbilityType], [VarianceCategory], [AssignedReviewer]) VALUES (5279364, N'Internal Review', N'Unassigned', CAST(N'2016-04-08T10:57:24.253' AS DateTime), N'User Name ABC', CAST(N'2016-04-08T00:00:00' AS Date), 0, N'Payer 1', N'Posting Issue', N'Unassigned')
GO

INSERT INTO [dbo].[tblComboBoxesSystemValues] ([Code], [CodeType], [Description])
VALUES
( '1', 'ReviewCategory', 'Add-Ons' ), 
( '2', 'ReviewCategory', 'Stop-Loss' ), 
( '3', 'ReviewCategory', 'Denial' ), 
( '4', 'ReviewCategory', 'Registration Related' ), 
( '5', 'ReviewCategory', 'Posting Error' ), 
( '6', 'ReviewCategory', 'Unknown Rates and Terms' ), 
( '7', 'ReviewCategory', 'Coding or Billing Error' ), 
( '8', 'ReviewCategory', 'Underpayments' ), 
( '9', 'ReviewCategory', 'Patient' ), 
( '1', 'ReviewReason', 'MRI' ), 
( '2', 'ReviewReason', 'High Per Diem Not Paid' ), 
( '3', 'ReviewReason', 'No Precert' ), 
( '4', 'ReviewReason', 'CT' ), 
( '5', 'ReviewReason', 'High Discount Not Paid' ), 
( '6', 'ReviewReason', 'No Auth' ), 
( '7', 'ReviewReason', 'High-Cost Drug' ), 
( '8', 'ReviewReason', 'Low Trim Not Paid' ), 
( '9', 'ReviewReason', 'No Coverage at Service' ), 
( '10', 'ReviewReason', 'Implant' ), 
( '11', 'ReviewReason', 'Timely Filing' ),
( '1', 'VarianceCategory', 'Contract Enhancement' ), 
( '2', 'VarianceCategory', 'Missing Data' ), 
( '4', 'VarianceCategory', 'Registration Error' ), 
( '6', 'VarianceCategory', 'Potential Underpayments' ), 
( '7', 'VarianceCategory', 'Posting Issue' ), 
( '8', 'VarianceCategory', 'Potential Overpayments' ), 
( '9', 'VarianceCategory', 'Other' ), 
( '10', 'VarianceCategory', 'Add-on' ),
( '1', 'AuditReason', 'Paid Correctly Adjustment Rvw Needed' ), 
( '2', 'AuditReason', 'Paid Correctly Variance Related to Payment on Other Claim' ), 
( '3', 'AuditReason', 'Appeal Denied - No Auth- Denial Upheld' ), 
( '4', 'AuditReason', 'Appeal Denied - Paid Per Contract' ), 
( '5', 'AuditReason', 'Appeal Denied - Not Medically Necessary' ),
( '0', 'acAuditResultFlag', 'Pending' ), 
( '1', 'acAuditResultFlag', 'No Action Required' ), 
( '10', 'acAuditResultFlag', 'Payer Review Succeeded' ), 
( '12', 'acAuditResultFlag', 'Payer Review Rejected' ), 
( '13', 'acAuditResultFlag', 'Payer Appeal Succeeded' ), 
( '14', 'acAuditResultFlag', 'Payer Review/Patient Liability' ), 
( '15', 'acAuditResultFlag', 'Payer Appeal Rejected' ), 
( '16', 'acAuditResultFlag', 'Payer Review/Other Payer Liability' ), 
( '17', 'acAuditResultFlag', 'Payer Appeal/Patient Liability' ), 
( '19', 'acAuditResultFlag', 'Payer Appeal/Other Payer Liability' ), 
( '8', 'acAuditResultFlag', 'Referral - External Collections' ), 
( '9', 'acAuditResultFlag', 'Referral - Internal' )


DROP TABLE IF EXISTS [dbo].[tblPaymentDetail]
GO
CREATE TABLE [dbo].[tblPaymentDetail]
(
	[IncrementId] [INT] NOT NULL IDENTITY (1,1),
	[Entity] [VARCHAR](20),
	[IncrementName] [VARCHAR](20),
	[DateAdded] [DATETIME],
	[PostingDate] [DATETIME],
	[Amount] [MONEY],
	[ExcludedAmount] [MONEY],
	[AdjustCode] [VARCHAR](50),
	[AdjustCodeDesc] [VARCHAR](255),
	[SumValue] [TINYINT],
	[BatchCode] [VARCHAR](50),
	[ClaimNumber] [VARCHAR](50),
	[CheckNumber] [VARCHAR](30),
	[PayerCodeLink] [INT],
	[ContractID] [INT],
	[AdjustCodeLink] [INT],
	[AdjustType] [INT],
	[InterimBillStartDate] [DATETIME],
	[InterimBillEndDate] [DATETIME],
	[PayerType] [INT],
	[PostedBy] [VARCHAR](100),
	[AdjustmentServiceDate] [DATE],
	[PatientID] [INT]
) ON [PRIMARY]
GO
INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Payer 1', N'Contractual', CAST(N'2016-11-03T01:55:38.687' AS DateTime), CAST(N'2016-11-01T00:00:00.000' AS DateTime), 646.7500, 0.0000, N'6010', N'CONTRACTUAL DISCOUNT (INSURANCE)', 1, N'3077', N'', NULL, 77, 491, 16559, 12, NULL, NULL, 11, NULL, NULL, 5279364)
GO
INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Payer 1', N'Contractual', CAST(N'2016-11-25T01:29:43.673' AS DateTime), CAST(N'2016-11-23T00:00:00.000' AS DateTime), -646.7500, 0.0000, N'6010', N'CONTRACTUAL DISCOUNT (INSURANCE)', 1, N'3099', N'', NULL, 77, 758, 16559, 12, NULL, NULL, 11, NULL, NULL, 5279364)
GO
INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Payer 1', N'Contractual', CAST(N'2016-11-25T01:29:43.707' AS DateTime), CAST(N'2016-11-23T00:00:00.000' AS DateTime), 646.7500, 0.0000, N'6010', N'CONTRACTUAL DISCOUNT (INSURANCE)', 1, N'3099', N'', NULL, 77, 758, 16559, 12, NULL, NULL, 11, NULL, NULL, 5279364)
GO
INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Payer 1', N'Actual Payment', CAST(N'2016-11-25T01:29:43.597' AS DateTime), CAST(N'2016-11-23T00:00:00.000' AS DateTime), 348.2500, 0.0000, N'5013', N'INSURANCE PAYMENT (INSURANCE)', 1, N'3099', N'', NULL, 77, 758, 16536, 15, NULL, NULL, 11, NULL, NULL, 5279364)

-- some columns defined in tblColumnsCustomDetail is not available in tblWorklistData, 
-- set those columns invisible temporarily

UPDATE [dbo].[tblColumnsCustomDetail] SET [FieldVisible]=0 WHERE [CustomID]=1 AND [FieldName] IN ('Selected', 'Name_FederalTaxID', 'Payer1PendingRecoveryDiff', 'DateHidden')
GO

-- insert default layout record for user 2

INSERT [dbo].[tblColumnsCustom] ([UserID], [CustomName], [DefaultView], [LayoutDescription]) VALUES (2, 'default 2', 1, 'Default layout for user 2')
GO

INSERT [dbo].[tblColumnsCustomDetail] (CustomID, FieldName, FieldLocation, FieldWidth, FieldVisible) VALUES
( 2, 'ActualPatientID', 0, 100, 1 ), 
( 2, 'Name_PatientName', 1, 100, 1 ), 
( 2, 'PaymentStatus', 2, 100, 1 ), 
( 2, 'Name_ContractID', 3, 100, 1 ), 
( 2, 'PayerID', 4, 100, 1 ), 
( 2, 'ServiceCode', 5, 100, 1 ), 
( 2, 'AIScore', 6, 100, 1 ), 
( 2, 'AdmissionDate', 7, 100, 1 ), 
( 2, 'DischargeDate', 8, 100, 1 ), 
( 2, 'AdjTotalCharges', 9, 100, 1 ), 
( 2, 'ExpectedReimburse', 10, 100, 1 )
GO

-- insert another layout record for user 2
INSERT [dbo].[tblColumnsCustom] ([UserID], [CustomName], [DefaultView], [LayoutDescription]) VALUES (2, 'Option 1', 0, 'Optional layout for user 2')
GO

INSERT [dbo].[tblColumnsCustomDetail] (CustomID, FieldName, FieldLocation, FieldWidth, FieldVisible) VALUES
( 3, 'ActualPatientID', 0, 100, 0 ), 
( 3, 'Name_PatientName', 1, 100, 1 ), 
( 3, 'PaymentStatus', 2, 100, 1 ), 
( 3, 'Name_ContractID', 3, 100, 1 ), 
( 3, 'PayerID', 4, 100, 1 ), 
( 3, 'ServiceCode', 5, 100, 0 ), 
( 3, 'AIScore', 6, 100, 1 ), 
( 3, 'AdmissionDate', 7, 100, 1 ), 
( 3, 'DischargeDate', 8, 100, 0 ), 
( 3, 'AdjTotalCharges', 9, 100, 1 ), 
( 3, 'ExpectedReimburse', 10, 100, 1 )
GO

-- remove invalid column records
DELETE FROM [dbo].[tblColumns] WHERE [DataField] IN ('Selected', 'Count', 'RowNumber') OR [DataField] IS NULL
GO
DELETE FROM [dbo].[tblColumnsCustomDetail] WHERE [FieldName] IN ('Selected', 'Count', 'RowNumber', 'EmployerName', 'ExportFlag', 'FinancialClass3', 'FinancialClass4', 'InsuredName', 'MaxOut', 'Name_ContractID2', 'Name_ContractID3', 'Name_ContractID4', 'Name_FederalTaxID', 'Name_OriginalContractID', 'Payer1Closes', 'Payer1PendingRecoveryDiff', 'SupBillDate', 'SupBillFlag')
GO
