namespace TaskScheduler
{
    static class Program
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

            EmailService emailService = new EmailService();
            
            TaskScheduler taskScheduler = new TaskScheduler(emailService);
            taskScheduler.AddTask(myTask);
            taskScheduler.Start();
            Thread.Sleep(10000);
            taskScheduler.Stop();
        }
    }
}