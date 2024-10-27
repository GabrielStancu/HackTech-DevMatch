using Core.CQRS;
using LlamaSphere.AppUser.Models;
using Marten;

namespace LlamaSphere.AppUser.Features.Developers;

public record DeleteDeveloperCommand(Guid Id) : ICommand<DeleteDeveloperResult>;

public record DeleteDeveloperResult(bool Success);

public class DeleteDeveloperCommandHandler : ICommandHandler<DeleteDeveloperCommand, DeleteDeveloperResult>
{
    private readonly IDocumentSession _session;

    public DeleteDeveloperCommandHandler(IDocumentSession session)
    {
        _session = session;
    }


    public async Task<DeleteDeveloperResult> Handle(DeleteDeveloperCommand command, CancellationToken cancellationToken)
    {
        _session.Delete<Developer>(command.Id);
        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteDeveloperResult(true);
    }
}
