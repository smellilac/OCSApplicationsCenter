public class ActivityConverter : JsonConverter<Activity>
{
    public override Activity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (Enum.TryParse(reader.GetString(), true, out Activity result))
        {
            return result;
        }

        return default;
    }

    public override void Write(Utf8JsonWriter writer, Activity value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}