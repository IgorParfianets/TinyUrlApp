using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Authentication;
using System.Security.Claims;
using TinyUrl.API.Models.Request;
using TinyUrl.API.Models.Responce;
using TinyUrl.Core.Abstractions;
using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.API.Controllers
{
    /// <summary>
    ///     Controller that provides API endpoints for the Url resource.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUrlService _urlService;

        public UrlController(IMapper mapper,
            IUrlService linkService)
        {
            _mapper = mapper;
            _urlService = linkService;
        }

        /// <summary>
        ///     Return all urls
        /// </summary>
        /// <returns>All urls by identifiers</returns>
        /// <response code="200">Return all urls</response>
        /// <response code="401">Failed to get user identifiers</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<UrlDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = GetUserIdentifier();

                var urls = await _urlService.GetAllUrlsByUserIdAsync(userId);
                return Ok(urls);
            }
            catch (AuthenticationException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return Unauthorized(new ErrorModel() { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, (new ErrorModel() { Message = "Unexpected error on the server side." }));
            }
        }

        /// <summary>
        ///     Create short url
        /// </summary>
        /// <param name="model">Contains original url and alias</param>
        /// <returns>Original url and short url</returns>
        /// <response code="201">Short url successfully created and returned</response>
        /// <response code="400">Invalid inputed data</response>
        /// <response code="401">Failed to get user identifier</response>
        /// <response code="409">Alias already exists in storage</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UrlResponceModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]     
        public async Task<IActionResult> CreateUrl([FromBody] UrlRequestModel model)
        {
            try
            {
                bool isValidUrl = _urlService.ValidateUrl(model.OriginalUrl);

                if (!isValidUrl)
                {
                    string message = "Invalid inputed URL.";
                    Log.Warning(message, model.OriginalUrl);
                    return BadRequest(new ErrorModel() { Message = message});         // 422
                }

                string alias = model.Alias; 
                if (string.IsNullOrEmpty(alias) || alias.Length <= 5)
                {
                    string message = "The Alias must be at least 5 characters.";
                    Log.Warning(message, model.Alias);
                    return BadRequest(new ErrorModel() { Message = message });
                }

                bool isExistShortUrl = await _urlService.CheckExistensAliasAsync(alias);
                if (isExistShortUrl)
                {
                    string message = "The Alias is not available.";
                    Log.Warning(message, model.Alias);
                    return Conflict(new ErrorModel() { Message = message });
                }

                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                string shortenedUrl = $"{baseUrl}/{alias}";

                var urlResponse = new UrlResponceModel()
                {
                    OriginalUrl = model.OriginalUrl,
                    ShortUrl = shortenedUrl,
                };

                var urlDto = _mapper.Map<UrlDto>(urlResponse);
                urlDto.Alias = alias;

                Guid? userId = null;

                if (User.Claims.Any()) 
                    userId = GetUserIdentifier();
                
                int result = await _urlService.AddUrlAsync(urlDto, userId);

                if (result > 0)
                {
                    Log.Information("Url successfully created for authorized user.", nameof(model));
                    return Ok(urlResponse);
                }
                Log.Warning("Some data is invalid");
                return BadRequest(new ErrorModel() { Message = "Failed to create URL." });
            }
            catch (AuthenticationException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return Unauthorized(new ErrorModel() { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, (new ErrorModel() { Message = "Unexpected error on the server side." }));
            }
        }

        /// <summary>
        ///     Remove url record
        /// </summary>
        /// <param name="alias">Part of name url</param>
        /// <response code="204">Url successfully removed</response>
        /// <response code="400">Alias cannot be empty</response>
        /// <response code="404">Record with that alias not found</response>
        /// <response code="500">Unexpected error on the server side.</response>
        [HttpDelete("{alias}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUrl(string alias)
        {
            try
            {
                var userId = GetUserIdentifier();

                if (string.IsNullOrEmpty(alias))
                {
                    Log.Warning("Alias cannot be empty");
                    throw new ArgumentNullException(nameof(alias));
                }

                int result = await _urlService.RemoveUrlByAlias(alias, userId);
                if(result > 0)
                {
                    Log.Information("Url successfully deleted");
                    return NoContent();
                }

                string message = $"Record with alias: {alias} not found.";
                Log.Information(message);
                return NotFound(new ErrorModel() { Message = message });
            }
            catch (AuthenticationException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return Unauthorized(new ErrorModel() { Message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest(new ErrorModel() { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, (new ErrorModel() { Message = "Unexpected error on the server side." }));
            }
        }

        /// <summary>
        ///     Return User identifier
        /// </summary>
        /// <returns>User identifier <see cref="Guid"></returns>
        /// <exception cref="AuthenticationException">If failed to get user identifier</exception>
        private Guid GetUserIdentifier()
        {
            var userClaim = User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier && Guid.TryParse(claim.Value, out _));

            if (userClaim == null)
                throw new AuthenticationException(nameof(userClaim));

            return Guid.Parse(userClaim.Value);
        }
    }
}
