namespace CognitiveOverloadLMS.Models
{
    public class BehaviorData
    {
        public List<MouseMovement> MouseMovements { get; set; } = new();
        public List<TypingEvent> TypingEvents { get; set; } = new();
        public List<HeadPosition> HeadPositions { get; set; } = new();
        public List<HesitationPause> HesitationPauses { get; set; } = new();
        public double AverageMouseSpeed { get; set; }
        public double AverageTypingSpeed { get; set; }
        public double AverageHeadMovement { get; set; }
        public double HeartRate { get; set; }
    }
    
    public class MouseMovement
    {
        public DateTime Timestamp { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double Speed { get; set; } // pixels per second
        public double Acceleration { get; set; }
    }
    
    public class TypingEvent
    {
        public DateTime Timestamp { get; set; }
        public string Key { get; set; } = string.Empty;
        public int DelaySinceLastKeyMs { get; set; }
        public double Speed { get; set; } // keys per minute
    }
    
    public class HeadPosition
    {
        public DateTime Timestamp { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double MovementDelta { get; set; }
    }
    
    public class HesitationPause
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationSeconds { get; set; }
        public string Location { get; set; } = string.Empty; // where in the game
    }
}