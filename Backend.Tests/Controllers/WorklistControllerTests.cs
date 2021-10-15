using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMMC.Exceptions;
using PMMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMMC.UnitTests.Controllers
{
    /// <summary>
    /// Tests for worklist controller
    /// </summary>
    [TestClass]
    public class WorklistControllerTests : BaseTest
    {
        /// <summary>
        /// Test get all worklist layouts
        /// </summary>
        [TestMethod]
        public void TestGetAllWorklistLayouts()
        {
            SiteAdminLogin();
            var result = _worklistController.GetAllWorklistLayouts();
            AssertResult(result);
        }

        /// <summary>
        /// Test get worklist layouts by id
        /// </summary>
        [TestMethod]
        public void TestGetWorklistLayoutById()
        {
            SiteAdminLogin();
            var result = _worklistController.GetWorklistLayoutById(2);
            AssertResult(result);
        }

        /// <summary>
        /// Test get worklist layouts by id
        /// </summary>
        [TestMethod]
        public void TestGetWorklistLayoutById2()
        {
            SiteAdminLogin();
            var result = _worklistController.GetWorklistLayoutById(3);
            AssertResult(result);
        }

        /// <summary>
        /// Test get worklist layouts by id wrong userId
        /// </summary>
        [TestMethod]
        public void TestGetWorklistLayoutById3()
        {
            AccountManagementLogin();
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _worklistController.GetWorklistLayoutById(3), "Worklist layout by id='3' not belongs to the current user");
        }

        /// <summary>
        /// Test get worklist layouts by id not found
        /// </summary>
        [TestMethod]
        public void TestGetWorklistLayoutById4()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.GetWorklistLayoutById(99), "Worklist layout by id='99' not found");
        }

        /// <summary>
        /// Test create worklist layout
        /// </summary>
        [TestMethod]
        public void TestCreateWorklistLayout()
        {
            SiteAdminLogin();
            var result = _worklistController.CreateWorklistLayout(PrepareWorklistLayout());
            AssertResult(result);
        }

        /// <summary>
        /// Test create worklist layout without columns
        /// </summary>
        [TestMethod]
        public void TestCreateWorklistLayout2()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Columns.Clear();
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.CreateWorklistLayout(worklistLayout), "Columns array cannot be empty");
        }

        /// <summary>
        /// Test create worklist layout with existent name
        /// </summary>
        [TestMethod]
        public void TestCreateWorklistLayout3()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Name = "New";
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.CreateWorklistLayout(worklistLayout), "Worklist layout with name='New' exists in database");
        }

        /// <summary>
        /// Test create worklist layout
        /// </summary>
        [TestMethod]
        public void TestCreateWorklistLayout4()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.IsDefault = false;
            worklistLayout.Columns.Add(new WorklistColumnLayout
            {
                FieldName = "PayerPayment",
                Location = 2,
                IsVisible = false,
                Width = 50
            });
            var result = _worklistController.CreateWorklistLayout(worklistLayout);
            AssertResult(result);
        }

        /// <summary>
        /// Test create worklist layout with existent location
        /// </summary>
        [TestMethod]
        public void TestCreateWorklistLayout5()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Columns.Add(new WorklistColumnLayout
            {
                FieldName = "PayerPayment",
                Location = 1,
                IsVisible = true,
                Width = 50
            });
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.CreateWorklistLayout(worklistLayout), "Columns locations must be unique");
        }

        /// <summary>
        /// Test create worklist layout with existent column name
        /// </summary>
        [TestMethod]
        public void TestCreateWorklistLayout6()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Columns.Add(new WorklistColumnLayout
            {
                FieldName = "PaymentStatus",
                Location = 2,
                IsVisible = true,
                Width = 50
            });
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.CreateWorklistLayout(worklistLayout), "Columns field names must be unique");
        }

        /// <summary>
        /// Test create worklist layout with not found column name
        /// </summary>
        [TestMethod]
        public void TestCreateWorklistLayout7()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Columns.Add(new WorklistColumnLayout
            {
                FieldName = "not found",
                Location = 2,
                IsVisible = true,
                Width = 50
            });
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.CreateWorklistLayout(worklistLayout), "Columns names: not found are not valid");
        }

        /// <summary>
        /// Test create worklist layout with not visible column
        /// </summary>
        [TestMethod]
        public void TestCreateWorklistLayout8()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Columns[0].IsVisible = false;
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.CreateWorklistLayout(worklistLayout), "At least one column must be visible");
        }

        /// <summary>
        /// Test update worklist layout
        /// </summary>
        [TestMethod]
        public void TestUpdateWorklistLayout()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            _worklistController.UpdateWorklistLayout(2, worklistLayout);
            var updated = _worklistController.GetWorklistLayoutById(2);
            AssertResult(updated);
        }

        /// <summary>
        /// Test update worklist layout
        /// </summary>
        [TestMethod]
        public void TestUpdateWorklistLayout2()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.IsDefault = false;
            worklistLayout.Columns.Add(new WorklistColumnLayout
            {
                FieldName = "PayerPayment",
                Location = 2,
                IsVisible = false,
                Width = 50
            });
            _worklistController.UpdateWorklistLayout(3, worklistLayout);
            var updated = _worklistController.GetWorklistLayoutById(3);
            AssertResult(updated);
        }

        /// <summary>
        /// Test update worklist layout wrong id
        /// </summary>
        [TestMethod]
        public void TestUpdateWorklistLayout3()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _worklistController.UpdateWorklistLayout(1, worklistLayout), "Worklist layout by id='1' not belongs to the current user");
        }

        /// <summary>
        /// Test update worklist layout not found
        /// </summary>
        [TestMethod]
        public void TestUpdateWorklistLayout4()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.UpdateWorklistLayout(999, worklistLayout), "Worklist layout by id='999' not found");
        }

        /// <summary>
        /// Test update worklist layout
        /// </summary>
        [TestMethod]
        public void TestUpdateWorklistLayout5()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Columns[0].Id = 1;
            _worklistController.UpdateWorklistLayout(2, worklistLayout);
            var updated = _worklistController.GetWorklistLayoutById(2);
            AssertResult(updated);
        }

        /// <summary>
        /// Test update worklist layout not found column id
        /// </summary>
        [TestMethod]
        public void TestUpdateWorklistLayout6()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Columns[0].Id = 999;
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.UpdateWorklistLayout(2, worklistLayout), "Worklist columns layout with layout id = 2 and column ids = '999' not found in database");
        }

        /// <summary>
        /// Test update worklist layout not found column id
        /// </summary>
        [TestMethod]
        public void TestUpdateWorklistLayout7()
        {
            SiteAdminLogin();
            var worklistLayout = PrepareWorklistLayout();
            worklistLayout.Name = "New";
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.UpdateWorklistLayout(2, worklistLayout), "Worklist layout with name='New' exists in database");
        }

        /// <summary>
        /// Test delete worklist layout
        /// </summary>
        [TestMethod]
        public void TestDeleteWorklistLayout()
        {
            SiteAdminLogin();
            _worklistController.DeleteWorklistLayout(2);
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.GetWorklistLayoutById(2), "Worklist layout by id='2' not found");
        }

        /// <summary>
        /// Test delete worklist layout
        /// </summary>
        [TestMethod]
        public void TestDeleteWorklistLayout2()
        {
            SiteAdminLogin();
            _worklistController.DeleteWorklistLayout(3);
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.GetWorklistLayoutById(3), "Worklist layout by id='3' not found");
        }

        /// <summary>
        /// Test get revcpt codes
        /// </summary>
        [TestMethod]
        public void TestGetRevCptCodes()
        {
            SiteAdminLogin();
            var codes = _worklistController.GetRevCptCodes(65245453);
            AssertResult(codes);
        }

        /// <summary>
        /// Test get revcpt codes with not found patientId
        /// </summary>
        [TestMethod]
        public void TestGetRevCptCodes2()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.GetRevCptCodes(999), "Patient with patientId='999' not found");
        }

        /// <summary>
        /// Test get charge codes
        /// </summary>
        [TestMethod]
        public void TestGetChargeCodes()
        {
            SiteAdminLogin();
            var codes = _worklistController.GetChargeCodes(65245453);
            AssertResult(codes);
        }

        /// <summary>
        /// Test create charge codes
        /// </summary>
        [TestMethod]
        public void TestCreateChargeCode()
        {
            SiteAdminLogin();
            var codes = _worklistController.CreateChargeCode(65245453, PrepareChargeCodeDetail());
            AssertResult(codes);
        }

        /// <summary>
        /// Test update charge code
        /// </summary>
        [TestMethod]
        public void TestUpdateChargeCode()
        {
            SiteAdminLogin();
            _worklistController.UpdateChargeCode(65245453, 1, PrepareChargeCodeDetail());
            var updated = _worklistController.GetChargeCodes(65245453);
            AssertResult(updated.Where(x => x.Id == 1).First());
        }

        /// <summary>
        /// Test update charge code with not found charge code
        /// </summary>
        [TestMethod]
        public void TestUpdateChargeCode2()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.UpdateChargeCode(65245453, 999, PrepareChargeCodeDetail()), "Charge code by patientId='65245453' and id='999' not found");
        }

        /// <summary>
        /// Test delete charge code
        /// </summary>
        [TestMethod]
        public void TestUpdateChargeCode3()
        {
            SiteAdminLogin();
            _worklistController.DeleteChargeCode(65245453, 1);
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.DeleteChargeCode(65245453, 1), "Charge code by patientId='65245453' and id='1' not found");
        }

        /// <summary>
        /// Test get claims history
        /// </summary>
        [TestMethod]
        public void TestGetClaimsHistory()
        {
            SiteAdminLogin();
            var claims = _worklistController.GetClaimsHistory(65245453);
            AssertResult(claims);
        }

        /// <summary>
        /// Test professional claims
        /// </summary>
        [TestMethod]
        public void TestGetProfessionalClaims()
        {
            SiteAdminLogin();
            var claims = _worklistController.GetProfessionalClaims(65245453);
            AssertResult(claims);
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts()
        {
            SiteAdminLogin();
            var result = _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 0,
                FollowUp = 0,
                Status = 0,
                AccountAge = 0,
                HiddenRecords = 0,
                LayoutId = 2,
                Limit = 10,
                Offset = 0,
                SortBy = "BillingDate",
                SortOrder = "Asc"
            });
            AssertResult(result);
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts2()
        {
            SiteAdminLogin();
            var result = _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 1,
                FollowUp = 1,
                Status = 1,
                AccountAge = 1,
                HiddenRecords = 1
            });
            AssertResult(result);
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts3()
        {
            SiteAdminLogin();
            var viewRules = new List<ViewRule> {
                new ViewRule
                {
                    FieldId = 8,
                    Value = "15",
                    Operand = "EQUAL TO"
                },
                new ViewRule
                {
                    FieldId = 9,
                    Value = "10",
                    Operand = "GREATER THAN"
                },
                new ViewRule
                {
                    FieldId = 10,
                    Value = "10",
                    Operand = "LESS THAN"
                },
                new ViewRule
                {
                    FieldId = 11,
                    Value = "10",
                    BeginRange = "10",
                    EndRange = "20",
                    Operand = "BETWEEN"
                }
                ,
                new ViewRule
                {
                    FieldId = 72,
                    Value = "10",
                    BeginRange = "12/12/2012 15:15",
                    EndRange = "12/12/2012 15:15",
                    Operand = "BETWEEN"
                },
                new ViewRule
                {
                    FieldId = 19,
                    Value = "test",
                    Operand = "STARTS WITH"
                },
                new ViewRule
                {
                    FieldId = 19,
                    Value = "test",
                    Operand = "ENDS WITH"
                },
                new ViewRule
                {
                    FieldId = 19,
                    Value = "test",
                    Operand = "CONTAINS"
                },
                new ViewRule
                {
                    FieldId = 2,
                    Value = "test",
                    ValueId = "5631",
                    Operand = "NOT IN"
                },
                new ViewRule
                {
                    FieldId = 2,
                    Value = "test2",
                    ValueId = "5631",
                    Operand = "NOT IN"
                }
            };
            _viewsController.CreateViewRule(2, viewRules);
            var result = _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 2,
                FollowUp = 2,
                Status = 0,
                AccountAge = 2,
                HiddenRecords = 0
            });
            AssertResult(result);
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts4()
        {
            SiteAdminLogin();
            var result = _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 1,
                FollowUp = 3,
                Status = 1,
                AccountAge = 3,
                HiddenRecords = 1
            });
            AssertResult(result);
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts5()
        {
            SiteAdminLogin();
            var result = _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 1,
                FollowUp = 4,
                Status = 1,
                AccountAge = 4,
                HiddenRecords = 1
            });
            AssertResult(result);
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts6()
        {
            SiteAdminLogin();
            var result = _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 1,
                FollowUp = 4,
                Status = 1,
                AccountAge = 5,
                HiddenRecords = 1
            });
            AssertResult(result);
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts7()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 1,
                FollowUp = 4,
                Status = 1,
                AccountAge = 5,
                HiddenRecords = 1,
                SortBy = "invalid"
            }), "Sort By='invalid' is not valid");
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts8()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 1,
                FollowUp = 4,
                Status = 1,
                AccountAge = 5,
                HiddenRecords = 1,
                SortOrder = "invalid"
            }), "Sort Order='invalid' is not valid");
        }

        /// <summary>
        /// Test search worklist accounts
        /// </summary>
        [TestMethod]
        public void TestSearchWorklistAccounts9()
        {
            SiteAdminLogin();
            var layout = PrepareWorklistLayout();
            layout.IsDefault = false;
            _worklistController.UpdateWorklistLayout(2, layout);
            var result = _worklistController.SearchWorklistAccounts(new WorklistAccountSearchQuery()
            {
                ViewId = 2,
                Auditor = 1,
                FollowUp = 4,
                Status = 1,
                AccountAge = 5,
                HiddenRecords = 1
            });
            AssertResult(result);
        }

        /// <summary>
        /// Test get contact info 1
        /// </summary>
        [TestMethod]
        public void TestGetContactInfo1()
        {
            SiteAdminLogin();
            var contactInfo = _worklistController.GetContactInfo(3475203);
            contactInfo.EventDate = DateTime.MinValue;
            AssertResult(contactInfo);
        }

        /// <summary>
        /// Test get contact info 2
        /// </summary>
        [TestMethod]
        public void TestGetContactInfo2()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.GetContactInfo(999999), "Patient with patientId='999999' not found");
        }

        /// <summary>
        /// Test update contact info
        /// </summary>
        [TestMethod]
        public void TestUpdateContactInfo()
        {
            SiteAdminLogin();
            var newContactInfo = PrepareContactInfo();
            _worklistController.UpdateContactInfo(3475203, newContactInfo);
            var updated = _worklistController.GetContactInfo(3475203);
            updated.EventDate = DateTime.MinValue;
            AssertResult(updated);
        }

        /// <summary>
        /// Test get eor
        /// </summary>
        [TestMethod]
        public void TestGetEor()
        {
            SiteAdminLogin();
            var eor = _worklistController.GetEor(3475203);
            Assert.IsTrue(eor.ElementAt(0).StartsWith("Episodic"));
        }

        /// <summary>
        /// Test get detail reimb
        /// </summary>
        [TestMethod]
        public void TestGetDetailReimb()
        {
            SiteAdminLogin();
            var detailReimb = _worklistController.GetDetailReimb(3475203);
            AssertResult(detailReimb);
        }

        /// <summary>
        /// Test get audit status history
        /// </summary>
        [TestMethod]
        public void TestGetAuditStatusHistory()
        {
            SiteAdminLogin();
            var auditStatusHistory = _worklistController.GetAuditStatusHistory(3475203);
            AssertResult(auditStatusHistory);
        }

        /// <summary>
        /// Test get payments
        /// </summary>
        [TestMethod]
        public void TestGetPayments()
        {
            SiteAdminLogin();
            var payments = _worklistController.GetPayments(3475203);
            AssertResult(payments);
        }

        /// <summary>
        /// Test create Payment 1
        /// </summary>
        [TestMethod]
        public void TestCreatePayment1()
        {
            SiteAdminLogin();
            var paymentDetail = PreparePaymentDetail();
            var result = _worklistController.CreatePayment(3475203, paymentDetail);
            AssertResult(result);
        }

        /// <summary>
        /// Test create Payment 2
        /// </summary>
        [TestMethod]
        public void TestCreatePayment2()
        {
            SiteAdminLogin();
            var paymentDetail = PreparePaymentDetail();
            paymentDetail.PaymentType = "Contractual";
            var result = _worklistController.CreatePayment(3475203, paymentDetail);
            AssertResult(result);
        }

        /// <summary>
        /// Test create Payment 3
        /// </summary>
        [TestMethod]
        public void TestCreatePayment3()
        {
            SiteAdminLogin();
            var paymentDetail = PreparePaymentDetail();
            paymentDetail.PaidBy = "invalid";
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.CreatePayment(3475203, paymentDetail), "PaidBy: 'invalid' is invalid. Value must be one of 'Patient,Payer 1,Payer 2,Payer 3,Payer 4,Other Payer'");

        }

        /// <summary>
        /// Test create Payment 4
        /// </summary>
        [TestMethod]
        public void TestCreatePayment4()
        {
            SiteAdminLogin();
            var paymentDetail = PreparePaymentDetail();
            paymentDetail.PaidBy = "Other Payer";
            ExceptionAssert.ThrowsException<BadRequestException>(() => _worklistController.CreatePayment(3475203, paymentDetail), "Payment type must be 'Miscellaneous' for 'Other Payer'");

        }

        /// <summary>
        /// Test create Payment 5
        /// </summary>
        [TestMethod]
        public void TestCreatePayment5()
        {
            SiteAdminLogin();
            var paymentDetail = PreparePaymentDetail();
            paymentDetail.PaidBy = "Other Payer";
            paymentDetail.PaymentType = "Miscellaneous";
            var created = _worklistController.CreatePayment(3475203, paymentDetail);
            AssertResult(created);
        }

        /// <summary>
        /// Test get payment details
        /// </summary>
        [TestMethod]
        public void TestGetPaymentDetails()
        {
            SiteAdminLogin();
            var paymentDetails = _worklistController.GetPaymentDetails(3475203, 1);
            AssertResult(paymentDetails);
        }

        /// <summary>
        /// Test delete payment details
        /// </summary>
        [TestMethod]
        public void TestDeletePaymentDetails()
        {
            SiteAdminLogin();
            _worklistController.DeletePaymentDetails(3475203, 5);
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.DeletePaymentDetails(3475203, 5), "Other payment with patientId='3475203' and payerNumber='5' not found");
        }

        /// <summary>
        /// Test get other payments
        /// </summary>
        [TestMethod]
        public void TestGetOtherPayments()
        {
            SiteAdminLogin();
            var otherPayments = _worklistController.GetOtherPayments(3475203);
            AssertResult(otherPayments);
        }

        /// <summary>
        /// Test update other payment
        /// </summary>
        [TestMethod]
        public void TestUpdateOtherPayment()
        {
            SiteAdminLogin();
            var otherPaymentUpdate = PrepareOtherPaymentUpdate();
            _worklistController.UpdateOtherPayment(3475203, otherPaymentUpdate);
            var otherPayments = _worklistController.GetOtherPayments(3475203);
            AssertResult(otherPayments);
        }

        /// <summary>
        /// Test update other payment not found
        /// </summary>
        [TestMethod]
        public void TestUpdateOtherPaymentNotFound()
        {
            SiteAdminLogin();
            var otherPaymentUpdate = PrepareOtherPaymentUpdate();
            otherPaymentUpdate.Entity = "not found";
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.UpdateOtherPayment(3475203, otherPaymentUpdate), "Other payment with patientId='3475203', entity='not found' and incrementName='Miscellaneous' not found");
        }

        /// <summary>
        /// Test commit payment
        /// </summary>
        [TestMethod]
        public void TestCommitPayment()
        {
            SiteAdminLogin();
            _worklistController.CommitPayment(3475203, 5);
            ExceptionAssert.ThrowsException<NotFoundException>(() => _worklistController.CommitPayment(3475203, 5), "Other payment with patientId='3475203' and payerNumber='5' not found");
        }

        /// <summary>
        /// Test get icd codes
        /// </summary>
        [TestMethod]
        public void TestGetIcdCodes()
        {
            SiteAdminLogin();
            var result = _worklistController.GetIcdCodes(3475203);
            AssertResult(result);
        }

        /// <summary>
        /// Creates and returns new worklist layout
        /// </summary>
        private WorklistLayout PrepareWorklistLayout()
        {
            return new WorklistLayout
            {
                Name = "new layout",
                Description = "description",
                IsDefault = true,
                Columns = new List<WorklistColumnLayout>()
                {
                    new WorklistColumnLayout
                    {
                        FieldName = "PaymentStatus",
                        Location = 1,
                        IsVisible = true,
                        Width = 50
                    }
                }
            };
        }

        /// <summary>
        /// Creates and returns new charge code detail
        /// </summary>
        private ChargeCodeDetail PrepareChargeCodeDetail()
        {
            return new ChargeCodeDetail
            {
                ChargeCode = "charge code 1",
                RevenueCode = "revenue",
                DeniedCharges = 41654.5,
                Description = "description",
                CptCode = "cpt code",
                NonCoveredCharges = 56.22,
                TransactionId = "123",
                Units = 452,
                NonBilledCharges = 71.1,
                ServiceDate = new DateTime(2012, 12, 12),
                Charges = 85,
                Cost = 985.125
            };
        }

        /// <summary>
        /// Creates and returns new contact info
        /// </summary>
        private ContactInfo PrepareContactInfo()
        {
            return new ContactInfo
            {
                FacilityName = "My Facility",
                TaxId = "123",
                Npi = "string",
                PayeeName = "Patient23, Test",
                PayerPhone = null,
                SubscriberId = "SUBID 23",
                InsuredSsn = null,
                InsuredGroupName = "INS GRP I",
                PatientName = "Patient23, Test",
                DateOfBirth = new DateTime(2012, 12, 12),
                AccountNumber = "65245453",
                TotalCharges = 995.0,
                InsuredName = "Insured23, Name",
                AdmitDate = new DateTime(2012, 12, 12),
                DischargeDate = new DateTime(2012, 12, 12),
                PatientType = 16871,
                MedicalRecordNo = "123",
                SocialSecurityNo = "",
                AuditStatus = "Closed",
                CommittedAmount = 0,
                Argument = "string",
                Auditor = "string",
                VarCatSuggestion = "(none)",
                PursuingReason = null,
                FollowUpDate = new DateTime(2012, 12, 12),
                EventDate = new DateTime(2012, 12, 12),
                StartDate = new DateTime(2012, 12, 12),
                Agency = "string",
                Responsibility = "Payer 1",
                Type = null,
                Duration = 0,
                Note = "my notes"
            };
        }

        /// <summary>
        /// Creates and returns new payment detail
        /// </summary>
        private PaymentDetail PreparePaymentDetail()
        {
            return new PaymentDetail
            {
                PaidBy = "Payer 1",
                PaymentType = "Actual Payment",
                ImportDate = new DateTime(2012, 12, 12),
                PostingDate = new DateTime(2012, 12, 12),
                Amount = 100,
                ExcludedAmount = 0,
                AdjustCode = "202",
                AdjustCodeDescription = "string"
            };
        }

        /// <summary>
        /// Creates and returns new other payment update
        /// </summary>
        private OtherPaymentUpdate PrepareOtherPaymentUpdate()
        {
            return new OtherPaymentUpdate
            {
                Entity = "Other Payer",
                IncrementName = "Miscellaneous",
                AccountNumber = "66106024",
                AdjustCode = "333",
                PayerId = "123"
            };
        }
    }
}
