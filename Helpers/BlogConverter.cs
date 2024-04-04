using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using ScriveAPI.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ScriveAPI.Helpers
{
    public class BlogConverter : JsonConverter<Blog>
    {
        public override Blog Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Blog value, JsonSerializerOptions options)
        {
            var document = value.ToBsonDocument();
            writer.WriteRawValue(document.ToJson());
        }
    }
}
