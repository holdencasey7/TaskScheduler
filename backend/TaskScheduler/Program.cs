using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using JsonSerializer = Amazon.Lambda.Serialization.Json.JsonSerializer;

[assembly: LambdaSerializer(typeof(JsonSerializer))]
namespace TaskScheduler
{
    public class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Creating Email Service");
            EmailService emailService = new EmailService();
            Console.WriteLine("Starting TaskExecutor");
            TaskExecutor taskExecutor = new TaskExecutor(emailService);
            _ = await taskExecutor.RunScheduledTasks();
            Console.WriteLine("Ending TaskExecutor");
        }
    }

    public class Function
    {
        public async Task<string> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            await Program.Main();
            return JsonConvert.SerializeObject(new { Message = "Main method was triggered." });
        }
    }
}