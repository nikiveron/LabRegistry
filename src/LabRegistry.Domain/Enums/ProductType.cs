using System.Runtime.Serialization;

namespace LabRegistry.Domain.Enums;

public enum ProductType
{
    [EnumMember(Value = "ПО (Программное обеспечение)")]
    Software,
    [EnumMember(Value = "ПАК (Программно-аппаратный комплекс)")]
    HardwareSoftware
}
