namespace GoalFlow.Models
{
    public class Goal
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Unique ID for tracking
        public string Name { get; set; } // "Target"
        public string Description { get; set; } // "What"
        public int Points { get; set; } // "How much" (Renamed from Cost for clarity)
        public string Periodicity { get; set; } // "How" (Daily, Weekly, Date, etc.)
        public DateTime TargetDate { get; set; } // "Until"
        public string Category { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime? LastCompletedDate { get; set; } 
        
        // Helper for UI Color
        public string CategoryColor => Category switch
        {
            "Finance" => "#2E7D32",
            "Education" => "#1565C0",
            "Health" => "#C62828",
            "Personal" => "#EF6C00",
            _ => "#555555"
        };
        
        // Helper for Icon
        public string CategoryIcon => Category switch
        {
            "Finance" => "💰",
            "Education" => "🎓",
            "Health" => "🍎",
            "Personal" => "💡",
            _ => "❓"
        };
    }
}