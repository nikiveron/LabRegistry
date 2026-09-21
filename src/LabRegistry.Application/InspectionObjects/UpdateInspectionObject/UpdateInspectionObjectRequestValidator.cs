using FluentValidation;
using LabRegistry.Domain;
using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Tools;

namespace LabRegistry.Application.InspectionObjects.UpdateInspectionObject;

public class UpdateInspectionObjectRequestValidator
    : AbstractValidator<UpdateInspectionObjectRequest>
{
    public UpdateInspectionObjectRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ExceptionMessagesConsts.ValidationObjectIdIsRequired)
            .NotEqual(Guid.Empty)
            .WithMessage(ExceptionMessagesConsts.ValidationObjectIdCannotBeEmpty);

        RuleFor(x => x.Body)
            .SetValidator(new UpdateInspectionObjectRequestBodyValidator());
    }

    private class UpdateInspectionObjectRequestBodyValidator
        : AbstractValidator<UpdateInspectionObjectRequestBody>
    {
        public UpdateInspectionObjectRequestBodyValidator()
        {
            RuleFor(x => x.ProductResult)
                .Must(value =>
                    string.IsNullOrEmpty(value) ||
                    EnumExtensions.IsValidEnumValue<ProductResult>(value))
                .WithMessage(ExceptionMessagesConsts.ProductResultMustBeValid);

            RuleFor(x => x.Comment)
                .MaximumLength(AppConstants.InspectionObjectCommentLength)
                .WithMessage(ExceptionMessagesConsts.CommentLengthLimit)
                .When(x => !string.IsNullOrEmpty(x.Comment));
        }
    }
}