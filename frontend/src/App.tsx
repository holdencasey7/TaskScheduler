import TaskForm from "./TaskForm";
import { Task } from "./types/Task";
import { AxiosResponse } from "axios";

function App() {
    const handleTaskAdded = (task: AxiosResponse<Task>) => {
        console.log("Task added", task);
    };

    return (
        <div>
            <h1>Task Scheduler</h1>
            <TaskForm onTaskAdded={handleTaskAdded} />
        </div>
    );
}

export default App;
