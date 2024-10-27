using Core.CQRS;
using LlamaSphere.AppUser.Exceptions;
using LlamaSphere.AppUser.Models;
using Marten;

namespace LlamaSphere.AppUser.Features.Managers;

public record GetManagerByIdQuery(Guid Id) : IQuery<GetManagerByIdResult>;

public record GetManagerByIdResult(Manager Manager);

public class GetManagerByIdQueryHandler : IQueryHandler<GetManagerByIdQuery, GetManagerByIdResult>
{
    private readonly IDocumentSession _session;

    public GetManagerByIdQueryHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<GetManagerByIdResult> Handle(GetManagerByIdQuery query, CancellationToken cancellationToken)
    {
        var manager = await _session.LoadAsync<Manager>(query.Id, cancellationToken);

        if (manager is null)
        {
            throw new ManagerNotFoundException(query.Id);
        }

        return new GetManagerByIdResult(manager);
    }
}
