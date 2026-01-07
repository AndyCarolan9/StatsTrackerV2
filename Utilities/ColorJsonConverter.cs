using System.Text.Json;
using System.Text.Json.Serialization;

namespace StatsTrackerV2.Utilities
{
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? colorString = reader.GetString();
            if(colorString == null)
            {
                return new Color();
            }

            return Color.FromArgb(colorString);
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToArgbHex());
        }
    }
}