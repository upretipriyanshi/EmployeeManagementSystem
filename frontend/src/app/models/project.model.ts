export interface Project {
  projectId: number;
  projectName: string;
  description: string | null;
  startDate: string | null;
  endDate: string | null;
  status: string;
  createdBy: number;
  createdByName: string;
  createdAt: string;
}

export interface UpdateProject {
  projectName: string;
  description: string | null;
  startDate: string | null;
  endDate: string | null;
  status: string;
  createdBy: number;
}

export interface CreateProject {
  projectName: string;
  description: string | null;
  startDate: string | null;
  endDate: string | null;
  status: string;
  createdBy: number;
}
