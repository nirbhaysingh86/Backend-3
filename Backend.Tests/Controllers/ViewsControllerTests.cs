using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMMC.Exceptions;
using PMMC.Helpers;
using PMMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMMC.UnitTests.Controllers
{
    /// <summary>
    /// Tests for views controller
    /// </summary>
    [TestClass]
    public class ViewsControllerTests : BaseTest
    {
        /// <summary>
        /// Get all audits sql
        /// </summary>
        internal const string GetAuditsSql = "SELECT [AuditCounter] AS Id,[UserId],[OldValue],[NewValue],[OperationType],[ObjectType],[Timestamp] FROM [dbo].[tblAudits]";

        /// <summary>
        /// Test fields
        /// </summary>
        [TestMethod]
        public void TestFields()
        {
            SiteAdminLogin();
            var result = _viewsController.Fields();
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test field values DescField=Description
        /// </summary>
        [TestMethod]
        public void TestFieldValues1()
        {
            SiteAdminLogin();
            var result = _viewsController.FieldValues(4);
            Assert.IsTrue(result.Any());
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test field values DescField=Code
        /// </summary>
        [TestMethod]
        public void TestFieldValues2()
        {
            SiteAdminLogin();
            var result = _viewsController.FieldValues(251);
            Assert.IsTrue(result.Any());
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test field values DescField=[Code] + ' - ' + [Description]
        /// </summary>
        [TestMethod]
        public void TestFieldValues3()
        {
            SiteAdminLogin();
            var result = _viewsController.FieldValues(179);
            Assert.IsTrue(result.Any());
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test field values with not found id
        /// </summary>
        [TestMethod]
        public void TestFieldValuesWithNotFoundId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.FieldValues(99), "ViewField by id='99' not found");
        }

        /// <summary>
        /// Test field values with negative id
        /// </summary>
        [TestMethod]
        public void TestFieldValuesWithNegativeId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.FieldValues(-1), "fieldId should be positive.");
        }

        /// <summary>
        /// Test field values with not values selection type
        /// </summary>
        [TestMethod]
        public void TestFieldValuesWithNotValuesSelectionType()
        {
            SiteAdminLogin();
            var fieldId = 5;
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.FieldValues(5), $"ViewField by id='{fieldId}' is not values selection type");
        }

        /// <summary>
        /// Test get all views
        /// </summary>
        [TestMethod]
        public void TestGetAllViews()
        {
            SiteAdminLogin();
            var result = _viewsController.GetAllViews();
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test get view by id
        /// </summary>
        [TestMethod]
        public void TestGetViewById()
        {
            SiteAdminLogin();
            var result = _viewsController.GetViewById(2);
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test get view by negative id
        /// </summary>
        [TestMethod]
        public void TestGetViewByNegativeId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.GetViewById(-1), "id should be positive.");
        }

        /// <summary>
        /// Test get view by not found id
        /// </summary>
        [TestMethod]
        public void TestGetViewByNotFoundId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.GetViewById(9999), "Views by id='9999' not found");
        }

        /// <summary>
        /// Test get view by not owner
        /// </summary>
        [TestMethod]
        public void TestGetViewByNotOwnerId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _viewsController.GetViewById(1), "Views by id='1' not belongs to the current user");
        }

        /// <summary>
        /// Test create view
        /// </summary>
        [TestMethod]
        public void TestCreateView()
        {
            SiteAdminLogin();
            var audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(!audits.Any());
            Assert.IsTrue(_viewsController.GetViewById(2).IsDefault);
            var view = PrepareView();
            var defaultView = _viewsController.CreateView(view);
            var defaultViewId = defaultView.Id;
            Assert.IsTrue(defaultViewId > 0);
            Assert.IsTrue(defaultView.ViewOwner > 0);
            Assert.AreEqual(defaultView.ViewOwner, _testSettings.SiteAdmin.UserID);
            view.ViewOwner = _testSettings.SiteAdmin.UserID;
            // update old default view 
            Assert.IsFalse(_viewsController.GetViewById(2).IsDefault);
            var getDefaultView = _viewsController.GetViewById(defaultView.Id);
            // create and get view by new id and will find same result
            defaultView.Should().BeEquivalentTo(getDefaultView);

            // validate audits 
            audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(audits.Any());
            var audit = audits.First();
            Assert.IsTrue(audit.Id > 0);
            Assert.IsNull(audit.OldValue);
            Assert.IsNotNull(audit.Timestamp);
            Assert.AreEqual(LoggerHelper.GetObjectDescription(defaultView), audit.NewValue);
            Assert.AreEqual(OperationType.Create, audit.OperationType);
            Assert.AreEqual(nameof(View), audit.ObjectType);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, audit.UserId);
            view.IsDefault = false;
            view.Name = $"{view.Name}NotDefault";
            var notDefaultView = _viewsController.CreateView(view);
            Assert.IsTrue(notDefaultView.Id > 0);
            var getNotDefaultView = _viewsController.GetViewById(notDefaultView.Id);
            Assert.IsFalse(getNotDefaultView.IsDefault);
            notDefaultView.ViewOwner = _testSettings.SiteAdmin.UserID;
            notDefaultView.Should().BeEquivalentTo(getNotDefaultView);
            // default view not changed
            Assert.IsTrue(_viewsController.GetViewById(defaultViewId).IsDefault);

        }

        /// <summary>
        /// Test create view with exist view name
        /// </summary>
        [TestMethod]
        public void TestCreateViewWithExistViewName()
        {
            SiteAdminLogin();
            var existView = _viewsController.GetViewById(2);
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateView(existView), $"Views with name='{existView.Name}' exists in database");
        }

        /// <summary>
        /// Test create view with invalid auditor
        /// </summary>
        [TestMethod]
        public void TestCreateViewWithInvalidAuditor()
        {
            SiteAdminLogin();
            var views = PrepareView();
            views.Limits.Auditor = 99;
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateView(views), $"Auditor with value='{views.Limits.Auditor}' is invalid");
        }

        /// <summary>
        /// Test create view with invalid follow up
        /// </summary>
        [TestMethod]
        public void TestCreateViewWithInvalidFollowUp()
        {
            SiteAdminLogin();
            var views = PrepareView();
            views.Limits.FollowUp = 99;
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateView(views), $"FollowUp with value='{views.Limits.FollowUp}' is invalid");
        }

        /// <summary>
        /// Test create view with invalid status
        /// </summary>
        [TestMethod]
        public void TestCreateViewWithInvalidStatus()
        {
            SiteAdminLogin();
            var views = PrepareView();
            views.Limits.Status = 99;
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateView(views), $"Status with value='{views.Limits.Status}' is invalid");
        }

        /// <summary>
        /// Test create with invalid account age
        /// </summary>
        [TestMethod]
        public void TestCreateViewWithInvalidAccountAge()
        {
            SiteAdminLogin();
            var views = PrepareView();
            views.Limits.AccountAge = 99;
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateView(views), $"AccountAge with value='{views.Limits.AccountAge}' is invalid");
        }

        /// <summary>
        /// Test create view with invalid hidden records
        /// </summary>
        [TestMethod]
        public void TestCreateViewWithInvalidHiddenRecords()
        {
            SiteAdminLogin();
            var views = PrepareView();
            views.Limits.HiddenRecords = 99;
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateView(views), $"HiddenRecords with value='{views.Limits.HiddenRecords}' is invalid");
        }

        /// <summary>
        /// Test update view
        /// </summary>
        [TestMethod]
        public void TestUpdateView()
        {
            SiteAdminLogin();
            var audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(!audits.Any());
            var id = 2;
            var view = _viewsController.GetViewById(id);
            var oldValue = LoggerHelper.GetObjectDescription(view);
            Assert.IsTrue(view.IsDefault);
            view.Name = "new view Name";
            view.Description = "new view desc";
            view.IsDefault = false;
            view.Limits = new DefaultViewLimits
            {
                Auditor = (view.Limits.Auditor + 1) % 3,
                FollowUp = (view.Limits.FollowUp + 1) % 5,
                Status = (view.Limits.Status + 1) % 2,
                AccountAge = (view.Limits.AccountAge + 1) % 6,
                HiddenRecords = (view.Limits.HiddenRecords + 1) % 2,
            };

            _viewsController.UpdateView(id, view);
            var getView = _viewsController.GetViewById(id);
            Assert.IsFalse(getView.IsDefault);
            view.Should().BeEquivalentTo(getView);

            //validate audit
            audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(audits.Any());
            var audit = audits.First();
            Assert.IsTrue(audit.Id > 0);
            Assert.IsNotNull(audit.Timestamp);
            Assert.AreEqual(oldValue, audit.OldValue);
            Assert.AreEqual(LoggerHelper.GetObjectDescription(view), audit.NewValue);
            Assert.AreEqual(OperationType.Update, audit.OperationType);
            Assert.AreEqual(nameof(View), audit.ObjectType);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, audit.UserId);

            view.IsDefault = true;
            _viewsController.UpdateView(id, view);
            getView = _viewsController.GetViewById(id);
            Assert.IsTrue(getView.IsDefault);
            view.Should().BeEquivalentTo(getView);

            // update again to ensure could update with new name again successfully
            _viewsController.UpdateView(id, view);
        }

        /// <summary>
        /// Test update view by negative id
        /// </summary>
        [TestMethod]
        public void TestUpdateViewByNegativeId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.UpdateView(-1, PrepareView()), "id should be positive.");
        }

        /// <summary>
        /// Test update view by not found id
        /// </summary>
        [TestMethod]
        public void TestUpdateViewByNotFoundId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.UpdateView(9999, PrepareView()), "Views by id='9999' not found");
        }

        /// <summary>
        /// Test update view by not owner
        /// </summary>
        [TestMethod]
        public void TestUpdateViewByNotOwnerId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _viewsController.UpdateView(1, PrepareView()), "Views by id='1' not belongs to the current user");
        }

        /// <summary>
        /// Test update view with exist view name
        /// </summary>
        [TestMethod]
        public void TestUpdateViewWithExistViewName()
        {
            SiteAdminLogin();
            var existView = _viewsController.CreateView(PrepareView());
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.UpdateView(2, existView), $"Views with name='{existView.Name}' exists in database");
        }

        /// <summary>
        /// Test update view with invalid auditor
        /// </summary>
        [TestMethod]
        public void TestUpdateViewWithInvalidAuditor()
        {
            // other fields are similar or share same validation logic for create and update view methods
            SiteAdminLogin();
            var views = PrepareView();
            views.Limits.Auditor = 99;
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.UpdateView(2, views), $"Auditor with value='{views.Limits.Auditor}' is invalid");
        }

        /// <summary>
        /// Test delete view
        /// </summary>
        [TestMethod]
        public void TestDeleteView()
        {
            SiteAdminLogin();
            var audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(!audits.Any());

            var id = 2;
            var view = _viewsController.GetViewById(id);
            var oldValue = LoggerHelper.GetObjectDescription(view);
            Assert.IsNotNull(view);
            var viewRules = _viewsController.GetAllViewRules(id);
            Assert.IsTrue(viewRules.Any());
            Assert.AreEqual(5, viewRules.Count());
            var oldViewRules = LoggerHelper.GetObjectDescription(viewRules.First());
            var rules = _viewsController.GetAllViewRules(id);
            Assert.IsNotNull(rules);
            Assert.IsTrue(rules.Any());
            _viewsController.DeleteView(id);

            // view rules deleted
            Assert.ThrowsException<NotFoundException>(() => _viewsController.GetAllViewRules(id));
            // view deleted
            Assert.ThrowsException<NotFoundException>(() => _viewsController.GetViewById(id));

            //validate 2 audits for delete view 
            audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(audits.Any());
            Assert.AreEqual(viewRules.Count() + 1, audits.Count());
            //valiadte view rules audit
            var viewRulesAudit = audits.First();
            Assert.IsTrue(viewRulesAudit.Id > 0);
            Assert.IsNull(viewRulesAudit.NewValue);
            Assert.IsNotNull(viewRulesAudit.Timestamp);
            Assert.AreEqual(oldViewRules, viewRulesAudit.OldValue);
            Assert.AreEqual(OperationType.Delete, viewRulesAudit.OperationType);
            Assert.AreEqual(nameof(ViewRule), viewRulesAudit.ObjectType);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, viewRulesAudit.UserId);
            // validate view audit
            var viewAudit = audits.Last();
            Assert.IsTrue(viewAudit.Id > 0);
            Assert.IsNull(viewAudit.NewValue);
            Assert.IsNotNull(viewAudit.Timestamp);
            Assert.AreEqual(oldValue, viewAudit.OldValue);
            Assert.AreEqual(OperationType.Delete, viewAudit.OperationType);
            Assert.AreEqual(nameof(View), viewAudit.ObjectType);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, viewAudit.UserId);
        }

        /// <summary>
        /// Test delete view with negative id
        /// </summary>
        [TestMethod]
        public void TestDeleteViewByNegativeId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.DeleteView(-1), "id should be positive.");
        }

        /// <summary>
        /// Test delete view by not found id
        /// </summary>
        [TestMethod]
        public void TestDeleteViewByNotFoundId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.DeleteView(9999), "Views by id='9999' not found");
        }

        /// <summary>
        /// Test delete view by not owner
        /// </summary>
        [TestMethod]
        public void TestDeleteViewByNotOwnerId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _viewsController.DeleteView(1), "Views by id='1' not belongs to the current user");
        }

        /// <summary>
        /// Test get all view rules
        /// </summary>
        [TestMethod]
        public void TestGetAllViewRules()
        {
            SiteAdminLogin();
            var id = 2;
            var result = _viewsController.GetAllViewRules(id);
            Assert.IsTrue(result.Any());
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test create view rule with date range type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithDateRangeType()
        {
            SiteAdminLogin();
            var audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(!audits.Any());

            int id = 2;
            var viewRule = PrepareViewRule();
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.DateRangeSelectionType, getResult.viewField.SelectionType);
            result.Should().BeEquivalentTo(getResult);

            //validate audit
            audits = TestHelper.Query<Audit>(GetAuditsSql).Where(x=> OperationType.Create.Equals(x.OperationType));
            Assert.IsTrue(audits.Any());
            var audit = audits.First();
            Assert.IsTrue(audit.Id > 0);
            Assert.IsNull(audit.OldValue);
            Assert.IsNotNull(audit.Timestamp);
            Assert.AreEqual(LoggerHelper.GetObjectDescription(result), audit.NewValue);
            Assert.AreEqual(OperationType.Create, audit.OperationType);
            Assert.AreEqual(nameof(ViewRule), audit.ObjectType);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, audit.UserId);
        }

        /// <summary>
        /// Test create view rule with invalid date range type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidDateRangeType1()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = PrepareViewRule();
            viewRule.BeginRange = null;
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.CreateViewRule(id, viewRules), "BeginRange cannot be null");
        }

        /// <summary>
        /// Test create view rule with invalid date range type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidDateRangeType2()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = PrepareViewRule();
            viewRule.EndRange = " ";
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.CreateViewRule(id, viewRules), "EndRange cannot be empty");
        }

        /// <summary>
        /// Test create view rule with invalid date range type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidDateRangeType3()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = PrepareViewRule();
            viewRule.Operand = " ";
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.CreateViewRule(id, viewRules), "Operand cannot be empty");
        }

        /// <summary>
        /// Test create view rule with invalid date range type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidDateRangeType4()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = PrepareViewRule();
            viewRule.BeginRange = "Invalid";
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules), "ViewRule by field BeginRange='Invalid' exist invalid date range format for [MM/dd/yyyy]");
        }

        /// <summary>
        /// Test create view rule with invalid date range type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidDateRangeType5()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = PrepareViewRule();
            viewRule.BeginRange = "12/31/1996";
            viewRule.EndRange = "12/31/1986";
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules), "ViewRule by field id 5 must exist BeginRange<=EndRange");
        }

        /// <summary>
        /// Test create view rule with DateTimeRange type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithDateTimeRangeType()
        {
            SiteAdminLogin();
            var audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(!audits.Any());

            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 72,
                BeginRange = "01/01/1900 11:00",
                EndRange = "12/31/1986 23:00",
                Operand = "BETWEEN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.DateTimeRangeSelectionType, getResult.viewField.SelectionType);
            result.Should().BeEquivalentTo(getResult);

            //validate audit
            audits = TestHelper.Query<Audit>(GetAuditsSql).Where(x => OperationType.Create.Equals(x.OperationType));
            Assert.IsTrue(audits.Any());
            var audit = audits.First();
            Assert.IsTrue(audit.Id > 0);
            Assert.IsNull(audit.OldValue);
            Assert.IsNotNull(audit.Timestamp);
            Assert.AreEqual(LoggerHelper.GetObjectDescription(result), audit.NewValue);
            Assert.AreEqual(OperationType.Create, audit.OperationType);
            Assert.AreEqual(nameof(ViewRule), audit.ObjectType);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, audit.UserId);
        }

        /// <summary>
        /// Test create view rule with number type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithNumberType()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 11,
                Value = "12",
                Operand = ">"
            };
            var viewRules = new List<ViewRule> { viewRule }; 
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.NumberSelectionType, getResult.viewField.SelectionType);
            result.Should().BeEquivalentTo(getResult);
        }

        /// <summary>
        /// Test create view rule with invalid number type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidNumberType1()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 11,
                Value = null,
                Operand = ">"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.CreateViewRule(id, viewRules), "Value cannot be null");
        }

        /// <summary>
        /// Test create view rule with invalid number type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidNumberType2()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 11,
                Value = "11",
                Operand = " "
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.CreateViewRule(id, viewRules), "Operand cannot be empty");
        }

        /// <summary>
        /// Test create view rule with invalid number type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidNumberType3()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 11,
                Value = "invalid",
                Operand = ">"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules), "ViewRule by field 11 must exist valid number type value");
        }

        /// <summary>
        /// Test create view rule with text type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithTextType()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 19,
                Value = "start",
                Operand = "BEGINS WITH"
            };
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.TextSelectionType, getResult.viewField.SelectionType);
            result.Should().BeEquivalentTo(getResult);
        }

        /// <summary>
        /// Test create view rule with Percent type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithPercentType()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 35,
                Value = "0.12",
                Operand = ">"
            };
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.PercentSelectionType, getResult.viewField.SelectionType);
            result.Should().BeEquivalentTo(getResult);
        }

        /// <summary>
        /// Test create view rule with invalid Percent type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidPercentType1()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 35,
                Value = "-1",
                Operand = ">"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules), "ViewRule by field 35 must exist valid percent value");
        }

        /// <summary>
        /// Test create view rule with invalid Percent type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithInvalidPercentType2()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 35,
                Value = "120", //must be number between 0-1
                Operand = ">"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules), "ViewRule by field 35 must exist valid percent value");
        }

        /// <summary>
        /// Test create view rule with combo code type and store Description as value
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType1()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 18,
                Value = "INPATIENT",
                ValueId = "131",
                Operand = "IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.ValuesSelectionType, getResult.viewField.SelectionType, true);
            Assert.AreEqual(viewRule.ValueId, result.ValueId);
            Assert.AreEqual("INPATIENT", result.Value); // use Description to store value 
            Assert.IsNotNull(getResult.viewField.ComboCodeType);
            result.Should().BeEquivalentTo(getResult);
        }

        /// <summary>
        /// Test create view rule with combo code type and store Code as value
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType2()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 251,
                Value = "111",
                ValueId = "27",
                Operand = "IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.ValuesSelectionType, getResult.viewField.SelectionType, true);
            Assert.AreEqual(viewRule.ValueId, result.ValueId);
            Assert.AreEqual("111", result.Value); // use Code to store value 
            Assert.IsNotNull(getResult.viewField.ComboCodeType);
            result.Should().BeEquivalentTo(getResult);
        }

        /// <summary>
        /// Test create view rule with combo code type and store [Code] + ' - ' + [Description] as value
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType3()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 179,
                Value = "1959-DC TO HOME OR SELF CARE (RO DISCHA)",
                ValueId = "52",
                Operand = "IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.ValuesSelectionType, getResult.viewField.SelectionType, true);
            Assert.AreEqual(viewRule.ValueId, result.ValueId);
            Assert.AreEqual("1959-DC TO HOME OR SELF CARE (RO DISCHA)", result.Value); // use [Code] + ' - ' + [Description] to store value 
            Assert.IsNotNull(getResult.viewField.ComboCodeType);
            result.Should().BeEquivalentTo(getResult);
        }

        /// <summary>
        /// Test create view rule with combo code type and use unknown DescField
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType4()
        {
            SiteAdminLogin();
            var descField = "Unknown";
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Inpatient",
                ValueId = "201",
                Operand = "NOT IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            TestHelper.Execute($"UPDATE [dbo].[tblViewFields] SET [DescField]='{descField}' WHERE [FieldID]={viewRule.FieldId}");
            ExceptionAssert.ThrowsException<InternalServerErrorException>(() => _viewsController.CreateViewRule(id, viewRules),
               $"ViewFields by id='{viewRule.FieldId}' exist not supported DescField '{descField}'");
        }


        /// <summary>
        /// Test create view rule with combo code type and use invalid Operand
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType5()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Inpatient",
                ValueId = "201",
                Operand = "invalid"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules),
               $"ViewRule by field id='{viewRule.FieldId}' is values selection type and must exist valid Operand [IN,NOT IN]");
        }

        /// <summary>
        /// Test create view rule with combo code type and use null valueId
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType6()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Inpatient",
                ValueId = null,
                Operand = "IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<ArgumentNullException>(() => _viewsController.CreateViewRule(id, viewRules), "ValueId cannot be null");
        }

        /// <summary>
        /// Test create view rule with combo code type and use invalid number type valueId
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType7()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Inpatient",
                ValueId = "invalid",
                Operand = "IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules), $"ViewRule by field id='{viewRule.FieldId}' is values selection type and must exist int type of valueId");
        }

        /// <summary>
        /// Test create view rule with combo code type and use negative number type valueId
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType8()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Inpatient",
                ValueId = "-1",
                Operand = "IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules), $"ViewRule by field id='{viewRule.FieldId}' is values selection type and must exist positive valueId");
        }


        /// <summary>
        /// Test create view rule with combo code type and use not found valueId
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType9()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Inpatient",
                ValueId = "999",
                Operand = "IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.CreateViewRule(id, viewRules), $"FieldValue by id='{viewRule.ValueId}' not found");
        }

        /// <summary>
        /// Test create view rule with combo code type and use invalid value
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType10()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Invalid",
                ValueId = "201",
                Operand = "IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _viewsController.CreateViewRule(id, viewRules), $"ViewRule by field {viewRule.FieldId} is values selection type and exist invalid value");
        }


        /// <summary>
        /// Test create view rule with combo code type and use invalid value Id and get rule will throw error
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithComboCodeType11()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Inpatient",
                ValueId = "201",
                Operand = "NOT IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            TestHelper.Execute($"UPDATE [dbo].[tblViewRules] SET [ValueID]='Invalid' WHERE [RuleID]={result.Id}");
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.GetViewRule(id, result.Id), "FieldValue by code='Invalid' and codeType='ServiceCode' not found");
        }

        /// <summary>
        /// Test create view rule with system combo code type
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithSystemComboCodeType()
        {
            SiteAdminLogin();
            int id = 2;
            var viewRule = new ViewRule
            {
                FieldId = 4,
                Value = "Inpatient",
                ValueId = "201",
                Operand = "NOT IN"
            };
            var viewRules = new List<ViewRule> { viewRule };
            var results = _viewsController.CreateViewRule(id, viewRules);
            Assert.IsTrue(results.Any());
            var result = results.First();
            Assert.IsTrue(result.Id > 0);
            // assert
            AssertResult(result);
            var getResult = _viewsController.GetViewRule(id, result.Id);
            Assert.AreEqual(_appSettings.ValuesSelectionType, getResult.viewField.SelectionType, true);
            Assert.AreEqual(viewRule.ValueId, result.ValueId);
            Assert.AreEqual("Inpatient", result.Value); // use Description to store value 
            Assert.IsNotNull(getResult.viewField.SystemComboCodeType);
            result.Should().BeEquivalentTo(getResult);
        }

        /// <summary>
        /// Test create view rule with negative view id
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithNegativeViewId()
        {
            SiteAdminLogin();
            var viewRules = new List<ViewRule> { PrepareViewRule() };
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.CreateViewRule(-1, viewRules), "viewId should be positive.");
        }

        /// <summary>
        /// Test create view rule with not found view id
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleByNotFoundViewId()
        {
            SiteAdminLogin();
            var viewRules = new List<ViewRule> { PrepareViewRule() };
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.CreateViewRule(9999, viewRules), "Views by id='9999' not found");
        }

        /// <summary>
        /// Test create view rule by not owner
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleByNotOwnerViewId()
        {
            SiteAdminLogin();
            var viewRules = new List<ViewRule> { PrepareViewRule() };
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _viewsController.CreateViewRule(1, viewRules), "Views by id='1' not belongs to the current user");
        }

        /// <summary>
        /// Test create view rule with not found field id
        /// </summary>
        [TestMethod]
        public void TestCreateViewRuleWithNotFoundFieldId()
        {
            SiteAdminLogin();
            int id = 2;
            var fieldId = 9999;
            var viewRule = PrepareViewRule();
            viewRule.FieldId = fieldId;
            var viewRules = new List<ViewRule> { viewRule };
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.CreateViewRule(id, viewRules), $"ViewField by id='{fieldId}' not found");
        }

        /// <summary>
        /// Test get view rule
        /// </summary>
        [TestMethod]
        public void TestGetViewRule()
        {
            SiteAdminLogin();
            var id = 2;
            var result = _viewsController.GetViewRule(id, 6);
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test get view rule with negative view id
        /// </summary>
        [TestMethod]
        public void TestGetViewRuleWithNegativeViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.GetViewRule(-1, 6), "viewId should be positive.");
        }

        /// <summary>
        /// Test get view rule by not found view id
        /// </summary>
        [TestMethod]
        public void TestGetViewRuleByNotFoundViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.GetViewRule(9999, 6), "Views by id='9999' not found");
        }

        /// <summary>
        /// Test get view rule by not owner view id
        /// </summary>
        [TestMethod]
        public void TestGetViewRuleByNotOwnerViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _viewsController.GetViewRule(1, 6), "Views by id='1' not belongs to the current user");
        }

        /// <summary>
        /// Test get view rule by not found rule id
        /// </summary>
        [TestMethod]
        public void TestGetViewRuleByNotFoundRuleId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.GetViewRule(2, 9999), "ViewRule by viewId='2',ruleId='9999' not found");
        }

        /// <summary>
        /// Test update view rule without changing type
        /// </summary>
        [TestMethod]
        public void TestUpdateViewRuleWithoutChangeType()
        {
            SiteAdminLogin();
            var audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(!audits.Any());
            var id = 2;
            var ruleId = 7;
            var rule = _viewsController.GetViewRule(id, ruleId);
            var oldValue = LoggerHelper.GetObjectDescription(rule);
            rule.Value = "new Value";
            rule.Operand = "new Operand";
            _viewsController.UpdateViewRule(id, ruleId, rule);
            var udpatedRule = _viewsController.GetViewRule(id, ruleId);
            Assert.AreEqual(rule.Value, udpatedRule.Value);
            Assert.AreEqual(rule.Operand, udpatedRule.Operand);

            //validate audit
            audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(audits.Any());
            var audit = audits.First();
            Assert.IsTrue(audit.Id > 0);
            Assert.IsNotNull(audit.Timestamp);
            Assert.AreEqual(oldValue, audit.OldValue);
            Assert.AreEqual(LoggerHelper.GetObjectDescription(rule), audit.NewValue);
            Assert.AreEqual(OperationType.Update, audit.OperationType);
            Assert.AreEqual(nameof(ViewRule), audit.ObjectType);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, audit.UserId);
        }

        /// <summary>
        /// Test update view rule and change type
        /// </summary>
        [TestMethod]
        public void TestUpdateViewRuleAndChangeType()
        {
            SiteAdminLogin();
            var id = 2;
            var ruleId = 6;
            // date range type
            var rule = _viewsController.GetViewRule(id, ruleId);
            Assert.IsNull(rule.viewField.ComboCodeType);
            rule.FieldId = 4;
            rule.Value = "Inpatient";
            rule.ValueId = "201";
            rule.Operand = "IN";
            rule.viewField = null;
            rule.BeginRange = null;
            rule.EndRange = null;
            _viewsController.UpdateViewRule(id, ruleId, rule);
            var udpatedRule = _viewsController.GetViewRule(id, ruleId);
            Assert.AreEqual(rule.Value, udpatedRule.Value);
            Assert.AreEqual(rule.Operand, udpatedRule.Operand);
            var getResult = _viewsController.GetViewRule(id, ruleId);
            Assert.AreEqual(_appSettings.ValuesSelectionType, getResult.viewField.SelectionType, true);
            Assert.AreEqual("201", getResult.ValueId);
            Assert.IsNotNull(getResult.viewField.SystemComboCodeType);
            rule.Should().BeEquivalentTo(getResult);

        }

        /// <summary>
        /// Test update view rule with negative view id
        /// </summary>
        [TestMethod]
        public void TestUpdateViewRuleWithNegativeViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.UpdateViewRule(-1, 6, PrepareViewRule()), "viewId should be positive.");
        }

        /// <summary>
        /// Test update view rule by not found view id
        /// </summary>
        [TestMethod]
        public void TestUpdateViewRuleByNotFoundViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.UpdateViewRule(9999, 6, PrepareViewRule()), "Views by id='9999' not found");
        }

        /// <summary>
        /// Test update view rule by not owner view id
        /// </summary>
        [TestMethod]
        public void TestUpdateViewRuleByNotOwnerViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _viewsController.UpdateViewRule(1, 6, PrepareViewRule()), "Views by id='1' not belongs to the current user");
        }

        /// <summary>
        /// Test update view rule by not found rule id
        /// </summary>
        [TestMethod]
        public void TestUpdateViewRuleByNotFoundRulId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.UpdateViewRule(2, 9999, PrepareViewRule()), "ViewRule by viewId='2',ruleId='9999' not found");
        }

        /// <summary>
        /// Test delete view rule
        /// </summary>
        [TestMethod]
        public void TestDeleteViewRule()
        {
            SiteAdminLogin();
            var audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(!audits.Any());
            var id = 2;
            var ruleId = 6;
            var rule = _viewsController.GetViewRule(id, ruleId);
            var oldValue = LoggerHelper.GetObjectDescription(rule);
            Assert.IsNotNull(rule);

            _viewsController.DeleteViewRule(id, ruleId);
            Assert.ThrowsException<NotFoundException>(() => _viewsController.GetViewRule(id, ruleId));

            //validate audit
            audits = TestHelper.Query<Audit>(GetAuditsSql);
            Assert.IsTrue(audits.Any());
            var audit = audits.First();
            Assert.IsTrue(audit.Id > 0);
            Assert.IsNull(audit.NewValue);
            Assert.IsNotNull(audit.Timestamp);
            Assert.AreEqual(oldValue, audit.OldValue);
            Assert.AreEqual(OperationType.Delete, audit.OperationType);
            Assert.AreEqual(nameof(ViewRule), audit.ObjectType);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, audit.UserId);
        }

        /// <summary>
        /// Test delete view rule with negative id
        /// </summary>
        [TestMethod]
        public void TestDeleteViewRuleWithNegativeViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentException>(() => _viewsController.DeleteViewRule(-1, 6), "viewId should be positive.");
        }

        /// <summary>
        /// Test delete view rule by not found view id
        /// </summary>
        [TestMethod]
        public void TestDeleteViewRuleByNotFoundViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.DeleteViewRule(9999, 6), "Views by id='9999' not found");
        }

        /// <summary>
        /// Test delete view rule by not owner
        /// </summary>
        [TestMethod]
        public void TestDeleteViewRuleByNotOwnerViewId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _viewsController.DeleteViewRule(1, 6), "Views by id='1' not belongs to the current user");
        }

        /// <summary>
        /// Test delete view rule by not found rule id
        /// </summary>
        [TestMethod]
        public void TestDeleteViewRuleByNotFoundRulId()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<NotFoundException>(() => _viewsController.DeleteViewRule(2, 9999), "ViewRule by viewId='2',ruleId='9999' not found");
        }

        /// <summary>
        /// Prepare view for test
        /// </summary>
        /// <returns>view for test</returns>
        private View PrepareView()
        {
            return new View
            {
                Name = "Test View Name",
                Description = "Test View Description",
                Limits = new DefaultViewLimits
                {
                    Auditor = 2,
                    FollowUp = 4,
                    Status = 1,
                    AccountAge = 5,
                    HiddenRecords = 0,
                },
                IsDefault = true
            };
        }

        /// <summary>
        /// Prepare view rule for test
        /// </summary>
        /// <returns>view rule for test</returns>
        private ViewRule PrepareViewRule()
        {
            var viewRule = new ViewRule
            {
                FieldId = 5,
                BeginRange = "01/01/1900",
                EndRange = "12/31/1986",
                Operand = "BETWEEN"
            };
            return viewRule;
        }
    }
}
