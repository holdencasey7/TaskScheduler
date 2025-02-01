export interface Task {
    id: string;
    name: string;
    description?: string;
    executionTime: string;
    isCompleted: boolean;
    timesDone: number;
}
