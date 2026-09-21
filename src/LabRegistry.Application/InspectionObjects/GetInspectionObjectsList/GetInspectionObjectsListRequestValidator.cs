using FluentValidation;
using LabRegistry.Domain;
using LabRegistry.Domain.Enums;
using LabRegistry.Domain.Exceptions;
using LabRegistry.Infrastructure.Tools;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObjectsList
{
    public class GetInspectionObjectsListRequestValidator : AbstractValidator<GetInspectionObjectsListRequest>
    {
        public GetInspectionObjectsListRequestValidator()
        {
            RuleFor(x => x.NamePart)
                .MaximumLength(AppConstants.InspectionObjectNameLength)
                .WithMessage(ExceptionMessagesConsts.NameLengthUnder200)
                .When(x => !string.IsNullOrEmpty(x.NamePart));

            RuleFor(x => x.ProductType)
                .Must(IsValidEnumValue<ProductType>)
                .WithMessage(ExceptionMessagesConsts.ProductTypeMustBeValid)
                .When(x => !string.IsNullOrEmpty(x.ProductType));

            RuleFor(x => x.ProductResult)
                .Must(IsValidEnumValue<ProductResult>)
                .WithMessage(ExceptionMessagesConsts.ProductResultMustBeValid)
                .When(x => !string.IsNullOrEmpty(x.ProductResult));
        }

        private static bool IsValidEnumValue<T>(string? value) where T : struct, Enum
        {
            if (string.IsNullOrEmpty(value))
                return true;

            return EnumExtensions.ParseFromEnumMember<T>(value) != null;
        }
    }
}
