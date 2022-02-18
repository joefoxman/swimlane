using Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.ViewModels;

namespace Web.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase {
        private readonly IQueryService _queryService;

        public ServicesController(IQueryService queryService) { 
            _queryService = queryService;
        }

        /// <summary>
        /// Run services for an IP or Domain name
        /// </summary>
        /// <remarks>
        /// Will execute services for an IP or Domain. 
        /// If no services are given will default to a list of services
        /// Runs the services in parrellel
        /// </remarks>
        /// <param name="model">Either an IP address or a Domain name and list of services to run.</param>
        [HttpPost(Name = "RunServices")]
        [ProducesResponseType(typeof(ServiceWrapperModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RunServices([FromBody] ServiceInputModel model) {
            try {
                return Ok(await _queryService.RunServices(model));
            }
            catch (Exception) {
                // TODO: Log this error
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}