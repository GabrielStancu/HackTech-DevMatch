using Core.CQRS;
using Marten;

namespace LlamaSphere.Project.Features.Projects;

public record DeleteProjectCommand(Guid Id) : ICommand<DeleteProjectResult>;

public record DeleteProjectResult(bool Success);

public class DeleteProjectCommandHandler : ICommandHandler<DeleteProjectCommand, DeleteProjectResult>
{
    private readonly IDocumentSession _session;

    public DeleteProjectCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<DeleteProjectResult> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        _session.Delete<Models.Project>(command.Id);
        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteProjectResult(true);
    }
}
