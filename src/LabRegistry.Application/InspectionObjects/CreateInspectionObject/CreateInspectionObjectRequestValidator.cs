using FluentValidation;
using LabRegistry.Domain;
using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Tools;

namespace LabRegistry.Application.InspectionObjects.CreateInspectionObject;

public class CreateInspectionObjectRequestValidator : AbstractValidator<CreateInspectionObjectRequest>
{
    public CreateInspectionObjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage(ExceptionMessagesConsts.NameIsRequired)
            .MaximumLength(AppConstants.InspectionObjectNameLength)
                .WithMessage(ExceptionMessagesConsts.NameLengthLimit);

        RuleFor(x => x.Version)
            .NotEmpty()
                .WithMessage(ExceptionMessagesConsts.VersionIsRequired)
            .MaximumLength(AppConstants.InspectionObjectVersionLength)
                .WithMessage(ExceptionMessagesConsts.VersionLengthLimit);

        RuleFor(x => x.ProductType)
            .NotEmpty()
                .WithMessage(ExceptionMessagesConsts.ProductTypeIsRequired)
            .Must(EnumExtensions.IsValidEnumValue<ProductType>)
                .WithMessage(ExceptionMessagesConsts.ProductTypeMustBeValid);

        RuleFor(x => x.RecieptDate)
            .NotEmpty()
                .WithMessage(ExceptionMessagesConsts.ReceiptDateIsRequired)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow)
                .WithMessage(ExceptionMessagesConsts.ReceiptDateMustBeValid);

        RuleFor(x => x.Comment)
            .MaximumLength(AppConstants.InspectionObjectCommentLength)
                .WithMessage(ExceptionMessagesConsts.CommentLengthLimit)
            .When(x => !string.IsNullOrEmpty(x.Comment));
    }
}