# Task Scheduler Backend

A simple task scheduler implemented in C# with persistent storage using JSON. The Task Scheduler runs tasks on a specified schedule and supports recurring tasks. Tasks are loaded from a JSON file and can be added, removed, or updated during runtime. API built with ASP.NET Core.

## Features

- Schedule tasks with a start time and duration.
- Support for recurring tasks.
- Store tasks in a JSON file for persistence.
- Run tasks in the background at specified intervals.
- Remove tasks from the scheduler.
- Send email notifications when tasks are executed.
- Unit testing with mockable email service.

## Requirements

- .NET SDK 6.0 or later
- A Gmail account (or other SMTP service) for email notifications (set up via [User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0)).

## Setup

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/TaskScheduler.git
   cd TaskScheduler
   ```

2. Install dependencies:
   If you don't have them already, install the .NET SDK 6.0 or later by following the [installation instructions](https://dotnet.microsoft.com/download).

3. Set up email configuration:
   - Set your email user and app password in User Secrets:
     ```
     dotnet user-secrets set "Email:User" "your_email@gmail.com"
     dotnet user-secrets set "Email:Password" "your_app_password"
     ```

4. Build the project:
   ```
   dotnet build
   ```

5. Run the application:
   ```
   dotnet run
   ```

## Usage

The task scheduler application supports basic functionality via the `TaskScheduler` class. You can add, remove, and manage tasks by interacting with the `TaskScheduler` instance.

### Adding a Task

Tasks can be added to the scheduler as follows:
```
var task = new Task
{
    Id = Guid.NewGuid(),
    Name = "Sample Task",
    Description = "This is a sample task",
    StartTime = DateTime.Now.AddMinutes(1),
    Duration = TimeSpan.FromMinutes(5),
    IsRecurring = false
};

scheduler.AddTask(task);
```

### Removing a Task

You can remove a task by its ID:
```
scheduler.RemoveTask(task.Id);
```

### Running the Scheduler

You can start the scheduler to run tasks at their specified times:
```
scheduler.Start();
```

### Stopping the Scheduler

You can stop the scheduler by calling:
```
scheduler.Stop();
```

## Unit Tests

Unit tests for the `TaskScheduler` class are written using xUnit. To run the tests, use the following command:

```
dotnet test
```

The tests include the following scenarios:

1. Adding tasks results in tasks being saved in the JSON file.
2. Existing tasks are loaded from the JSON file on startup.
3. Clearing tasks removes tasks from both memory and the JSON file.
4. Task execution updates the JSON file with task status.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
