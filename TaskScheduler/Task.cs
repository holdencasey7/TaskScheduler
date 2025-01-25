namespace TaskScheduler;

public class Task
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsRecurring { get; set; }
    public TimeSpan? RecurrenceInterval { get; set; }

    public override string ToString()
    {
        return $"Task: {Name}, Starts at: {StartTime}, Duration: {Duration}, Recurring: {IsRecurring}";
    }
}