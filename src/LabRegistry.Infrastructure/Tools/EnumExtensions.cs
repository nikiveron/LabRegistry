using System.Reflection;
using System.Runtime.Serialization;

namespace LabRegistry.Infrastructure.Tools;

public static class EnumExtensions
{
    public static string GetEnumMemberValue<T>(T enumValue) where T : Enum
    {
        var type = enumValue.GetType();
        var name = Enum.GetName(type, enumValue);

        if (string.IsNullOrEmpty(name)) return string.Empty;

        var field = type.GetField(name);
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>(false);

        return attribute?.Value ?? name;
    }

    public static T? ParseFromEnumMember<T>(string? value) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attribute?.Value is not null &&
                string.Equals(attribute.Value, value, StringComparison.OrdinalIgnoreCase))
            {
                return (T)field.GetValue(null)!;
            }
        }

        return null;
    }

    public static bool IsValidEnumValue<T>(string? value) where T : struct, Enum
    {
        if (string.IsNullOrEmpty(value))
            return true;

        return ParseFromEnumMember<T>(value) != null;
    }
}
