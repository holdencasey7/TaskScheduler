using System.Text.Json;

namespace TaskScheduler;

public class TaskScheduler
{
    private List<Task> _tasks;
    private readonly string _filePath;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly IEmailService _emailService;

    public TaskScheduler(IEmailService emailService, string filePath = "tasks.json")
    {
        _tasks = LoadTasks();
        _filePath = filePath;
        _emailService = emailService;
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
        SendTaskReminder(task);
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
    
    public void RemoveTask(Guid id)
    {
        var taskToRemove = _tasks.FirstOrDefault(t => t.Id == id);
        if (taskToRemove != null)
        {
            _tasks.Remove(taskToRemove);
            SaveTasks();
            Console.WriteLine($"Task with ID {id} removed successfully.");
        }
        else
        {
            Console.WriteLine($"Task with ID {id} not found.");
        }
    }
    
    public void ClearTasks()
    {
        _tasks.Clear();

        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }

    private void SendTaskReminder(Task task)
    {
        string subject = $"Task Reminder! {task.Name} is Due!";
        string body = $"Due at {task.StartTime}\n{task.Description ?? "No description provided."}";
        string recipient = "me@holdencasey.com";
        try
        {
            _emailService.SendEmail(recipient, subject, body);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}