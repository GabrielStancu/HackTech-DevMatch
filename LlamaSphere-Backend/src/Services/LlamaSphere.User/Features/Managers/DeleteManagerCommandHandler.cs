using Core.CQRS;
using LlamaSphere.AppUser.Models;
using Marten;

namespace LlamaSphere.AppUser.Features.Managers;

public record DeleteManagerCommand(Guid Id) : ICommand<DeleteManagerResult>;

public record DeleteManagerResult(bool Success);

public class DeleteManagerCommandHandler : ICommandHandler<DeleteManagerCommand, DeleteManagerResult>
{
    private readonly IDocumentSession _session;

    public DeleteManagerCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<DeleteManagerResult> Handle(DeleteManagerCommand command, CancellationToken cancellationToken)
    {
        _session.Delete<Manager>(command.Id);
        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteManagerResult(true);
    }
}
