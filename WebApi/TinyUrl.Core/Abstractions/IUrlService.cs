using TinyUrl.Core.DataTransferObjects;

namespace TinyUrl.Core.Abstractions
{
    public interface IUrlService
    {
        public bool ValidateUrl(string originalUrl);
        public Task<bool> CheckExistensAliasAsync(string alias);
        public Task<int> AddUrlAsync(UrlDto dto, Guid? userId);
        public Task<string> GetOriginalUrlByAlias(string alias);
        public Task<int> RemoveUrlByAlias(string alias, Guid userId);
        public Task<IEnumerable<UrlDto>> GetAllUrlsByUserIdAsync(Guid userId);
    }
}
