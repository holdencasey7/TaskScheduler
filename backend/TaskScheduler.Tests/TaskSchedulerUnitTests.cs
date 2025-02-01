using Moq;

namespace TaskScheduler.Tests;
using System.Text.Json;

public class TaskSchedulerUnitTests
{
    [Fact]
    public void Task_Initialization_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var task = new TaskModel
        {
            Name = "Test Task",
            ExecutionTime = DateTime.Now.AddMinutes(5)
        };
        
        // Assert
        Assert.Equal("Test Task", task.Name);
    }
    
    [Fact]
    public void Tasks_ShouldHaveUniqueUuids()
    {
        // Arrange
        var task1 = new TaskModel { Name = "Task 1" };
        var task2 = new TaskModel { Name = "Task 2" };

        // Act
        var uuid1 = task1.TaskId;
        var uuid2 = task2.TaskId;

        // Assert
        Assert.NotEqual(uuid1, uuid2);
    }
}