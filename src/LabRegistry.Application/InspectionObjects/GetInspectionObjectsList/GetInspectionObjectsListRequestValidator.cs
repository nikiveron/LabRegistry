using FluentValidation;
using LabRegistry.Domain;
using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Tools;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;

public class GetInspectionObjectsListRequestValidator : AbstractValidator<GetInspectionObjectsListRequest>
{
    public GetInspectionObjectsListRequestValidator()
    {
        RuleFor(x => x.NamePart)
            .MaximumLength(AppConstants.InspectionObjectNameLength)
            .WithMessage(ExceptionMessagesConsts.NameLengthLimit)
            .When(x => !string.IsNullOrEmpty(x.NamePart));

        RuleFor(x => x.ProductType)
            .Must(EnumExtensions.IsValidEnumValue<ProductType>)
            .WithMessage(ExceptionMessagesConsts.ProductTypeMustBeValid)
            .When(x => !string.IsNullOrEmpty(x.ProductType));

        RuleFor(x => x.ProductResult)
            .Must(EnumExtensions.IsValidEnumValue<ProductResult>)
            .WithMessage(ExceptionMessagesConsts.ProductResultMustBeValid)
            .When(x => !string.IsNullOrEmpty(x.ProductResult));

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(ExceptionMessagesConsts.PageMustBePositive);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(ExceptionMessagesConsts.PageSizeMustBeBetween1And100);
    }
}
