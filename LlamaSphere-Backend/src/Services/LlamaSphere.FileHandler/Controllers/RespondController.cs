using LlamaSphere.API.DTOs;
using LlamaSphere.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LlamaSphere.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RespondController : ControllerBase
{
    private readonly IResponseEmailSender _responseEmailSender;

    public RespondController(IResponseEmailSender responseEmailSender)
    {
        _responseEmailSender = responseEmailSender;
    }

    [HttpPost]
    public async Task<ActionResult> SendResponse(EmailResponse emailResponse)
    {
        await _responseEmailSender.SendResponseEmailAsync(emailResponse);
        return Ok();
    }
}
