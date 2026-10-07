export interface ProjectMember {
  projectId: number;
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  roleName: string;
  joinedAt: string;
}

export interface AddProjectMember {
  userId: number;
}
