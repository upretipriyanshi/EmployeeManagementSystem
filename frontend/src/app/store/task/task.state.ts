import { Task } from '../../models/task.model';

export interface TaskState {
  tasks: Task[];
  selectedTask: Task | null;
  loading: boolean;
  error: string | null;

  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export const initialTaskState: TaskState = {
  tasks: [],
  selectedTask: null,
  loading: false,
  error: null,

  totalCount: 0,
  pageNumber: 1,
  pageSize: 5,
  totalPages: 0,
};
