namespace GoalFlow.Models
{
    public class GoalCompletionRecord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string GoalId { get; set; }      // Links to the parent goal
        public string GoalName { get; set; }    // Snapshot of name at time of completion
        public int Points { get; set; }         // Points earned
        public DateTime DateCompleted { get; set; }
    }
}