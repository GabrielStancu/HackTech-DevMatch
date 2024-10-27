using LlamaSphere.AppUser.DTOs.Managers;
using LlamaSphere.AppUser.Features.Managers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LlamaSphere.AppUser.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManagerController : ControllerBase
{

    private readonly IMediator _mediator;

    public ManagerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetManagerByIdResponse>> GetManagerById(Guid id)
    {
        var result = await _mediator.Send(new GetManagerByIdQuery(id));
        var response = result.Adapt<GetManagerByIdResponse>();

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<GetManagersResponse>> GetManagers([FromQuery] GetManagersRequest request)
    {
        var command = request.Adapt<GetManagersQuery>();
        var result = await _mediator.Send(command);
        var response = result.Adapt<GetManagersResponse>();

        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<CreateManagerResponse>> CreateManager([FromBody] CreateManagerRequest request)
    {
        var command = request.Adapt<CreateManagerCommand>();
        var result = await _mediator.Send(command);
        var response = result.Adapt<CreateManagerResponse>();

        return Created($"/managers/{response.Id}", response);
    }

    [HttpPost]
    public async Task<ActionResult<UpdateManagerResponse>> UpdateManager([FromBody] UpdateManagerRequest request)
    {
        var command = request.Adapt<UpdateManagerCommand>();
        var result = await _mediator.Send(command);
       var response = result.Adapt<UpdateManagerResponse>();

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<DeleteManagerResponse>> DeleteManager(Guid id)
    {
        var command = new DeleteManagerCommand(id);
        var result = await _mediator.Send(command);
        var response = result.Adapt<DeleteManagerResponse>();

        return Ok(response);
    }
}