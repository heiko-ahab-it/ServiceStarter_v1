using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceStarterDataModel.DTOs
{
    [JsonConverter(typeof(CategoryEnumConverter))]
    public enum CATEGORY
    {

        [JsonPropertyName("service")] SERVICE = 0,
        [JsonPropertyName("port_test")] PORT_TEST = 1,
        [JsonPropertyName("log_test")] LOG_TEST = 2
    }

    public class CategoryEnumConverter : JsonConverter<CATEGORY>
    {
        public override CATEGORY Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? enumString = reader.GetString();

            if (string.IsNullOrEmpty(enumString))
                return default;

            // Suche nach dem Enum-Wert, der das passende [JsonPropertyName] Attribut hat
            foreach (var field in typeof(CATEGORY).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<JsonPropertyNameAttribute>();

                // Vergleich des JSON-Strings mit dem Attribut-Namen oder dem Namen des Enums selbst
                if (attribute != null && string.Equals(attribute.Name, enumString, StringComparison.OrdinalIgnoreCase))
                {
                    return (CATEGORY)field.GetValue(null)!;
                }

                if (string.Equals(field.Name, enumString, StringComparison.OrdinalIgnoreCase))
                {
                    return (CATEGORY)field.GetValue(null)!;
                }
            }

            throw new JsonException($"Der Wert '{enumString}' konnte nicht auf {nameof(CATEGORY)} gemappt werden.");
        }

        public override void Write(Utf8JsonWriter writer, CATEGORY value, JsonSerializerOptions options)
        {
            // Optional: Rückwärts-Mapping für die Serialisierung
            var field = typeof(CATEGORY).GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<JsonPropertyNameAttribute>();
            writer.WriteStringValue(attribute?.Name ?? value.ToString().ToLower());
        }
    }
    

}
