using Core.CQRS;
using Marten;
using Marten.Pagination;

namespace LlamaSphere.Project.Features.Projects;

public record GetProjectsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProjectsResult>;

public record GetProjectsResult(IEnumerable<Models.Project> Project);

public class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, GetProjectsResult>
{
    private readonly IDocumentSession _session;

    public GetProjectsQueryHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<GetProjectsResult> Handle(GetProjectsQuery query, CancellationToken cancellationToken)
    {
        var projects = await _session
            .Query<Models.Project>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetProjectsResult(projects);
    }
}
