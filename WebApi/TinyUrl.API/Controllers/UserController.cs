using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TinyUrl.API.Models.Request;
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

        public UserController(IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegistrationUserRequestModel model)
        {
            try
            {
                if (!model.Password.Equals(model.PasswordConfirmation))
                    throw new ArgumentNullException(nameof(model), "Different password and confirmed password");

                bool isExistUserEmail = await _userService.IsExistEmailAsync(model.Email);

                if (isExistUserEmail)
                    throw new ArgumentException("The same entry already exists in the storage.", nameof(model));

                var userDto = _mapper.Map<UserDto>(model);

                if (userDto != null)                 
                {
                    var result = await _userService.RegisterUserAsync(userDto);

                    if (result > 0)
                    {
                        return Ok();
                    }
                }
                return BadRequest(); // todo add Exception
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
                return StatusCode(500);
            }
        }
    }
}
