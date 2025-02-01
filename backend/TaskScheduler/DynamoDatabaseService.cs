using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace TaskScheduler;

public class DynamoDatabaseService : IDatabaseService
{ 
    private readonly DynamoDBContext _dbContext;

    public DynamoDatabaseService()
    {
        var client = new AmazonDynamoDBClient();
        _dbContext = new DynamoDBContext(client);
    }

    public DynamoDBContext GetDbContext()
    {
        return _dbContext;
    }

    public async Task<bool> SaveTaskAsync(TaskModel taskModel)
    {
        Console.WriteLine($"Saving task {taskModel.TaskId} ...");
        try
        {
            await _dbContext.SaveAsync(taskModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

        Console.WriteLine($"Saved task {taskModel.TaskId} !");
        return true;
    }

    public async Task<TaskModel?> GetTaskAsync(Guid taskId)
    {
        try
        {
            TaskModel task = await _dbContext.LoadAsync<TaskModel>(taskId);
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<bool> DeleteTaskAsync(Guid taskId)
    {
        try
        {
            await _dbContext.DeleteAsync<TaskModel>(taskId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }

        return true;
    }
}