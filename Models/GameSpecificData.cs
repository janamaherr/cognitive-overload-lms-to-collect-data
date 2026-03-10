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
        public string OriginalWord { get; set; } = string.Empty;
        public string ScrambledWord { get; set; } = string.Empty;
        public string UserAnswer { get; set; } = string.Empty;
        public int Attempts { get; set; }
        
        // Arrow Challenge Data - Simplified
        public int Score { get; set; }
        
        public int BestScore { get; set; }
    
        public List<ArrowThrow> ArrowThrows { get; set; } = new();
    }
    
    public class ArrowThrow
    {
        public string Timestamp { get; set; } = string.Empty;
        
        public double Angle { get; set; }
        
        public bool HitAnotherArrow { get; set; }
        
        public double RotationSpeed { get; set; }
    }
}