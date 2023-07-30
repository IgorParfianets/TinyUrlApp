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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = GetUserIdentifier();

                var urls = await _urlService.GetAllUrlsByUserIdAsync(userId);
                return Ok(urls);

            }
            catch (AuthenticationException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
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
            catch (AuthenticationException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveUrl(string alias)
        {
            try
            {
                if (string.IsNullOrEmpty(alias))
                {
                    Log.Warning("Alias cannot be empty");
                    throw new ArgumentNullException();
                }

                int result = await _urlService.RemoveUrlByAlias(alias);
                if(result > 0)
                {
                    Log.Information("Url successfully deleted");
                    return NoContent();
                }
                return NotFound();
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

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
