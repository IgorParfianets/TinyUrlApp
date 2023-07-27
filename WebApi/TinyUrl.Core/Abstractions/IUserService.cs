using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.Core.Abstractions
{
    public interface IUserService
    {
        Task<int> RegisterUserAsync(UserDto userDto);
        Task<bool> IsExistEmailAsync(string email);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<bool> CheckUserPasswordAsync(string email, string password);
    }
}
