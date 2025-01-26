import axios from "axios";
import { Task } from "./types/Task";

const API_BASE_URL = "http://localhost:5000/api/task";

export const getTasks = async () => {
    const response = await axios.get(API_BASE_URL);
    return response.data;
};

export const addTask = async (task: Task) => {
    const taskData = {
        name: task.name,
        description: task.description,
        startTime: task.startTime,
        duration: task.duration,
        isRecurring: task.isRecurring,
        recurrenceInterval: task.recurrenceInterval,
        isCompleted: task.isCompleted,
        timesDone: task.timesDone,
        maxTimesDone: task.maxTimesDone,
    };
    const response = await axios.post(API_BASE_URL, taskData, {
        headers: {
            "Content-Type": "application/json",
        },
    });
    return response.data;
};

export const updateTask = async (task: Task) => {
    const response = await axios.put(`${API_BASE_URL}/${task.id}`, task);
    return response.data;
};

export const deleteTask = async (id: string) => {
    const response = await axios.delete(`${API_BASE_URL}/${id}`);
    return response.data;
};

export const clearScheduler = async () => {
    const response = await axios.post(`${API_BASE_URL}/clear`);
    return response.data;
};

export const startScheduler = async () => {
    const response = await axios.post(`${API_BASE_URL}/start`);
    return response.data;
};

export const stopScheduler = async () => {
    const response = await axios.post(`${API_BASE_URL}/stop`);
    return response.data;
};
