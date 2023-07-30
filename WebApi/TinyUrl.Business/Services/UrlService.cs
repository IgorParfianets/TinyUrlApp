using AutoMapper;
using MediatR;
using Serilog;
using System.Text.RegularExpressions;
using TinyUrl.Core.Abstractions;
using TinyUrl.Core.DataTransferObjects;
using TinyUrl.CQS.Commands;
using TinyUrl.CQS.Queries;
using TinyUrl.Database.Entities;

namespace TinyUrl.Business.Services
{
    public class UrlService : IUrlService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UrlService(IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<bool> CheckExistensAliasAsync(string alias)
        {
            var url = await _mediator.Send(new GetUrlByAliasQuery() { Alias = alias});
            return url != null;
        }

        public async Task<int> AddUrlAsync(UrlDto dto, Guid? userId)
        {
            var url = _mapper.Map<Url>(dto);

            if (userId != null)
                url.UserId = userId;

            return await _mediator.Send(new AddUrlCommand() { Url = url });
        }

        public bool ValidateUrl(string originalUrl)
        {
            var urlValidator = new Regex("^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$");
            return urlValidator.IsMatch(originalUrl);
        }

        public async Task<string> GetOriginalUrlByAlias(string alias)
        {
            var url = await _mediator.Send(new GetUrlByAliasQuery() { Alias = alias });

            return url?.OriginalUrl ?? string.Empty;
        }
        
        public async Task<int> RemoveUrlByAlias(string alias, Guid userId)
        {
            return await _mediator.Send(new DeleteUrlCommandByAlias() { Alias = alias, UserId = userId});
        }

        public async Task<IEnumerable<UrlDto>> GetAllUrlsByUserIdAsync(Guid userId)
        {
            var urls = await _mediator.Send(new GetAllUrlsByUserIdQuery() { UserId = userId });

            return urls != null && urls.Any()
                ? urls.Select(url => _mapper.Map<UrlDto>(url)).ToArray()
                : Enumerable.Empty<UrlDto>();
        }
    }
}
