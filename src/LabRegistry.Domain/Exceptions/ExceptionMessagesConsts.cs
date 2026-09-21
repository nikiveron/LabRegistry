namespace LabRegistry.Domain.Exceptions;

public static class ExceptionMessagesConsts
{
    #region GetInspectionObjectsListRequestValidator

    public const string NameLengthUnder200 = "Часть имени не должна превышать 200 символов";
    public const string ProductTypeMustBeValid = "Тип продукта должен быть допустимым значением";
    public const string ProductResultMustBeValid = "Результат продукта должен быть допустимым значением";

    #endregion

    #region GetInspectionObjectRequestValidator

    public const string ValidationObjectIdIsRequired = "Идентификатор объекта проверки обязателен";
    public const string ValidationObjectIdCannotBeEmpty = "Идентификатор объекта проверки не может быть пустым";

    #endregion
}
