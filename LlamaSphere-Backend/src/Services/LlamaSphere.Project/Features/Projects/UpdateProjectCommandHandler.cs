using Core.CQRS;
using FluentValidation;
using LlamaSphere.Project.Exceptions;
using Marten;

namespace LlamaSphere.Project.Features.Projects;

public record UpdateProjectCommand(Guid Id, string Name) : ICommand<UpdateProjectResult>;

public record UpdateProjectResult(bool Success);

public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectValidator()
    {
        RuleFor(d => d.Name).NotEmpty().WithMessage("Name is required");
    }
}

public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand, UpdateProjectResult>
{
    private readonly IDocumentSession _session;

    public UpdateProjectCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<UpdateProjectResult> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await _session.LoadAsync<Models.Project>(command.Id, cancellationToken);

        if (project is null)
        {
            throw new ProjectNotFoundException(command.Id);
        }

        project.Name = command.Name;

        _session.Update(project);
        await _session.SaveChangesAsync(cancellationToken);

        return new UpdateProjectResult(true);
    }
}
