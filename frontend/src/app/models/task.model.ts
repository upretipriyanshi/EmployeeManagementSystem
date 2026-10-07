export interface Task {
  taskId: number;
  projectId: number;
  projectName: string;

  assignedTo: number | null;
  assignedToName: string | null;

  title: string;
  description: string | null;

  status: string;
  priority: string;

  dueDate: string | null;

  createdBy: number;
  createdByName: string;

  createdAt: string;
  updatedAt: string | null;
}

export interface CreateTask {
  projectId: number;
  assignedTo: number | null;
  title: string;
  description: string | null;
  status: string;
  priority: string;
  dueDate: string | null;
  createdBy: number;
}

export interface UpdateTask {
  projectId: number;
  assignedTo: number | null;
  title: string;
  description: string | null;
  status: string;
  priority: string;
  dueDate: string | null;
  createdBy: number;
}
