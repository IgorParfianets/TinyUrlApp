using TinyUrl.API.Models.Responce;
using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.API.Utils
{
    public interface IJwtUtil
    {
        Task<TokenResponseModel> GenerateTokenAsync(UserDto dto);
        Task RemoveRefreshTokenAsync(Guid requestRefreshToken);
    }
}
