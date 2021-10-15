--	Create table that stores user information.
DROP TABLE IF EXISTS [dbo].[tblUsers];

CREATE TABLE [dbo].[tblUsers]
(
[UserCounter] [int] NOT NULL IDENTITY(1, 1),
[UserStamp] [timestamp] NULL,
[UserID] [int] NULL CONSTRAINT [tblUsers_DF001] DEFAULT (0),
[UserName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UserPassword] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProductFlag] [tinyint] NULL CONSTRAINT [tblUsers_DF002] DEFAULT (0),
[EMail] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UserType] [tinyint] NULL CONSTRAINT [tblUsers_DF003] DEFAULT (0),
[ProviderFlag] [tinyint] NULL CONSTRAINT [tblUsers_DF004] DEFAULT (0),
[Supervisor] [int] NULL CONSTRAINT [tblUsers_DF005] DEFAULT (0),
[UserOrder] [int] NULL CONSTRAINT [tblUsers_DF006] DEFAULT (1),
[LockFlag] [tinyint] NULL CONSTRAINT [tblUsers_DF007] DEFAULT (0),
[PasswordExpirationDate] [datetime] NULL,
[SchFileType] [varchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblUsers_DF009] DEFAULT ('zip'),
[SchZipFlag] [tinyint] NULL CONSTRAINT [tblUsers_DF008] DEFAULT (0),
[ActiveFlag] [tinyint] NULL CONSTRAINT [tblUsers_DF010] DEFAULT (0),
[EMailFlag] [tinyint] NULL CONSTRAINT [tblUsers_DF011] DEFAULT (0),
[AdminAccount] [int] NOT NULL CONSTRAINT [DF__tblUsers_18BAA3BB] DEFAULT ((0)),
[FailedPasswordAttemptCount] [int] NOT NULL CONSTRAINT [DF__tblUsers__Failed__22440DF5] DEFAULT ((0)),
[IsOnline] [bit] NOT NULL CONSTRAINT [DF__tblUsers__IsOnli__2338322E] DEFAULT ((0)),
[PhoneNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Notes] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Company] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Role] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OTP] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OTPExpirationDate] [datetime] NULL
) ON [PRIMARY];

CREATE CLUSTERED INDEX [Idx_UserName] ON [dbo].[tblUsers] ([UserName], [UserPassword]) WITH (FILLFACTOR=85) ON [PRIMARY];
ALTER TABLE [dbo].[tblUsers] ADD CONSTRAINT [tblUsers_PK1] PRIMARY KEY NONCLUSTERED ([UserCounter]) WITH (FILLFACTOR=85) ON [PRIMARY];
CREATE NONCLUSTERED INDEX [idx_UserID] ON [dbo].[tblUsers] ([UserID]) WITH (FILLFACTOR=85) ON [PRIMARY];
CREATE NONCLUSTERED INDEX [idx_UserID_Password] ON [dbo].[tblUsers] ([UserID], [UserPassword]) WITH (FILLFACTOR=85) ON [PRIMARY];

--Create an example user record
INSERT INTO tblUsers(UserID, UserName, UserPassword, ProductFlag, EMail, UserType, ProviderFlag, Supervisor, UserOrder, LockFlag, PasswordExpirationDate, SchFileType, SchZipFlag, ActiveFlag, EMailFlag, AdminAccount, FailedPasswordAttemptCount, IsOnline, PhoneNumber, Notes, Company, Role, Title, OTP, OTPExpirationDate)
VALUES ( 1, 'Test Username', '809711511511911111410049505152', 0, NULL, 1, 0, 0, 1, 0, N'2030-12-02T11:00:21.923', 'zip', 0, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL );

INSERT INTO tblUsers(UserID, UserName, UserPassword, Role, ProductFlag, EMail, UserType, ProviderFlag, Supervisor, UserOrder, LockFlag, PasswordExpirationDate, SchFileType, SchZipFlag, ActiveFlag, EMailFlag, AdminAccount, FailedPasswordAttemptCount, IsOnline, PhoneNumber, Notes, Company, Title, OTP, OTPExpirationDate)
VALUES (2, 'johndoe', '123456789', 'Site Admin', 0, 'john.doe@example.com', 1, 0, 0, 1, 0, N'2030-12-02T11:00:21.923', 'zip', 0, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL );

INSERT INTO tblUsers(UserID, UserName, UserPassword, Role, ProductFlag, EMail, UserType, ProviderFlag, Supervisor, UserOrder, LockFlag, PasswordExpirationDate, SchFileType, SchZipFlag, ActiveFlag, EMailFlag, AdminAccount, FailedPasswordAttemptCount, IsOnline, PhoneNumber, Notes, Company, Title, OTP, OTPExpirationDate)
VALUES (3, 'mike', '12345', 'Account Management', 0, 'mike@example.com', 1, 0, 0, 1, 0, N'2030-12-02T11:00:21.923', 'zip', 0, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL );

INSERT INTO tblUsers(UserID, UserName, UserPassword, Role, ProductFlag, EMail, UserType, ProviderFlag, Supervisor, UserOrder, LockFlag, PasswordExpirationDate, SchFileType, SchZipFlag, ActiveFlag, EMailFlag, AdminAccount, FailedPasswordAttemptCount, IsOnline, PhoneNumber, Notes, Company, Title, OTP, OTPExpirationDate)
VALUES (4, 'normal', '12345', 'Normal User', 0, 'normal@example.com', 1, 0, 0, 1, 0, N'2030-12-02T11:00:21.923', 'zip', 0, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL );

--	Table that stored combobox values
DROP TABLE IF EXISTS [dbo].[tblComboBoxesSystemValues];


CREATE TABLE [dbo].[tblComboBoxesSystemValues]
(
[CodeCounter] [int] NOT NULL IDENTITY(1, 1),
[Code] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CodeType] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RankOrder] [int] NULL
) ON [PRIMARY];
CREATE CLUSTERED INDEX [idx_CodeType] ON [dbo].[tblComboBoxesSystemValues] ([CodeType]) WITH (FILLFACTOR=85) ON [PRIMARY];
ALTER TABLE [dbo].[tblComboBoxesSystemValues] ADD CONSTRAINT [tblComboBoxesSystemValues_PK1] PRIMARY KEY NONCLUSTERED ([CodeCounter]) WITH (FILLFACTOR=85) ON [PRIMARY];
CREATE NONCLUSTERED INDEX [idx_CodeType_Rank] ON [dbo].[tblComboBoxesSystemValues] ([CodeType], [RankOrder]) WITH (FILLFACTOR=85) ON [PRIMARY];
CREATE NONCLUSTERED INDEX [idx_Code_Type] ON [dbo].[tblComboBoxesSystemValues] ([Code], [CodeType]) WITH (FILLFACTOR=85) ON [PRIMARY];


--	Default DDL values needed for View Admin.
INSERT INTO tblComboBoxesSystemValues(Code, CodeType, Description, RankOrder)
VALUES
( '0', 'AMAuditorType', 'All Records', 0 ), 
( '1', 'AMAuditorType', 'Assigned To Me', 1 ), 
( '2', 'AMAuditorType', 'Unassigned', 2 ), 
( '0', 'AMFollowUpType', 'All Records', 0 ), 
( '1', 'AMFollowUpType', 'Past Due', 1 ), 
( '2', 'AMFollowUpType', 'Due Today or Past Due', 2 ), 
( '3', 'AMFollowUpType', 'Due Within 1 Week', 3 ), 
( '4', 'AMFollowUpType', 'Due Within 1 Month', 4 ), 
( '0', 'AMRecordAgeType', 'All Records', 0 ), 
( '1', 'AMRecordAgeType', 'Within 1 Year', 1 ), 
( '2', 'AMRecordAgeType', 'Within 2 Years', 2 ), 
( '3', 'AMRecordAgeType', 'Within 3 Years', 3 ), 
( '4', 'AMRecordAgeType', 'Within 4 Years', 4 ), 
( '5', 'AMRecordAgeType', 'Within 5 Years', 5 ), 
( '0', 'AMRecordHiddenType', 'Exclude', 0 ), 
( '1', 'AMRecordHiddenType', 'Include', 1 ), 
( '0', 'AMStatusType', 'Exclude Closed', 0 ), 
( '1', 'AMStatusType', 'Include All', 1 ),
( '0', 'rpProviderFlag', 'All', 0 ), 
( '1', 'rpProviderFlag', 'Selected', 0 ), 
( '0', 'rpUserType', 'None', 0 ), 
( '1', 'rpUserType', 'Auditor/Analyst', 1 ),
( 'Automated Variance', 'CommunicationType', NULL, 0 ), 
( 'E-Mail', 'CommunicationType', NULL, 1 ), 
( 'Fax', 'CommunicationType', NULL, 2 ), 
( 'Phone Call', 'CommunicationType', NULL, 3 );

--	02) My Worklist 
DROP TABLE IF EXISTS [dbo].[tblColumns];
CREATE TABLE [dbo].[tblColumns]
(
[ColumnCounter] [int] NOT NULL IDENTITY(1, 1),
[ColumnStamp] [timestamp] NULL,
[AlternatingRows] [tinyint] NULL CONSTRAINT [tblColumns_DF001] DEFAULT (0),
[Aggregate] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AutoComplete] [tinyint] NULL CONSTRAINT [tblColumns_DF003] DEFAULT (0),
[AutoDropDown] [tinyint] NULL CONSTRAINT [tblColumns_DF004] DEFAULT (0),
[BackColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF005] DEFAULT ('Black'),
[Caption] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Caption2] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CaptionHeight] [int] NULL CONSTRAINT [tblColumns_DF006] DEFAULT (0),
[CellTips] [tinyint] NULL CONSTRAINT [tblColumns_DF007] DEFAULT (0),
[CellTipsDelay] [int] NULL CONSTRAINT [tblColumns_DF008] DEFAULT (0),
[CellTipsWidth] [int] NULL CONSTRAINT [tblColumns_DF009] DEFAULT (0),
[ColIndex] [int] NULL CONSTRAINT [tblColumns_DF010] DEFAULT (0),
[DataField] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DataWidth] [int] NULL CONSTRAINT [tblColumns_DF011] DEFAULT (0),
[DirectionAfterEnter] [tinyint] NULL CONSTRAINT [tblColumns_DF012] DEFAULT (0),
[EditorBackColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF013] DEFAULT ('Black'),
[EditorForeColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF014] DEFAULT ('Black'),
[EmptyRows] [tinyint] NULL CONSTRAINT [tblColumns_DF015] DEFAULT (0),
[EvenRowBackColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF016] DEFAULT ('Black'),
[EvenRowForeColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF017] DEFAULT ('Black'),
[ExtendRightColumn] [tinyint] NULL CONSTRAINT [tblColumns_DF018] DEFAULT (0),
[FetchStyle] [tinyint] NULL CONSTRAINT [tblColumns_DF019] DEFAULT (0),
[FilterOperator] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF046] DEFAULT ('='),
[ForeColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF020] DEFAULT ('Black'),
[HeadingStyle] [tinyint] NULL CONSTRAINT [tblColumns_DF021] DEFAULT (0),
[HeadingStyleBackColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF022] DEFAULT ('Black'),
[HeadingStyleForeColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF023] DEFAULT ('Black'),
[HorizontalAlignment] [tinyint] NULL CONSTRAINT [tblColumns_DF024] DEFAULT (0),
[MarqueeStyle] [tinyint] NULL CONSTRAINT [tblColumns_DF025] DEFAULT (0),
[MultiSelect] [tinyint] NULL CONSTRAINT [tblColumns_DF026] DEFAULT (0),
[NumberFormat] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ObjectName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ObjectType] [tinyint] NULL CONSTRAINT [tblColumns_DF027] DEFAULT (0),
[ObjectVersion] [int] NULL CONSTRAINT [tblColumns_DF028] DEFAULT (0),
[OddRowBackColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF029] DEFAULT ('Black'),
[OddRowForeColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF030] DEFAULT ('Black'),
[ProductFlag] [tinyint] NULL CONSTRAINT [tblColumns_DF031] DEFAULT (0),
[RecordSelectors] [tinyint] NULL CONSTRAINT [tblColumns_DF032] DEFAULT (0),
[RecordSelectorWidth] [int] NULL CONSTRAINT [tblColumns_DF033] DEFAULT (0),
[RowDividerColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF034] DEFAULT ('Black'),
[RowDividerStyle] [tinyint] NULL CONSTRAINT [tblColumns_DF035] DEFAULT (0),
[RowHeight] [int] NULL CONSTRAINT [tblColumns_DF036] DEFAULT (0),
[RowSubDividerColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF037] DEFAULT ('Black'),
[SelectedBackColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF038] DEFAULT ('Black'),
[SelectedForeColor] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [tblColumns_DF039] DEFAULT ('Black'),
[ShowFlag] [tinyint] NULL CONSTRAINT [tblColumns_DF040] DEFAULT (0),
[TabAction] [tinyint] NULL CONSTRAINT [tblColumns_DF041] DEFAULT (0),
[UserID] [int] NULL CONSTRAINT [tblColumns_DF042] DEFAULT (0),
[Visible] [tinyint] NULL CONSTRAINT [tblColumns_DF043] DEFAULT (0),
[Width] [int] NULL CONSTRAINT [tblColumns_DF044] DEFAULT (0),
[ColumnFooter] [tinyint] NULL CONSTRAINT [tblColumns_DF045] DEFAULT (0),
[FieldCategory] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ReadOnly] [int] NULL,
[UseForAMSummary] [int] NULL,
[VisibleBySystemFeature] [int] NULL,
[UnauthorizedPermission] [int] NULL
) ON [PRIMARY];
ALTER TABLE [dbo].[tblColumns] ADD CONSTRAINT [tblColumns_PK1] PRIMARY KEY CLUSTERED ([ColumnCounter]) WITH (FILLFACTOR=85) ON [PRIMARY];
CREATE NONCLUSTERED INDEX [idx_UserID_Form_Product] ON [dbo].[tblColumns] ([UserID], [ShowFlag], [ObjectName], [ObjectType], [ProductFlag]) WITH (FILLFACTOR=85) ON [PRIMARY];
CREATE NONCLUSTERED INDEX [ixNameTypeVer] ON [dbo].[tblColumns] ([ObjectName], [ObjectType], [ObjectVersion]) ON [PRIMARY];
INSERT INTO tblColumns(AlternatingRows, Aggregate, AutoComplete, AutoDropDown, BackColor, Caption, Caption2, CaptionHeight, CellTips, CellTipsDelay, CellTipsWidth, ColIndex, DataField, DataWidth, DirectionAfterEnter, EditorBackColor, EditorForeColor, EmptyRows, EvenRowBackColor, EvenRowForeColor, ExtendRightColumn, FetchStyle, FilterOperator, ForeColor, HeadingStyle, HeadingStyleBackColor, HeadingStyleForeColor, HorizontalAlignment, MarqueeStyle, MultiSelect, NumberFormat, ObjectName, ObjectType, ObjectVersion, OddRowBackColor, OddRowForeColor, ProductFlag, RecordSelectors, RecordSelectorWidth, RowDividerColor, RowDividerStyle, RowHeight, RowSubDividerColor, SelectedBackColor, SelectedForeColor, ShowFlag, TabAction, UserID, Visible, Width, ColumnFooter, FieldCategory, ReadOnly, UseForAMSummary, VisibleBySystemFeature, UnauthorizedPermission)
VALUES
( 0, NULL, 0, 0, 'LightSlateGray', NULL, NULL, 17, 2, 800, 0, 0, NULL, 0, 1, 'Lavender', 'Black', 0, 'LightBlue', 'Black', 1, 0, '=', 'Black', 0, 'LightSteelBlue', 'White', 1, 3, 0, NULL, 'fmAccountManagement', 0, 1, 'Window', 'Black', 0, 1, 17, 'DarkGray', 1, 15, 'DarkGray', 'LemonChiffon', 'Black', 0, 2, 0, 0, 0, 0, NULL, NULL, 0, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Select', '', 0, 0, 0, 0, 0, 'Selected', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, '', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 50, 0, 'System', NULL, 1, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Account Number', '', 0, 0, 0, 0, 0, 'ActualPatientID', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'String', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Billing Info', NULL, 0, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Patient Name', '', 0, 0, 0, 0, 0, 'Name_PatientName', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'String', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 150, 0, 'Demographics', NULL, 0, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Payer', 'Payment Status', 0, 0, 0, 0, 0, 'PaymentStatus', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'String', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Calculations', NULL, 1, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Contract Name', '', 0, 0, 0, 0, 0, 'Name_ContractID', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'String', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 200, 0, 'Billing Info', NULL, 1, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Payer ID', '', 0, 0, 0, 0, 0, 'PayerID', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'String', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Billing Info', NULL, 1, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Service', 'Category', 0, 0, 0, 0, 0, 'ServiceCode', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'String', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Billing Info', NULL, 1, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Admission Date', '', 0, 0, 0, 0, 0, 'AdmissionDate', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'MM/dd/yyyy', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Billing Info', NULL, 0, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Discharge Date', '', 0, 0, 0, 0, 0, 'DischargeDate', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'MM/dd/yyyy', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Billing Info', NULL, 0, NULL, NULL ), 
( 0, NULL, 0, 0, 'Black', 'Billing Date', '', 0, 0, 0, 0, 0, 'BillingDate', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 1, 0, 0, 'MM/dd/yyyy', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 0, 100, 0, 'Billing Info', NULL, 0, NULL, NULL ), 
( 0, 'Sum', 0, 0, 'Black', 'Adj Total', 'Charges', 0, 0, 0, 0, 0, 'AdjTotalCharges', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 0, 0, 0, 'Currency', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Billing Info', NULL, 1, NULL, NULL ), 
( 0, 'Sum', 0, 0, 'Black', 'Exp Reimburse', '', 0, 0, 0, 0, 0, 'ExpectedReimburse', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 0, 0, 0, 'Currency', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Calculations', NULL, 1, NULL, 318 ), 
( 0, 'Sum', 0, 0, 'Black', 'Payer', 'Est Payment', 0, 0, 0, 0, 0, 'EstPayerPortion', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 0, 0, 0, 'Currency', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 0, 100, 0, 'Payment/Adjustment', NULL, 1, NULL, 318 ), 
( 0, 'Sum', 0, 0, 'Black', 'Payer Payment', '', 0, 0, 0, 0, 0, 'PayerPayment', 0, 0, 'Black', 'Black', 0, 'Black', 'Black', 0, 0, '=', 'Black', 2, 'Black', 'White', 0, 0, 0, 'Currency', 'fmAccountManagement', 1, 1, 'Black', 'Black', 0, 0, 0, 'Black', 0, 0, 'Black', 'Black', 'Black', 0, 0, 0, 1, 100, 0, 'Payment/Adjustment', NULL, 1, NULL, NULL ); 

--	Create custom configuration table for system default values.
DROP TABLE IF EXISTS tblCustom_Configuration;

CREATE TABLE tblCustom_Configuration ( [AMDefaultAuditor] int, [AMDefaultFollowUp] int, [AMDefaultStatus] int, [AMDefaultRecordAge] int, [AMDefaultRecordHidden] int );
INSERT INTO tblCustom_Configuration
VALUES
( 0, 0, 1, 0, 1 );

DROP TABLE IF EXISTS [dbo].[tblViewFields];

CREATE TABLE [dbo].[tblViewFields]
(
[FieldID] [int] NOT NULL IDENTITY(1, 1),
[FieldName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Category] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[SelectionType] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ComboCodeType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SystemComboCodeType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SelectionQuery] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Calculation] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ExistsClause] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RuleModule] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LinkField] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DescField] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LinkCaption] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DescCaption] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HiddenLinkField] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ServiceCodeLink] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CPTLevelField] [int] NULL,
[IntegerFieldLink] [int] NOT NULL CONSTRAINT [DF__tblViewFi__Integ__29902A02] DEFAULT ((0)),
[UsedInQuickSearch] [int] NULL,
[DisableSetRange] [int] NULL,
[VisibleBySystemFeature] [int] NULL,
[UnauthorizedPermission] [int] NULL
) ON [PRIMARY];
ALTER TABLE [dbo].[tblViewFields] ADD CONSTRAINT [PK__tblViewF__C8B6FF2745976BD3] PRIMARY KEY CLUSTERED ([FieldID]) WITH (FILLFACTOR=85) ON [PRIMARY];

