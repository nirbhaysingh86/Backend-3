--	Correcting table schema & default data for RevCPTDetail table.
DROP TABLE IF EXISTS RevCPTDetail
GO

CREATE TABLE RevCPTDetail ( [PatientRevenueId] int, [patientid] int, [RevenueCode] varchar(100), [RevCodeDescription] varchar(255), [RevCodeId] int, [RevCodeTransId] varchar(155), [RevServiceDate] datetime, [RevUnits] decimal(11,3), [RevCharges] money, [RevDeniedCharges] money, [RevExcludedCharges] money, [RevNonBilledCharges] money, [RevCost] money, [PatientCPTId] int, [CPTCode] varchar(9), [CPTDescr] varchar(255), [Mod1] varchar(10), [CPTRevCode] varchar(30), [ServiceDate] datetime, [Units] decimal(11,3), [Charges] money, [Rate] int, [ExpectedReimburse] int, [Service] int, [Method] int, [PPC] int, [Terms] int, [SurgOrder] int, [ExcludedCharges] money, [NonBilledCharges] money, [DeniedCharges] money, [Cost] money, [Mod2] varchar(10), [CPTTransId] varchar(155), [ServiceType] varchar(1), [PhysicianID] varchar(100), [PhysicianCodeLink] int, [ServiceLocation] varchar(10), [PrimaryDeniedCharges] money, [P1ActiveDenialReasonCodes] varchar(max), [P1ActiveDenialRemarkCodes] varchar(max), [BillingProviderNPI] varchar(10), [ICD9D] varchar(10), [ICD9D_2] varchar(10), [ICD9D_3] varchar(10), [ICD9D_4] varchar(10), [ICD9D_5] varchar(10), [ICDRevisionNumber] int )
INSERT INTO RevCPTDetail
VALUES
( 8141768, 1, '0320', 'Radiology - Diagnostic', 16145, '032071020PDH20161025', N'2016-10-25T00:00:00', 1.000, 360.0000, 0.0000, 0.0000, 0.0000, 0.0000, 9470504, '71020', 'Chest x-ray 2vw frontal&latl', 'PD', '0320', N'2016-10-25T00:00:00', 1.000, 360.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, 0.0000, NULL, '032071020PDH20161025', 'H', '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 8141769, 1, '0730', 'EKG/ECG', 16344, '073093005PDH20161025', N'2016-10-25T00:00:00', 1.000, 635.0000, 0.0000, 0.0000, 0.0000, 0.0000, 9470505, '93005', 'Electrocardiogram tracing', 'PD', '0730', N'2016-10-25T00:00:00', 1.000, 635.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, 0.0000, NULL, '073093005PDH20161025', 'H', '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL )
GO

--	Correction for data stored in tblDetailReimb table to follow the pattern being used in all other detail tables.
--	For this project any patient detail can be hard-coded to use PatientID = 1 or the data can be replicated to match the key our system uses with is for it to match tblWorklistData.PatientID
UPDATE	tblDetailReimb
SET		PatientID = 1
GO

--	Correction for tblICDCodeDetail schema to include the PatientID for correct lookups.
IF NOT EXISTS ( SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tblICDCodeDetail]') AND name = 'PatientID')
BEGIN
	ALTER TABLE tblICDCodeDetail ADD PatientID INT
END
GO

--	Adding default value of 1 which is being used for all detail data examples for this project.
UPDATE	tblICDCodeDetail
SET		PatientID = 1
GO

--	Corrections for ClaimHistory table to match UI requirements.
IF NOT EXISTS ( SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ClaimHistory]') AND name = 'PrimaryPayer')
BEGIN
	ALTER TABLE dbo.ClaimHistory ADD PrimaryPayer VARCHAR(100)
END
GO

IF NOT EXISTS ( SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ClaimHistory]') AND name = 'DestinationPayer')
BEGIN
	ALTER TABLE dbo.ClaimHistory ADD DestinationPayer VARCHAR(100)
END
GO

