using Amazon.DynamoDBv2.DataModel;

namespace TaskScheduler;

public interface IDatabaseService
{
    DynamoDBContext GetDbContext();
    
    Task<bool> SaveTaskAsync(TaskModel taskModel);

    Task<TaskModel?> GetTaskAsync(Guid taskId);

    Task<bool> DeleteTaskAsync(Guid taskId);
}