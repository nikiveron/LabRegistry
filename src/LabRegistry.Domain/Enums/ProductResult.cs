using System.Runtime.Serialization;

namespace LabRegistry.Domain.Enums;

public enum ProductResult
{
    [EnumMember(Value = "В работе")]
    InProgress,
    [EnumMember(Value = "Соответствует")]
    Passed,
    [EnumMember(Value = "Не соответствует")]
    Failed
}