SET IDENTITY_INSERT [dbo].[tblViewFields] ON;
--	Add the data to the table
INSERT INTO tblViewFields(FieldID, FieldName, Category, Description, SelectionType, ComboCodeType, SystemComboCodeType, SelectionQuery, Calculation, ExistsClause, RuleModule, LinkField, DescField, LinkCaption, DescCaption, HiddenLinkField, ServiceCodeLink, CPTLevelField, IntegerFieldLink, UsedInQuickSearch, DisableSetRange, VisibleBySystemFeature, UnauthorizedPermission)
VALUES
( 1,'PaymentStatus', 'Calculations', 'Payer Payment Status', 'Values', NULL, 'PaymentStatus', NULL, NULL, NULL, 'CPRO Worklist', 'Code', 'Description', NULL, 'Value', NULL, NULL, 0, 1, 1, NULL, NULL, NULL ), 
(2, 'Name_ContractID', 'Billing Info', 'Contract Name', 'Values', NULL, NULL, 'SELECT ''FALSE'' as Selected, FacilityCounter, FacilityName, ContractID as ValueID, ContractName as Value FROM tblContracts JOIN tblFacility ON (tblFacility.FacilityID = tblContracts.FacilityID) WHERE Status IN (0, 1) ORDER BY FacilityName, ContractName', NULL, NULL, 'CPRO Worklist', NULL, NULL, 'Contract ID', 'Contract Name', NULL, NULL, 0, 1, 1, NULL, NULL, NULL ), 
(3, 'PayerCodeLink', 'Billing Info', 'Payer ID', 'Values', NULL, NULL, 'SELECT Selected = ''FALSE'', FacilityCounter = -1, FacilityName = ''<All Facilities>'', ValueID = PayerID, Value = PayerID, PayerName FROM tblPayers GROUP BY PayerID, PayerName ORDER BY PayerID, PayerName', NULL, 'EXISTS (SELECT 1 FROM dbo.tblPayers WHERE PayerCounter = vwWorklistCPRO.PayerCodeLink AND PayerID', 'CPRO Worklist', NULL, NULL, 'Payer ID', NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL ), 
(4, 'ServiceCode', 'Billing Info', 'Service Category', 'Values', NULL, 'ServiceCode', NULL, NULL, NULL, 'CPRO Worklist', 'Code', 'Description', NULL, 'Value', NULL, NULL, 0, 0, 1, NULL, NULL, NULL ), 
(5, 'AdmissionDate', 'Billing Info', 'Admission Date', 'DateRange', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, NULL ), 
(6, 'DischargeDate', 'Billing Info', 'Discharge Date', 'DateRange', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, NULL ), 
(7, 'BillingDate', 'Billing Info', 'Billing Date', 'DateRange', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, NULL ), 
(8, 'AdjTotalCharges', 'Billing Info', 'Adj Total Charges', 'Number', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, NULL ), 
(9, 'ExpectedReimburse', 'Calculations', 'Expected Reimbursement', 'Number', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, 318 ), 
(10, 'EstPayerPortion', 'Payment/Adjustment', 'Payer Est. Payment', 'Number', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 318 ), 
(11, 'PayerPayment', 'Payment/Adjustment', 'Payer Payment', 'Number', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, NULL ), 
(12, 'PayerPaymentDate', 'Payment/Adjustment', 'Payer Payment Date', 'DateRange', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, NULL ), 
(13, 'Payer1BalanceDue', 'Calculations', 'Payer Balance Due', 'Number', NULL, NULL, NULL, 'PM.EstPayerPortion - PM.PayerPayment - PM.PayerAdjust', NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, 318 ), 
(14, 'TotalBalanceDue', 'Calculations', 'Total Balance Due', 'Number', NULL, NULL, NULL, 'PM.EstPatientPortion + PM.EstPayerPortion + PM.Est2PayerPortion + PM.Est3PayerPortion + PM.Est4PayerPortion - (PM.PayerPayment - PM.Payer2Payment - PM.Payer3Payment - PM.Payer4Payment - PM.PatientPayment - PM.PayerAdjust - PM.Payer2Allowable - PM.Payer3Allowable - PM.Payer4Allowable - PM.PatientBadDebt)', NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 318 ),
(15, 'ContractAllowance', 'Calculations', 'Calculated Contractual', 'Number', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, 318 ), 
(16, 'HospitalContractual', 'Payment/Adjustment', 'Payer Contractual', 'Number', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL ), 
(17, 'ContractualVariance', 'Calculations', 'Contractual Variance', 'Number', NULL, NULL, NULL, 'PM.HospitalContractual - PM.ContractAllowance', NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, 318 ), 
(18, 'PatTypeCodeLink', 'Billing Info', 'Patient Type', 'Values', 'PatType', NULL, NULL, NULL, NULL, 'CPRO Worklist', 'ComboID', 'Description', NULL, 'Value', NULL, NULL, 0, 1, 1, NULL, NULL, NULL ), 
(19, 'CalcCode', 'Calculations', 'Calculation Code', 'Text', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL ),
(26, 'AdjTotalDays', 'Billing Info', 'LOS', 'Number', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL ), 
(27, 'Payer1AuditFlag', 'Audit Tracking', 'Payer 1 Audit Status', 'Values', NULL, 'AuditFlagAll', NULL, NULL, NULL, 'CPRO Worklist', 'Code', 'Description', NULL, 'Value', NULL, NULL, 0, 1, 1, 1, NULL, NULL ), 
(35, 'PctContractualToCharges', 'Calculations', '% Contractual to Charges', 'Percent', NULL, NULL, NULL, 'CASE PM2.TotalCharges WHEN 0 THEN Null ELSE Round((PM2.TotalCharges-PM.HospitalContractual)/PM2.TotalCharges,4) END', NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL ), 
(72, 'ImportDate', 'Calculations', 'Last Import Date', 'DateTimeRange', NULL, NULL, NULL, NULL, NULL, 'CPRO Worklist', NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 1, NULL, NULL, NULL ), 
(108, 'PatFinClass', 'Billing Info', 'Financial Class - Patient', 'Values', 'FinClass', NULL, NULL, NULL, NULL, 'CPRO Worklist', 'Code', 'Description', 'Code', 'Description', NULL, NULL, 0, 0, NULL, NULL, NULL, NULL ), 
(179, 'DischargeCodeLink', 'Billing Info', 'Discharge Status', 'Values', 'Discharge', NULL, NULL, NULL, NULL, 'CPRO Worklist', 'Code', '[Code] + '' - '' + [Description]', NULL, 'Value', 'ComboID', NULL, 0, 1, NULL, NULL, NULL, NULL ), 
(251, 'BillType', 'Billing Info', 'Last Bill Type', 'Values', 'BillType', NULL, NULL, NULL, NULL, 'CPRO Worklist', 'Code', 'Code', NULL, 'Value', NULL, NULL, 0, 0, NULL, NULL, NULL, NULL );

SET IDENTITY_INSERT [dbo].[tblViewFields] OFF;

--  Create table that holds example worklist records
DROP TABLE IF EXISTS [dbo].[tblWorklistData];

