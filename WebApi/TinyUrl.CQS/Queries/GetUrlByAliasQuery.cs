using MediatR;
using TinyUrl.Database.Entities;

namespace TinyUrl.CQS.Queries
{
    public class GetUrlByAliasQuery : IRequest<Url?>
    {
        public string Alias { get; set; }
    }
}
