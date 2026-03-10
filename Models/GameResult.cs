using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CognitiveOverloadLMS.Models
{
    public class GameResult
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        public string SessionId { get; set; } = string.Empty;
        public string GameType { get; set; } = string.Empty; // "Memory", "WordScramble", "ArrowChallenge"
        public int SectionNumber { get; set; } // 1, 2, or 3
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Score { get; set; }
        public double TotalTimeSeconds { get; set; }
        public bool Completed { get; set; }
        public BehaviorData BehaviorData { get; set; } = new();
        public GameSpecificData GameData { get; set; } = new();
    }
}