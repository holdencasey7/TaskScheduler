using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace TaskScheduler;

public class TaskExecutor
{
    private readonly IEmailService _emailService;
    private readonly DynamoDBContext _dbContext;

    public TaskExecutor(IEmailService emailService)
    {
        IDatabaseService databaseService = new DynamoDatabaseService();
        _dbContext = databaseService.GetDbContext();
        _emailService = emailService;
    }
    
    public async Task<bool> RunScheduledTasks()
    {
        var now = DateTime.Now;

        // Query tasks that are due
        var tasks = await _dbContext.ScanAsync<TaskModel>(
            new[] { new ScanCondition("StartTime", Amazon.DynamoDBv2.DocumentModel.ScanOperator.LessThanOrEqual, now) }
        ).GetRemainingAsync();
        foreach (var task in tasks)
        {
            if (now >= task.ExecutionTime && !task.IsCompleted)
            {
                var executed = await ExecuteTask(task);
            }
        }
        var updated = await UpdateTasks(tasks);
        return true;
    }

    private async Task<bool> UpdateTasks(List<TaskModel> tasks)
    {
        var allUpdated = true;
        foreach (var task in tasks)
        {
            try
            {
                await _dbContext.SaveAsync(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                allUpdated = false;
            }
        }
        return allUpdated;
    }
    private async Task<bool> ExecuteTask(TaskModel taskModel)
    {
        Console.WriteLine("Executing task: " + taskModel.Name);
        var sent = await SendTaskReminder(taskModel);
        if (sent)
        {
            taskModel.TimesDone += 1;
            taskModel.IsCompleted = true;
        }
        return sent;
    }
    
    private async Task<bool> SendTaskReminder(TaskModel taskModel)
    {
        string subject = $"Task Reminder! {taskModel.Name} is Due!";
        string body = $"Due at {taskModel.ExecutionTime}\n{taskModel.Description ?? "No description provided."}";
        string recipient = "me@holdencasey.com"; // TODO: probably make recipient field in TaskModel or something
        try
        {
            var sent = await _emailService.SendEmailAsync(recipient, subject, body);
            return sent;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}