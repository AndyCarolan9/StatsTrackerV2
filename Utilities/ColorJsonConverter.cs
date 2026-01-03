using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StatsTrackerV2.Utilities
{
    public class ColorJsonConverter : JsonConverter<System.Drawing.Color>
    {
        private static ColorConverter _converter = new();

        public override System.Drawing.Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (System.Drawing.Color)_converter.ConvertFromString(reader.GetString() ?? "")!;
        }

        public override void Write(Utf8JsonWriter writer, System.Drawing.Color value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(_converter.ConvertToString(value));
        }
    }
}