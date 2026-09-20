using FluentValidation;
using LabRegistry.Domain;
using LabRegistry.Domain.Enums;
using LabRegistry.Infrastructure.Tools;

namespace LabRegistry.Application.InspectionObjects.GetInspectionObjectsList
{
    public class GetInspectionObjectsListBodyRequestValidator : AbstractValidator<GetInspectionObjectsListRequest>
    {
        public GetInspectionObjectsListBodyRequestValidator()
        {
            RuleFor(x => x.NamePart)
                .MaximumLength(AppConstants.InspectionObjectNameLength)
                .WithMessage("Часть имени не должна превышать 200 символов")
                .When(x => !string.IsNullOrEmpty(x.NamePart));

            RuleFor(x => x.ProductType)
                .Must(IsValidEnumValue<ProductType>)
                .WithMessage("Тип продукта должен быть допустимым значением")
                .When(x => !string.IsNullOrEmpty(x.ProductType));

            RuleFor(x => x.ProductResult)
                .Must(IsValidEnumValue<ProductResult>)
                .WithMessage("Результат продукта должен быть допустимым значением")
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
