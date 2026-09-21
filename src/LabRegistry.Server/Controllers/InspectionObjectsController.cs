using LabRegistry.Application.InspectionObjects.CreateInspectionObject;
using LabRegistry.Application.InspectionObjects.GetInspectionObject;
using LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;
using LabRegistry.Application.InspectionObjects.UpdateInspectionObject;
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
    public async Task<IActionResult> GetInspectionObjectsList([AutoValidateAlways][FromQuery] GetInspectionObjectsListRequest request, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetInspectionObjectsListQuery(request.NamePart, EnumExtensions.ParseFromEnumMember<ProductType>(request.ProductType), EnumExtensions.ParseFromEnumMember<ProductResult>(request.ProductResult)), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInspectionObject([AutoValidateAlways] GetInspectionObjectRequest request, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetInspectionObjectQuery(request.Id), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateInspectionObject([AutoValidateAlways] CreateInspectionObjectRequest request, CancellationToken cancellationToken)
    {
        var createdInspectionObject = await mediator.Send(new CreateInspectionObjectCommand(request.Name, request.Version, request.ProductType, request.RecieptDate, request.Comment), cancellationToken);
        return CreatedAtAction(nameof(GetInspectionObject), new { id = createdInspectionObject.Id }, createdInspectionObject);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateInspectionObject([AutoValidateAlways] UpdateInspectionObjectRequest request, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateInspectionObjectCommand(request.Id, EnumExtensions.ParseFromEnumMember<ProductResult>(request.Body.ProductResult), request.Body.Comment), cancellationToken);
        return NoContent();
    }
}
