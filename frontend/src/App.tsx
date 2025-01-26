// import { useState } from "react";
import TaskForm from "./TaskForm";
import TaskList from "./TaskList";
import SchedulerControl from "./SchedulerControl";
import { Task } from "./types/Task";

function App() {
    // const [tasks, setTasks] = useState<Task[]>([])

    const handleTaskAdded = (task: Task) => {
        console.log(task);
    };

    return (
        <div>
            <h1>Task Scheduler</h1>
            <SchedulerControl />
            <TaskForm onTaskAdded={handleTaskAdded} />
            <TaskList />
        </div>
    );
}

export default App;
