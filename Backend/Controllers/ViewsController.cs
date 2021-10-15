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
    /// The views controller
    /// </summary>
    [Authorize]
    [ApiController]
    [DefaultRouting]
    public class ViewsController : BaseController
    {
        /// <summary>
        /// The logger used in controller
        /// </summary>
        private readonly ILogger<ViewsController> _logger;

        /// <summary>
        /// The view service used in controller
        /// </summary>
        private readonly IViewService _viewService;

        /// <summary>
        /// Constructor with logger,view service, http context accessor
        /// </summary>
        /// <param name="logger">the logger </param>
        /// <param name="viewService">the view service</param>
        /// <param name="httpContextAccessor">the http context accessor</param>
        public ViewsController(ILogger<ViewsController> logger, IViewService viewService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _viewService = viewService;
        }

        /// <summary>
        /// Get all view fields
        /// </summary>
        /// <returns>all view fields</returns>
        [HttpGet]
        [Route("[action]")]
        public IEnumerable<ViewField> Fields()
        {
            return _logger.Process(() => _viewService.Fields(CurrentUser), "get list of all available view fields");
        }

        /// <summary>
        /// Get all field values for rules that use 'values' as the SelectionType
        /// </summary>
        /// <param name="fieldId">the field id</param>
        /// <returns>all field values</returns>
        [HttpGet]
        [Route("fields/{fieldId}/values")]
        public IEnumerable<FieldValue> FieldValues([FromRoute] int fieldId)
        {
            return _logger.Process(() => _viewService.FieldValues(fieldId, CurrentUser),
                "get list of values for the field with SelectionType='values'",
                parameters: fieldId);
        }


        /// <summary>
        /// Create view
        /// </summary>
        /// <param name="views">the view to create</param>
        /// <returns>the new created view with id</returns>
        [HttpPost]
        [Route("")]
        public View CreateView([FromBody] View views)
        {
            return _logger.Process(() => _viewService.CreateView(views, CurrentUser), "creates new View",
                parameters: views);
        }

        /// <summary>
        /// Update view
        /// </summary>
        /// <param name="id">the view id</param>
        /// <param name="view">the view to update</param>
        /// <returns>the updated view</returns>
        [HttpPut]
        [Route("{id}")]
        public View UpdateView(int id, [FromBody] View view)
        {
            return _logger.Process(() => _viewService.UpdateView(id, view, CurrentUser), "updates View with the given Id",
                parameters: new object[] {id, view});
        }

        /// <summary>
        /// Get all views for current user
        /// </summary>
        /// <returns>all views for current user</returns>
        [HttpGet]
        [Route("")]
        public IEnumerable<View> GetAllViews()
        {
            return _logger.Process(() => _viewService.GetAllViews(CurrentUser), "get all current user's Views");
        }

        /// <summary>
        /// Get view by id
        /// </summary>
        /// <param name="id">the view id</param>
        /// <returns>match view by id</returns>
        [HttpGet]
        [Route("{id}")]
        public View GetViewById([FromRoute] int id)
        {
            return _logger.Process(() => _viewService.GetViewById(id, CurrentUser), "get View with the given Id",
                parameters: id);
        }

        /// <summary>
        /// Delete view by id
        /// </summary>
        /// <param name="viewId">the view id</param>
        [HttpDelete]
        [Route("{viewId}")]
        public void DeleteView([FromRoute] int viewId)
        {
            _logger.Process(() => _viewService.DeleteView(viewId, CurrentUser), "deletes View with the given Id",
                parameters: viewId);
        }

        /// <summary>
        /// Get all view rules
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <returns>all view rules</returns>
        [HttpGet]
        [Route("{viewId}/rules")]
        public IEnumerable<ViewRule> GetAllViewRules([FromRoute] int viewId)
        {
            return _logger.Process(() => _viewService.GetAllViewRules(viewId, CurrentUser), "gets View Rules",
                parameters: viewId);
        }

        /// <summary>
        /// Create view rules
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="viewRules">the view rules to create</param>
        /// <returns>the new created view rules with id</returns>
        [HttpPost]
        [Route("{viewId}/rules")]
        public IEnumerable<ViewRule> CreateViewRule([FromRoute] int viewId, [FromBody] IEnumerable<ViewRule> viewRules)
        {
            return _logger.Process(() => _viewService.CreateViewRule(viewId, viewRules, CurrentUser),
                "creates new View Rule", parameters: new object[] {viewId, viewRules});
        }

        /// <summary>
        /// Get view rule with view id and rule id
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <returns>match view rule with view id and rule id</returns>
        [HttpGet]
        [Route("{viewId}/rules/{ruleId}")]
        public ViewRule GetViewRule([FromRoute] int viewId, [FromRoute] int ruleId)
        {
            return _logger.Process(() => _viewService.GetViewRule(viewId, ruleId, CurrentUser),
                "gets View Rule with given Id", parameters: new object[] {viewId, ruleId});
        }

        /// <summary>
        /// Update view rule
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <param name="viewRule">the view rule to update</param>
        /// <returns>the updated view rule </returns>
        [HttpPut]
        [Route("{viewId}/rules/{ruleId}")]
        public ViewRule UpdateViewRule([FromRoute] int viewId, [FromRoute] int ruleId, [FromBody] ViewRule viewRule)
        {
            return _logger.Process(() => _viewService.UpdateViewRule(viewId, ruleId, viewRule, CurrentUser),
                "updates View Rule with given Id", parameters: new object[] {viewId, ruleId, viewRule});
        }

        /// <summary>
        /// Delete view rule
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        [HttpDelete]
        [Route("{viewId}/rules/{ruleId}")]
        public void DeleteViewRule([FromRoute] int viewId, [FromRoute] int ruleId)
        {
            _logger.Process(() => _viewService.DeleteViewRule(viewId, ruleId, CurrentUser),
                "deletes View Rule with given Id", parameters: new object[] {viewId, ruleId});
        }
    }
}