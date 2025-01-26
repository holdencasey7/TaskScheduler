export interface Task {
    id: string;
    name: string;
    description?: string;
    startTime: string;
    duration: string;
    isRecurring: boolean;
    recurrenceInterval?: string;
    isCompleted: boolean;
    timesDone: number;
    maxTimesDone?: number;
}