CREATE TABLE [dbo].[tblWorklistData]
(
[Marked] INT,
[InUseByUser] VARCHAR(255),
[PatientID] INT NOT NULL,
[ActualPatientID] VARCHAR(30),
[Name_PatientName] VARCHAR(100),
[PatientLast] VARCHAR(50),
[SSN] VARCHAR(11),
[PaymentStatus] TINYINT,
[Name_ContractID] VARCHAR(50),
[ContractID] INT,
[PayerID] VARCHAR(60),
[Payer2ID] VARCHAR(60),
[Name_Payer2ID] VARCHAR(150),
[Payer3ID] VARCHAR(60),
[Name_Payer3ID] VARCHAR(150),
[Payer4ID] VARCHAR(60),
[Name_Payer4ID] VARCHAR(150),
[ServiceCode] VARCHAR(25),
[AdmissionDate] DATETIME,
[DischargeDate] DATETIME,
[BillingDate] DATETIME,
[AdjTotalCharges] MONEY,
[ExpectedReimburse] MONEY,
[EstPayerPortion] MONEY,
[PayerPayment] MONEY,
[PayerPaymentDate] DATETIME,
[Payer1BalanceDue] MONEY,
[TotalBalanceDue] MONEY,
[ContractAllowance] MONEY,
[HospitalContractual] MONEY,
[ContractualVariance] MONEY,
[Name_PatTypeCodeLink] VARCHAR(255),
[PatTypeCodeLink] INT,
[CalcCode] VARCHAR(255),
[CalcCodeType] VARCHAR(20),
[CalcService] VARCHAR(50),
[CalcReim] VARCHAR(50),
[Outlier] VARCHAR(25),
[OutlierAmt] MONEY,
[AddReimburse] MONEY,
[Name_DischargeCodeLink] VARCHAR(255),
[DischargeCodeLink] INT,
[DischargeStatusCode] VARCHAR(100),
[CalcDRG] VARCHAR(10),
[DRG] VARCHAR(10),
[DRG2] VARCHAR(10),
[AdjTotalDays] SMALLINT,
[FacilityID] VARCHAR(30),
[FacilityName] VARCHAR(50),
[CalcPayerContractual] MONEY,
[ContractualAdjust] MONEY,
[PctContractualToCharges] MONEY,
[PctContractualToExpReim] MONEY,
[AllowableDiff] MONEY,
[BalanceDueP1P2] MONEY,
[Name_PhysicianName] VARCHAR(100),
[PhysicianID] VARCHAR(100),
[AttendingPhysicianNPI] VARCHAR(100),
[DRG4] VARCHAR(10),
[Status] TINYINT,
[Name_Status] VARCHAR(255),
[ClaimNumber] VARCHAR(50),
[Name_Contract2ID] VARCHAR(60),
[Contract2ID] INT,
[Contract3ID] INT,
[Contract4ID] INT,
[ContractualDiscount] MONEY,
[CoPay] DECIMAL(9, 4),
[Deductible] MONEY,
[DeniedCharges] MONEY,
[DeniedDays] SMALLINT,
[ZeroBalanceDate] DATE,
[DeniedReasonCodeLink] INT,
[DeniedChargesDate] DATETIME,
[DRG3] VARCHAR(10),
[ElapsedDays] INT,
[EstPatientPortion] MONEY,
[PatFinClass] VARCHAR(10),
[PatFinClassCodeLink] INT,
[Name_PatFinClass] VARCHAR(255),
[FinancialClass2] VARCHAR(10),
[Name_FinancialClass2] VARCHAR(255),
[ExportDate] DATETIME,
[ImportDate] DATETIME,
[InsuredGroupName] VARCHAR(100),
[InvoiceID] VARCHAR(25),
[MedicalRecordID] VARCHAR(30),
[NonBillCharges] MONEY,
[ExcludedCharges] MONEY,
[OriginalDRG] VARCHAR(10),
[NameOfOriginalContractID] VARCHAR(60),
[OriginalContractID] INT,
[OriginalPayerID] VARCHAR(60),
[PatientBalanceDue] MONEY,
[PatientPayment] MONEY,
[PatientPaymentDate] DATETIME,
[PatientBadDebt] MONEY,
[Zip] VARCHAR(20),
[PayerAllowable] MONEY,
[Name_PayerID] VARCHAR(150),
[ExportContractual] MONEY,
[PayerAdjust] MONEY,
[Payer2Allowable] MONEY,
[Payer3Allowable] MONEY,
[Payer4Allowable] MONEY,
[CalcDeniedReim] MONEY,
[FinancialClass] VARCHAR(10),
[FinClassCodeLink] INT,
[Name_FinancialClass] VARCHAR(255),
[Payer2BalanceDue] MONEY,
[Est2PayerPortion] MONEY,
[Payer2Payment] MONEY,
[Payer2PaymentDate] DATETIME,
[Est3PayerPortion] MONEY,
[Payer3Payment] MONEY,
[Payer3PaymentDate] DATETIME,
[Est4PayerPortion] MONEY,
[Payer4Payment] MONEY,
[Payer4PaymentDate] DATETIME,
[CPT] VARCHAR(10),
[ICD9_D] VARCHAR(10),
[ICD9] VARCHAR(10),
[PhysicianCharges] MONEY,
[DocReimburse] MONEY,
[SubscriberID] VARCHAR(50),
[TotalCharges] MONEY,
[TotalDays] SMALLINT,
[CalcPatientPortion] MONEY,
[PayerCodeLink] INT,
[MedicareRate] MONEY,
[MedicareHospCap] MONEY,
[MedicareDSH] MONEY,
[MedicareIME] MONEY,
[MedicareFedCap] MONEY,
[MedicareDSHCap] MONEY,
[Age] TINYINT,
[BusOfficePhone] VARCHAR(14),
[AddressBill] VARCHAR(80),
[CityBill] VARCHAR(30),
[StateBill] CHAR(2),
[ZipBill] VARCHAR(10),
[SupplimentalBillingReportID] INT,
[Payer1_Product] VARCHAR(25),
[Payer1_Network] VARCHAR(15),
[Payer1_Population] VARCHAR(50),
[Remarks] VARCHAR(2500),
[MedicareOutlierCap] MONEY,
[MedicareCase] TINYINT,
[MedicareIMECap] MONEY,
[Payer2CodeLink] INT,
[Payer3CodeLink] INT,
[Payer4CodeLink] INT,
[OrganizationType] VARCHAR(50),
[ContractType] VARCHAR(50),
[ContractStatus] TINYINT,
[ContractState] TINYINT,
[FinExportFlag] TINYINT,
[GroupCode2] VARCHAR(30),
[GroupCode2Desc] VARCHAR(255),
[GroupCode3] VARCHAR(30),
[GroupCode3Desc] VARCHAR(255),
[PayerDRG] VARCHAR(10),
[DeniedReason] VARCHAR(50),
[HiddenByUser] INT,
[Name_HiddenByUser] VARCHAR(50),
[AdmittingDiagnosisCode] VARCHAR(10),
[PayerPlanCode] VARCHAR(150),
[Payer2PlanCode] VARCHAR(150),
[Payer3PlanCode] VARCHAR(150),
[Payer4PlanCode] VARCHAR(150),
[PayerPlanName] VARCHAR(150),
[Payer2PlanName] VARCHAR(150),
[Payer3PlanName] VARCHAR(150),
[Payer4PlanName] VARCHAR(150),
[ExpectedReimburseCalcType] VARCHAR(20),
[AllowableVariance] MONEY,
[AdjustTypeUpdatedDate] DATETIME,
[CalcBillingDate] DATETIME,
[CalcPayerPaymentDate] DATETIME,
[HasDenials] TINYINT,
[HasActiveDenials] INT,
[InitialRemitEffectiveDate] DATE,
[CurrentRemitEffectiveDate] DATE,
[ContractualDate] DATETIME,
[TotalPayments] MONEY,
[TotalWriteOffs] MONEY,
[ClaimReceived] TINYINT,
[PctPaidToTotalCharges] MONEY,
[PctPaidToTotalChargesWithWO] MONEY,
[PctPaidToExpReim] MONEY,
[PctPaidToExpReimWithWO] MONEY,
[AdmissionSource] INT,
[AdmissionSourceCode] VARCHAR(100),
[DateOfDeath] DATETIME,
[PrimaryDeniedCharges] MONEY,
[SecondaryDeniedCharges] MONEY,
[TotalDeniedCharges] MONEY,
[OriginalPlanID] VARCHAR(30),
[DocumentRenderedDate] DATETIME,
[DocumentRenderedFlag] INT NOT NULL,
[LastAppliedAutoView] VARCHAR(100),
[LastAppliedAutoViewID] INT,
[NationalProviderID] VARCHAR(25),
[License] VARCHAR(25),
[MedicareWithhold] MONEY,
[EligibilityDate] DATETIME,
[BillType] VARCHAR(5),
[CalcEpisode] TINYINT,
[CustomServiceLocationID] INT,
[CustomServiceLocationCode] VARCHAR(278),
[RegistrationNote] VARCHAR(30),
[MasterPatientIdentifier] VARCHAR(15),
[DeniedPayerID] VARCHAR(60),
[FinancialStatusID] INT,
[FinancialStatusCode] VARCHAR(308),
[PrimaryServiceID] INT,
[PrimaryServiceCode] VARCHAR(268),
[RegistrarID] VARCHAR(5),
[RegistrarID2] VARCHAR(5),
[BillingConfirmationNumber] VARCHAR(20),
[BillingConfirmationDate] DATE,
[ReportedEstimatedPatientResponsibility] MONEY,
[ReportedPatientBalance] MONEY,
[Insured2GroupName] VARCHAR(100),
[Insured3GroupName] VARCHAR(100),
[ReportedRemitTotalCharges] MONEY,
[AdmissionType] INT,
[AdmissionTypeCode] VARCHAR(100),
[IncludedInFinExport] TINYINT,
[PatientRespCollected] MONEY,
[RACFollowupDate] DATETIME,
[EstimatedDenialRecovery] DECIMAL(30, 9),
[MomBabyID] VARCHAR(30),
[Cost] MONEY,
[DirectFixCost] MONEY,
[DirectVarCost] MONEY,
[IndirectFixCost] MONEY,
[IndirectVarCost] MONEY,
[BillingStatusID] INT,
[BirthDate] DATETIME,
[ClinicalCategoryID] INT,
[LastModTSCalc] DATETIME,
[LastModTSRecord] DATETIME,
[LastModID] INT,
[LastModUser] VARCHAR(50) NOT NULL,
[NumClaimLevelDenials] INT,
[NumClaims] INT,
[NumLineLevelDenials] INT,
[NumRemits] INT,
[PatientFileID] VARCHAR(30),
[TransplantPhases] INT,
[FederalTaxID] VARCHAR(15),
[CalculatedNonCoveredCharges] MONEY,
[DeptID] INT,
[DeptCode] VARCHAR(30),
[DeptCodeDesc] VARCHAR(255),
[PrimaryProjectedVarianceType] VARCHAR(255) NOT NULL,
[PrimaryProjectedVariance] VARCHAR(MAX) NOT NULL,
[PrimaryProjectedVarianceStatus] VARCHAR(9) NOT NULL,
[LastContactDate] DATETIME,
[HACRPAmount] DECIMAL(11, 4),
[LowVolumeAdjustment] DECIMAL(11, 4),
[MDHAddOnFactor] DECIMAL(11, 4),
[ReadmissionAmount] DECIMAL(11, 4),
[SCHAmount] DECIMAL(11, 4),
[SequestrationAmount] DECIMAL(11, 4),
[UncomCareAmount] DECIMAL(11, 4),
[VBPAmount] DECIMAL(11, 4),
[PrimaryCPTs] VARCHAR(100),
[PotentialReimDifference] MONEY,
[ContractAffiliationStatus] VARCHAR(17) NOT NULL,
[FacilityServiceID] INT,
[PrimaryCAReasonCode] VARCHAR(50),
[PrimaryCARemarkCode] VARCHAR(50),
[AgeOfClaim] INT,
[AgeOfFirstDenial] INT,
[AgeOfActiveDenial] INT,
[DischargeToPaymentDays] INT,
[BillToPaymentDays] INT,
[WorkedToPaymentDays] INT,
[LastPayerPaymentDate] DATETIME,
[OtherPayerTransactions] TINYINT,
[PaymentChange] MONEY,
[TotalPayerPayments] MONEY,
[ReportingServiceGroupID] INT,
[ReportingServiceGroupTitle] VARCHAR(100),
[ComparisonExpectedReimburse] MONEY,
[PctExpReimToCompareCalcExpReim] MONEY,
[PctTotalPmtsToCompareCalcExpReim] MONEY,
[NumClaimLevelInactiveDenials] INT,
[NumLineLevelInactiveDenials] INT,
[Payer2ExpectedReimburse] MONEY,
[Payer2ContractAllowance] MONEY,
[RecoveryTouches] INT,
[RecoveryTotalDuration] INT,
[Score] VARCHAR(1),
[WinRatio] NUMERIC(38, 6),
[WeightedRecoveryScore] NUMERIC(38, 6),
[CurrentRecoveryDays] INT,
[AvgRecoveryTouches] INT,
[AvgRecoveryEffort] INT,
[AvgRecoveryDays] INT,
[InsuredName] VARCHAR(100),
[ReportedCharges] MONEY,
[ReportedBalance] MONEY,
[ReportedExpectedReimbursement] MONEY,
[EAPGCalcCode] VARCHAR(700),
[EAPGReimbursement] MONEY,
[CalcContractIDHosp] INT,
[CalcContractNameHosp] VARCHAR(50),
[CalcContractIDPhys] INT,
[CalcContractNamePhys] VARCHAR(50),
[ActivityDate] DATETIME,
[AuditorID] INT,
[Name_AuditorID] VARCHAR(50) NOT NULL,
[ReviewerUserID] INT,
[ReviewerUserName] VARCHAR(50) NOT NULL,
[Payer1AuditFlag] INT NOT NULL,
[Payer1CalcEstRecovery] MONEY NOT NULL,
[RecoveryReim] MONEY NOT NULL,
[RecoveryCalcFlag] INT NOT NULL,
[Payer1EstRecovery] MONEY NOT NULL,
[Payer1StartDate] DATE,
[VarianceCategoryId] INT,
[Payer1ReviewCategory] INT,
[Payer1ReviewReason] INT,
[Payer1ReviewStage] INT,
[Payer1RecoveryCommittedAmount] MONEY,
[Payer1RecoveryPursuingVariance] MONEY,
[RecoveryArgument] VARCHAR(MAX),
[Payer1Reviews] BIT NOT NULL,
[Payer1ReviewDate] DATE,
[Payer1AuditDate] DATE,
[Payer1PendingRecovery] MONEY NOT NULL,
[Payer1AuditRecovery] MONEY NOT NULL,
[Payer1AppealDate] DATE,
[Payer1AppealRecovery] MONEY NOT NULL,
[Payer1Appeals] INT NOT NULL,
[Payer1Audits] INT NOT NULL,
[Payer1ReviewRecovery] MONEY NOT NULL,
[CollectionID] INT,
[CollectionCode] VARCHAR(100),
[CollectionAgency] VARCHAR(255),
[Payer1ReferralDate] DATE,
[Payer1PendingCollections] MONEY NOT NULL,
[Payer1Collections] MONEY NOT NULL,
[Payer1PendingCollectionsDiff] MONEY NOT NULL,
[Payer1CloseDate] DATE,
[Payer1AuditReason] INT,
[Payer1AutoCloseFlag] INT NOT NULL,
[Payer1ResultFlag] INT NOT NULL,
[RacStatusId] INT NOT NULL,
[rcyP2_AuditFlag] INT NOT NULL,
[rcyP2_EstRecoveryAmtSystemCalculated] MONEY NOT NULL,
[rcyP2_StartDate] DATE,
[rcyP2_CommittedAmt] MONEY,
[rcyP2_PursuingVarianceAmt] MONEY,
[rcyP2_InternalReviewStartDate] DATE,
[rcyP2_HasInternalReview] BIT NOT NULL,
[rcyP2_ReviewCategoryID] INT,
[rcyP2_ReviewReasonID] INT,
[rcyP2_VarianceCategoryID] INT,
[rcyP2_ReviewStageID] INT,
[rcyP2_ReviewStage] VARCHAR(255),
[rcyP2_AuditStartDate] DATE,
[rcyP2_AuditSubmittedRecoveryAmt] MONEY NOT NULL,
[rcyP2_AuditCollectedRecoveryAmt] MONEY NOT NULL,
[rcyP2_AuditUncollectedRecoveryAmt] MONEY NOT NULL,
[rcyP2_AppealStartDate] DATE,
[rcyP2_AppealCollectedRecoveryAmt] MONEY NOT NULL,
[rcyP2_AppealCount] INT NOT NULL,
[rcyP2_ReviewStartDate] DATE,
[rcyP2_ReviewCollectedRecoveryAmt] MONEY NOT NULL,
[rcyP2_ReviewCount] INT NOT NULL,
[rcyP2_ReferralAgencyID] INT,
[rcyP2_ReferralAgency_Title] VARCHAR(255),
[rcyP2_ReferralStartDate] DATE,
[rcyP2_ReferralCollectedRecoveryAmt] MONEY NOT NULL,
[rcyP2_ReferralSubmittedRecoveryAmt] MONEY NOT NULL,
[rcyP2_ReferralUncollectedRecoveryAmt] MONEY NOT NULL,
[rcyP2_CloseReasonID] INT,
[rcyP2_CloseDate] DATE,
[rcyP2_CloseResultFlag] INT NOT NULL,
[HISAuditor] VARCHAR(255),
[HISAuditDate] DATETIME,
[HISAuditFlag] VARCHAR(50),
[HISComments] VARCHAR(MAX),
[HISCommentDate] DATETIME,
[Ins2BillDate] DATETIME,
[CodedDate] DATETIME,
[InsPlan2PolicyNo] VARCHAR(100),
[InsPlan2GroupNo] VARCHAR(100),
[InsPlan3PolicyNo] VARCHAR(100),
[InsPlan3GroupNo] VARCHAR(100),
[InsPlan4PolicyNo] VARCHAR(100),
[InsPlan4GroupNo] VARCHAR(100),
[BadDebtTransferAmt] MONEY,
[BadDebtTransferDate] DATETIME,
[LastActivityDate] DATETIME,
[OutForCollectionsBalance] MONEY,
[CommunicationType] VARCHAR(255),
[PrimaryCADeniedRemarkCode] VARCHAR(50),
[AIScore] FLOAT
);

