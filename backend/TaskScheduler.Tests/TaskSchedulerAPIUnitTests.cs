using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TaskScheduler.Tests;

public class TaskSchedulerAPIUnitTests : IClassFixture<WebApplicationFactory<TaskSchedulerAPI.Program>>
{
    private readonly HttpClient _client;

    public TaskSchedulerAPIUnitTests(WebApplicationFactory<TaskSchedulerAPI.Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async void GetAllTasks_ReturnsOkResponse()
    {
        // Act
        var response = await _client.GetAsync("/api/task");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }
    
    [Fact]
    public async void AddTask_ReturnsOkResponse()
    {
        // Arrange
        var task = new
        {
            Id = Guid.NewGuid(),
            Name = "Test Task",
            Description = "This is a test task",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var content = new StringContent(
            JsonSerializer.Serialize(task),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/task", content);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void RemoveTask_ReturnsOkResponse()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/task/{taskId}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async void StartScheduler_ReturnsOkResponse()
    {
        // Act
        var response = await _client.PostAsync("/api/task/start", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async void StopScheduler_ReturnsOkResponse()
    {
        // Act
        var response = await _client.PostAsync("/api/task/stop", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void ClearScheduler_ReturnsOkResponse()
    {
        // Act
        var response = await _client.PostAsync("/api/task/clear", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void AddTask_ActuallyAddsTaskToScheduler()
    {
        // Arrange
        var task = new
        {
            Id = Guid.NewGuid(),
            Name = "New Task",
            Description = "This is a new task",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var content = new StringContent(
            JsonSerializer.Serialize(task),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/task", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var getResponse = await _client.GetAsync("/api/task");
        getResponse.EnsureSuccessStatusCode();
        var tasks = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains(task.Name, tasks);
        
        var clear = await _client.PostAsync("/api/task/clear", null);
        clear.EnsureSuccessStatusCode();
    }

    [Fact]
    public async void AddTask_PersistsToJsonFile()
    {
        // Arrange
        var task = new
        {
            Id = Guid.NewGuid(),
            Name = "New Task",
            Description = "This is a new task",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var content = new StringContent(
            JsonSerializer.Serialize(task),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/task", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var tasksJsonFilePath = "apitasks.json";
        var tasksJson = File.ReadAllText(tasksJsonFilePath);
        Assert.Contains(task.Name, tasksJson);
        
        var clear = await _client.PostAsync("/api/task/clear", null);
        clear.EnsureSuccessStatusCode();
    }

}