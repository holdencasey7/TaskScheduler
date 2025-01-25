namespace TaskScheduler.Tests;

public class TaskUnitTests
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

        // Act & Assert
        Assert.Equal(1, task.Id);
        Assert.Equal("Test Task", task.Name);
        Assert.Equal(TimeSpan.FromMinutes(30), task.Duration);
        Assert.True(task.IsRecurring);
        Assert.Equal(TimeSpan.FromHours(1), task.RecurrenceInterval);
    }
}