INSERT INTO tblWorklistData
VALUES
( 0, NULL, 3475203, '65245453', 'Patient1, Test', 'Patient1', '', 13, 'Contract Example 582', 582, '2000003', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2015-12-11T00:00:00', N'2016-05-09T00:00:00', N'2016-05-24T00:00:00', 1272868.7600, 814636.0100, 814336.0100, 817731.0700, N'2016-08-05T00:00:00', -3395.0600, -3395.0600, 300.0000, 454837.6800, 454537.6800, 'Inpatient', 16889, 'MS-660', 'DRG', 'Surgical', 'Straight Discount', 'None', 0.0000, 0.0000, 'Discharge code 1959', 1959, '01', 'MS-660', '660', '6313', 150, '1', 'My Facility', 454837.6800, 0.0000, 0.6426, 1.0041, -33002.8800, -3095.0600, 'Physician Name', '', NULL, 'MS-660', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.0002, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '6313', 59, 300.0000, '102', 16828, 'Patient FC 102', '', NULL, N'2020-03-18T16:57:04.19', N'2016-08-09T01:28:47.33', 'INS GRP I', '', '1', 0.0000, 0.0000, 'MS-660', 'Contract Example 205', 205, '2000003', 0.0000, 300.0000, N'2015-12-15T00:00:00', 0.0000, '12345', 847638.8900, 'Payer 2000003', 300.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Q61.4', '', 0.0000, 0.0000, 'SUBID 1', 1272868.7600, 150, 300.0000, 40, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Aetna', 'Managed Care', 0, 9, 1, NULL, NULL, NULL, NULL, '660', 'N/A', NULL, NULL, 'Q61.4', '20012900', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', 33002.8800, N'2016-07-06T11:32:19.857', N'2016-01-11T00:00:00', N'2016-03-10T00:00:00', 1, 0, N'2016-03-10T00:00:00', N'2016-08-05T00:00:00', N'2016-08-02T00:00:00', 818031.0700, 454837.6800, 1, 0.6426, 0.6426, 1.0041, 1.0041, 5, '5', NULL, 0.0000, 0.0000, 0.0000, '20012900', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '117', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 1, 300.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2015-12-11T00:00:00', NULL, N'2016-08-09T01:29:32.977', N'2020-03-18T16:57:04.19', 0, 'User 0', 0, 12, 0, 33, '1273052.76', NULL, '123', 0.0000, 0, NULL, NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1838, 1841, NULL, 88, 207, 119, N'2016-08-05T00:00:00', 0, NULL, 817731.0700, NULL, NULL, NULL, NULL, NULL, 0, 65, NULL, NULL, 0, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, 'Insured1, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 4, 0.0000, 814636.0100, 10, -3395.0600, N'2016-04-08T00:00:00', 11, NULL, NULL, NULL, NULL, -455090.4000, NULL, 1, N'2016-04-08T00:00:00', NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 3872107, '65524906', 'Patient2, Test', 'Patient2', '', 11, 'Contract Example 773', 773, '2000007', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-03-25T00:00:00', N'2017-04-19T00:00:00', N'2017-04-28T00:00:00', 15199611.0900, 3141495.7900, 3141495.7900, 407751.1900, N'2017-06-09T00:00:00', 2733744.6000, 2733744.6000, 12058115.3000, 12058115.3100, 0.0100, '(none)', 0, '5834', 'DRG', 'All Services', 'Relative Weights', 'High', 1494445.5400, 0.0000, 'Discharge code 1974', 1974, '20', '5834', '003', '5834', 390, '1', 'My Facility', 12058115.3100, 0.0000, 0.2066, 0.9999, 2733744.6000, 2733744.6000, 'Physician Name', '', NULL, '5834', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7933, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '5834', 52, 0.0000, '106', 16832, 'Patient FC 106', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '2', 0.0000, 0.0000, '5834', 'Contract Example 491', 491, '2000007', 0.0000, 0.0000, NULL, 0.0000, '12345', 407751.1900, 'Payer 2000007', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.01', '5A15223', 0.0000, 0.0000, 'SUBID 2', 15199611.0900, 390, NULL, 77, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Managed Care Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '003', 'N/A', NULL, NULL, 'Z38.01', '20019300', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -2733744.6000, NULL, N'2016-04-25T00:00:00', N'2016-06-16T00:00:00', 0, 1, N'2016-06-14T00:00:00', N'2017-04-27T00:00:00', N'2017-04-28T00:00:00', 407751.1900, 12058115.3100, 1, 0.0268, 0.0268, 0.1297, 0.1297, 5, '5', NULL, 5827723.9900, 0.0000, 5827723.9900, '20019300', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '117', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 0, 0.0000, NULL, 1656355.712437800, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-03-25T00:00:00', NULL, N'2018-03-01T21:32:58.223', N'2018-03-01T21:32:58.223', 51, 'User 51', 0, 13, 83, 12, '15199611.10', NULL, '123', 0.0000, 0, NULL, NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, '31', NULL, 1493, 1764, 1485, 51, 410, NULL, N'2017-06-09T00:00:00', 0, NULL, 407751.1900, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured2, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4270876, '65699429', 'Patient3, Test', 'Patient3', '', 21, 'Contract Example 773', 773, '2000007', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-05-31T00:00:00', N'2017-04-13T00:00:00', N'2017-05-02T00:00:00', 5002079.4000, 1044656.0700, 1044656.0700, 0.0000, N'2017-06-05T00:00:00', 1044656.0700, 1044656.0700, 3957423.3300, 7915126.8400, 3957703.5100, '(none)', 0, '6314', 'DRG', 'All Services', 'Relative Weights', 'High', 430434.0000, 0.0000, 'Discharge code 1959', 1959, '01', '6314', '004', '6314', 317, '1', 'My Facility', 7915126.8400, 0.0000, -0.5823, -2.7885, 918093.5200, 1044656.0700, 'Physician Name', '', NULL, '6314', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7911, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '6314', 35, 0.0000, '106', 16832, 'Patient FC 106', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '3', 0.0000, 0.0000, '6334', 'Contract Example 1', 1, '2000175', 0.0000, 0.0000, NULL, 0.0000, '12345', 126562.5500, 'Payer 2000007', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.01', '0B110F4', 0.0000, 0.0000, 'SUBID 3', 5002079.4000, 317, NULL, 77, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Managed Care Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '793', 'N/A', NULL, NULL, 'Z38.01', '20019300', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -918093.5200, NULL, N'2016-06-30T00:00:00', N'2016-08-04T00:00:00', 0, 1, N'2016-08-02T00:00:00', N'2017-06-01T00:00:00', N'2017-05-05T00:00:00', 0.0000, 7915126.8400, 1, 0.0000, 0.0000, 0.0000, 0.0000, 5, '5', NULL, 562806.0700, 0.0000, 562806.0700, '20028860', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '117', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 0, 0.0000, NULL, 159960.741215400, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-05-31T00:00:00', NULL, N'2018-03-01T21:32:59.113', N'2018-03-01T21:32:59.113', 51, 'User 51', 0, 11, 23, 19, '5002904.42', NULL, '123', 0.0000, 0, NULL, NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, '22', NULL, 1499, 1753, 1753, 53, 340, NULL, N'2017-06-05T00:00:00', 0, NULL, 0.0000, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured3, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4589352, '65823117', 'Patient4, Test', 'Patient4', '', 14, 'Contract Example 868', 868, '2000009', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-07-16T00:00:00', N'2017-03-29T00:00:00', N'2017-05-05T00:00:00', 3889218.8900, 859589.3600, 859589.3600, 813651.3200, N'2017-05-19T00:00:00', 45938.0400, 45938.0400, 3029629.5300, 3074052.2500, 44422.7200, '(none)', 0, '0044', 'DRG', 'All Services', 'Relative Weights', 'High', 0.0000, 0.0000, 'Discharge code 1959', 1959, '01', '0044', '003', '0044', 256, '1', 'My Facility', 3074052.2500, 0.0000, 0.2095, 0.9483, -6103681.7800, 45938.0400, 'Physician Name', '', NULL, '0044', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7789, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '0044', 24, 0.0000, '3', 16840, 'Patient FC 3', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '4', 0.0000, 0.0000, '0044', 'Contract Example 490', 490, '2000009', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000009', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Q21.3', '5A1955Z', 0.0000, 0.0000, 'SUBID 4', 3889218.8900, 256, NULL, 81, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '0044', 'N/A', NULL, NULL, 'Q21.3', '20000800', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -859589.3600, N'2016-10-25T09:24:31.04', N'2016-08-16T00:00:00', N'2016-09-09T00:00:00', 1, 1, N'2016-09-08T00:00:00', N'2017-06-02T00:00:00', N'2017-06-05T00:00:00', 813651.3200, 3074052.2500, 1, 0.2092, 0.2092, 0.9465, 0.9465, 4, '4', NULL, 3885504.8900, 0.0000, 3885504.8900, '20000800', NULL, 0, NULL, NULL, NULL, NULL, 0.0000, NULL, '111', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, '1', 0, 0.0000, NULL, 587177.498976800, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-06-07T00:00:00', NULL, N'2021-05-18T09:54:02.367', N'2021-05-19T00:03:15.993', 171, 'User 171', 1, 12, 0, 18, '3892116.90', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, NULL, 'User Modified', NULL, '18', NULL, 1514, 1716, 1449, 51, 276, NULL, N'2017-05-19T00:00:00', 0, 0.0000, 813651.3200, NULL, NULL, NULL, NULL, NULL, 11, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, 'Insured4, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 4, 0.0000, 859589.3600, 2, 44422.7200, N'2021-05-18T00:00:00', NULL, NULL, NULL, NULL, NULL, 150.0000, NULL, 1, N'2021-05-18T00:00:00', NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4625878, '65843000', 'Patient5, Test', 'Patient5', '', 11, 'Contract Example 561', 561, '2000001', '2000009', 'Payer 2000009', NULL, NULL, NULL, NULL, 'Inpatient', N'2016-07-25T00:00:00', N'2017-04-10T00:00:00', N'2017-05-12T00:00:00', 5888595.1600, 3768700.9000, 3768700.9000, 3903720.7100, N'2017-05-03T00:00:00', -135019.8100, -135019.8100, 2119894.2600, 2119894.2600, 0.0000, '(none)', 0, 'MS-004', 'DRG', 'Surgical', 'Straight Discount', 'None', 0.0000, 0.0000, 'Discharge code 1959', 1959, '01', 'MS-004', '004', '0054', 259, '1', 'My Facility', 2119894.2600, 0.0000, 0.6399, 1.0000, -686896.8500, -135019.8100, 'Physician Name', '', NULL, 'MS-004', 1, 'New/Modified Calculated Record', '', 'Contract Example 892', 892, NULL, NULL, 0.3600, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '0054', 40, 0.0000, '101', 16827, 'Patient FC 101', '', NULL, NULL, N'2017-10-19T09:34:28.59', 'INS GRP I', '', '5', 0.0000, 0.0000, 'MS-790', 'Contract Example 561', 561, '2000001', 0.0000, 0.0000, NULL, 0.0000, '12345', 4455597.7500, 'Payer 2000001', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'P29.3', NULL, 0.0000, 0.0000, 'SUBID 5', 5888595.1600, 259, -750.0000, 15, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 89, 0, 0, 'UHC', 'Managed Care', 0, 9, 0, '', NULL, '', NULL, '790', 'N/A', NULL, NULL, 'P29.3', '20011600', '20022500', NULL, NULL, '', '', NULL, NULL, 'System Recalculate', 686896.8500, N'2016-10-25T09:24:31.04', N'2016-08-19T00:00:00', N'2016-09-28T00:00:00', 0, 0, N'2016-09-28T00:00:00', N'2017-06-02T00:00:00', N'2017-04-24T00:00:00', 3903720.7100, 2119894.2600, 1, 0.6629, 0.6629, 1.0358, 1.0358, 4, '4', NULL, 0.0000, 0.0000, 0.0000, '20011600', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '114', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, '2', 1, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-02-26T00:00:00', NULL, N'2017-05-15T10:28:20.94', N'2017-10-19T09:34:28.59', -1, 'User -1', 0, 57, 0, 66, '5888595.18', NULL, '123', 0.0000, 0, NULL, NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1502, NULL, NULL, 23, 257, NULL, N'2017-05-03T00:00:00', 0, NULL, 3903720.7100, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured5, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 2, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, N'2016-11-25T00:00:00', NULL, 1, 1, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4713661, '65892648', 'Patient6, Test', 'Patient6', '', 13, 'Contract Example 562', 562, '2000093', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-08-11T00:00:00', N'2017-06-14T00:00:00', N'2017-05-08T00:00:00', 3130637.8800, 679160.8300, 679160.8300, 204032.9900, N'2017-03-22T00:00:00', 475127.8400, 475127.8400, 2451477.0500, 2254118.0200, -197359.0300, 'INPATIENT', 16876, '5884', 'DRG', 'All Services', 'Relative Weights', 'High', 250817.3600, 0.0000, 'Discharge code 1976', 1976, '30', '5884', '790', '5884', 307, '1', 'My Facility', 2254118.0200, 0.0000, 0.2799, 1.2905, 475127.8400, 475127.8400, 'Physician Name', '', NULL, '5884', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7830, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '5884', 9, 0.0000, '107', 16833, 'Patient FC 107', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '6', 0.0000, 0.0000, '6024', 'Contract Example 1', 1, '2000175', 0.0000, 0.0000, NULL, 0.0000, '12345', 204032.9900, 'Payer 2000093', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.30', '02PY33Z', 0.0000, 0.0000, 'SUBID 6', 3130637.8800, 307, NULL, 268, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'CHIP', 'Medicaid', 0, 9, 0, '', NULL, '', NULL, '602', 'N/A', NULL, NULL, 'Z38.30', '20019900', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -475127.8400, N'2016-10-25T09:24:31.04', N'2016-09-12T00:00:00', N'2016-09-21T00:00:00', 0, 1, N'2016-09-20T00:00:00', N'2017-05-23T00:00:00', N'2017-05-16T00:00:00', 204032.9900, 2254118.0200, 1, 0.0651, 0.0651, 0.3004, 0.3004, 5, '5', NULL, 2605722.4300, 0.0000, 2605722.4300, '20028820', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '113', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 0, 0.0000, NULL, 642701.437359500, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-08-11T00:00:00', NULL, N'2018-03-01T21:19:50.24', N'2018-03-01T21:19:50.24', 51, 'User 51', 0, 7, 129, 7, '3572292.28', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, 'A1', 'N10', 1437, 1662, 1459, -84, 191, NULL, N'2017-03-22T00:00:00', 0, NULL, 204032.9900, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, 'Insured6, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 6, 0.0000, 679160.8300, 2, -197359.0300, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 2, 1, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4754708, '65911593', 'Patient7, Test', 'Patient7', '', 13, 'Contract Example 892', 892, '2000009', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-08-18T00:00:00', N'2017-06-09T00:00:00', N'2017-06-16T00:00:00', 2932578.7700, 997076.7800, 997076.7800, 169662.2300, N'2017-03-31T00:00:00', 827414.5500, 827414.5500, 1935501.9900, 0.0000, -1935501.9900, 'INPATIENT', 16876, '4404', 'DRG', 'All Services', 'Relative Weights', 'High', 1520972.3200, 0.0000, 'Discharge code 1959', 1959, '01', '4404', '652', '4404', 295, '1', 'My Facility', 0.0000, 0.0000, 1.0000, 2.9411, -1935501.9900, 827414.5500, 'Physician Name', '', NULL, '4404', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.6600, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '4404', 31, 0.0000, '3', 16840, 'Patient FC 3', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '7', 0.0000, 0.0000, '4404', 'Contract Example 568', 568, '2000009', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000009', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'N18.6', '0TY10Z0', 0.0000, 0.0000, 'SUBID 7', 2932578.7700, 295, NULL, 89, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '9999', 'N/A', NULL, NULL, 'N18.6', '20022500', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -997076.7800, N'2016-10-25T09:24:31.04', N'2016-09-20T00:00:00', N'2016-10-21T00:00:00', 0, 0, N'2016-10-21T00:00:00', N'2017-05-26T00:00:00', N'2017-06-15T00:00:00', 169662.2300, 0.0000, 1, 0.0578, 0.0578, 0.1701, 0.1701, 1, '1', NULL, 0.0000, 0.0000, 0.0000, '20022500', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '111', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, '2', 0, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2006-10-29T00:00:00', NULL, N'2018-03-01T21:50:56.897', N'2018-03-01T21:50:56.897', 51, 'User 51', 0, 7, 0, 7, '2934041.22', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1442, NULL, NULL, -70, 192, NULL, N'2017-03-31T00:00:00', 0, NULL, 169662.2300, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured7, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4756513, '65918567', 'Patient8, Test', 'Patient8', '', 11, 'Contract Example 868', 868, '2000009', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-08-19T00:00:00', N'2017-04-07T00:00:00', N'2017-04-14T00:00:00', 4006763.5900, 858473.7700, 858473.7700, 410939.9400, N'2017-05-16T00:00:00', 447533.8300, 447533.8300, 3148289.8200, 3148289.8200, 0.0000, '(none)', 0, '5884', 'DRG', 'All Services', 'Relative Weights', 'High', 66638.9700, 0.0000, 'Discharge code 1974', 1974, '20', '5884', '789', '5884', 231, '1', 'My Facility', 3148289.8200, 0.0000, 0.2142, 1.0000, -6296579.6400, 447533.8300, 'Physician Name', '', NULL, '5884', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7857, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '5884', 8, 0.0000, '3', 16840, 'Patient FC 3', '', NULL, NULL, N'2017-10-13T01:48:31.797', 'INS GRP I', '', '8', 0.0000, 0.0000, '5884', 'Contract Example 1', 1, '2000175', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000009', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.00', '5A1955Z', 0.0000, 0.0000, 'SUBID 8', 4006763.5900, 231, NULL, 81, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '9999', 'N/A', NULL, NULL, 'Z38.00', '20000800', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -858473.7700, N'2016-10-25T09:24:31.04', N'2016-09-22T00:00:00', N'2016-09-30T00:00:00', 0, 1, N'2016-09-30T00:00:00', N'2017-05-26T00:00:00', N'2017-04-14T00:00:00', 410939.9400, 3148289.8200, 1, 0.1025, 0.1025, 0.4786, 0.4786, 5, '5', NULL, 628233.0900, 0.0000, 628233.0900, '20028800', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '111', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 1, 0.0000, NULL, 94938.584560800, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-08-19T00:00:00', NULL, N'2017-05-23T12:10:42.83', N'2017-10-13T01:48:31.797', -1, 'User -1', 1, 6, 0, 5, '4075001.60', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, 'B13', NULL, 1505, 1652, 1533, 39, 236, NULL, N'2017-05-16T00:00:00', 0, NULL, 410939.9400, NULL, NULL, NULL, NULL, NULL, 10, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured8, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 2, 0.0000, 0.0000, 0, 0.0000, NULL, 11, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, N'2016-10-19T00:00:00', NULL, 1, 1, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4785177, '65925600', 'Patient9, Test', 'Patient9', '', 14, 'Contract Example 561', 561, '2000001', '2000001', 'Payer 2000001', NULL, NULL, NULL, NULL, 'Inpatient', N'2016-08-23T00:00:00', N'2017-04-26T00:00:00', N'2017-05-18T00:00:00', 2613616.1600, 1673161.7000, 1673161.7000, 1158091.3400, N'2017-06-05T00:00:00', 515070.3600, 515070.3600, 940454.4600, 1455524.8200, 515070.3600, 'INPATIENT', 16876, 'MS-003', 'DRG', 'Surgical', 'Straight Discount', 'None', 0.0000, 0.0000, 'Discharge code 1959', 1959, '01', 'MS-003', '003', '5884', 246, '1', 'My Facility', 1455524.8200, 0.0000, 0.4430, 0.6921, 1432441.8200, 515070.3600, 'Physician Name', '', NULL, 'MS-003', 1, 'New/Modified Calculated Record', '', 'Contract Example 51', 51, NULL, NULL, 0.3598, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '5884', 19, 0.0000, '101', 16827, 'Patient FC 101', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '9', -699.0000, 0.0000, 'MS-790', 'Contract Example 561', 561, '2000001', 0.0000, 0.0000, NULL, 0.0000, '12345', 240719.8800, 'Payer 2000001', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.01', NULL, 0.0000, 0.0000, 'SUBID 9', 2613616.1600, 246, NULL, 21, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 14, 0, 0, 'UHC', 'Managed Care', 0, 9, 0, '', NULL, '', NULL, '5934', 'N/A', NULL, NULL, 'Z38.01', '20001200', '20011400', NULL, NULL, '', '', NULL, NULL, 'System Recalculate', -1432441.8200, NULL, N'2016-09-23T00:00:00', N'2016-10-12T00:00:00', 0, 0, N'2016-10-12T00:00:00', N'2017-06-05T00:00:00', N'2017-06-05T00:00:00', 1158091.3400, 1455524.8200, 1, 0.4430, 0.4430, 0.6921, 0.6921, 5, '5', NULL, 0.0000, 0.0000, 0.0000, '20001200', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '114', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 0, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-08-23T00:00:00', NULL, N'2018-03-01T21:16:59.82', N'2018-03-01T21:16:59.82', 51, 'User 51', 0, 16, 0, 14, '2612917.16', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1486, NULL, NULL, 40, 255, NULL, N'2017-06-05T00:00:00', 0, NULL, 1158091.3400, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured9, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4816918, '65936747', 'Patient10, Test', 'Patient10', '', 14, 'Contract Example 878', 878, '2000177', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-08-27T00:00:00', N'2017-04-01T00:00:00', N'2017-05-22T00:00:00', 4980263.3900, 1049495.1700, 1049495.1700, 335214.3900, N'2017-05-22T00:00:00', 714280.7800, 714280.7800, 3930768.2200, 3931637.0100, 868.7900, '(none)', 0, '6304', 'DRG', 'All Services', 'Relative Weights', 'High', 237820.8000, 0.0000, 'Discharge code 1959', 1959, '01', '6304', '270', '6304', 217, '1', 'My Facility', 3931637.0100, 0.0000, 0.2109, 1.0013, -7864655.2300, 714280.7800, 'Physician Name', '', NULL, '6304', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7892, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '6304', 12, 0.0000, '107', 16833, 'Patient FC 107', '', NULL, NULL, N'2017-10-13T09:35:14.76', 'INS GRP I', '', '10', 0.0000, 0.0000, '270', 'Contract Example 587', 587, '2000175', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000177', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Q22.4', NULL, 0.0000, 0.0000, 'SUBID 10', 4982513.3900, 217, NULL, 363, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '2004', 'N/A', NULL, NULL, 'Q22.4', '20029000', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -1049495.1700, NULL, N'2016-09-30T00:00:00', N'2016-10-12T00:00:00', 0, 0, N'2016-10-11T00:00:00', N'2017-05-22T00:00:00', N'2017-04-11T00:00:00', 335214.3900, 3931637.0100, 1, 0.0673, 0.0673, 0.3194, 0.3194, 4, '4', NULL, 0.0000, 0.0000, 0.0000, '20028850', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '115', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, '2', 1, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-08-23T00:00:00', NULL, N'2017-05-23T16:29:54.913', N'2017-10-13T09:35:14.76', -1, 'User -1', 0, 8, 0, 9, '4979138.39', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1511, NULL, NULL, 51, 234, NULL, N'2017-05-22T00:00:00', 0, NULL, 335214.3900, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured10, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 2, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, N'2016-10-23T00:00:00', NULL, 1, 1, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4874495, '65974742', 'Patient11, Test', 'Patient11', '', 14, 'Contract Example 773', 773, '2000007', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-09-10T00:00:00', N'2017-06-14T00:00:00', N'2017-06-06T00:00:00', 5150557.4100, 1091841.0000, 1091841.0000, 316865.0500, N'2017-04-24T00:00:00', 774975.9500, 774975.9500, 4058716.4100, 5634130.2200, 1575413.8100, 'INPATIENT', 16876, '5884', 'DRG', 'All Services', 'Relative Weights', 'High', 180318.0300, 0.0000, 'Discharge code 1976', 1976, '30', '5884', '004', '5884', 277, '1', 'My Facility', 5634130.2200, 0.0000, -0.0938, -0.4428, 774975.9500, 774975.9500, 'Physician Name', '', NULL, '5884', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7880, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '5884', 43, 0.0000, '106', 16832, 'Patient FC 106', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '11', 0.0000, 0.0000, '790', 'Contract Example 1', 1, '2000175', 0.0000, 0.0000, NULL, 0.0000, '12345', 316865.0500, 'Payer 2000007', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.01', NULL, 0.0000, 0.0000, 'SUBID 11', 5150557.4100, 277, NULL, 77, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Managed Care Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '', 'N/A', NULL, NULL, 'Z38.01', '20019300', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -774975.9500, NULL, N'2016-10-10T00:00:00', N'2016-11-22T00:00:00', 0, 0, N'2016-11-18T00:00:00', N'2017-04-20T00:00:00', N'2017-06-06T00:00:00', 316865.0500, 5634130.2200, 1, 0.0615, 0.0615, 0.2902, 0.2902, 5, '5', NULL, 0.0000, 0.0000, 0.0000, '20028860', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '113', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 0, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-09-10T00:00:00', NULL, N'2018-03-01T21:32:59.913', N'2018-03-01T21:32:59.913', 51, 'User 51', 0, 8, 0, 8, '5272299.21', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1437, NULL, NULL, -51, 196, NULL, N'2017-04-24T00:00:00', 0, NULL, 316865.0500, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured11, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4874511, '65974856', 'Patient12, Test', 'Patient12', '', 13, 'Contract Example 889', 889, '2000176', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-09-10T00:00:00', N'2017-05-06T00:00:00', N'2017-05-08T00:00:00', 3202160.2400, 673374.4500, 673374.4500, 183034.2100, N'2017-06-15T00:00:00', 490340.2400, 490340.2400, 2528785.7900, 1453834.6700, -1074951.1200, 'INPATIENT', 16876, '5933', 'DRG', 'All Services', 'Relative Weights', 'High', 253130.3100, 0.0000, 'Discharge code 1976', 1976, '30', '5933', '790', '5933', 238, '1', 'My Facility', 1453834.6700, 0.0000, 0.5459, 2.5963, -3982620.4600, 490340.2400, 'Physician Name', '', NULL, '5933', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7897, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '5933', 24, 0.0000, '107', 16833, 'Patient FC 107', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '12', 0.0000, 0.0000, '790', 'Contract Example 640', 640, '2000176', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000176', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'P22.0', '0BJ08ZZ', 0.0000, 0.0000, 'SUBID 12', 3202160.2400, 238, NULL, 362, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '', 'N/A', NULL, NULL, 'P22.0', '20028900', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -673374.4500, NULL, N'2016-10-10T00:00:00', N'2016-11-03T00:00:00', 0, 0, NULL, NULL, N'2017-06-15T00:00:00', 183034.2100, 1453834.6700, 1, 0.0571, 0.0571, 0.2718, 0.2718, 4, '4', NULL, 0.0000, 0.0000, 0.0000, '20028900', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '113', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, '1', 0, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-09-10T00:00:00', NULL, N'2018-03-01T21:50:35.087', N'2018-03-01T21:50:35.087', 51, 'User 51', 0, 0, 0, 0, '2571729.45', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1476, NULL, NULL, 40, 248, NULL, N'2017-06-15T00:00:00', 0, NULL, 183034.2100, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured12, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 4945919, '66011386', 'Patient13, Test', 'Patient13', '', 13, 'Contract Example 173', 173, '2000209', '2000009', 'Payer 2000009', NULL, NULL, NULL, NULL, 'Inpatient', N'2016-09-22T00:00:00', N'2017-04-06T00:00:00', N'2017-04-24T00:00:00', 3480633.1100, 1914315.7600, 1913718.4500, 2074032.2000, N'2017-04-19T00:00:00', -160313.7500, -159716.4400, 1566317.3500, 1254081.0800, -312236.2700, '(none)', 0, 'None', 'None', 'All Services', 'Straight Discount', 'None', 0.0000, 0.0000, 'Discharge code 1976', 1976, '30', NULL, '003', '6304', 196, '1', 'My Facility', 1254081.0800, 0.0000, 0.6396, 1.1631, -2820398.4300, -159716.4400, 'Physician Name', '', NULL, NULL, 1, 'New/Modified Calculated Record', '', 'Contract Example 892', 892, NULL, NULL, 0.4500, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '6094', 39, 0.0000, '1', 16824, 'Patient FC 1', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '13', 0.0000, 0.0000, '793', 'Contract Example 561', 561, '2000001', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000209', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 597.3100, 597.3100, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.00', '0DN64ZZ', 0.0000, 0.0000, 'SUBID 13', 3480633.1100, 196, 597.3100, 409, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 89, 0, 0, 'None', NULL, 0, 9, 0, '', NULL, '', NULL, '793', 'N/A', NULL, NULL, 'Z38.00', '20036200', '20022500', NULL, NULL, '', '', NULL, NULL, 'System Recalculate', -1914315.7600, NULL, N'2016-10-10T00:00:00', N'2016-11-18T00:00:00', 0, 0, N'2016-11-18T00:00:00', N'2017-06-02T00:00:00', N'2017-06-08T00:00:00', 2074032.2000, 1254081.0800, 1, 0.5958, 0.5958, 1.0834, 1.0834, 5, '5', NULL, 0.0000, 0.0000, 0.0000, '20011600', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '113', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 0, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-09-22T00:00:00', NULL, N'2018-03-01T21:14:13.713', N'2018-03-01T21:14:13.713', 51, 'User 51', 0, 25, 0, 34, '3480412.12', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1506, NULL, NULL, 13, 191, NULL, N'2017-04-19T00:00:00', 0, NULL, 2074032.2000, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured13, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 5023249, '66028400', 'Patient14, Test', 'Patient14', '', 14, 'Contract Example 878', 878, '2000177', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-09-28T00:00:00', N'2017-04-14T00:00:00', N'2017-04-21T00:00:00', 2786506.0100, 804733.6600, 804733.6600, 42764.9500, N'2017-03-29T00:00:00', 761968.7100, 761968.7100, 1981772.3500, 2024126.0500, 42353.7000, '(none)', 0, '0014', 'DRG', 'Medical', 'Relative Weights', 'High', 576464.4000, 0.0000, 'Discharge code 1959', 1959, '01', '0014', '003', '0014', 198, '1', 'My Facility', 2024126.0500, 0.0000, 0.2735, 0.9473, -4005898.4000, 761968.7100, 'Physician Name', '', NULL, '0014', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7112, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '0014', 21, 0.0000, '107', 16833, 'Patient FC 107', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '14', 0.0000, 0.0000, '640', 'Contract Example 587', 587, '2000177', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000177', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'E43', '0FY00Z0', 0.0000, 0.0000, 'SUBID 14', 2786506.0100, 198, NULL, 363, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '0014', 'N/A', NULL, NULL, 'E46', '20029000', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -804733.6600, NULL, N'2017-01-06T00:00:00', N'2017-01-27T00:00:00', 0, 0, N'2017-01-26T00:00:00', N'2017-05-23T00:00:00', N'2017-06-15T00:00:00', 42764.9500, 2024126.0500, 1, 0.0153, 0.0153, 0.0531, 0.0531, 2, '2', NULL, 0.0000, 0.0000, 0.0000, '20029000', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '117', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, '2', 0, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-02-25T00:00:00', NULL, N'2018-03-01T21:48:46.64', N'2018-03-01T21:48:46.64', 51, 'User 51', 0, 4, 0, 8, '2786506.02', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1498, NULL, NULL, -16, 82, NULL, N'2017-03-29T00:00:00', 0, NULL, 42764.9500, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured14, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 5068197, '66042707', 'Patient15, Test', 'Patient15', '', 21, 'Contract Example 51', 51, '2000001', '2000175', 'Payer 2000175', NULL, NULL, NULL, NULL, 'Inpatient', N'2016-10-03T00:00:00', N'2017-04-08T00:00:00', N'2017-04-17T00:00:00', 2580390.9900, 1057960.3100, 1057960.3100, 0.0000, N'2017-05-19T00:00:00', 1057960.3100, 1057960.3100, 1522430.6800, 1522430.6800, 0.0000, '(none)', 0, 'None', 'None', 'All Services', 'Straight Discount', 'None', 0.0000, 0.0000, 'Discharge code 1959', 1959, '01', NULL, '790', '5884', 187, '1', 'My Facility', 1522430.6800, 0.0000, 0.4100, 1.0000, -3044861.3600, 1057960.3100, 'Physician Name', '', NULL, NULL, 1, 'New/Modified Calculated Record', '', 'Contract Example 1', 1, NULL, NULL, 0.5899, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '5884', 29, 0.0000, '101', 16827, 'Patient FC 101', '', NULL, NULL, N'2017-05-23T14:47:22.637', 'INS GRP I', '', '15', 0.0000, 0.0000, '790', 'Contract Example 1', 1, '2000175', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000001', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.01', '', 0.0000, 0.0000, 'SUBID 15', 2580390.9900, 187, NULL, 14, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 361, 0, 0, 'Managed Care Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '5892', 'N/A', NULL, NULL, 'Z38.01', '20011400', '20028800', NULL, NULL, '', '', NULL, NULL, 'System Recalculate', -1057960.3100, NULL, N'2016-10-19T00:00:00', N'2016-11-17T00:00:00', 0, 0, N'2016-11-17T00:00:00', N'2017-05-19T00:00:00', N'2017-04-17T00:00:00', 0.0000, 1522430.6800, 1, 0.0000, 0.0000, 0.0000, 0.0000, 5, '5', NULL, 0.0000, 0.0000, 0.0000, '20028800', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '114', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 1, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-10-03T00:00:00', NULL, N'2017-05-23T14:47:45.91', N'2017-05-23T14:47:45.91', 0, 'User 0', 0, 12, 0, 15, '2578190.99', NULL, '123', 0.0000, 0, NULL, NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1504, NULL, NULL, 41, 212, NULL, N'2017-05-19T00:00:00', 0, NULL, 0.0000, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured15, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 5068224, '66044111', 'Patient16, Test', 'Patient16', '', 21, 'Contract Example 772', 772, '2000007', '2000009', 'Payer 2000009', NULL, NULL, NULL, NULL, 'Inpatient', N'2016-10-03T00:00:00', N'2017-04-21T00:00:00', N'2017-05-23T00:00:00', 6958018.3300, 1430025.3700, 1429950.3700, 0.0000, N'2017-06-12T00:00:00', 1429950.3700, 1430025.3700, 5527992.9600, 5527992.9600, 0.0000, '(none)', 0, '7104', 'DRG', 'All Services', 'Relative Weights', 'High', 333156.6000, 0.0000, 'Discharge code 1984', 1984, '62', '7104', '853', '7104', 200, '1', 'My Facility', 5527992.9600, 0.0000, 0.2055, 1.0000, -11055985.9200, 1430025.3700, 'Physician Name', '', NULL, '7104', 1, 'New/Modified Calculated Record', '', 'Contract Example 868', 868, NULL, NULL, 0.7944, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '7104', 47, 0.0000, '106', 16832, 'Patient FC 106', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '16', 0.0000, 0.0000, '870', 'Contract Example 492', 492, '2000007', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000007', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 75.0000, 75.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'A41.01', '5A1935Z', 0.0000, 0.0000, 'SUBID 16', 6958018.3300, 200, 75.0000, 79, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 13, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 81, 0, 0, 'Managed Care Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '9999', 'N/A', NULL, NULL, 'A41.9', '20001800', '20000800', NULL, NULL, '', '', NULL, NULL, 'System Recalculate', -1430025.3700, NULL, N'2016-11-02T00:00:00', N'2016-12-19T00:00:00', 0, 1, N'2016-12-14T00:00:00', N'2017-06-02T00:00:00', N'2017-05-23T00:00:00', 0.0000, 5527992.9600, 1, 0.0000, 0.0000, 0.0000, 0.0000, 1, '1', NULL, 8227998.8800, 876536.9700, 9104535.8500, '20001800', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '111', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, '1', 0, 0.0000, NULL, 2471024.108580000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2003-05-26T00:00:00', NULL, N'2018-03-01T21:32:08.983', N'2018-03-01T21:32:08.983', 51, 'User 51', 1, 10, 91, 8, '6958018.33', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, '22', NULL, 1491, 1586, 1449, 52, 222, NULL, N'2017-06-12T00:00:00', 0, NULL, 0.0000, NULL, NULL, NULL, NULL, NULL, 1, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured16, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 5073959, '66048802', 'Patient17, Test', 'Patient17', '', 11, 'Contract Example 868', 868, '2000009', '2000000', 'Payer 2000000', NULL, NULL, NULL, NULL, 'Inpatient', N'2016-10-05T00:00:00', N'2017-04-03T00:00:00', N'2017-05-12T00:00:00', 1413210.2500, 358719.4000, 358719.4000, 358719.7100, N'2017-05-22T00:00:00', -0.3100, -0.3100, 1054490.8500, 1054490.5400, -0.3100, '(none)', 0, '6024', 'DRG', 'All Services', 'Relative Weights', 'High', 151904.8300, 0.0000, 'Discharge code 1963', 1963, '05', '6024', '326', '6024', 180, '1', 'My Facility', 1054490.5400, 0.0000, 0.2538, 1.0000, -2108981.3900, -0.3100, 'Physician Name', '', NULL, '6024', 1, 'New/Modified Calculated Record', '', 'Contract Example 142', 142, NULL, NULL, 0.7461, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '6024', 88, 0.0000, '3', 16840, 'Patient FC 3', '', NULL, NULL, N'2017-05-23T16:10:04.28', 'INS GRP I', '', '17', 0.0000, 0.0000, '326', 'Contract Example 142', 142, '2000000', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000009', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Q39.0', '0DS50ZZ', 0.0000, 0.0000, 'SUBID 17', 1413210.2500, 180, NULL, 81, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 6, 0, 0, 'Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '6024', 'N/A', NULL, NULL, 'Q39.0', '20000800', '20028700', NULL, NULL, '', '', NULL, NULL, 'System Recalculate', -358719.4000, NULL, N'2016-10-24T00:00:00', N'2017-01-20T00:00:00', 0, 1, N'2017-01-20T00:00:00', N'2017-05-26T00:00:00', N'2017-05-19T00:00:00', 358719.7100, 1054490.5400, 1, 0.2538, 0.2538, 1.0000, 1.0000, 4, '4', NULL, 276753.2200, 0.0000, 276753.2200, '20028700', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '111', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, '1', 1, 0.0000, NULL, 41822.946606400, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-10-04T00:00:00', NULL, N'2017-05-23T16:29:55.717', N'2017-05-23T16:29:55.717', 0, 'User 0', 1, 8, 0, 2, '1418325.25', NULL, '123', 0.0000, 0, NULL, NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, 'B13', NULL, 1509, 1533, 1533, 49, 210, NULL, N'2017-05-22T00:00:00', 0, NULL, 358719.7100, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured17, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 2, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, N'2017-01-24T00:00:00', NULL, 1, 1, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 5088957, '66055104', 'Patient18, Test', 'Patient18', '', 13, 'Contract Example 781', 781, '2000093', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-10-06T00:00:00', N'2017-06-14T00:00:00', N'2017-05-02T00:00:00', 2896289.4500, 612033.8200, 612033.8200, 238866.1700, N'2017-05-19T00:00:00', 373167.6500, 373167.6500, 2284255.6300, 2073291.5400, -210964.0900, 'INPATIENT', 16876, '6094', 'DRG', 'All Services', 'Relative Weights', 'High', 245263.7500, 0.0000, 'Discharge code 1976', 1976, '30', '6094', '003', '6314', 251, '1', 'My Facility', 2073291.5400, 0.0000, 0.2841, 1.3446, 373167.6500, 373167.6500, 'Physician Name', '', NULL, '6094', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7886, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '6094', 11, 0.0000, '107', 16833, 'Patient FC 107', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '18', 0.0000, 0.0000, '003', 'Contract Example 1', 1, '2000175', 0.0000, 0.0000, NULL, 0.0000, '12345', 238866.1700, 'Payer 2000093', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.01', '0DJ08ZZ', 0.0000, 0.0000, 'SUBID 18', 2896289.4500, 251, NULL, 268, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'None', NULL, 0, 9, 0, '', NULL, '', NULL, '633', 'N/A', NULL, NULL, 'Z38.01', '20019900', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -373167.6500, NULL, N'2016-11-07T00:00:00', N'2016-11-18T00:00:00', 1, 1, N'2016-11-17T00:00:00', N'2017-05-18T00:00:00', N'2017-05-15T00:00:00', 238866.1700, 2073291.5400, 1, 0.0824, 0.0824, 0.3902, 0.3902, 5, '5', NULL, 1458462.9000, 0.0000, 1458462.9000, '20028820', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '113', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 0, 0.0000, NULL, 359729.874285000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-10-06T00:00:00', NULL, N'2018-03-01T21:40:53.063', N'2018-03-01T21:40:53.063', 51, 'User 51', 0, 5, 80, 5, '3447373.66', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, 'A1', 'N10', 1437, 1611, 1522, -26, 193, NULL, N'2017-05-19T00:00:00', 0, NULL, 238866.1700, NULL, NULL, NULL, NULL, NULL, 0, 21, NULL, NULL, 0, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, 'Insured18, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 6, 0.0000, 612033.8200, 2, -210964.0900, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 2, 1, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 5270027, '66116488', 'Patient19, Test', 'Patient19', '', 13, 'Contract Example 773', 773, '2000007', NULL, NULL, NULL, NULL, NULL, NULL, 'Inpatient', N'2016-10-27T00:00:00', N'2017-06-14T00:00:00', N'2017-04-24T00:00:00', 3865372.5300, 829663.2900, 829663.2900, 316865.0500, N'2017-06-08T00:00:00', 512798.2400, 512798.2400, 3035709.2400, 2344621.7300, -691087.5100, 'INPATIENT', 16876, '5884', 'DRG', 'All Services', 'Relative Weights', 'High', 64222.8600, 0.0000, 'Discharge code 1976', 1976, '30', '5884', '004', '5884', 230, '1', 'My Facility', 2344621.7300, 0.0000, 0.3934, 1.8329, 512798.2400, 512798.2400, 'Physician Name', '', NULL, '5884', 1, 'New/Modified Calculated Record', '', NULL, NULL, NULL, NULL, 0.7853, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '5884', 17, 0.0000, '106', 16832, 'Patient FC 106', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '19', 0.0000, 0.0000, '790', 'Contract Example 1', 1, '2000175', 0.0000, 0.0000, NULL, 0.0000, '12345', 316865.0500, 'Payer 2000007', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Z38.01', '', 0.0000, 0.0000, 'SUBID 19', 3865372.5300, 230, NULL, 77, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 0, 0, 0, 'Managed Care Medicaid', NULL, 0, 9, 0, '', NULL, '', NULL, '', 'N/A', NULL, NULL, 'Z38.01', '20019300', NULL, NULL, NULL, '', NULL, NULL, NULL, 'System Recalculate', -512798.2400, NULL, N'2016-11-28T00:00:00', N'2016-12-15T00:00:00', 1, 0, N'2016-12-09T00:00:00', N'2017-05-10T00:00:00', N'2017-04-28T00:00:00', 316865.0500, 2344621.7300, 1, 0.0819, 0.0819, 0.3819, 0.3819, 5, '5', NULL, 0.0000, 0.0000, 0.0000, '20028860', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '113', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4, '4', 0, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-10-27T00:00:00', NULL, N'2018-03-01T21:33:01.687', N'2018-03-01T21:33:01.687', 51, 'User 51', 0, 6, 0, 6, '4039545.49', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1437, 1624, NULL, -6, 192, NULL, N'2017-06-08T00:00:00', 0, NULL, 316865.0500, NULL, NULL, NULL, NULL, NULL, 0, 20, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Insured19, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 0, 0.0000, 0.0000, 0, 0.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 0, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 0, NULL, 5275268, '66120000', 'Patient20, Test', 'Patient20', '', 13, 'Contract Example 657', 657, '2000002', '2000177', 'Payer 2000177', NULL, NULL, '2000026', 'Payer 2000026', 'Inpatient', N'2016-10-30T00:00:00', N'2017-06-14T00:00:00', N'2017-06-13T00:00:00', 7492478.9100, 4720261.7100, 4035762.5500, 3184771.1900, N'2017-06-12T00:00:00', 850991.3600, 1535490.5200, 2772217.2000, 2557806.3800, -214410.8200, 'INPATIENT', 16876, 'MS-003', 'DRG', 'Surgical', 'Straight Discount', 'None', 0.0000, 0.0000, 'Discharge code 1976', 1976, '30', 'MS-003', '003', '1604', 227, '1', 'My Facility', 2557806.3800, 0.0000, 0.6586, 1.0454, -5330023.5800, 1535490.5200, 'Physician Name', '', NULL, 'MS-003', 1, 'New/Modified Calculated Record', '', 'Contract Example 878', 878, NULL, 885, 0.3700, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, '1604', 23, 0.0000, '100', 16826, 'Patient FC 100', '', NULL, NULL, N'2018-03-01T21:08:14.427', 'INS GRP I', '', '20', 0.0000, 0.0000, '270', 'Contract Example 657', 657, '2000002', 0.0000, 0.0000, NULL, 0.0000, '12345', 0.0000, 'Payer 2000002', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, '', 0, NULL, 684499.1600, 684499.1600, 0.0000, NULL, 0.0000, 0.0000, NULL, 0.0000, 0.0000, NULL, '', 'Q23.4', NULL, 0.0000, 0.0000, 'SUBID 20', 7492478.9100, 227, 684499.1600, 26, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0, 0.0000, 363, 0, 6718, 'BCBS', 'Managed Care', 0, 9, 0, '', NULL, '', NULL, '306', 'N/A', NULL, NULL, 'Q23.4', '20012300', '20029000', NULL, '20019710', '', '', NULL, '', 'System Recalculate', -4720261.7100, NULL, N'2016-11-16T00:00:00', N'2016-12-09T00:00:00', 0, 0, N'2016-12-09T00:00:00', N'2017-06-05T00:00:00', N'2017-06-13T00:00:00', 3184771.1900, 2557806.3800, 1, 0.4250, 0.4250, 0.6747, 0.6747, 4, '4', NULL, 0.0000, 0.0000, 0.0000, '20040100', NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, '113', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, '1', 0, 0.0000, NULL, 0.000000000, '', 0.0000, 0.0000, 0.0000, 0.0000, 0.0000, NULL, N'2016-10-17T00:00:00', NULL, N'2018-03-01T21:25:38.86', N'2018-03-01T21:25:38.86', 51, 'User 51', 0, 42, 0, 57, '7483326.28', NULL, '123', 0.0000, 0, '', NULL, '(none)', '(none)', '(none)', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'User Modified', NULL, NULL, NULL, 1437, NULL, NULL, -2, 208, NULL, N'2017-06-12T00:00:00', 0, NULL, 3184771.1900, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, NULL, 0, NULL, NULL, NULL, 'Insured20, Name', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 'Unassigned', NULL, 'Unassigned', 6, 0.0000, 4720261.7100, 10, 850991.3600, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0.0000, 0.0000, NULL, 0.0000, 0, 0, 0.0000, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 2, 1, -1, 0, 0.0000, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, 0.0000, 0, NULL, 0.0000, 0, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ); 

