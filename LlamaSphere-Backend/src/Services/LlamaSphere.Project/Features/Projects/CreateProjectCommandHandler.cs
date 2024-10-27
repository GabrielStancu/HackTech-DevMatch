using Core.CQRS;
using FluentValidation;
using Marten;

namespace LlamaSphere.Project.Features.Projects;

public record CreateProjectCommand(string Name) : ICommand<CreateProjectResult>;

public record CreateProjectResult(Guid Id);

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(d => d.Name).NotEmpty().WithMessage("Name is required");
    }
}

public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, CreateProjectResult>
{
    private readonly IDocumentSession _session;

    public CreateProjectCommandHandler(IDocumentSession session)
    {
        _session = session;
    }


    public async Task<CreateProjectResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Models.Project
        {
            Name = request.Name
        };

        _session.Store(project);
        await _session.SaveChangesAsync(cancellationToken);

        return new CreateProjectResult(project.Id);
    }
}