IF NOT EXISTS ( SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ClaimHistory]') AND name = 'DestinationPayerResp')
BEGIN
	ALTER TABLE dbo.ClaimHistory ADD DestinationPayerResp VARCHAR(100)
END
GO

IF NOT EXISTS ( SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ClaimHistory]') AND name = 'ClaimFreqType')
BEGIN
	ALTER TABLE dbo.ClaimHistory ADD ClaimFreqType VARCHAR(100)
END
GO

UPDATE	ClaimHistory
SET		PrimaryPayer = 'Payer Name',
		DestinationPayer = 'Other Payer Name',
		DestinationPayerResp = 'Primary',
		ClaimFreqType = 'Admit thru Discharge'
GO

--	Modification to ClaimDetail table to match UI requirements
IF NOT EXISTS ( SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ClaimDetail]') AND name = 'Modifier')
BEGIN
	ALTER TABLE dbo.ClaimDetail ADD Modifier VARCHAR(100)
END
GO

UPDATE	ClaimDetail
SET		Modifier = '00'
GO

--	Add missing field to detail table.
ALTER TABLE tblPaymentDetail ADD PatientID INT NULL
GO

--	Populate the missing data for the example account provided in initial database.
UPDATE	tblPaymentDetail
SET		PatientID = 1
FROM	tblPaymentDetail
WHERE	PatientID IS NULL
GO


