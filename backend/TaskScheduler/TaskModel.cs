using Amazon.DynamoDBv2.DataModel;

namespace TaskScheduler;


[DynamoDBTable("ScheduledTasks")]
public class TaskModel
{
    [DynamoDBHashKey]
    public Guid TaskId { get; set; } = Guid.NewGuid();
    public DateTime ExecutionTime { get; set; } = DateTime.Now;
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public int TimesDone { get; set; }

    public override string ToString()
    {
        return $"Task: {Name}, Starts at: {ExecutionTime}";
    }
}