export interface Employee {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;

  roleId: number;
  roleName: string;

  departmentId: number | null;
  departmentName: string | null;

  isActive: boolean;
  createdAt: string;
}

export interface CreateEmployee {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  roleId: number;
  departmentId: number | null;
}

export interface UpdateEmployee {
  firstName: string;
  lastName: string;
  email: string;
  roleId: number;
  departmentId: number | null;
  isActive: boolean;
}
