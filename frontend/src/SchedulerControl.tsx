import { MouseEvent, MouseEventHandler } from "react";
import { startScheduler, stopScheduler } from "./TaskService";

const SchedulerControl = () => {
    const handleStart: MouseEventHandler = async (
        e: MouseEvent<HTMLButtonElement>
    ) => {
        e.preventDefault();
        await startScheduler();
    };

    const handleStop: MouseEventHandler = async (
        e: MouseEvent<HTMLButtonElement>
    ) => {
        e.preventDefault();
        await stopScheduler();
    };

    return (
        <div>
            <button onClick={handleStart}>Start</button>
            <button onClick={handleStop}>Stop</button>
        </div>
    );
};

export default SchedulerControl;
