using Core.CQRS;
using LlamaSphere.AppUser.Models;
using Marten;

namespace LlamaSphere.AppUser.Features.Managers;

public record CreateManagerCommand(string FirstName, string LastName) : ICommand<CreateManagerResult>;

public record CreateManagerResult(Guid Id);

public class CreateManagerCommandHandler : ICommandHandler<CreateManagerCommand, CreateManagerResult>
{
    private readonly IDocumentSession _session;

    public CreateManagerCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<CreateManagerResult> Handle(CreateManagerCommand command, CancellationToken cancellationToken)
    {
        var manager = new Manager
        {
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        _session.Store(manager);
        await _session.SaveChangesAsync(cancellationToken);

        return new CreateManagerResult(manager.Id);
    }
}
