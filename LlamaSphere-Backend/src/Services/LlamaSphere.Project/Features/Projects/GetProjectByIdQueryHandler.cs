using Core.CQRS;
using LlamaSphere.Project.Exceptions;
using Marten;

namespace LlamaSphere.Project.Features.Projects;

public record GetProjectByIdQuery(Guid Id) : IQuery<GetProjectByIdResult>;

public record GetProjectByIdResult(Models.Project Project);

public class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, GetProjectByIdResult>
{
    private readonly IDocumentSession _session;

    public GetProjectByIdQueryHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<GetProjectByIdResult> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken)
    {
        var project = await _session.LoadAsync<Models.Project>(query.Id, cancellationToken);

        if (project is null)
        {
            throw new ProjectNotFoundException(query.Id);
        }

        return new GetProjectByIdResult(project);
    }
}
