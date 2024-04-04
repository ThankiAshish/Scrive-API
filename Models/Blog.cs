using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ScriveAPI.Models
{
    public class Blog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        [BsonRequired]
        public string Title { get; set; }

        [BsonElement("summary")]
        [BsonRequired]
        public string Summary { get; set; }

        [BsonElement("cover")]
        [BsonRequired]
        public string Cover { get; set; }

        [BsonElement("content")]
        [BsonRequired]
        public string Content { get; set; }

        [BsonElement("author")]
        [BsonRequired]
        [BsonIgnoreIfNull]
        public User Author { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; }
    }
}