--	Add missing system value entries to tblComboBoxesSystemValues table.
INSERT INTO tblComboBoxesSystemValues (Code, CodeType, Description)
VALUES
( '111', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Admit thru Discharge Claim' ), 
( '112', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Interim - First Claim' ), 
( '113', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Interim - Continuing Claim (Used by non-PPS acute care facilities)' ), 
( '114', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Interim - Last Claim (Used by non-PPS acute care facilities)' ), 
( '115', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Late Charge(s) Only Claim (Use for inpatient Part A bill for ancillary services for non-PPS facilities)' ), 
( '117', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Replacement of Prior Claim' ), 
( '121', 'BillType', 'Hospital, Inpatient (Medicare Part B only), Admit thru Discharge Claim' ), 
( '127', 'BillType', 'Hospital, Inpatient (Medicare Part B only), Replacement of Prior Claim' ), 
( '131', 'BillType', 'Hospital, Outpatient, Admit thru Discharge Claim' ), 
( '132', 'BillType', 'Hospital, Outpatient, Interim ?First Claim' ), 
( '133', 'BillType', 'Hospital, Outpatient, Interim ?Continuing Claims (Not Valid for PPS Bills)' ), 
( '134', 'BillType', 'Hospital, Outpatient, Interim ?Last Claim (Not Valid for PPS Bills)' ), 
( '135', 'BillType', 'Hospital, Outpatient, Late Charge(s) Only Claim' ), 
( '137', 'BillType', 'Hospital, Outpatient, Replacement of Prior Claim' ), 
( '138', 'BillType', 'Hospital, Outpatient, Void/Cancel of Prior Claim' ), 
( '141', 'BillType', 'Hospital, Other (for hospital referenced diagnostic services, or home health not under a plan of treatment), Admit thru Discharge Claim' ), 
( '147', 'BillType', 'Hospital, Other (for hospital referenced diagnostic services, or home health not under a plan of treatment), Replacement of Prior Claim' ), 
( '210', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Non-Payment/Zero Claim' ), 
( '211', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Admit thru Discharge Claim' ), 
( '212', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Interim - First Claim' ), 
( '214', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Interim - Last Claim' ), 
( '215', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Late Charge(s) Only Claim (Used for inpatient Part A bill for inpatient ancillary services)' ), 
( '217', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Replacement of Prior Claim' ), 
( '221', 'BillType', 'Skilled Nursing, Inpatient (Medicare Part B only), Admit thru Discharge Claim' ), 
( '721', 'BillType', 'Clinic, Hospital Based or Independent Renal Dialysis Center, Admit thru Discharge Claim' ), 
( '1959', 'Discharge', 'DC TO HOME OR SELF CARE (RO DISCHA)' ), 
( '17573', 'Discharge', 'System Added' ), 
( '1960', 'Discharge', 'DC/TRANS TO A SHORT-TERM HOSP INPT' ), 
( '1961', 'Discharge', 'DC/TRANS TO SNF WITH MCARE CERTIFIC' ), 
( '1962', 'Discharge', 'DC/TRANS TO AN INTERM CARE FAC(ICF)' ), 
( '1963', 'Discharge', 'DC/TRANS TO ANOTHER TYPE OF INSTITU' ), 
( '1964', 'Discharge', 'DC/TRANS TO HOME UNDER HOME HLTH OR' ), 
( '1965', 'Discharge', 'LEFT AGAINST MEDICAL ADVICE' ), 
( '1966', 'Discharge', 'UNDER CARE OF HOME IV PROVIDER' ), 
( '1967', 'Discharge', 'ADMITTED INPT - THIS HOSPITAL' ), 
( '1968', 'Discharge', 'NEONATE DISCHG - OTHER HOSP' ), 
( '1969', 'Discharge', 'PSYCH DISCHARGE - OTHER HOSP' ), 
( '1970', 'Discharge', 'ICF - MENTAL RETARDATION' ), 
( '863783', 'Discharge', 'Coretest' ), 
( '863898', 'Discharge', 'Coretest' ), 
( '1971', 'Discharge', 'TERTIARY AFTERCARE HOSPITAL' ), 
( '1972', 'Discharge', 'DOMICILIARY CARE FAC ADMISSION' ), 
( '1973', 'Discharge', 'TERMINATED CARE - ELIGIBIILITY' ), 
( '1974', 'Discharge', 'EXPIRED' ), 
( '1975', 'Discharge', 'DISCH/TRANS COURT/LAW ENFORCEMENT' ), 
( '17420', 'Discharge', 'All Other Transfer' ), 
( '17421', 'Discharge', 'Death' ), 
( '17422', 'Discharge', 'Death' ), 
( '17423', 'Discharge', 'Death' ), 
( '17424', 'Discharge', 'Death' ), 
( '17425', 'Discharge', 'Death' ), 
( '17426', 'Discharge', 'Death' ), 
( '17427', 'Discharge', 'Death' ), 
( '1976', 'Discharge', 'STILL PATIENT RETURN FOR OP SERVICE' ), 
( '1977', 'Discharge', 'EXPRD HOME - MCARE HOSPICE' ), 
( '1978', 'Discharge', 'EXPRD MED FAC - MCARE HOSPICE' ), 
( '1979', 'Discharge', 'EXPRD PLCE UNK - MCARE HOSPICE' ), 
( '1980', 'Discharge', 'DC/TRANS TO A FED HEALTH CARE FACIL' ), 
( '1981', 'Discharge', 'HOSPICE HOME DISCHG' ), 
( '1982', 'Discharge', 'HOSPICE MEDICAL FACILITY' ), 
( '1983', 'Discharge', 'DC/TRANS WITHIN THIS INST TO SWG BD' ), 
( '1984', 'Discharge', 'DC/TRANS TO AN INPATIENT REHAB FACI' ), 
( '1985', 'Discharge', 'DC/TRANS TO LONG TERM CARE HOSPITAL' ), 
( '17176', 'Discharge', 'System Added' ), 
( '1986', 'Discharge', 'DC/TRANS TO A PSYCHIATRIC HOSPITAL' ), 
( '1987', 'Discharge', 'CRITICAL ACCESS HOSP' ), 
( '1988', 'Discharge', 'OTHER FACILITY DISCHG/TRANSFER' ), 
( '1989', 'Discharge', 'OUTPT SERVICES REFERRAL - GFH' ), 
( '16963', 'Discharge', 'Unknown' ), 
( '16964', 'Discharge', 'Unknown' ), 
( '16965', 'Discharge', 'Unknown' ), 
( '16966', 'Discharge', 'Unknown' ), 
( '1990', 'Discharge', 'CARE  PLAN COMPLETED - MCAID OUTPT' ), 
( '1991', 'Discharge', 'PRE-ADMISSION - MCAID OUTPT' ), 
( '16967', 'Discharge', 'Unknown' ), 
( '863378', 'Discharge', 'Unknown' ), 
( '1', 'FinClass', 'Commercial' ), 
( '10', 'FinClass', 'Blue Shield' ), 
( '100', 'FinClass', 'Blue Cross Blue Shield' ), 
( '101', 'FinClass', 'United Healthcare' ), 
( '102', 'FinClass', 'Aetna' ), 
( '103', 'FinClass', 'Cigna' ), 
( '104', 'FinClass', 'Private Healthcare Systems' ), 
( '105', 'FinClass', 'Humana' ), 
( '106', 'FinClass', 'Texas Children''s Health Plan' ), 
( '107', 'FinClass', 'Medicaid Managed Care' ), 
( '108', 'FinClass', 'International' ), 
( '109', 'FinClass', 'CHIP' ), 
( '11', 'FinClass', 'Medigap' ), 
( '110', 'FinClass', 'MEDICAID PENDING' ), 
( '12', 'FinClass', 'Other' ), 
( '123', 'FinClass', 'coretest' ), 
( '12345', 'FinClass', 'coretestJan2018' ), 
( '2', 'FinClass', 'Medicare' ), 
( '206', 'FinClass', 'TCHP Member' ), 
( '207', 'FinClass', 'OON Medicaid or CHIP' ), 
( '210', 'FinClass', 'Medicaid or CHIP Pending' ), 
( '3', 'FinClass', 'Medicaid' ), 
( '4', 'FinClass', 'Self-pay' ), 
( '5', 'FinClass', 'Worker''s Comp' ), 
( '6', 'FinClass', 'Tricare' ), 
( '7', 'FinClass', 'Champva' ), 
( '8', 'FinClass', 'Group Health Plan' ), 
( '9', 'FinClass', 'FECA Black Lung' ), 
( '16876', 'PatType', 'INPATIENT' ), 
( '16871', 'PatType', 'OUTPATIENT' ), 
( '16872', 'PatType', 'EMERGENCY' ), 
( '16875', 'PatType', 'OBSERVATION' ), 
( '16894', 'PatType', 'Recurring' ), 
( '16895', 'PatType', 'HOSPITAL AMBULATORY SURGERY INACTIVE' ), 
( '16896', 'PatType', 'NEWBORN INACTIVE' ), 
( '16873', 'PatType', 'SPECIMEN' ), 
( '16897', 'PatType', 'BOARDER BABY INACTIVE' ), 
( '16898', 'PatType', 'DECEASED - ORGAN DONOR INACTIVE' ), 
( '16882', 'PatType', 'Hospital, Inpatient (Including Medicare Part A), Interim - First Claim' ), 
( '16874', 'PatType', 'DAY SURGERY' ), 
( '16899', 'PatType', 'HOME HEALTH CARE INACTIVE' ), 
( '16883', 'PatType', 'Hospital, Inpatient (Including Medicare Part A), Late Charge(s) Only Claim (Use for inpatient Part A bill for ancillary services for non-PPS facilities)' ), 
( '16900', 'PatType', 'INSTITUTIONAL INACTIVE' ), 
( '16901', 'PatType', 'OUTPATIENT IN A BED INACTIVE' ), 
( '16902', 'PatType', 'DEFAULT INACTIVE' ), 
( '16903', 'PatType', 'DENTAL INACTIVE' ), 
( '16904', 'PatType', 'UNIDENTIFIED INACTIVE' ), 
( '16905', 'PatType', 'Ambulance Inactive' ), 
( '16906', 'PatType', 'LEARNING SUPPORT CLINIC INACTIVE' ), 
( '16907', 'PatType', 'SURGERY ADMIT' ), 
( '16877', 'PatType', 'INPATIENT REHAB' ), 
( '16908', 'PatType', 'FERTILITY' ), 
( '863794', 'PatType', 'coretest' ), 
( '175151', 'PatType', 'System Added' ), 
( '16884', 'PatType', 'Day Surgery' ), 
( '16885', 'PatType', 'Emergency' ), 
( '16886', 'PatType', 'Automatically added from ADT data.' ), 
( '16887', 'PatType', 'Surgery Admit' ), 
( '16888', 'PatType', 'Inpatient Rehab' ), 
( '16889', 'PatType', 'Inpatient' ), 
( '16890', 'PatType', 'Outpatient' ), 
( '16909', 'PatType', 'Specimen' ), 
( '16891', 'PatType', 'Renal Series' ), 
( '16892', 'PatType', 'Series' ), 
( '16893', 'PatType', 'Observation' ),
( '0', 'AuditFlagAll', 'Pending' ), 
( '4', 'AuditFlagAll', 'Internal Review' ), 
( '6', 'AuditFlagAll', 'Reopen' ), 
( '1', 'AuditFlagAll', 'Payer Review Requested' ), 
( '11', 'AuditFlagAll', 'Payer Review/Payment Approved' ), 
( '12', 'AuditFlagAll', 'Payer Review Reconsideration Requested' ), 
( '16', 'AuditFlagAll', 'Payer Review Reopen' ), 
( '20', 'AuditFlagAll', 'Payer Appeal Requested' ), 
( '21', 'AuditFlagAll', 'Payer Appeal/Payment Approved' ), 
( '22', 'AuditFlagAll', 'Payer Appeal Reconsideration Requested' ), 
( '26', 'AuditFlagAll', 'Payer Appeal Reopen' ), 
( '30', 'AuditFlagAll', 'Referral' ), 
( '3', 'AuditFlagAll', 'Closed Pending' ), 
( '2', 'AuditFlagAll', 'Closed' ), 
( '0', 'ContractState', 'Not Started' ), 
( '1', 'ContractState', 'Not Audited' ), 
( '6', 'ContractState', 'Terminated' ), 
( '7', 'ContractState', 'Loading' ), 
( '9', 'ContractState', 'Production' ), 
( '11', 'ContractState', 'Loading Complete' ), 
( '14', 'ContractState', 'Validation In Process' ), 
( '15', 'ContractState', 'Validation Complete' ), 
( '23', 'ContractState', 'Loading Requested' ), 
( '25', 'ContractState', 'On Hold' ), 
( '0', 'PaymentStatus', 'Unknown' ), 
( '1', 'PaymentStatus', 'Awaiting Payment' ), 
( '2', 'PaymentStatus', 'Bill Needed' ), 
( '11', 'PaymentStatus', 'Paid as Expected' ), 
( '12', 'PaymentStatus', 'Non-Covered' ), 
( '13', 'PaymentStatus', 'Overpaid' ), 
( '14', 'PaymentStatus', 'Underpaid' ), 
( '21', 'PaymentStatus', 'Denied/Zero Paid' ), 
( '22', 'PaymentStatus', 'Denied/Underpaid' ), 
( 'Inpatient', 'ServiceCode', 'Inpatient' ), 
( 'Outpatient', 'ServiceCode', 'Outpatient' ), 
( 'Physician', 'ServiceCode', 'Physician' ), 
( '0', 'Status', 'Capitation' ), 
( '1', 'Status', 'New/Modified Calculated Record' ), 
( '3', 'Status', 'Not Calculated' ), 
( '4', 'Status', 'Incalculable Due to Missing Data' ), 
( '0', 'YesNo', 'No' ), 
( '1', 'YesNo', 'Yes' )
GO

-- audits table
DROP TABLE IF EXISTS [dbo].[tblAudits]
GO

CREATE TABLE [dbo].[tblAudits]
(
[AuditCounter] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[OldValue]  varchar(MAX) NULL,
[NewValue]  varchar(MAX) NULL,
[OperationType] [varchar](10) NOT NULL,
[ObjectType] [varchar](20) NOT NULL,
[Timestamp] [datetime] NOT NULL
) ON [PRIMARY]
GO


