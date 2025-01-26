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
        return Ok(_scheduler.Tasks);
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] TaskScheduler.Task task)
    {
        _scheduler.AddTask(task);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateTask(System.Guid id, [FromBody] TaskScheduler.Task task)
    {
        _scheduler.RemoveTask(id);
        _scheduler.AddTask(task);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult RemoveTask(Guid id)
    {
        _scheduler.RemoveTask(id);
        return Ok();
    }

    [HttpPost("start")]
    public IActionResult StartScheduler()
    {
        _scheduler.Start();
        return Ok();
    }

    [HttpPost("stop")]
    public IActionResult StopScheduler()
    {
        _scheduler.Stop();
        return Ok();
    }
    
    [HttpPost("clear")]
    public IActionResult ClearScheduler()
    {
        _scheduler.ClearTasks();
        return Ok();
    }
}
