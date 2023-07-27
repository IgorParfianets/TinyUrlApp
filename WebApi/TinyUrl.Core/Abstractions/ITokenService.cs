namespace TinyUrl.Core.Abstractions
{
    public interface ITokenService
    {
        Task<int> CreateRefreshTokenAsync(Guid tokenValue, Guid userId);
        Task RemoveRefreshTokenAsync(Guid tokenValue);
    }
}
