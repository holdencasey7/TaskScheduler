using Xunit.Abstractions;

namespace TaskScheduler.Tests;
using System.Text.Json;

public class TaskSchedulerUnitTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TaskSchedulerUnitTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Task_Initialization_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var task = new Task
        {
            Name = "Test Task",
            StartTime = DateTime.Now.AddMinutes(5),
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = true,
            RecurrenceInterval = TimeSpan.FromHours(1)
        };
        
        // Act and Assert
        Assert.Equal("Test Task", task.Name);
        Assert.Equal(TimeSpan.FromMinutes(30), task.Duration);
        Assert.True(task.IsRecurring);
        Assert.Equal(TimeSpan.FromHours(1), task.RecurrenceInterval);
    }
    
    [Fact]
    public void Tasks_ShouldHaveUniqueUuids()
    {
        // Arrange
        var task1 = new Task { Name = "Task 1" };
        var task2 = new Task { Name = "Task 2" };

        // Act
        var uuid1 = task1.Id;
        var uuid2 = task2.Id;

        // Assert
        Assert.NotEqual(uuid1, uuid2);
    }
    
    [Fact]
    public void TaskScheduler_ShouldExecuteRecurringTask()
    {
        // Arrange
        var scheduler = new TaskScheduler("testing.json");
        var task = new Task
        {
            Name = "Test Recurring Task",
            StartTime = DateTime.Now.AddSeconds(1),
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = true,
            RecurrenceInterval = TimeSpan.FromSeconds(2)
        };
        scheduler.AddTask(task);

        // Act
        Thread.Sleep(2000);
        scheduler.RunScheduledTasks();

        // Assert
        Assert.False(task.IsCompleted);
        Assert.True(task.StartTime > DateTime.Now.AddSeconds(1));
        
        scheduler.ClearTasks();
    }
    
    [Fact]
    public void TaskScheduler_ShouldExecuteSingleTask()
    {
        // Arrange
        var scheduler = new TaskScheduler("testing.json");
        var task = new Task
        {
            Name = "Test Single Task",
            StartTime = DateTime.Now.AddSeconds(1),
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = false
        };
        scheduler.AddTask(task);

        // Act
        Thread.Sleep(2000);
        scheduler.RunScheduledTasks();

        // Assert
        Assert.True(task.IsCompleted);
        
        scheduler.ClearTasks();
    }
    
    [Fact]
    public void ClearTasks_ShouldRemoveAllTasksAndDeleteFile()
    {
        // Arrange
        var scheduler = new TaskScheduler("testing.json");
        scheduler.AddTask(new Task { Name = "Test Task 1" });
        scheduler.RunScheduledTasks();
        scheduler.AddTask(new Task { Name = "Test Task 2" });

        // Act
        scheduler.ClearTasks();
        Assert.Equal(0, scheduler.TaskCount);
        Assert.False(File.Exists("testing.json"));
        
        scheduler.ClearTasks();
    }
    
    [Fact]
    public void AddTask_ShouldSaveTaskToJson()
    {
        // Arrange
        var scheduler = new TaskScheduler("testing.json");
        var task = new Task
        {
            Name = "a83fKoI32",
            StartTime = DateTime.Now,
            Duration = TimeSpan.FromHours(1)
        };

        // Act
        scheduler.AddTask(task);

        // Assert
        Assert.True(File.Exists("testing.json"), "testing.json file should exist after adding a task.");
        var fileContent = File.ReadAllText("testing.json");
        Assert.True(fileContent.Contains(task.Name), "Task name should be present in the JSON file.");
        
        scheduler.ClearTasks();
    }
    
    [Fact]
    public void LoadTasks_ShouldLoadExistingTasksFromJson()
    {
        // Arrange
        var scheduler = new TaskScheduler("testing.json");
        var tasks = new List<Task>
        {
            new Task { Name = "Task 1", StartTime = DateTime.Now, Duration = TimeSpan.FromHours(1) },
            new Task { Name = "Task 2", StartTime = DateTime.Now.AddHours(2), Duration = TimeSpan.FromHours(2) }
        };
        File.WriteAllText("testing.json", JsonSerializer.Serialize(tasks));

        // Act
        scheduler.SetTasksFromFile("testing.json");

        // Assert
        Assert.Equal(2, scheduler.TaskCount);
        Assert.Equal(tasks[0].Name, scheduler.Tasks[0].Name);
        
        scheduler.ClearTasks();
    }
}