using LlamaSphere.Project.DTOs.Projects;
using LlamaSphere.Project.Features.Projects;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LlamaSphere.Project.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Models.Project>> GetProjectById(Guid id)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(id));
        var response = result.Adapt<GetProjectByIdResponse>();

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Project>>> GetProjects([FromQuery] GetProjectsRequest request)
    {
        var command = request.Adapt<GetProjectsQuery>();
        var result = await _mediator.Send(command);
        var response = result.Adapt<GetProjectsResponse>();

        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<Models.Project>> CreateProject([FromBody] CreateProjectRequest request)
    {
        var command = request.Adapt<CreateProjectCommand>();
        var result = await _mediator.Send(command);
        var response = result.Adapt<CreateProjectResponse>();

        return Created($"/Projects/{response.Id}", response);
    }

    [HttpPost]
    public async Task<ActionResult<Models.Project>> UpdateProject([FromBody] UpdateProjectRequest request)
    {
        var command = request.Adapt<UpdateProjectCommand>();
        var result = await _mediator.Send(command);
        var response = result.Adapt<UpdateProjectResponse>();

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Models.Project>> DeleteProject(Guid id)
    {
        var command = new DeleteProjectCommand(id);
        var result = await _mediator.Send(command);
        var response = result.Adapt<DeleteProjectResponse>();

        return Ok(response);
    }
}
