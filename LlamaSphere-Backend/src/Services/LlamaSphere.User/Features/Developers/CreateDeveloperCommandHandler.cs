using Core.CQRS;
using FluentValidation;
using LlamaSphere.AppUser.Models;
using Marten;

namespace LlamaSphere.AppUser.Features.Developers;

public record CreateDeveloperCommand(string FirstName, string LastName) : ICommand<CreateDeveloperResult>;

public record CreateDeveloperResult(Guid Id);

public class CreateDeveloperCommandValidator : AbstractValidator<CreateDeveloperCommand>
{
    public CreateDeveloperCommandValidator()
    {
        RuleFor(d => d.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(d => d.LastName).NotEmpty().WithMessage("Last name is required");
    }
}

public class CreateDeveloperCommandHandler : ICommandHandler<CreateDeveloperCommand, CreateDeveloperResult>
{
    private readonly IDocumentSession _session;

    public CreateDeveloperCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<CreateDeveloperResult> Handle(CreateDeveloperCommand command, CancellationToken cancellationToken)
    {
        var developer = new Developer
        {
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        _session.Store(developer);
        await _session.SaveChangesAsync(cancellationToken);

        return new CreateDeveloperResult(developer.Id);
    }
}
