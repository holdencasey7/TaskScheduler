import { FormEvent, FormEventHandler, useState } from "react";
import { Task } from "./types/Task";
import { addTask } from "./TaskService";
import { AxiosResponse } from "axios";

interface TaskFormProps {
    onTaskAdded: (task: AxiosResponse<Task>) => void;
}

const TaskForm: React.FC<TaskFormProps> = ({ onTaskAdded }) => {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [executionTime, setExecutionTime] = useState(
        new Date().toISOString()
    );

    const handleDateChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const localDate = new Date(event.target.value);
        const isoDateTime = localDate.toISOString();
        setExecutionTime(isoDateTime);
    };

    const handleSubmit: FormEventHandler<HTMLFormElement> = async (
        e: FormEvent<HTMLFormElement>
    ) => {
        e.preventDefault();
        const newTask: Task = {
            id: crypto.randomUUID(),
            name,
            description,
            executionTime: executionTime,
            isCompleted: false,
            timesDone: 0,
        };
        const addedTask = await addTask(newTask);
        if (addedTask) {
            onTaskAdded(addedTask);
        }
        setName("");
        setDescription("");
        setExecutionTime(new Date().toISOString());
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
            <label htmlFor="datetime">Reminder Time:</label>
            <input
                type="datetime-local"
                id="datetime"
                name="datetime"
                value={new Date(executionTime).toISOString().slice(0, 16)}
                onChange={handleDateChange}
            />
            <button type="submit">Add</button>
        </form>
    );
};

export default TaskForm;
