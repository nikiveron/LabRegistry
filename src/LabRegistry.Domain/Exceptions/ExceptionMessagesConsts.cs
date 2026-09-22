namespace LabRegistry.Domain.Exceptions;

public static class ExceptionMessagesConsts
{
    #region InspectionObjectsCommon

    public const string NameIsRequired = "Наименование объекта обязательно";
    public static readonly string NameLengthLimit = $"Имя не должно превышать {AppConstants.InspectionObjectNameLength} символов";
    public const string VersionIsRequired = "Версия объекта обязательна";
    public static readonly string VersionLengthLimit = $"Версия не должна превышать {AppConstants.InspectionObjectVersionLength} символов";
    public const string ProductTypeIsRequired = "Тип объекта обязателен";
    public const string ProductTypeMustBeValid = "Тип объекта должен быть допустимым значением";
    public const string ReceiptDateIsRequired = "Дата поступления объекта обязательна";
    public const string ReceiptDateMustBeValid = "Дата поступления объекта не может быть в будущем";
    public const string ProductResultMustBeValid = "Результат объекта должен быть допустимым значением";
    public static readonly string CommentLengthLimit = $"Примечание не должно превышать {AppConstants.InspectionObjectCommentLength} символов";

    #endregion

    #region InspectionObjectsRepository

    public const string InspectionObjectMustHaveNameAndVersion = "Объект проверки должен содержать поля \"Наименование\" и \"Версия\"";

    #endregion

    #region GetInspectionObjectsList

    public const string PageMustBePositive = "Номер страницы должен быть положительным";
    public const string PageSizeMustBeBetween1And100 = "Размер страницы должен быть между 1 и 100";

    #endregion

    #region GetInspectionObject

    public const string ValidationObjectIdIsRequired = "Идентификатор объекта проверки обязателен";
    public const string ValidationObjectIdCannotBeEmpty = "Идентификатор объекта проверки не может быть пустым";

    public const string InspectionObjectNotFound = "Объект проверки не найден";

    #endregion

    #region CreateInspectionObject

    public const string ProductTypeIsInvalid = "Некорректный формат типа объекта";

    #endregion

    #region DB

    public const string DbConnectionFailed = "Не удалось подключиться к базе данных. Попробуйте позже";

    #endregion
}
