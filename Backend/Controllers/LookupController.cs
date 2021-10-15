using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMMC.Attributes;
using PMMC.Helpers;
using PMMC.Interfaces;
using PMMC.Models;
using System;
using System.Collections.Generic;

namespace PMMC.Controllers
{
    /// <summary>
    /// The lookup controller
    /// </summary>
    [Authorize]
    [ApiController]
    [DefaultRouting]
    public class LookupController : BaseController
    {
        /// <summary>
        /// The logger used in controller
        /// </summary>
        private readonly ILogger<LookupController> _logger;

        /// <summary>
        /// The lookup service used in controller
        /// </summary>
        private readonly ILookupService _lookupService;

        /// <summary>
        /// Constructor with logger,lookup service,http context accessor
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="lookupService">the lookup service</param>
        /// <param name="httpContextAccessor">the http context accessor</param>
        public LookupController(ILogger<LookupController> logger, ILookupService lookupService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _lookupService = lookupService;
        }

        /// <summary>
        /// Get all system values by code type
        /// </summary>
        /// <param name="codeType">the code type</param>
        /// <returns>all system values by code type</returns>
        [HttpGet]
        [Route("[action]")]
        public IEnumerable<SystemValue> SystemValues([FromQuery] string codeType)
        {
            return _logger.Process(() => _lookupService.SystemValues(codeType, CurrentUser),
                "gets the system values for the given code type",
                parameters: codeType);
        }

        /// <summary>
        /// Get ict codes
        /// </summary>
        /// <returns>ict codes</returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult IcdCodes()
        {
            // https://discussions.topcoder.com/discussion/10744/lookup-endpoints
            throw new NotImplementedException("Final fix");
        }

        /// <summary>
        /// Get revCptCodes
        /// </summary>
        /// <returns>revCptCodes</returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult RevCptCodes()
        {
            // https://discussions.topcoder.com/discussion/10744/lookup-endpoints
            throw new NotImplementedException("Final fix");
        }
    }
}