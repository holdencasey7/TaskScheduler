using System.Text.Json;

namespace TaskScheduler;

public class TaskScheduler
{
    private List<Task> _tasks;
    private readonly string _filePath;
    private CancellationTokenSource? _cancellationTokenSource;

    public TaskScheduler(string filePath = "tasks.json")
    {
        _tasks = LoadTasks();
        _filePath = filePath;
    }
    
    public int TaskCount => _tasks.Count;
    
    public List<Task> Tasks => _tasks;

    public void Start()
    {
        if (_cancellationTokenSource != null)
        {
            Console.WriteLine("Scheduler is already running.");
            return;
        }
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;
        
        System.Threading.Tasks.Task.Run(async () =>
        {
            Console.WriteLine("Scheduler started.");
            while (!cancellationToken.IsCancellationRequested)
            {
                RunScheduledTasks();
                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }, cancellationToken);
    }
    
    public void Stop()
    {
        if (_cancellationTokenSource == null)
        {
            Console.WriteLine("Scheduler is not running.");
            return;
        }

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = null;
        Console.WriteLine("Scheduler stopped.");
    }
    
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

                if (task is { IsRecurring: true, RecurrenceInterval: not null, IsCompleted: false })
                {
                    task.StartTime = now.Add(task.RecurrenceInterval.Value);
                }
            }
        }
        SaveTasks();
    }

    private void ExecuteTask(Task task)
    {
        Console.WriteLine("Executing task: " + task.Name);
        task.TimesDone += 1;
        task.IsCompleted = true;
        if (!task.IsRecurring) return;
        if (task.MaxTimesDone is null || task.TimesDone < task.MaxTimesDone.Value)
        {
            task.IsCompleted = false;
        }
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