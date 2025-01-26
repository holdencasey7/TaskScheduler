using Microsoft.Extensions.Configuration;

namespace TaskScheduler
{
    class Program
    {
        static void Main()
        {
            Task myTask = new Task
            {
                Name = "My Secondly Task",
                Description = "My Task description",
                Duration = TimeSpan.FromMinutes(30),
                StartTime = DateTime.Now,
                IsRecurring = true,
                RecurrenceInterval = TimeSpan.FromSeconds(5),
            };
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            EmailService emailService = new EmailService(configuration);
            
            TaskScheduler taskScheduler = new TaskScheduler(emailService);
            taskScheduler.AddTask(myTask);
            taskScheduler.Start();
            Thread.Sleep(10000);
            taskScheduler.Stop();
        }
    }
}