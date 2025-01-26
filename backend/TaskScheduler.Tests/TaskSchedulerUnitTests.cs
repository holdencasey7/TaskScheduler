using Moq;

namespace TaskScheduler.Tests;
using System.Text.Json;

public class TaskSchedulerUnitTests
{
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
        
        // Assert
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
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
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
    public void TaskScheduler_ShouldExecuteRecurringTaskNTimes()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
        var task = new Task
        {
            Name = "Test Recurring Task",
            StartTime = DateTime.Now,
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = true,
            RecurrenceInterval = TimeSpan.FromSeconds(1),
            MaxTimesDone = 2
        };
        scheduler.AddTask(task);

        // Act and Assert
        scheduler.RunScheduledTasks();
        Assert.False(task.IsCompleted);
        Assert.True(task.StartTime > DateTime.Now);
        Thread.Sleep(1100);
        scheduler.RunScheduledTasks();
        Assert.True(task.IsCompleted);
        
        scheduler.ClearTasks();
    }
    
    [Fact]
    public void TaskScheduler_ShouldExecuteSingleTask()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
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
    public void TaskScheduler_ShouldExecuteWhenStarted()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
        var task = new Task
        {
            Name = "Test Single Task",
            StartTime = DateTime.Now,
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = true,
            RecurrenceInterval = TimeSpan.FromSeconds(1)
        };
        scheduler.AddTask(task);

        // Act
        scheduler.Start();
        Thread.Sleep(3000);
        scheduler.Stop();

        // Assert
        Assert.False(task.IsCompleted);
        Assert.True(task.TimesDone > 2);
        
        scheduler.ClearTasks();
    }
    
    [Fact]
    public void TaskScheduler_ShouldExecuteTaskWhenAddedAfterStart()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
        var task = new Task
        {
            Name = "Test Single Task",
            StartTime = DateTime.Now,
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = false
        };

        // Act
        scheduler.Start();
        Thread.Sleep(1000);
        scheduler.AddTask(task);
        Thread.Sleep(1000);
        scheduler.Stop();

        // Assert
        Assert.True(task.IsCompleted);
        
        scheduler.ClearTasks();
    }
    
    [Fact]
    public void ClearTasks_ShouldRemoveAllTasksAndDeleteFile()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
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
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
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
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
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
    
    [Fact]
    public void RemoveTask_TaskRemovedFromJsonButOtherTasksRemain()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var scheduler = new TaskScheduler(mockEmailService.Object, "testing.json");
        var task1 = new Task
        {
            Name = "Task 1",
            StartTime = DateTime.Now.AddMinutes(10),
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = false
        };
        var task2 = new Task
        {
            Name = "Task 2",
            StartTime = DateTime.Now.AddMinutes(20),
            Duration = TimeSpan.FromMinutes(45),
            IsRecurring = true
        };
        scheduler.AddTask(task1);
        scheduler.AddTask(task2);

        // Act
        scheduler.RemoveTask(task1.Id);

        // Assert
        var jsonContent = File.ReadAllText("testing.json");
        Assert.False(jsonContent.Contains(task1.Id.ToString()), "Removed task should no longer be in the JSON file.");
        Assert.True(jsonContent.Contains(task2.Id.ToString()), "Remaining task should still be in the JSON file.");
        
        scheduler.ClearTasks();
    }

}