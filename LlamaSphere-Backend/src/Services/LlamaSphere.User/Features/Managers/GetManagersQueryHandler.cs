using Core.CQRS;
using LlamaSphere.AppUser.Models;
using Marten;
using Marten.Pagination;

namespace LlamaSphere.AppUser.Features.Managers;

public record GetManagersQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetManagersResult>;

public record GetManagersResult(IEnumerable<Manager> Managers);

public class GetManagersQueryHandler : IQueryHandler<GetManagersQuery, GetManagersResult>
{
    private readonly IDocumentSession _session;

    public GetManagersQueryHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<GetManagersResult> Handle(GetManagersQuery query, CancellationToken cancellationToken)
    {
        var managers = await _session
            .Query<Manager>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetManagersResult(managers);
    }
}