--	This table stores the header information for any saved view.
DROP TABLE IF EXISTS [dbo].[tblUserViews];

CREATE TABLE [dbo].[tblUserViews]
(
[ViewID] [int] NOT NULL IDENTITY(1, 1),
[ViewName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ViewDesc] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ViewOwner] [int] NULL,
[CPTView] [int] NOT NULL,
[DefaultView] [int] NOT NULL,
[DefaultAuditor] [int] NOT NULL,
[DefaultFollowUp] [int] NOT NULL,
[DefaultStatus] [int] NOT NULL,
[DefaultRecordAge] [int] NOT NULL,
[DefaultRecordHidden] [int] NOT NULL
) ON [PRIMARY];

ALTER TABLE [dbo].[tblUserViews] ADD CONSTRAINT [PK__tblUserV__1E371C1641C6DAEF] PRIMARY KEY CLUSTERED ([ViewID]) WITH (FILLFACTOR=85) ON [PRIMARY];

INSERT INTO tblUserViews(ViewName, ViewDesc, ViewOwner, CPTView, DefaultView, DefaultAuditor, DefaultFollowUp, DefaultStatus, DefaultRecordAge, DefaultRecordHidden)
VALUES
('All', 'All', 1, 0, 0, 0, 0, 1, 0, 1),
('All Name', 'All Desc', 2, 0, 1, 0, 0, 1, 0, 1),
('All', 'All', 3, 0, 0, 0, 0, 1, 0, 1);

