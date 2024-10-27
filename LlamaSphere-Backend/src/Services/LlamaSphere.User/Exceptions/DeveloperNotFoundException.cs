using Core.Exceptions;

namespace LlamaSphere.AppUser.Exceptions;

public class DeveloperNotFoundException : NotFoundException
{
    public DeveloperNotFoundException(Guid id) : base("Developer", id)
    {
    }
}
