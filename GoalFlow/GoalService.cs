using System.Text.Json;
using GoalFlow.Models;

namespace GoalFlow.Services
{
    public static class GoalService
    {
        private const string GoalsKey = "UserGoals_v1";
        private const string CompletionsKey = "GoalCompletions_v1";

        // --- GOALS ---
        public static async Task SaveGoalAsync(Goal goal)
        {
            var goals = await GetGoalsAsync();
            var existing = goals.FirstOrDefault(g => g.Id == goal.Id);
            if (existing != null)
            {
                goals.Remove(existing);
                goals.Add(goal);
            }
            else
            {
                goals.Add(goal);
            }
            Preferences.Set(GoalsKey, JsonSerializer.Serialize(goals));
        }

        public static async Task DeleteGoalAsync(string id)
        {
            var goals = await GetGoalsAsync();
            var existing = goals.FirstOrDefault(g => g.Id == id);
            if (existing != null)
            {
                goals.Remove(existing);
                Preferences.Set(GoalsKey, JsonSerializer.Serialize(goals));
            }
        }

        public static async Task<List<Goal>> GetGoalsAsync()
        {
            string json = Preferences.Get(GoalsKey, string.Empty);
            return string.IsNullOrEmpty(json) 
                ? new List<Goal>() 
                : JsonSerializer.Deserialize<List<Goal>>(json) ?? new List<Goal>();
        }

        // --- COMPLETIONS ---
        public static async Task AddCompletionRecordAsync(GoalCompletionRecord record)
        {
            var records = await GetAllCompletionsAsync();
            records.Add(record);

            // Sort by date descending (newest first)
            records = records.OrderByDescending(r => r.DateCompleted).ToList();

            Preferences.Set(CompletionsKey, JsonSerializer.Serialize(records));

            // Also update the goal's last completed date
            var goals = await GetGoalsAsync();
            var goal = goals.FirstOrDefault(g => g.Id == record.GoalId);
            if (goal != null)
            {
                goal.LastCompletedDate = record.DateCompleted;
                await SaveGoalAsync(goal);
            }
        }

        public static async Task<List<GoalCompletionRecord>> GetAllCompletionsAsync()
        {
            string json = Preferences.Get(CompletionsKey, string.Empty);
            return string.IsNullOrEmpty(json)
                ? new List<GoalCompletionRecord>()
                : JsonSerializer.Deserialize<List<GoalCompletionRecord>>(json) ?? new List<GoalCompletionRecord>();
        }

        public static async Task<List<GoalCompletionRecord>> GetCompletionsForGoalAsync(string goalId)
        {
            var all = await GetAllCompletionsAsync();
            return all.Where(r => r.GoalId == goalId).ToList();
        }
    }
}