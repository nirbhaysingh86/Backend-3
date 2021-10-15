--	Data and schema corrections for the RemitHistory & RemitDetail tables:
DROP TABLE IF EXISTS [dbo].[RemitHistory]
GO

CREATE TABLE [dbo].[RemitHistory](
	[ID] [INT] NULL,
	[PatientID] [INT] NULL,
	[ActualPatientID] [VARCHAR](100) NULL,
	[ClaimNumber] [VARCHAR](100) NULL,
	[BillType] [VARCHAR](5) NULL,
	[ServiceCode] [VARCHAR](25) NULL,
	[ClaimFilingIndicatorID] [INT] NULL,
	[ClaimFilingIndicatorCode] [VARCHAR](10) NULL,
	[ClaimFilingIndicatorNotes] [VARCHAR](500) NULL,
	[ClaimFilingIndicatorTitle] [VARCHAR](100) NULL,
	[ContractCode] [VARCHAR](50) NULL,
	[StatementFromDate] [DATE] NULL,
	[StatementToDate] [DATE] NULL,
	[ClaimStatusID] [INT] NULL,
	[ClaimStatusCode] [VARCHAR](5) NULL,
	[ClaimStatusDescr] [VARCHAR](500) NULL,
	[ClaimPaymentGroupID] [INT] NULL,
	[PatientResponsibilityAmt] [DECIMAL](18, 2) NULL,
	[PatientPaidAmt] [DECIMAL](18, 2) NULL,
	[CoverageAmt] [DECIMAL](18, 2) NULL,
	[DiscountAmt] [DECIMAL](18, 2) NULL,
	[InterestAmt] [DECIMAL](18, 2) NULL,
	[ActualCoveredQty] [DECIMAL](18, 2) NULL,
	[ActualCoinsuredQty] [DECIMAL](18, 2) NULL,
	[EffectiveDate] [DATE] NULL,
	[ProductionDate] [DATE] NULL,
	[BatchCode] [VARCHAR](100) NULL,
	[CheckNumber] [VARCHAR](100) NULL,
	[CompanyID] [VARCHAR](50) NULL,
	[CompanySubID] [VARCHAR](50) NULL,
	[ReportingStatus] [VARCHAR](500) NULL,
	[ReceivedDate] [DATE] NULL,
	[Name] [VARCHAR](100) NULL,
	[PayeeID] [VARCHAR](100) NULL,
	[FederalTaxpayerID] [VARCHAR](100) NULL,
	[ImportResults_IsInPlay] [BIT] NULL,
	[ImportResults_ReasonNotInPlay] [VARCHAR](max) NULL,
	[ImportResults_PositionUsed] [INT] NULL,
	[ImportResults_LastUpdatedDate] [DATETIME] NULL,
	[PatientControlNumber] [VARCHAR](100) NULL,
	[ClaimFrequencyType] [VARCHAR](20) NULL,
	[AdjAmt] [MONEY] NULL,
	[DenialAmt] [MONEY] NULL,
	[AdjRsnCodes] [VARCHAR](20) NULL,
	[RemarkCodes] [VARCHAR](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[RemitHistory] ([ID], [PatientID], [ActualPatientID], [ClaimNumber], [BillType], [ServiceCode], [ClaimFilingIndicatorID], [ClaimFilingIndicatorCode], [ClaimFilingIndicatorNotes], [ClaimFilingIndicatorTitle], [ContractCode], [StatementFromDate], [StatementToDate], [ClaimStatusID], [ClaimStatusCode], [ClaimStatusDescr], [ClaimPaymentGroupID], [PatientResponsibilityAmt], [PatientPaidAmt], [CoverageAmt], [DiscountAmt], [InterestAmt], [ActualCoveredQty], [ActualCoinsuredQty], [EffectiveDate], [ProductionDate], [BatchCode], [CheckNumber], [CompanyID], [CompanySubID], [ReportingStatus], [ReceivedDate], [Name], [PayeeID], [FederalTaxpayerID], [ImportResults_IsInPlay], [ImportResults_ReasonNotInPlay], [ImportResults_PositionUsed], [ImportResults_LastUpdatedDate], [PatientControlNumber], [ClaimFrequencyType], [AdjAmt], [DenialAmt], [AdjRsnCodes], [RemarkCodes]) VALUES (1, 1, N'66106024', N'16307E12324', NULL, N'Outpatient', 10, N'HM', NULL, N'Health Maintenance Organization', NULL, NULL, NULL, 1, N'1', N'Processed as Primary', 19751249, NULL, NULL, CAST(348.25 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, CAST(N'2016-11-14' AS Date), CAST(N'2016-11-03' AS Date), NULL, N'081000605386671', N'1760486264', NULL, NULL, CAST(N'2016-11-03' AS Date), N'My Facility', NULL, N'123', 1, NULL, 1, CAST(N'2017-07-08T20:11:10.000' AS DateTime), N'6610602400', NULL, 0.0000, 0.0000, NULL, NULL)
GO

DROP TABLE IF EXISTS RemitDetail
GO

CREATE TABLE [dbo].[RemitDetail](
	[ID] [INT] NULL,
	[RemitID] [INT] NULL,
	[Seq] [INT] NULL,
	[LineNumber] [VARCHAR](100) NULL,
	[ServiceStartDate] [DATE] NULL,
	[ServiceEndDate] [DATE] NULL,
	[RevenueCode] [VARCHAR](50) NULL,
	[CPTCode] [VARCHAR](50) NULL,
	[CPT_Description] [VARCHAR](255) NULL,
	[OtherCode] [VARCHAR](50) NULL,
	[Qty] [DECIMAL](11, 3) NULL,
	[PaidQty] [DECIMAL](11, 3) NULL,
	[ActualAllowedAmt] [DECIMAL](18, 2) NULL,
	[DeductionAmt] [DECIMAL](18, 2) NULL,
	[CreatedDate] [DATETIME] NULL,
	[Charges] [MONEY] NULL,
	[AdjRsnCode] [VARCHAR](20) NULL,
	[RemarkCode] [VARCHAR](20) NULL,
	[DenialAmt] [MONEY] NULL
) ON [PRIMARY]
GO

INSERT [dbo].[RemitDetail] ([ID], [RemitID], [Seq], [LineNumber], [ServiceStartDate], [ServiceEndDate], [RevenueCode], [CPTCode], [CPT_Description], [OtherCode], [Qty], [PaidQty], [ActualAllowedAmt], [DeductionAmt], [CreatedDate], [Charges], [AdjRsnCode], [RemarkCode], [DenialAmt]) VALUES (1, 1, 1, N'001', CAST(N'2016-10-25' AS Date), CAST(N'2016-10-25' AS Date), N'0320', NULL, NULL, NULL, CAST(1.000 AS Decimal(11, 3)), CAST(1.000 AS Decimal(11, 3)), CAST(126.00 AS Decimal(18, 2)), NULL, CAST(N'2016-11-24T02:26:40.413' AS DateTime), 635.0000, N'45', NULL, 0.0000)
GO
INSERT [dbo].[RemitDetail] ([ID], [RemitID], [Seq], [LineNumber], [ServiceStartDate], [ServiceEndDate], [RevenueCode], [CPTCode], [CPT_Description], [OtherCode], [Qty], [PaidQty], [ActualAllowedAmt], [DeductionAmt], [CreatedDate], [Charges], [AdjRsnCode], [RemarkCode], [DenialAmt]) VALUES (2, 1, 2, N'002', CAST(N'2016-10-25' AS Date), CAST(N'2016-10-25' AS Date), N'0730', NULL, NULL, NULL, CAST(1.000 AS Decimal(11, 3)), CAST(1.000 AS Decimal(11, 3)), CAST(222.25 AS Decimal(18, 2)), NULL, CAST(N'2016-11-24T02:26:40.430' AS DateTime), 360.0000, N'45', NULL, 0.0000)
GO
