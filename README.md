# Task Scheduler

A full-stack task reminder scheduling application with a C# .NET backend, React Vite frontend, and AWS services for serverless functionality.

## Table of Contents

- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Backend](#backend)
- [Frontend](#frontend)
- [AWS Services](#aws-services)
- [Setup](#setup)
- [Running Locally](#running-locally)
- [License](#license)

## Overview

This project is a task scheduling application where users can create, update, and track tasks. The frontend is built with React and Vite, while the backend is built with C# .NET and utilizes AWS serverless technologies like Lambda, API Gateway, and DynamoDB for data storage.

## Technologies Used

- **Frontend**: React, Vite, TypeScript
- **Backend**: C# .NET, AWS Lambda, API Gateway, DynamoDB
- **AWS Services**: 
  - **Lambda** for serverless compute
  - **API Gateway** for handling HTTP requests
  - **DynamoDB** for storing task data
  - **EmailService** for notifications
- **Others**: AWS SDK for .NET

## Backend

The backend of this application is built using C# .NET with AWS Lambda functions to handle HTTP requests. The key backend features include:

- **Task management**: Allows adding, deleting, and retrieving tasks using AWS Lambda and DynamoDB.
- **Serverless architecture**: AWS Lambda and API Gateway are used to handle HTTP requests and execute code without maintaining servers.
- **DynamoDB**: The tasks are stored in DynamoDB, an AWS NoSQL database, with simple read and write operations.
- **Authentication**: API Gateway is secured with IAM roles and permissions.

### C# .NET AWS Lambda Function

A Lambda function processes requests from the React frontend. It interacts with DynamoDB to store and retrieve tasks, making the application scalable and cost-efficient.

Example of Lambda function:

```
public class Function
{
    private readonly DynamoDBContext _dbContext;

    public Function()
    {
        var client = new AmazonDynamoDBClient();
        _dbContext = new DynamoDBContext(client);
    }

    public async Task<APIGatewayProxyResponse> AddTaskAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var task = JsonSerializer.Deserialize<TaskModel>(request.Body);
            await _dbContext.SaveAsync(task);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "Task added successfully!",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
        catch (Exception ex)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = $"Error: {ex.Message}"
            };
        }
    }
}
```

## Frontend

The frontend is built with React and Vite for a fast and modern user interface. It allows users to create tasks by picking a date and time, entering a task name, and submitting it to the backend. The app uses TypeScript to ensure type safety across components.

### React and Vite Frontend

The frontend allows users to:

- Select a date and time for the task using a date-time picker.
- Submit tasks to the backend using API requests.
- View the list of tasks stored in DynamoDB.

Example of React component that submits tasks:

```
const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
        const response = await fetch('/tasks', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                name: taskName,
                executionTime: taskDate,
            }),
        });

        if (response.ok) {
            alert('Task added successfully!');
        } else {
            alert('Error adding task');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error adding task');
    }
};
```

## AWS Services

### AWS Lambda

AWS Lambda is used to run the backend functions. It allows us to execute code in response to API Gateway HTTP requests without managing servers.

- **Lambda function**: The Lambda function is triggered by API Gateway when a request is made to create or fetch tasks. It interacts with DynamoDB to store and retrieve tasks.

### AWS API Gateway

API Gateway is used to expose HTTP endpoints that the frontend communicates with. It routes requests to Lambda functions for processing.

- **Endpoints**: 
  - `POST /tasks`: Adds a new task to DynamoDB.

### AWS DynamoDB

DynamoDB is used to store the task data. It is a fast and flexible NoSQL database service.

- **ScheduledTasks Table**: Stores tasks with fields such as task name, execution time, and status.

### AWS EmailService

AWS SES (Simple Email Service) is used to send notifications (like task reminders or task updates) to users.

## Setup

Follow the instructions below to set up the backend and frontend for local development and deployment.

### Backend Setup (C# .NET)

1. Create a new .NET Lambda project using the AWS Lambda template.
2. Add the necessary AWS NuGet packages:
    - `AWSSDK.DynamoDBv2`
    - `Amazon.Lambda.Core`
    - `Amazon.Lambda.APIGatewayEvents`
3. Write Lambda functions to handle task-related HTTP requests.

### Frontend Setup (React with Vite)

1. Initialize a new React project with Vite.
2. Install necessary dependencies like `react-router-dom` and `axios` for HTTP requests.
3. Create components for task input, list display, and handling form submissions.

## Running Locally

1. Run the frontend application:
    ```
    npm run dev
    ```
2. Run the backend locally using the AWS SAM CLI or deploy to AWS Lambda and test with API Gateway.
3. Make sure your AWS credentials are configured to interact with DynamoDB and Lambda.

## License

This project is licensed under the MIT License.
