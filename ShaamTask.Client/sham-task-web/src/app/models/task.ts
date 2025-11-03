export type Priority = 'Low' | 'Medium' | 'High';
export type Status = 'Pending' | 'InProgress' | 'Completed';

export interface Task {
  id: number;
  title: string;
  description?: string | null;
  priority: Priority;
  dueDate: string; // YYYY-MM-DD
  status: Status;
}
