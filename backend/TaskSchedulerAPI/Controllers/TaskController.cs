using Microsoft.AspNetCore.Mvc;
using TaskScheduler;


namespace TaskSchedulerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
        .AddUserSecrets<TaskScheduler.Program>()
        .Build();
    private static readonly EmailService EmailService = new EmailService(Configuration);
    private static readonly TaskScheduler.TaskScheduler _scheduler = new TaskScheduler.TaskScheduler(EmailService,"apitasks.json");

    [HttpGet]
    public IActionResult GetAllTasks()
    {
        Console.WriteLine("GetAllTasks Request");
        return Ok(_scheduler.Tasks);
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] TaskScheduler.Task task)
    {
        Console.WriteLine("AddTask Request");
        _scheduler.AddTask(task);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateTask(System.Guid id, [FromBody] TaskScheduler.Task task)
    {
        Console.WriteLine("UpdateTask Request");
        _scheduler.RemoveTask(id);
        _scheduler.AddTask(task);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult RemoveTask(Guid id)
    {
        Console.WriteLine("RemoveTask Request");
        _scheduler.RemoveTask(id);
        return Ok();
    }

    [HttpPost("start")]
    public IActionResult StartScheduler()
    {
        Console.WriteLine("Start Request");
        _scheduler.Start();
        return Ok();
    }

    [HttpPost("stop")]
    public IActionResult StopScheduler()
    {
        Console.WriteLine("Stop Request");
        _scheduler.Stop();
        return Ok();
    }
    
    [HttpPost("clear")]
    public IActionResult ClearScheduler()
    {
        Console.WriteLine("Clear Request");
        _scheduler.ClearTasks();
        return Ok();
    }
}
