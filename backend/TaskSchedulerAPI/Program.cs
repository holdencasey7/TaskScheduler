using System;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TaskSchedulerAPI
{
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
                await _dbContext.SaveAsync(task);
                
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = "Task added successfully!",
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = $"Error: {ex.Message}"
                };
            }
        }
    }

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
    }
}