--	Create the table that stores the configured rules.
DROP TABLE IF EXISTS [dbo].[tblViewRules];

CREATE TABLE [dbo].[tblViewRules]
(
[RuleID] [int] NOT NULL IDENTITY(1, 1),
[ViewID] [int] NOT NULL,
[FieldID] [int] NOT NULL,
[FacilityID] [int] NOT NULL,
[ValueID] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Value] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BeginRange] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EndRange] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Operand] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Operand2] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY];
ALTER TABLE [dbo].[tblViewRules] ADD CONSTRAINT [PK__tblViewR__110458C24967FCB7] PRIMARY KEY CLUSTERED ([RuleID]) WITH (FILLFACTOR=85) ON [PRIMARY];
CREATE NONCLUSTERED INDEX [ixViewFieldFacVal] ON [dbo].[tblViewRules] ([ViewID], [FieldID], [FacilityID], [ValueID]) ON [PRIMARY];


INSERT INTO tblViewRules(ViewID, FieldID, FacilityID, ValueID, Value, BeginRange, EndRange, Operand, Operand2)
VALUES
( 1, 2, 1, '1', 'My First Contract', NULL, NULL, 'IN', NULL ),
( 1, 5, -1, NULL, NULL, '01/01/1900', '12/31/1986', 'BETWEEN', NULL ),
( 1, 11, -1, NULL, '0', NULL, NULL, 'GREATER THAN', NULL ),
( 1, 19, -1, NULL, '5441', NULL, NULL, 'STARTS WITH', '5664' ),
( 1, 26, -1, '5631', '5631', NULL, NULL, 'LESS THAN', NULL ),
( 2, 2, 1, '1', 'My First Contract', NULL, NULL, 'IN', NULL ),
( 2, 5, -1, NULL, NULL, '01/01/1900', '12/31/1986', 'BETWEEN', NULL ),
( 2, 11, -1, NULL, '0', NULL, NULL, 'GREATER THAN', NULL ),
( 2, 19, -1, NULL, '5441', NULL, NULL, 'STARTS WITH', '5664' ),
( 2, 26, -1, '5631', '5632', NULL, NULL, 'LESS THAN', NULL );

