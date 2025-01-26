import { FormEvent, FormEventHandler, useState } from "react";
import { Task } from "./types/Task";
import { addTask } from "./TaskService";

interface TaskFormProps {
    onTaskAdded: (task: Task) => void;
}

const TaskForm: React.FC<TaskFormProps> = ({ onTaskAdded }) => {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [startTime, setStartTime] = useState("");
    const [duration, setDuration] = useState("");

    const handleSubmit: FormEventHandler<HTMLFormElement> = async (
        e: FormEvent<HTMLFormElement>
    ) => {
        e.preventDefault();
        const newTask: Task = {
            id: "",
            name,
            description,
            startTime,
            duration,
            isRecurring: false,
            isCompleted: false,
            timesDone: 0,
        };
        const addedTask = await addTask(newTask);
        onTaskAdded(addedTask);
        setName("");
        setDescription("");
        setStartTime("");
        setDuration("");
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Add Task</h2>
            <label>
                Name:
                <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />
            </label>
            <label>
                Description:
                <input
                    type="text"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                />
            </label>
            <label>
                Start Time:
                <input
                    type="text"
                    value={startTime}
                    onChange={(e) => setStartTime(e.target.value)}
                />
            </label>
            <label>
                Duration:
                <input
                    type="text"
                    value={duration}
                    onChange={(e) => setDuration(e.target.value)}
                />
            </label>
            <button type="submit">Add</button>
        </form>
    );
};

export default TaskForm;
