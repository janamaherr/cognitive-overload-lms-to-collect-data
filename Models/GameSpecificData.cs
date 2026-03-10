using MongoDB.Bson.Serialization.Attributes;

namespace CognitiveOverloadLMS.Models
{
    public class GameSpecificData
    {
        // Memory Game Data
        public int GridSize { get; set; }
        public List<int> SequenceShown { get; set; } = new();
        public List<int> SequenceClicked { get; set; } = new();
        public int CorrectClicks { get; set; }
        
        // Word Scramble Data
        [BsonElement("wordsCompleted")]
        public List<WordResult> WordsCompleted { get; set; } = new();
        
        [BsonElement("correctCount")]
        public int CorrectCount { get; set; }
        
        [BsonElement("totalWords")]
        public int TotalWords { get; set; }
        
        [BsonElement("averageWPM")]
        public double AverageWPM { get; set; }
        
        [BsonElement("accuracy")]
        public double Accuracy { get; set; }
        
        // Arrow Challenge Data - Simplified
        public int Score { get; set; }
        
        public int BestScore { get; set; }
    
        public List<ArrowThrow> ArrowThrows { get; set; } = new();
    }
    
    public class WordResult
    {
        [BsonElement("word")]
        public string Word { get; set; } = string.Empty;
        
        [BsonElement("scrambled")]
        public string Scrambled { get; set; } = string.Empty;
        
        [BsonElement("userAnswer")]
        public string UserAnswer { get; set; } = string.Empty;
        
        [BsonElement("isCorrect")]
        public bool IsCorrect { get; set; }
        
        [BsonElement("timeTaken")]
        public double TimeTaken { get; set; }
        
        [BsonElement("wpm")]
        public double Wpm { get; set; }
        
        [BsonElement("startTime")]
        public string StartTime { get; set; } = string.Empty;
        
        [BsonElement("endTime")]
        public string EndTime { get; set; } = string.Empty;
    }
    
    public class ArrowThrow
    {
        [BsonElement("timestamp")]
        public string Timestamp { get; set; } = string.Empty;
        
        [BsonElement("angle")]
        public double Angle { get; set; }
        
        [BsonElement("hitAnotherArrow")]
        public bool HitAnotherArrow { get; set; }
        
        [BsonElement("rotationSpeed")]
        public double RotationSpeed { get; set; }
    }
}