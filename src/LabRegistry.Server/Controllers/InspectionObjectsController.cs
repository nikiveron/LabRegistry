using LabRegistry.Application.InspectionObjects.CreateInspectionObject;
using LabRegistry.Application.InspectionObjects.GetInspectionObject;
using LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;
using LabRegistry.Application.InspectionObjects.UpdateInspectionObject;
using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
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
    [ProducesResponseType(typeof(GetInspectionObjectsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetInspectionObjectsList([AutoValidateAlways][FromQuery] GetInspectionObjectsListRequest request, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(
            new GetInspectionObjectsListQuery(
                request.NamePart,
                EnumExtensions.ParseFromEnumMember<ProductType>(request.ProductType),
                EnumExtensions.ParseFromEnumMember<ProductResult>(request.ProductResult),
                request.Page,
                request.PageSize),
            cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetInspectionObjectModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetInspectionObject([AutoValidateAlways] GetInspectionObjectRequest request, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetInspectionObjectQuery(request.Id), cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateInspectionObjectModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CreateInspectionObject([AutoValidateAlways] CreateInspectionObjectRequest request, CancellationToken cancellationToken)
    {
        var createdInspectionObject = await mediator.Send(
            new CreateInspectionObjectCommand(
                request.Name,
                request.Version,
                request.ProductType,
                request.RecieptDate,
                request.Comment),
            cancellationToken);
        return CreatedAtAction(nameof(GetInspectionObject), new { id = createdInspectionObject.Id }, createdInspectionObject);
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(List<ExceptionResponse>), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> UpdateInspectionObject([AutoValidateAlways] UpdateInspectionObjectRequest request, CancellationToken cancellationToken)
    {
        await mediator.Send(
            new UpdateInspectionObjectCommand(
                request.Id,
                EnumExtensions.ParseFromEnumMember<ProductResult>(request.Body.ProductResult),
                request.Body.Comment),
            cancellationToken);
        return NoContent();
    }
}