SET IDENTITY_INSERT tblComboBoxesSystemValues ON;
INSERT INTO tblComboBoxesSystemValues([CodeCounter], Code, CodeType, Description)
VALUES
(27, '111', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Admit thru Discharge Claim' ), 
(28, '112', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Interim - First Claim' ), 
(29, '113', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Interim - Continuing Claim (Used by non-PPS acute care facilities)' ), 
(30, '114', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Interim - Last Claim (Used by non-PPS acute care facilities)' ), 
(31, '115', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Late Charge(s) Only Claim (Use for inpatient Part A bill for ancillary services for non-PPS facilities)' ), 
(32, '117', 'BillType', 'Hospital, Inpatient (Including Medicare Part A), Replacement of Prior Claim' ), 
(33, '121', 'BillType', 'Hospital, Inpatient (Medicare Part B only), Admit thru Discharge Claim' ), 
(34, '127', 'BillType', 'Hospital, Inpatient (Medicare Part B only), Replacement of Prior Claim' ), 
(35, '131', 'BillType', 'Hospital, Outpatient, Admit thru Discharge Claim' ), 
(36, '132', 'BillType', 'Hospital, Outpatient, Interim ?First Claim' ), 
(37, '133', 'BillType', 'Hospital, Outpatient, Interim ?Continuing Claims (Not Valid for PPS Bills)' ), 
(38, '134', 'BillType', 'Hospital, Outpatient, Interim ?Last Claim (Not Valid for PPS Bills)' ), 
(39, '135', 'BillType', 'Hospital, Outpatient, Late Charge(s) Only Claim' ), 
(40, '137', 'BillType', 'Hospital, Outpatient, Replacement of Prior Claim' ), 
(41, '138', 'BillType', 'Hospital, Outpatient, Void/Cancel of Prior Claim' ), 
(42, '141', 'BillType', 'Hospital, Other (for hospital referenced diagnostic services, or home health not under a plan of treatment), Admit thru Discharge Claim' ), 
(43, '147', 'BillType', 'Hospital, Other (for hospital referenced diagnostic services, or home health not under a plan of treatment), Replacement of Prior Claim' ), 
(44, '210', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Non-Payment/Zero Claim' ), 
(45, '211', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Admit thru Discharge Claim' ), 
(46, '212', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Interim - First Claim' ), 
(47, '214', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Interim - Last Claim' ), 
(48, '215', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Late Charge(s) Only Claim (Used for inpatient Part A bill for inpatient ancillary services)' ), 
(49, '217', 'BillType', 'Skilled Nursing, Inpatient (Including Medicare Part A), Replacement of Prior Claim' ), 
(50, '221', 'BillType', 'Skilled Nursing, Inpatient (Medicare Part B only), Admit thru Discharge Claim' ), 
(51, '721', 'BillType', 'Clinic, Hospital Based or Independent Renal Dialysis Center, Admit thru Discharge Claim' ), 
(52, '1959', 'Discharge', 'DC TO HOME OR SELF CARE (RO DISCHA)' ), 
(53, '17573', 'Discharge', 'System Added' ), 
(54, '1960', 'Discharge', 'DC/TRANS TO A SHORT-TERM HOSP INPT' ), 
(55, '1961', 'Discharge', 'DC/TRANS TO SNF WITH MCARE CERTIFIC' ), 
(56, '1962', 'Discharge', 'DC/TRANS TO AN INTERM CARE FAC(ICF)' ), 
(57, '1963', 'Discharge', 'DC/TRANS TO ANOTHER TYPE OF INSTITU' ), 
(58, '1964', 'Discharge', 'DC/TRANS TO HOME UNDER HOME HLTH OR' ), 
(103, '1', 'FinClass', 'Commercial' ), 
(104, '10', 'FinClass', 'Blue Shield' ), 
(105, '100', 'FinClass', 'Blue Cross Blue Shield' ), 
(106, '101', 'FinClass', 'United Healthcare' ), 
(131, '16876', 'PatType', 'INPATIENT' ), 
(132, '16871', 'PatType', 'OUTPATIENT' ), 
(133, '16872', 'PatType', 'EMERGENCY' ), 
(168, '0', 'AuditFlagAll', 'Pending' ), 
(169, '4', 'AuditFlagAll', 'Internal Review' ), 
(170, '6', 'AuditFlagAll', 'Reopen' ), 
(181, '2', 'AuditFlagAll', 'Closed' ), 
(201, 'Inpatient', 'ServiceCode', 'Inpatient' ), 
(202, 'Outpatient', 'ServiceCode', 'Outpatient' ), 
(203, 'Physician', 'ServiceCode', 'Physician' );
SET IDENTITY_INSERT tblComboBoxesSystemValues OFF;

DROP TABLE IF EXISTS [dbo].[tblAudits];
CREATE TABLE [dbo].[tblAudits]
(
[AuditCounter] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[OldValue]  varchar(MAX) NULL,
[NewValue]  varchar(MAX) NULL,
[OperationType] [varchar](10) NOT NULL,
[ObjectType] [varchar](20) NOT NULL,
[Timestamp] [datetime] NOT NULL
) ON [PRIMARY];

DROP TABLE IF EXISTS [dbo].[tblColumnsCustomDetail];
DROP TABLE IF EXISTS [dbo].[tblColumnsCustom];
CREATE TABLE [dbo].[tblColumnsCustom]
(
[CustomID] [INT] NOT NULL IDENTITY(1, 1),
[UserID] [INT] NULL,
[CustomName] [VARCHAR] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DefaultView] [INT] NOT NULL,
[LayoutDescription] [VARCHAR] (275) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
) ON [PRIMARY];
ALTER TABLE [dbo].[tblColumnsCustom] ADD CONSTRAINT [PK__tblColum__EC6CBE52346CDFD1] PRIMARY KEY CLUSTERED ([CustomID]) WITH (FILLFACTOR=85) ON [PRIMARY];

CREATE TABLE [dbo].[tblColumnsCustomDetail]
(
[CustomDetailID] [INT] NOT NULL IDENTITY(1, 1),
[CustomID] [INT] NOT NULL,
[FieldName] [VARCHAR] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FieldLocation] [INT] NOT NULL,
[FieldWidth] [INT] NOT NULL,
[FieldVisible] [INT] NOT NULL
) ON [PRIMARY];
ALTER TABLE [dbo].[tblColumnsCustomDetail] ADD CONSTRAINT [PK__tblColum__3482AA16383D70B5] PRIMARY KEY CLUSTERED ([CustomDetailID]) WITH (FILLFACTOR=85) ON [PRIMARY];
CREATE NONCLUSTERED INDEX [ixCustomID] ON [dbo].[tblColumnsCustomDetail] ([CustomID]) INCLUDE ([CustomDetailID], [FieldName], [FieldLocation], [FieldWidth], [FieldVisible]) ON [PRIMARY];
ALTER TABLE [dbo].[tblColumnsCustomDetail] ADD CONSTRAINT [FK_tblColumnsCustomDetail_tblColumnsCustom] FOREIGN KEY ([CustomID]) REFERENCES [dbo].[tblColumnsCustom] ([CustomID]) ON DELETE CASCADE;

SET IDENTITY_INSERT tblColumnsCustom ON;
INSERT INTO tblColumnsCustom(CustomID, UserID, CustomName, DefaultView, LayoutDescription)
VALUES
( 1, 1, 'Default', 1, NULL ),
( 2, 2, 'Default', 1, NULL ),
( 3, 2, 'New', 0, 'description' );
SET IDENTITY_INSERT tblColumnsCustom OFF;

SET IDENTITY_INSERT tblColumnsCustomDetail ON;
INSERT INTO tblColumnsCustomDetail(CustomDetailID, CustomID, FieldName, FieldLocation, FieldWidth, FieldVisible)
VALUES
( 1, 2, 'PayerPayment', 0, 50, 1 ), 
( 2, 2, 'ActualPatientID', 1, 100, 1 ), 
( 3, 2, 'Name_PatientName', 2, 150, 1 ), 
( 4, 2, 'PaymentStatus', 3, 100, 1 ), 
( 5, 2, 'Name_ContractID', 4, 200, 1 ), 
( 6, 2, 'PayerID', 5, 100, 1 ), 
( 7, 2, 'ServiceCode', 6, 100, 1 ), 
( 8, 2, 'AdmissionDate', 7, 100, 1 ), 
( 9, 2, 'DischargeDate', 8, 100, 1 ), 
( 10, 2, 'BillingDate', 9, 100, 1 ),
( 11, 1, 'PayerPayment', 0, 50, 1 );
SET IDENTITY_INSERT tblColumnsCustomDetail OFF;

DROP TABLE IF EXISTS RevCPTDetail;
CREATE TABLE RevCPTDetail ( [PatientRevenueId] int, [patientid] int, [RevenueCode] varchar(100), [RevCodeDescription] varchar(255), [RevCodeId] int, [RevCodeTransId] varchar(155), [RevServiceDate] datetime, [RevUnits] decimal(11,3), [RevCharges] money, [RevDeniedCharges] money, [RevExcludedCharges] money, [RevNonBilledCharges] money, [RevCost] money, [PatientCPTId] int, [CPTCode] varchar(9), [CPTDescr] varchar(255), [Mod1] varchar(10), [CPTRevCode] varchar(30), [ServiceDate] datetime, [Units] decimal(11,3), [Charges] money, [Rate] int, [ExpectedReimburse] int, [Service] int, [Method] int, [PPC] int, [Terms] int, [SurgOrder] int, [ExcludedCharges] money, [NonBilledCharges] money, [DeniedCharges] money, [Cost] money, [Mod2] varchar(10), [CPTTransId] varchar(155), [ServiceType] varchar(1), [PhysicianID] varchar(100), [PhysicianCodeLink] int, [ServiceLocation] varchar(10), [PrimaryDeniedCharges] money, [P1ActiveDenialReasonCodes] varchar(max), [P1ActiveDenialRemarkCodes] varchar(max), [BillingProviderNPI] varchar(10), [ICD9D] varchar(10), [ICD9D_2] varchar(10), [ICD9D_3] varchar(10), [ICD9D_4] varchar(10), [ICD9D_5] varchar(10), [ICDRevisionNumber] int );
INSERT INTO RevCPTDetail
VALUES
( 8141768, 65245453, '0320', 'Radiology - Diagnostic', 16145, '032071020PDH20161025', N'2016-10-25T00:00:00', 1.000, 360.0000, 0.0000, 0.0000, 0.0000, 0.0000, 9470504, '71020', 'Chest x-ray 2vw frontal&latl', 'PD', '0320', N'2016-10-25T00:00:00', 1.000, 360.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, 0.0000, NULL, '032071020PDH20161025', 'H', '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL ), 
( 8141769, 65245453, '0730', 'EKG/ECG', 16344, '073093005PDH20161025', N'2016-10-25T00:00:00', 1.000, 635.0000, 0.0000, 0.0000, 0.0000, 0.0000, 9470505, '93005', 'Electrocardiogram tracing', 'PD', '0730', N'2016-10-25T00:00:00', 1.000, 635.0000, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0.0000, 0.0000, 0.0000, 0.0000, NULL, '073093005PDH20161025', 'H', '', 0, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL );

DROP TABLE IF EXISTS [dbo].[ChargeCodeDetails];
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
) ON [PRIMARY];
ALTER TABLE [dbo].[ChargeCodeDetails] ADD CONSTRAINT [PK__chargeCode__EC6CB456346CDFD1] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR=85) ON [PRIMARY];
SET IDENTITY_INSERT [ChargeCodeDetails] ON;
INSERT [dbo].[ChargeCodeDetails] ([Id], [ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (1, N'65245453', N'2824320', 4299, N'TC ELECTROCARDIOGRAM-EKG, TRACING', 0.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 0, 0, N'93005', N'2282', 17031, N'0730', 16344, CAST(N'2016-10-27T00:00:00.000' AS DateTime), N'282432020161027', 0.0000, 0, NULL, NULL);
INSERT [dbo].[ChargeCodeDetails] ([Id], [ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (2, N'65245453', N'2824320', 4299, N'TC ELECTROCARDIOGRAM-EKG, TRACING', 0.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 0, 0, N'93005', N'2282', 17031, N'0730', 16344, CAST(N'2016-10-31T00:00:00.000' AS DateTime), N'282432020161031', 0.0000, 0, NULL, NULL);
INSERT [dbo].[ChargeCodeDetails] ([Id], [ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (3, N'65245453', N'2824320', 4299, N'TC ELECTROCARDIOGRAM-EKG, TRACING', 635.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 1, 0, N'93005', N'2282', 17031, N'0730', 16344, CAST(N'2016-10-25T00:00:00.000' AS DateTime), N'282432020161025', 0.0000, 0, NULL, NULL);
INSERT [dbo].[ChargeCodeDetails] ([Id], [ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (4, N'65245453', N'4230000', 7812, N'TC CHEST, 2 VIEWS.', 0.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 0, 0, N'71020', N'2523', 17059, N'0320', 16145, CAST(N'2016-10-27T00:00:00.000' AS DateTime), N'423000020161027', 0.0000, 0, NULL, NULL);
INSERT [dbo].[ChargeCodeDetails] ([Id], [ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (5, N'65245453', N'4230000', 7812, N'TC CHEST, 2 VIEWS.', 0.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 0, 0, N'71020', N'2523', 17059, N'0320', 16145, CAST(N'2016-10-31T00:00:00.000' AS DateTime), N'423000020161031', 0.0000, 0, NULL, NULL);
INSERT [dbo].[ChargeCodeDetails] ([Id], [ActualPatientID], [Code], [CodeId], [Description], [charges], [Cost], [DeniedCharges], [ExcludedCharges], [FacilityID], [PatientID], [Units], [CareType], [CPT], [DeptCode], [DeptID], [RevenueCode], [RevenueID], [ServiceDate], [TransactionID], [NonBillCharges], [DeniedUnits], [PostingDate], [PostedBy]) VALUES (6, N'65245453', N'4230000', 7812, N'TC CHEST, 2 VIEWS.', 360.0000, 0.0000, 0.0000, 0.0000, N'1', 1, 1, 0, N'71020', N'2523', 17059, N'0320', 16145, CAST(N'2016-10-25T00:00:00.000' AS DateTime), N'423000020161025', 0.0000, 0, NULL, NULL);
SET IDENTITY_INSERT [ChargeCodeDetails] OFF;

DROP TABLE IF EXISTS [dbo].[ClaimDetail];
CREATE TABLE [dbo].[ClaimDetail](
	[ID] [int] NULL,
	[iClaimId] [int] NULL,
	[LineNumber] [int] NULL,
	[RevenueCode] [varchar](50) NULL,
	[CPTCode] [varchar](50) NULL,
	[CPT_Description] [varchar](255) NULL,
	[ServiceDate] [date] NULL,
	[Quantity] [decimal](11, 3) NULL,
	[QuantityTypeID] [int] NULL,
	[QuantityTypeCode] [varchar](100) NULL,
	[QuantityTypeDescr] [varchar](250) NULL,
	[TotalCharges] [decimal](18, 2) NULL,
	[NonCoveredCharges] [decimal](18, 2) NULL,
	[CreatedDate] [datetime] NULL,
	[Modifier] [varchar](100) NULL
) ON [PRIMARY];
DROP TABLE IF EXISTS [dbo].[ClaimHistory];
CREATE TABLE [dbo].[ClaimHistory](
	[ID] [int] NULL,
	[PatientID] [int] NULL,
	[BillingDate] [date] NULL,
	[BillType] [varchar](5) NULL,
	[ActualPatientId] [varchar](100) NULL,
	[ClaimNumber] [varchar](100) NULL,
	[StatementFromDate] [date] NULL,
	[StatementToDate] [date] NULL,
	[TotalCharges] [decimal](18, 2) NULL,
	[TotalNonCoveredCharges] [decimal](18, 2) NULL,
	[PatientEstAmtDue] [decimal](18, 2) NULL,
	[AdmitDate] [date] NULL,
	[AdmitHour] [int] NULL,
	[AdmitTypeID] [int] NULL,
	[AdmitTypeCode] [varchar](100) NULL,
	[AdmitTypeDescr] [varchar](500) NULL,
	[AdmitSourceID] [int] NULL,
	[AdmitSourceCode] [varchar](100) NULL,
	[AdmitSourceDescr] [varchar](250) NULL,
	[DischargeHour] [int] NULL,
	[DischargeStatusID] [int] NULL,
	[DischargeStatusCode] [varchar](100) NULL,
	[DischargeStatusDescr] [varchar](500) NULL,
	[AccidentState] [varchar](100) NULL,
	[PPSCode] [varchar](50) NULL,
	[AdmittingDiagnosisCode] [varchar](50) NULL,
	[ImportFile] [varchar](500) NULL,
	[ImportFileVersion] [varchar](100) NULL,
	[ImportFileTrace] [varchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[PrimaryPayer] [varchar](100) NULL,
	[DestinationPayer] [varchar](100) NULL,
	[DestinationPayerResp] [varchar](100) NULL,
	[ClaimFreqType] [varchar](100) NULL
) ON [PRIMARY];
DROP TABLE IF EXISTS [dbo].[RemitDetail];
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
) ON [PRIMARY];
DROP TABLE IF EXISTS [dbo].[RemitHistory];
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
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

INSERT [dbo].[ClaimDetail] ([ID], [iClaimId], [LineNumber], [RevenueCode], [CPTCode], [CPT_Description], [ServiceDate], [Quantity], [QuantityTypeID], [QuantityTypeCode], [QuantityTypeDescr], [TotalCharges], [NonCoveredCharges], [CreatedDate], [Modifier]) VALUES (1, 1, 1, N'0320', N'71020', N'Chest x-ray 2vw frontal&latl', CAST(N'2016-10-25' AS Date), CAST(1.000 AS Decimal(11, 3)), 1, N'UN', N'Units', CAST(360.00 AS Decimal(18, 2)), NULL, CAST(N'2016-11-02T02:13:12.703' AS DateTime), N'00');
INSERT [dbo].[ClaimDetail] ([ID], [iClaimId], [LineNumber], [RevenueCode], [CPTCode], [CPT_Description], [ServiceDate], [Quantity], [QuantityTypeID], [QuantityTypeCode], [QuantityTypeDescr], [TotalCharges], [NonCoveredCharges], [CreatedDate], [Modifier]) VALUES (2, 1, 2, N'0730', N'93005', N'Electrocardiogram tracing', CAST(N'2016-10-25' AS Date), CAST(1.000 AS Decimal(11, 3)), 1, N'UN', N'Units', CAST(635.00 AS Decimal(18, 2)), NULL, CAST(N'2016-11-02T02:13:12.720' AS DateTime), N'00');
INSERT [dbo].[ClaimHistory] ([ID], [PatientID], [BillingDate], [BillType], [ActualPatientId], [ClaimNumber], [StatementFromDate], [StatementToDate], [TotalCharges], [TotalNonCoveredCharges], [PatientEstAmtDue], [AdmitDate], [AdmitHour], [AdmitTypeID], [AdmitTypeCode], [AdmitTypeDescr], [AdmitSourceID], [AdmitSourceCode], [AdmitSourceDescr], [DischargeHour], [DischargeStatusID], [DischargeStatusCode], [DischargeStatusDescr], [AccidentState], [PPSCode], [AdmittingDiagnosisCode], [ImportFile], [ImportFileVersion], [ImportFileTrace], [CreatedDate], [PrimaryPayer], [DestinationPayer], [DestinationPayerResp], [ClaimFreqType]) VALUES (1, 1, CAST(N'2016-11-01' AS Date), N'131', N'65245453', NULL, CAST(N'2016-10-25' AS Date), CAST(N'2016-10-25' AS Date), CAST(995.00 AS Decimal(18, 2)), NULL, NULL, CAST(N'2016-10-25' AS Date), NULL, 3, N'3', N'Elective, permitting adequate time to schedule accommodations', 1, N'1', N'Physician referral or Normal Delivery.', NULL, 1, N'01', N'Discharged to home or self care (routine discharge)', NULL, NULL, NULL, N'837I_20161101_245.txt', N'5010I', N'20161101.0001.2504', CAST(N'2016-11-02T02:13:12.703' AS DateTime), N'Payer Name', N'Other Payer Name', N'Primary', N'Admit thru Discharge');
INSERT [dbo].[RemitDetail] ([ID], [RemitID], [Seq], [LineNumber], [ServiceStartDate], [ServiceEndDate], [RevenueCode], [CPTCode], [CPT_Description], [OtherCode], [Qty], [PaidQty], [ActualAllowedAmt], [DeductionAmt], [CreatedDate], [Charges], [AdjRsnCode], [RemarkCode], [DenialAmt]) VALUES (1, 1, 1, N'001', CAST(N'2016-10-25' AS Date), CAST(N'2016-10-25' AS Date), N'0320', NULL, NULL, NULL, CAST(1.000 AS Decimal(11, 3)), CAST(1.000 AS Decimal(11, 3)), CAST(126.00 AS Decimal(18, 2)), NULL, CAST(N'2016-11-24T02:26:40.413' AS DateTime), 635.0000, N'45', NULL, 0.0000)
INSERT [dbo].[RemitDetail] ([ID], [RemitID], [Seq], [LineNumber], [ServiceStartDate], [ServiceEndDate], [RevenueCode], [CPTCode], [CPT_Description], [OtherCode], [Qty], [PaidQty], [ActualAllowedAmt], [DeductionAmt], [CreatedDate], [Charges], [AdjRsnCode], [RemarkCode], [DenialAmt]) VALUES (2, 1, 2, N'002', CAST(N'2016-10-25' AS Date), CAST(N'2016-10-25' AS Date), N'0730', NULL, NULL, NULL, CAST(1.000 AS Decimal(11, 3)), CAST(1.000 AS Decimal(11, 3)), CAST(222.25 AS Decimal(18, 2)), NULL, CAST(N'2016-11-24T02:26:40.430' AS DateTime), 360.0000, N'45', NULL, 0.0000)
INSERT [dbo].[RemitHistory] ([ID], [PatientID], [ActualPatientID], [ClaimNumber], [BillType], [ServiceCode], [ClaimFilingIndicatorID], [ClaimFilingIndicatorCode], [ClaimFilingIndicatorNotes], [ClaimFilingIndicatorTitle], [ContractCode], [StatementFromDate], [StatementToDate], [ClaimStatusID], [ClaimStatusCode], [ClaimStatusDescr], [ClaimPaymentGroupID], [PatientResponsibilityAmt], [PatientPaidAmt], [CoverageAmt], [DiscountAmt], [InterestAmt], [ActualCoveredQty], [ActualCoinsuredQty], [EffectiveDate], [ProductionDate], [BatchCode], [CheckNumber], [CompanyID], [CompanySubID], [ReportingStatus], [ReceivedDate], [Name], [PayeeID], [FederalTaxpayerID], [ImportResults_IsInPlay], [ImportResults_ReasonNotInPlay], [ImportResults_PositionUsed], [ImportResults_LastUpdatedDate], [PatientControlNumber], [ClaimFrequencyType], [AdjAmt], [DenialAmt], [AdjRsnCodes], [RemarkCodes]) VALUES (1, 1, N'65245453', N'16307E12324', NULL, N'Outpatient', 10, N'HM', NULL, N'Health Maintenance Organization', NULL, NULL, NULL, 1, N'1', N'Processed as Primary', 19751249, NULL, NULL, CAST(348.25 AS Decimal(18, 2)), NULL, NULL, NULL, NULL, CAST(N'2016-11-14' AS Date), CAST(N'2016-11-03' AS Date), NULL, N'081000605386671', N'1760486264', NULL, NULL, CAST(N'2016-11-03' AS Date), N'My Facility', NULL, N'123', 1, NULL, 1, CAST(N'2017-07-08T20:11:10.000' AS DateTime), N'6610602400', NULL, 0.0000, 0.0000, NULL, NULL)

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
( '9', 'acAuditResultFlag', 'Referral - Internal' );

DROP TABLE IF EXISTS [dbo].[tblEOR];
CREATE TABLE [dbo].[tblEOR] (
	[PatientID] INT,
	[EOB] VARCHAR(MAX)
) ON [PRIMARY];
INSERT [dbo].[tblEOR] ([PatientID], [EOB]) VALUES ( 3475203, 'Episodic

10/25/2016

(Ancillary) Expected reimbursement includes a percentage of remaining ancillary charges as follows: $995.00 x 35.00 %.

Total Expected Reimbursement: $348.25' );

DROP TABLE IF EXISTS [dbo].[tblDetailReimb];
CREATE TABLE [dbo].[tblDetailReimb] (
	[CodeReimID] int,
	[ActualPatientID] varchar(30),
	[AdjTotalCharges] money,
	[Code] varchar(30),
	[CodeType] varchar(30),
	[ExpectedReimburse] money,
	[FacilityID] varchar(30),
	[PatientID] int,
	[Service] varchar(50),
	[Units] int,
	[CaseType] varchar(30),
	[CalcSurgical] int,
	[TermsDiff] money,
	[ServiceDate] datetime,
	[OutlierAmt] money,
	[TermsDiffFlag] tinyint,
	[LatePayDiff] money,
	[ReimbAtLowOfChgsOrTerms] int,
	[ServiceType] int,
	[ReimbursementMethod] int,
	[PPC] int,
	[Rate] int
) ON [PRIMARY];
INSERT [dbo].[tblDetailReimb] ([CodeReimID], [ActualPatientID], [AdjTotalCharges], [Code], [CodeType], [ExpectedReimburse], [FacilityID], [PatientID], [Service], [Units], [CaseType], [CalcSurgical], [TermsDiff], [ServiceDate], [OutlierAmt], [TermsDiffFlag], [LatePayDiff], [ReimbAtLowOfChgsOrTerms], [ServiceType], [ReimbursementMethod], [PPC], [Rate]) VALUES ( 1, '66106024', 995.0000, 'Ancillary', 'Ancillary', 348.2500, '1', 3475203, 'Ancillary', 0, 'Ancillary', 0, 0.0000, N'2016-10-25T00:00:00', 0.0000, 0, 0.0000, 0, NULL, NULL, NULL, NULL )

DROP TABLE IF EXISTS [dbo].[AuditHistory];
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
) ON [PRIMARY];
INSERT [dbo].[AuditHistory] ([PatientID], [Name_Payer1AuditFlag], [AssignedAuditor], [ImportDate], [Name_AuditorID], [EventDate], [DaysElapsed], [ResponsbilityType], [VarianceCategory], [AssignedReviewer]) VALUES (3475203, N'Internal Review', N'Unassigned', CAST(N'2016-04-08T10:57:24.253' AS DateTime), N'User Name ABC', CAST(N'2016-04-08T00:00:00' AS Date), 0, N'Payer 1', N'Posting Issue', N'Unassigned');

DROP TABLE IF EXISTS [dbo].[tblPaymentDetail];
CREATE TABLE [dbo].[tblPaymentDetail] (
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
) ON [PRIMARY];

INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Payer 1', N'Contractual', CAST(N'2016-11-03T01:55:38.687' AS DateTime), CAST(N'2016-11-01T00:00:00.000' AS DateTime), 646.7500, 0.0000, N'6010', N'CONTRACTUAL DISCOUNT (INSURANCE)', 1, N'3077', N'', NULL, 40, 491, 16559, 12, NULL, NULL, 11, NULL, NULL, 3475203);
INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Payer 1', N'Contractual', CAST(N'2016-11-25T01:29:43.673' AS DateTime), CAST(N'2016-11-23T00:00:00.000' AS DateTime), -646.7500, 0.0000, N'6010', N'CONTRACTUAL DISCOUNT (INSURANCE)', 1, N'3099', N'', NULL, 40, 758, 16559, 12, NULL, NULL, 11, NULL, NULL, 3475203);
INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Payer 1', N'Contractual', CAST(N'2016-11-25T01:29:43.707' AS DateTime), CAST(N'2016-11-23T00:00:00.000' AS DateTime), 646.7500, 0.0000, N'6010', N'CONTRACTUAL DISCOUNT (INSURANCE)', 1, N'3099', N'', NULL, 40, 758, 16559, 12, NULL, NULL, 11, NULL, NULL, 3475203);
INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Payer 1', N'Actual Payment', CAST(N'2016-11-25T01:29:43.597' AS DateTime), CAST(N'2016-11-23T00:00:00.000' AS DateTime), 348.2500, 0.0000, N'5013', N'INSURANCE PAYMENT (INSURANCE)', 1, N'3099', N'', NULL, 40, 758, 16536, 15, NULL, NULL, 11, NULL, NULL, 3475203);
INSERT [dbo].[tblPaymentDetail] ([Entity], [IncrementName], [DateAdded], [PostingDate], [Amount], [ExcludedAmount], [AdjustCode], [AdjustCodeDesc], [SumValue], [BatchCode], [ClaimNumber], [CheckNumber], [PayerCodeLink], [ContractID], [AdjustCodeLink], [AdjustType], [InterimBillStartDate], [InterimBillEndDate], [PayerType], [PostedBy], [AdjustmentServiceDate], [PatientID]) VALUES (N'Other Payer', N'Miscellaneous', CAST(N'2016-11-25T01:29:43.597' AS DateTime), CAST(N'2016-11-23T00:00:00.000' AS DateTime), 100.0000, 0.0000, N'5013', N'INSURANCE PAYMENT (INSURANCE)', 1, N'3099', N'', NULL, 0, 758, 16536, 15, NULL, NULL, 11, NULL, NULL, 3475203);

DROP TABLE IF EXISTS [dbo].[tblPaymentSummary];
CREATE TABLE [dbo].[tblPaymentSummary] (
	[PatientID] INT,
	[ActualPatientID] varchar(30),
	[PayerNumber] int,
	[PayerNumberLabel] varchar(30),
	[PayerID] varchar(100),
	[PayerName] varchar(200),
	[ContractName] varchar(200),
	[PaymentDate] date,
	[EstPayAllowable] money,
	[ActualPayment] money,
	[WriteOff] money,
	[BalanceDue] money
) ON [PRIMARY];
INSERT [dbo].[tblPaymentSummary] ([PatientID], [ActualPatientID], [PayerNumber], [PayerNumberLabel], [PayerID], [PayerName], [ContractName], [PaymentDate], [EstPayAllowable], [ActualPayment], [WriteOff], [BalanceDue]) VALUES ( 3475203, '66106024', 0, 'Patient', 'N/A', 'N/A', 'N/A', NULL, 0.0000, 0.0000, 0.0000, 0.0000 );
INSERT [dbo].[tblPaymentSummary] ([PatientID], [ActualPatientID], [PayerNumber], [PayerNumberLabel], [PayerID], [PayerName], [ContractName], [PaymentDate], [EstPayAllowable], [ActualPayment], [WriteOff], [BalanceDue]) VALUES ( 3475203, '66106024', 1, 'Payer 1', '2000007', 'Payer Name', 'Contract Name', N'2016-11-23T00:00:00', 348.2500, 348.2500, 0.0000, 0.0000 );
INSERT [dbo].[tblPaymentSummary] ([PatientID], [ActualPatientID], [PayerNumber], [PayerNumberLabel], [PayerID], [PayerName], [ContractName], [PaymentDate], [EstPayAllowable], [ActualPayment], [WriteOff], [BalanceDue]) VALUES ( 3475203, '66106024', 2, 'Payer 2', 'N/A', 'N/A', 'N/A', NULL, 0.0000, 0.0000, 0.0000, 0.0000 );
INSERT [dbo].[tblPaymentSummary] ([PatientID], [ActualPatientID], [PayerNumber], [PayerNumberLabel], [PayerID], [PayerName], [ContractName], [PaymentDate], [EstPayAllowable], [ActualPayment], [WriteOff], [BalanceDue]) VALUES ( 3475203, '66106024', 3, 'Payer 3', 'N/A', 'N/A', 'N/A', NULL, 0.0000, 0.0000, 0.0000, 0.0000 );
INSERT [dbo].[tblPaymentSummary] ([PatientID], [ActualPatientID], [PayerNumber], [PayerNumberLabel], [PayerID], [PayerName], [ContractName], [PaymentDate], [EstPayAllowable], [ActualPayment], [WriteOff], [BalanceDue]) VALUES ( 3475203, '66106024', 4, 'Payer 4', 'N/A', 'N/A', 'N/A', NULL, 0.0000, 0.0000, 0.0000, 0.0000 );
INSERT [dbo].[tblPaymentSummary] ([PatientID], [ActualPatientID], [PayerNumber], [PayerNumberLabel], [PayerID], [PayerName], [ContractName], [PaymentDate], [EstPayAllowable], [ActualPayment], [WriteOff], [BalanceDue]) VALUES ( 3475203, '66106024', 5, 'Other Payer', 'N/A', 'N/A', 'Contract Example 773', N'2016-11-23T00:00:00', 100.0000, 100.0000, 0.0000, 0.0000 );


DROP TABLE IF EXISTS [dbo].[tblICDCodeDetail];
CREATE TABLE [dbo].[tblICDCodeDetail] (
	[Id] int,
	[ActualPatientID] varchar(30),
	[FacilityID] varchar(30),
	[Code] varchar(10),
	[Type] varchar(1),
	[ICDRevisionNumber] int,
	[POA] varchar(1),
	[Rank] tinyint,
	[Description] varchar(255),
	[ServiceDate] datetime,
	[TransactionID] varchar(50),
	[PatientID] INT
) ON [PRIMARY];
INSERT [dbo].[tblICDCodeDetail] ([Id], [ActualPatientID], [FacilityID], [Code], [Type], [ICDRevisionNumber], [POA], [Rank], [Description], [ServiceDate], [TransactionID], [PatientID]) VALUES ( 1, '66106024', '1', 'Z01.810', 'D', 10, 'Y', 3, 'Encounter for preprocedural cardiovascular examination', NULL, 'Z01.81020161025', 3475203 );
INSERT [dbo].[tblICDCodeDetail] ([Id], [ActualPatientID], [FacilityID], [Code], [Type], [ICDRevisionNumber], [POA], [Rank], [Description], [ServiceDate], [TransactionID], [PatientID]) VALUES ( 2, '66106024', '1', 'R94.31', 'D', 10, 'Y', 2, 'Abnormal electrocardiogram [ECG] [EKG]', NULL, 'R94.3120161025', 3475203 );
INSERT [dbo].[tblICDCodeDetail] ([Id], [ActualPatientID], [FacilityID], [Code], [Type], [ICDRevisionNumber], [POA], [Rank], [Description], [ServiceDate], [TransactionID], [PatientID]) VALUES ( 3, '66106024', '1', 'Q21.0', 'D', 10, 'Y', 1, 'Ventricular septal defect', NULL, 'Q21.020161025', 3475203 );
