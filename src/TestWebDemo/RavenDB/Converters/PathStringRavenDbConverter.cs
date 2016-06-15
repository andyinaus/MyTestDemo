using System;
using Microsoft.Owin;
using Raven.Imports.Newtonsoft.Json;

namespace TestWebDemo.RavenDB.Converters
{
    /// <summary>
	/// Adapt json converter to a RavenDB json converter
	/// </summary>
	/// <remarks>
	/// RavenDb includes an embedded json.net. 
	/// Because of this need to adapt the <see cref="PathStringConverter"/> to match ravens internal classes :(
	/// Since the convert is simple, just using code-copy.
	/// </remarks>
	public class PathStringRavenDbConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is PathString)
            {
                var data = (PathString)value;
                writer.WriteValue(data.Value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return new PathString(reader.Value.ToString());
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PathString);
        }
    }
}