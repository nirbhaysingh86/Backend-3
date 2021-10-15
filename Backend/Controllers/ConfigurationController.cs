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
    /// The configuration controller
    /// </summary>
    [Authorize]
    [ApiController]
    [DefaultRouting]
    public class ConfigurationController : BaseController
    {
        /// <summary>
        /// The logger used in controller
        /// </summary>
        private readonly ILogger<ConfigurationController> _logger;

        /// <summary>
        /// The configuration service
        /// </summary>
        private readonly IConfigurationService _configurationService;

        /// <summary>
        /// Constructor with logger,configuration service,http context accessor
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="configurationService">the configuration service</param>
        /// <param name="httpContextAccessor">the http context accessor</param>
        public ConfigurationController(ILogger<ConfigurationController> logger,
            IConfigurationService configurationService, IHttpContextAccessor httpContextAccessor) : base(
            httpContextAccessor)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Get all worklist columns
        /// </summary>
        /// <returns>all worklist columns</returns>
        [HttpGet]
        [Route("worklist/columns")]
        public IEnumerable<WorklistColumn> WorkListColumns()
        {
            return _logger.Process(() => _configurationService.WorkListColumns(CurrentUser),
                "gets the configuration of worklist columns");
        }

        /// <summary>
        /// Get default view limits
        /// </summary>
        /// <returns>default view limits</returns>
        [HttpGet]
        [Route("[action]")]
        public DefaultViewLimits DefaultViewLimits()
        {
            return _logger.Process(() => _configurationService.DefaultViewLimits(CurrentUser),
                "gets the default view limits");
        }
    }
}