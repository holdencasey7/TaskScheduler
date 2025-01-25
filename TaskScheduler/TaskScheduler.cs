using System.Text.Json;

namespace TaskScheduler;

public class TaskScheduler
{
    private List<Task> _tasks;
    private readonly string _filePath;

    public TaskScheduler(string filePath = "tasks.json")
    {
        _tasks = LoadTasks();
        _filePath = filePath;
    }
    
    public int TaskCount => _tasks.Count;
    
    public List<Task> Tasks => _tasks; 

    public void AddTask(Task task)
    {
        _tasks.Add(task);
        SaveTasks();
    }

    public void RunScheduledTasks()
    {
        var now = DateTime.Now;
        foreach (var task in _tasks)
        {
            if (now >= task.StartTime && !task.IsCompleted)
            {
                ExecuteTask(task);

                if (task is { IsRecurring: true, RecurrenceInterval: not null })
                {
                    task.StartTime = now.Add(task.RecurrenceInterval.Value);
                    task.IsCompleted = false;
                }
            }
        }
        SaveTasks();
    }

    private void ExecuteTask(Task task)
    {
        Console.WriteLine("Executing task: " + task.Name);
        task.IsCompleted = true;
    }

    private void SaveTasks()
    {
        Console.WriteLine($"Saving tasks to: {Path.GetFullPath(_filePath)}");
        var json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    private void SaveTasks(string filePath)
    {
        Console.WriteLine($"Saving tasks to: {Path.GetFullPath(filePath)}");
        var json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
    
    private List<Task> LoadTasks()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
        }
        return new List<Task>();
    }

    private List<Task> LoadTasks(string filePath)
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
        }
        return new List<Task>();
    }

    public void SetTasksFromFile(string filePath)
    {
        _tasks = LoadTasks(filePath);
    }
    
    public void ClearTasks()
    {
        _tasks.Clear();

        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
}