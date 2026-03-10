using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CognitiveOverloadLMS.Models
{
    public class GameResult
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        [BsonElement("sessionId")]
        public string SessionId { get; set; } = string.Empty;
        
        [BsonElement("gameType")]
        public string GameType { get; set; } = string.Empty;
        
        [BsonElement("sectionNumber")]
        public int SectionNumber { get; set; }
        
        [BsonElement("startTime")]
        public DateTime StartTime { get; set; }
        
        [BsonElement("endTime")]
        public DateTime EndTime { get; set; }
        
        [BsonElement("score")]
        public int Score { get; set; }
        
        [BsonElement("totalTimeSeconds")]
        public double TotalTimeSeconds { get; set; }
        
        [BsonElement("completed")]
        public bool Completed { get; set; }
        
        [BsonElement("behaviorData")]
        public BehaviorData BehaviorData { get; set; } = new();
        
        [BsonElement("gameData")]
        public GameSpecificData GameData { get; set; } = new();
    }
}