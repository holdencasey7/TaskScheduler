namespace TaskScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Task myTask = new Task
            {
                Name = "My Task",
                Description = "My Task description",
                Duration = TimeSpan.FromMinutes(30),
                StartTime = DateTime.Now,
                IsRecurring = true,
                RecurrenceInterval = TimeSpan.FromHours(1),
            };
            
            Console.WriteLine(myTask);
        }
    }
}