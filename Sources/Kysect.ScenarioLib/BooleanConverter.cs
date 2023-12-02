using Kysect.CommonLib.BaseTypes.Extensions;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kysect.ScenarioLib;

public class BooleanConverter : JsonConverter<bool>
{
    public static BooleanConverter Instance { get; } = new BooleanConverter();

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.String => reader.GetString() switch
            {
                "true" => true,
                "false" => false,
                _ => throw new JsonException()
            },
            _ => throw new JsonException(),
        };
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.ThrowIfNull();

        writer.WriteBooleanValue(value);
    }
}