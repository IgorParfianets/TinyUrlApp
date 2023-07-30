using Microsoft.AspNetCore.Mvc;
using Serilog;
using TinyUrl.API.Models.Responce;
using TinyUrl.Core.Abstractions;

namespace TinyUrl.API.Controllers
{
    [ApiController]
    public class DynamicController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public DynamicController(IUrlService linkService)
        {
            _urlService = linkService;
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> RedirectToMap() 
        {
            try
            {

                string alias = Request.Path.Value.Substring(1);
                string originalUrl = await _urlService.GetOriginalUrlByAlias(alias);

                if (string.IsNullOrEmpty(originalUrl))
                {
                    Log.Warning($"Original URL not found for alias: {alias}");
                    return NotFound();
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

// return NoContent();