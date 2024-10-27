using Core.CQRS;
using LlamaSphere.AppUser.Models;
using Marten;
using Marten.Pagination;

namespace LlamaSphere.AppUser.Features.Developers;

public record GetDevelopersQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetDevelopersResult>;

public record GetDevelopersResult(IEnumerable<Developer> Developers);

public class GetDevelopersQueryHandler : IQueryHandler<GetDevelopersQuery, GetDevelopersResult>
{
    private readonly IDocumentSession _session;

    public GetDevelopersQueryHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<GetDevelopersResult> Handle(GetDevelopersQuery query, CancellationToken cancellationToken)
    {
        var developers = await _session
            .Query<Developer>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetDevelopersResult(developers);
    }
}
