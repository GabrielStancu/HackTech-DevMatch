using Core.Exceptions;

namespace LlamaSphere.AppUser.Exceptions;

public class ManagerNotFoundException : NotFoundException
{
    public ManagerNotFoundException(Guid id) : base("Manager", id)
    {
    }
}
