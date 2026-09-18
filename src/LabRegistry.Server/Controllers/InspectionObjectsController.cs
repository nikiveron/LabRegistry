using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LabRegistry.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InspectionObjectsController(IMediator mediator) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> GetInspectionObjectsList(CancellationToken cancellationToken)
    {
        return Ok();
    }
}
