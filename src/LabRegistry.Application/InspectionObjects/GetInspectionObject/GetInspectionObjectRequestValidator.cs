using FluentValidation;
using LabRegistry.Domain.Exceptions;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObject;

public class GetInspectionObjectRequestValidator : AbstractValidator<GetInspectionObjectRequest>
{
    public GetInspectionObjectRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ExceptionMessagesConsts.ValidationObjectIdIsRequired)
            .NotEqual(Guid.Empty)
            .WithMessage(ExceptionMessagesConsts.ValidationObjectIdCannotBeEmpty);
    }
}
