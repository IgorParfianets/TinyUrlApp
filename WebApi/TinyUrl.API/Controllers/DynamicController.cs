using Microsoft.AspNetCore.Mvc;
using Serilog;
using TinyUrl.API.Models.Responce;
using TinyUrl.Core.Abstractions;

namespace TinyUrl.API.Controllers
{
    /// <summary>
    ///     Controller that provides API endpoints for Redirection.
    /// </summary>
    [ApiController]
    public class DynamicController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public DynamicController(IUrlService linkService)
        {
            _urlService = linkService;
        }

        /// <summary>
        ///     Accepts the request and redirects to the original url
        /// </summary>
        /// <response code="204">Original url not found in storage</response>
        /// <response code="302">Redirect to original url</response>
        /// <response code="500">Unexpected error on the server side</response>
        [HttpGet("{alias}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RedirectToMap() 
        {
            try
            {
                string alias = Request.Path.Value.Substring(1);
                string originalUrl = await _urlService.GetOriginalUrlByAlias(alias);

                if (string.IsNullOrEmpty(originalUrl))
                {
                    string message = $"Original URL not found for alias: {alias}";
                    Log.Warning(message);
                    return NoContent();
                }
                return Redirect(originalUrl);    
            }
            catch (Exception ex)
            {
                Log.Error($"An unexpected error occurred: {ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, new ErrorModel { Message = "Unexpected error on the server side." });
            }
        }
    }
}
