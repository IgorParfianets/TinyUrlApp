using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.Core.Abstractions
{
    public interface IUserService
    {
        Task<int> RegisterUserAsync(UserDto userDto);
    }
}
