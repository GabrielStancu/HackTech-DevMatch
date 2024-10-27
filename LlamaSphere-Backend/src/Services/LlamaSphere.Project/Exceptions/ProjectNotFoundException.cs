using Core.Exceptions;

namespace LlamaSphere.Project.Exceptions;

public class ProjectNotFoundException : NotFoundException
{
    public ProjectNotFoundException(Guid id) : base("Project", id)
    {
    }
}
