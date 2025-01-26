import { useEffect, useState } from "react";
import { Task } from "./types/Task";
import { getTasks, deleteTask } from "./TaskService";

const TaskList = () => {
    const [tasks, setTasks] = useState<Task[]>([]);

    useEffect(() => {
        const fetchTasks = async () => {
            const tasks = await getTasks();
            setTasks(tasks);
        };
        fetchTasks();
    }, []);

    const handleDelete = async (id: string) => {
        await deleteTask(id);
        setTasks(tasks.filter((task) => task.id !== id));
    };

    return (
        <div>
            <h1>Tasks</h1>
            <ul>
                {tasks.map((task) => (
                    <li key={task.id}>
                        <span>{task.name}</span>
                        <button onClick={() => handleDelete(task.id)}>
                            Delete
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default TaskList;
