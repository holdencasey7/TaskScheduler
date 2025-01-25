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

            TaskScheduler taskScheduler = new TaskScheduler();
            taskScheduler.AddTask(myTask);
            taskScheduler.RunScheduledTasks();
        }
    }
}