using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

public class SingleOrArrayConverter<T> : JsonConverter<List<T>>
{
    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = new List<T>();

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Read elements from the array
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                var element = JsonSerializer.Deserialize<T>(ref reader, options);
                result.Add(element);
            }
        }
        else
        {
            // Single value treated as a list of one element
            var element = JsonSerializer.Deserialize<T>(ref reader, options);
            result.Add(element);
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}

