namespace TaskScheduler.Tests;

public class TaskSchedulerUnitTests
{
    [Fact]
    public void Task_Initialization_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var task = new Task
        {
            Id = 1,
            Name = "Test Task",
            StartTime = DateTime.Now.AddMinutes(5),
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = true,
            RecurrenceInterval = TimeSpan.FromHours(1)
        };
        
        // Act and Assert
        Assert.Equal(1, task.Id);
        Assert.Equal("Test Task", task.Name);
        Assert.Equal(TimeSpan.FromMinutes(30), task.Duration);
        Assert.True(task.IsRecurring);
        Assert.Equal(TimeSpan.FromHours(1), task.RecurrenceInterval);
    }
    
    [Fact]
    public void TaskScheduler_ShouldExecuteRecurringTask()
    {
        // Arrange
        var scheduler = new TaskScheduler();
        var task = new Task
        {
            Id = 1,
            Name = "Test Recurring Task",
            StartTime = DateTime.Now.AddSeconds(1),
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = true,
            RecurrenceInterval = TimeSpan.FromSeconds(2)
        };
        scheduler.AddTask(task);

        // Act
        System.Threading.Thread.Sleep(2000);
        scheduler.RunScheduledTasks();

        // Assert
        Assert.False(task.IsCompleted);
        Assert.True(task.StartTime > DateTime.Now.AddSeconds(1));
    }
    
    [Fact]
    public void TaskScheduler_ShouldExecuteSingleTask()
    {
        // Arrange
        var scheduler = new TaskScheduler();
        var task = new Task
        {
            Id = 1,
            Name = "Test Recurring Task",
            StartTime = DateTime.Now.AddSeconds(1),
            Duration = TimeSpan.FromMinutes(30),
            IsRecurring = false
        };
        scheduler.AddTask(task);

        // Act
        System.Threading.Thread.Sleep(2000);
        scheduler.RunScheduledTasks();

        // Assert
        Assert.True(task.IsCompleted);
    }
}