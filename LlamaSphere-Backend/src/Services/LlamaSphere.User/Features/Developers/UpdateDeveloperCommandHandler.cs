using Core.CQRS;
using FluentValidation;
using LlamaSphere.AppUser.Exceptions;
using LlamaSphere.AppUser.Models;
using Marten;

namespace LlamaSphere.AppUser.Features.Developers;

public record UpdateDeveloperCommand(Guid Id, string FirstName, string LastName) : ICommand<UpdateDeveloperResult>;

public record UpdateDeveloperResult(bool Success);

public class UpdateDeveloperValidator : AbstractValidator<UpdateDeveloperCommand>
{
    public UpdateDeveloperValidator()
    {
        RuleFor(d => d.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(d => d.LastName).NotEmpty().WithMessage("Last name is required");
    }
}

public class UpdateDeveloperCommandHandler : ICommandHandler<UpdateDeveloperCommand, UpdateDeveloperResult>
{
    private readonly IDocumentSession _session;

    public UpdateDeveloperCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<UpdateDeveloperResult> Handle(UpdateDeveloperCommand command, CancellationToken cancellationToken)
    {
        var developer = await _session.LoadAsync<Developer>(command.Id, cancellationToken);

        if (developer is null)
        {
            throw new DeveloperNotFoundException(command.Id);
        }

        developer.FirstName = command.FirstName;
        developer.LastName = command.LastName;

        _session.Update(developer);
        await _session.SaveChangesAsync(cancellationToken);

        return new UpdateDeveloperResult(true);
    }
}
