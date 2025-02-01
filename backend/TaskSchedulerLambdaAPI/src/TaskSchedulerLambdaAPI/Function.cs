using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TaskSchedulerLambdaAPI;

public class Function
{
    private readonly DynamoDBContext _dbContext;

    public Function()
    {
        var client = new AmazonDynamoDBClient();
        _dbContext = new DynamoDBContext(client);
    }

    public async Task<APIGatewayProxyResponse> AddTaskAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var task = JsonSerializer.Deserialize<TaskModel>(request.Body);
            if (task == null)
            {
                context.Logger.LogLine("Deserialization failed");
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = "Invalid task data",
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }

            await _dbContext.SaveAsync(task);
            context.Logger.LogLine($"Task {task.Name} added successfully");

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "Task added successfully!",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error: {ex.Message}");
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = $"Error: {ex.Message}",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }

}

[DynamoDBTable("ScheduledTasks")]
public class TaskModel
{
    [DynamoDBHashKey]
    public Guid TaskId { get; set; } = Guid.NewGuid();

    [DynamoDBProperty("ExecutionTime")]
    public string ExecutionTime { get; set; } = DateTime.Now.ToString("o");

    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public int TimesDone { get; set; }
}