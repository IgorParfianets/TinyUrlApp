using Microsoft.AspNetCore.Mvc;
using Serilog;
using TinyUrl.API.Models.Request;
using TinyUrl.API.Models.Responce;
using TinyUrl.API.Utils;
using TinyUrl.Core.Abstractions;

namespace TinyUrl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtUtil _jwtUtil;

        public TokenController(IUserService userService,
            IJwtUtil jwtUtil)
        {
            _userService = userService;
            _jwtUtil = jwtUtil;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJwtToken([FromBody] LoginUserRequestModel request)
        {
            try
            {
                var isPassCorrect = await _userService.CheckUserPasswordAsync(request.Email, request.Password);
                if (!isPassCorrect)
                {
                    var message = "Password is incorrect.";
                    Log.Warning(message);
                    return BadRequest(new ErrorModel { Message = message });
                }

                var user = await _userService.GetUserByEmailAsync(request.Email);
                var response = await _jwtUtil.GenerateTokenAsync(user);

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest(new ErrorModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, (new ErrorModel() { Message = "Unexpected error on the server side." }));
            }
        }
    }
}
