using System.Runtime.Serialization;

namespace LabRegistry.Domain.Enums;

public enum ProductType
{
    [EnumMember(Value = "Программное обеспечение (ПО)")]
    Software,
    [EnumMember(Value = "Программно-аппаратный комплекс (ПАК)")]
    HardwareSoftware
}
