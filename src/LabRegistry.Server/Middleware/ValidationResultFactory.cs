using FluentValidation;
using FluentValidation.Results;
using LabRegistry.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using System.Net;

namespace LabRegistry.Server.Middleware;

public class ValidationResultFactory : IFluentValidationAutoValidationResultFactory
{
    public Task<IActionResult?> CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails validationProblemDetails,
        IDictionary<IValidationContext, ValidationResult> validationResults)
    {
        var errors = validationProblemDetails.Errors
            .SelectMany(x => x.Value.Select(message =>
                new ExceptionResponse(
                    ((int)HttpStatusCode.BadRequest).ToString(),
                    message)))
            .ToList();

        return Task.FromResult<IActionResult?>(
            new BadRequestObjectResult(errors));
    }
}