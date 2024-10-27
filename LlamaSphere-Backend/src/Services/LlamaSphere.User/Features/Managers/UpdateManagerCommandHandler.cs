using Core.CQRS;
using LlamaSphere.AppUser.Exceptions;
using LlamaSphere.AppUser.Models;
using Marten;

namespace LlamaSphere.AppUser.Features.Managers;

public record UpdateManagerCommand(Guid Id, string FirstName, string LastName) : ICommand<UpdateManagerResult>;

public record UpdateManagerResult(bool Success);

public class UpdateManagerCommandHandler : ICommandHandler<UpdateManagerCommand, UpdateManagerResult>
{
    private readonly IDocumentSession _session;

    public UpdateManagerCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<UpdateManagerResult> Handle(UpdateManagerCommand command, CancellationToken cancellationToken)
    {
        var manager = await _session.LoadAsync<Manager>(command.Id, cancellationToken);

        if (manager is null)
        {
            throw new ManagerNotFoundException(command.Id);
        }

        manager.FirstName = command.FirstName;
        manager.LastName = command.LastName;

        _session.Update(manager);
        await _session.SaveChangesAsync(cancellationToken);

        return new UpdateManagerResult(true);
    }
}
