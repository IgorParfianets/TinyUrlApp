using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TinyUrl.API.Models.Request;
using TinyUrl.API.Models.Responce;
using TinyUrl.API.Utils;
using TinyUrl.Core.Abstractions;
using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.API.Controllers
{
    /// <summary>
    /// Controller that provides API endpoints for the User resource.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IJwtUtil _jwtUtil;

        public UserController(IUserService userService,
            IMapper mapper,
            IJwtUtil jwtUtil)
        {
            _userService = userService;
            _mapper = mapper;
            _jwtUtil = jwtUtil;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegistrationUserRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!model.Password.Equals(model.PasswordConfirmation))
                    return BadRequest(new ErrorModel() { Message = "Different password and confirmed password" });

                bool isExistUserEmail = await _userService.IsExistEmailAsync(model.Email);

                if (isExistUserEmail)
                    return Conflict(new ErrorModel() { Message = "The same entry already exists in the storage." }); 

                var userDto = _mapper.Map<UserDto>(model);

                if (userDto != null)                 
                {
                    var result = await _userService.RegisterUserAsync(userDto);

                    if (result > 0)
                    {
                        var user = await _userService.GetUserByEmailAsync(model.Email);
                        var response = await _jwtUtil.GenerateTokenAsync(user);

                        return Ok(response);
                    }
                }
                return BadRequest(new ErrorModel() { Message = "Some register data is incorrect." });
            }
            catch (ArgumentNullException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest(new ErrorModel() { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                Log.Warning($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return Conflict(new ErrorModel { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500, (new ErrorModel() { Message = "Unexpected error on the server side." }));
            }
        }
    }
}
