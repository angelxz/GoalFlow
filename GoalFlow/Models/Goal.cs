namespace GoalFlow.Models
{
    public class Goal
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Unique ID for tracking
        public required string Name { get; set; } // "Target"
        public required string Description { get; set; } // "What"
        public int Points { get; set; } // "How much"
        public required string Periodicity { get; set; } // "How" (Daily, Weekly, Date, etc.)
        public DateTime TargetDate { get; set; } // "Until"
        public required string Category { get; set; }
        //public bool IsCompleted { get; set; }

        public DateTime? LastCompletedDate { get; set; }

        // Helper for Color
        public Microsoft.Maui.Graphics.Color CategoryColor
        {
            get
            {
                var _colorString = Category switch
                {
                    "Finance" => "#2E7D32",
                    "Education" => "#1565C0",
                    "Health" => "#C62828",
                    "Personal" => "#EF6C00",
                    _ => "#555555"
                };
                return Microsoft.Maui.Graphics.Color.FromArgb(_colorString);
            }
        }

        // Helper for Icon
        public string CategoryIcon => Category switch
        {
            "Finance" => "💰",
            "Education" => "🎓",
            "Health" => "🍎",
            "Personal" => "💡",
            _ => "❓"
        };

        //public Goal(string Name, string Description, int Points, string Periodicity, DateTime TargetDate, string Category)
        //{
        //    this.Name = Name;
        //    this.Description = Description;
        //    this.Points = Points;
        //    this.Periodicity = Periodicity;
        //    this.TargetDate = TargetDate;
        //    this.Category = Category;
        //    this.IsCompleted = false;
        //}
    }
}