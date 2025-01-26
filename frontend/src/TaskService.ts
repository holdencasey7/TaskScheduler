import axios from "axios";
import { Task } from "./types/Task";

const API_BASE_URL = "http://localhost:5000/api/tasks";

export const getTasks = async () => {
    const response = await axios.get(API_BASE_URL);
    return response.data;
};

export const addTask = async (task: Task) => {
    console.log("Adding task: ", task);
    const response = await axios.post(API_BASE_URL, task);
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
