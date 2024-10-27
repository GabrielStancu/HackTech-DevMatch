using Core.CQRS;
using LlamaSphere.AppUser.Exceptions;
using LlamaSphere.AppUser.Models;
using Marten;

namespace LlamaSphere.AppUser.Features.Developers;

public record GetDeveloperByIdQuery(Guid Id) : IQuery<GetDeveloperByIdResult>;

public record GetDeveloperByIdResult(Developer Developer);

public class GetDeveloperByIdQueryHandler : IQueryHandler<GetDeveloperByIdQuery, GetDeveloperByIdResult>
{
    private readonly IDocumentSession _session;

    public GetDeveloperByIdQueryHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<GetDeveloperByIdResult> Handle(GetDeveloperByIdQuery query, CancellationToken cancellationToken)
    {
        var developer = await _session.LoadAsync<Developer>(query.Id, cancellationToken);

        if (developer is null)
        {
            throw new DeveloperNotFoundException(query.Id);
        }

        return new GetDeveloperByIdResult(developer);
    }
}
