import TaskForm from "./TaskForm";
import { Task } from "./types/Task";
import { AxiosResponse } from "axios";

function App() {
    const handleTaskAdded = (task: AxiosResponse<Task>) => {
        console.log("Task added", task);
    };

    return (
        <div className="app-container">
            <h1>Task Scheduler</h1>
            <TaskForm onTaskAdded={handleTaskAdded} />
            <p>Created by Holden Casey in January 2025</p>
        </div>
    );
}

export default App;
