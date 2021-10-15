DROP TABLE IF EXISTS [dbo].[ChargeCodeDetails]
GO

CREATE TABLE [dbo].[ChargeCodeDetails]
(
	[Id] [INT] NOT NULL IDENTITY (1,1),
	[ActualPatientID] [VARCHAR](30) NULL,
	[Code] [VARCHAR](30) NULL,
	[CodeId] [INT] NULL,
	[Description] [VARCHAR](100) NULL,
	[charges] [MONEY] NULL,
	[Cost] [MONEY] NULL,
	[DeniedCharges] [MONEY] NULL,
	[ExcludedCharges] [MONEY] NULL,
	[FacilityID] [VARCHAR](30) NULL,
	[PatientID] [INT] NULL,
	[Units] [INT] NULL,
	[CareType] [TINYINT] NULL,
	[CPT] [VARCHAR](10) NULL,
	[DeptCode] [VARCHAR](30) NULL,
	[DeptID] [INT] NULL,
	[RevenueCode] [VARCHAR](10) NULL,
	[RevenueID] [INT] NULL,
	[ServiceDate] [DATETIME] NULL,
	[TransactionID] [VARCHAR](50) NULL,
	[NonBillCharges] [MONEY] NULL,
	[DeniedUnits] [INT] NULL,
	[PostingDate] [DATETIME] NULL,
	[PostedBy] [VARCHAR](100) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChargeCodeDetails] ADD CONSTRAINT [PK__chargeCode__EC6CB456346CDFD1] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR=85) ON [PRIMARY]
GO

INSERT [dbo].[ChargeCodeDetails] ([ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (N'66106024', N'2824320', 4299, N'TC ELECTROCARDIOGRAM-EKG, TRACING', 0.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 0, 0, N'93005', N'2282', 17031, N'0730', 16344, CAST(N'2016-10-27T00:00:00.000' AS DateTime), N'282432020161027', 0.0000, 0, NULL, NULL)
GO
INSERT [dbo].[ChargeCodeDetails] ([ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (N'66106024', N'2824320', 4299, N'TC ELECTROCARDIOGRAM-EKG, TRACING', 0.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 0, 0, N'93005', N'2282', 17031, N'0730', 16344, CAST(N'2016-10-31T00:00:00.000' AS DateTime), N'282432020161031', 0.0000, 0, NULL, NULL)
GO
INSERT [dbo].[ChargeCodeDetails] ([ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (N'66106024', N'2824320', 4299, N'TC ELECTROCARDIOGRAM-EKG, TRACING', 635.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 1, 0, N'93005', N'2282', 17031, N'0730', 16344, CAST(N'2016-10-25T00:00:00.000' AS DateTime), N'282432020161025', 0.0000, 0, NULL, NULL)
GO
INSERT [dbo].[ChargeCodeDetails] ([ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (N'66106024', N'4230000', 7812, N'TC CHEST, 2 VIEWS.', 0.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 0, 0, N'71020', N'2523', 17059, N'0320', 16145, CAST(N'2016-10-27T00:00:00.000' AS DateTime), N'423000020161027', 0.0000, 0, NULL, NULL)
GO
INSERT [dbo].[ChargeCodeDetails] ([ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (N'66106024', N'4230000', 7812, N'TC CHEST, 2 VIEWS.', 0.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 0, 0, N'71020', N'2523', 17059, N'0320', 16145, CAST(N'2016-10-31T00:00:00.000' AS DateTime), N'423000020161031', 0.0000, 0, NULL, NULL)
GO
INSERT [dbo].[ChargeCodeDetails] ([ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (N'66106024', N'4230000', 7812, N'TC CHEST, 2 VIEWS.', 360.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 1, 0, N'71020', N'2523', 17059, N'0320', 16145, CAST(N'2016-10-25T00:00:00.000' AS DateTime), N'423000020161025', 0.0000, 0, NULL, NULL)
GO

UPDATE [dbo].[RevCPTDetail] SET [patientid]=66106024 WHERE patientid=1
