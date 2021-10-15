DELETE FROM [dbo].[tblAudits];
DELETE FROM [dbo].[tblViewRules] WHERE [ViewID]>1;
DELETE FROM [dbo].[tblUserViews] WHERE [ViewID]>1;
DELETE FROM [dbo].[tblUsers] WHERE [UserID]>1;

SET IDENTITY_INSERT [dbo].[tblUsers] ON;
INSERT INTO tblUsers(UserCounter, UserID, UserName, UserPassword, Role, ProductFlag, EMail, UserType, ProviderFlag, Supervisor, UserOrder, LockFlag, PasswordExpirationDate, SchFileType, SchZipFlag, ActiveFlag, EMailFlag, AdminAccount, FailedPasswordAttemptCount, IsOnline, PhoneNumber, Notes, Company, Title, OTP, OTPExpirationDate)
VALUES (2, 2, 'johndoe', '123456789', 'Site Admin', 0, 'john.doe@example.com', 1, 0, 0, 1, 0, N'2030-12-02T11:00:21.923', 'zip', 0, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL );

INSERT INTO tblUsers(UserCounter,UserID, UserName, UserPassword, Role, ProductFlag, EMail, UserType, ProviderFlag, Supervisor, UserOrder, LockFlag, PasswordExpirationDate, SchFileType, SchZipFlag, ActiveFlag, EMailFlag, AdminAccount, FailedPasswordAttemptCount, IsOnline, PhoneNumber, Notes, Company, Title, OTP, OTPExpirationDate)
VALUES (3, 3, 'mike', '12345', 'Account Management', 0, 'mike@example.com', 1, 0, 0, 1, 0, N'2030-12-02T11:00:21.923', 'zip', 0, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL );

INSERT INTO tblUsers(UserCounter,UserID, UserName, UserPassword, Role, ProductFlag, EMail, UserType, ProviderFlag, Supervisor, UserOrder, LockFlag, PasswordExpirationDate, SchFileType, SchZipFlag, ActiveFlag, EMailFlag, AdminAccount, FailedPasswordAttemptCount, IsOnline, PhoneNumber, Notes, Company, Title, OTP, OTPExpirationDate)
VALUES (4, 4, 'normal', '12345', 'Normal User', 0, 'normal@example.com', 1, 0, 0, 1, 0, N'2030-12-02T11:00:21.923', 'zip', 0, 0, 0, 1, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL );
SET IDENTITY_INSERT [dbo].[tblUsers] OFF;

SET IDENTITY_INSERT [dbo].[tblUserViews] ON;
INSERT INTO tblUserViews(ViewID, ViewName, ViewDesc, ViewOwner, CPTView, DefaultView, DefaultAuditor, DefaultFollowUp, DefaultStatus, DefaultRecordAge, DefaultRecordHidden)
VALUES
(2, 'All Name', 'All Desc', 2, 0, 1, 0, 0, 1, 0, 1),
(3, 'All', 'All', 3, 0, 0, 0, 0, 1, 0, 1),
(4, 'ComboCodeType Example', 'Test', 2, 0, 0, 0, 0, 1, 0, 1 );
SET IDENTITY_INSERT [dbo].[tblUserViews] OFF;


--	Note:	For a Values rule each selected field/criteria is stored as a row in tblViewRules.
--			In this example view I have selected 2 'ServiceCode' criteria; and 3 of the Payer1AuditFlag, PatFinClass & BillType rule types.
--	Note 2:	Please ignore the FacilityID field.  Populating it with a 1 as a default is fine for the scope of this project.
INSERT INTO tblViewRules(ViewID, FieldID, FacilityID, ValueID, Value, BeginRange, EndRange, Operand, Operand2)
VALUES
(4, 4, -1, 'Inpatient', 'Inpatient', NULL, NULL, 'IN', NULL ),				--	tblViewFields.FieldID is 4 for ServiceCode
(4, 4, -1, 'Outpatient', 'Outpatient', NULL, NULL, 'IN', NULL ), 
(4, 27, -1, '0', 'Pending', NULL, NULL, 'IN', NULL ),						--	tblViewFields.FieldID is 27 for Payer1AuditFlag/AuditFlagAll
(4, 27, -1, '2', 'Closed', NULL, NULL, 'IN', NULL ), 
(4, 27, -1, '4', 'Internal Review', NULL, NULL, 'IN', NULL ), 
(4, 108, 1, '100', 'Blue Cross Blue Shield', NULL, NULL, 'IN', NULL ),		--	tblViewFields.FieldID is 108 for PatFinClass
(4, 108, 1, '102', 'Aetna', NULL, NULL, 'IN', NULL ), 
(4, 108, 1, '103', 'Cigna', NULL, NULL, 'IN', NULL ), 
(4, 251, 1, '111', '111', NULL, NULL, 'IN', NULL ),							--	tblViewFields.FieldID is 251 for BillType
(4, 251, 1, '115', '115', NULL, NULL, 'IN', NULL ), 
(4, 251, 1, '132', '132', NULL, NULL, 'IN', NULL );
