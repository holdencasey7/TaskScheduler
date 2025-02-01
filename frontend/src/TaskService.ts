import axios from "axios";
import { Task } from "./types/Task";

const API_BASE_URL =
    "https://r6t0qs3wp3.execute-api.us-east-1.amazonaws.com/default";

export const addTask = async (task: Task) => {
    const taskData = {
        TaskId: task.id,
        Name: task.name,
        Description: task.description,
        ExecutionTime: task.executionTime,
        IsCompleted: task.isCompleted,
        TimesDone: task.timesDone,
    };
    try {
        const response = await axios.post(`${API_BASE_URL}/tasks`, taskData, {
            headers: {
                "Content-Type": "application/json",
            },
        });
        alert("Task added successfully");
        return response;
    } catch (error) {
        console.error("Error adding task", error);
        alert("Error adding task");
        return null;
    }
};
