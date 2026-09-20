using LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;
using LabRegistry.Domain.Enums;
using LabRegistry.Infrastructure.Tools;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace LabRegistry.Server.Controllers;

[Route("api/inspection-objects")]
[ApiController]
public class InspectionObjectsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetInspectionObjectsList([AutoValidateAlways] [FromQuery] GetInspectionObjectsListRequest request, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetInspectionObjectsListQuery(request.NamePart, EnumExtensions.ParseFromEnumMember<ProductType>(request.ProductType), EnumExtensions.ParseFromEnumMember<ProductResult>(request.ProductResult)), cancellationToken));
    }
}
