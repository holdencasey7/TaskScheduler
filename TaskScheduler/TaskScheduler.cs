namespace TaskScheduler;

public class TaskScheduler
{
    private List<Task> _tasks;

    public TaskScheduler()
    {
        _tasks = new List<Task>();
    }

    public void AddTask(Task task)
    {
        _tasks.Add(task);
    }

    public void RunScheduledTasks()
    {
        var now = DateTime.Now;
        foreach (var task in _tasks)
        {
            if (now >= task.StartTime)
            {
                ExecuteTask(task);

                if (task is { IsRecurring: true, RecurrenceInterval: not null })
                {
                    task.StartTime = now.Add(task.RecurrenceInterval.Value);
                    task.IsCompleted = false;
                }
            }
        }
    }

    public void ExecuteTask(Task task)
    {
        Console.WriteLine("Executing task: " + task.Name);
        task.IsCompleted = true;
    }
}