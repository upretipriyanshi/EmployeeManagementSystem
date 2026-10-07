import { Component, OnInit, signal } from '@angular/core';
import { EmployeeService } from '../../services/employee';
import { Employee, CreateEmployee, UpdateEmployee } from '../../models/employee.model';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './employees.html',
  styleUrl: './employees.css',
})
export class Employees implements OnInit {
  employees = signal<Employee[]>([]);
  employeeForm!: FormGroup;

  showForm = signal(false);
  isLoading = signal(true);
  errorMessage = signal('');

  editingEmployeeId = signal<number | null>(null);

  constructor(
    private employeeService: EmployeeService,
    private formBuilder: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees.set(data);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading employees:', error);

        this.errorMessage.set('Unable to load employees.');
        this.isLoading.set(false);
      },
    });
  }

  showEmployeeForm(): void {
    this.showForm.set(true);
  }

  initializeForm(): void {
    this.employeeForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],

      lastName: ['', [Validators.required, Validators.maxLength(50)]],

      email: ['', [Validators.required, Validators.email, Validators.maxLength(150)]],

      password: ['', [Validators.required, Validators.minLength(6)]],

      roleId: [null, Validators.required],

      departmentId: [null],
    });
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    const employeeId = this.editingEmployeeId();
    const newEmployee: CreateEmployee = this.employeeForm.getRawValue();

    if (employeeId !== null) {
      const currentEmployee = this.employees().find((employee) => employee.userId === employeeId);
      if (!currentEmployee) {
        return;
      }

      const formValue = this.employeeForm.getRawValue();

      const updatedEmployee: UpdateEmployee = {
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        email: formValue.email,
        roleId: formValue.roleId,
        departmentId: formValue.departmentId,
        isActive: currentEmployee.isActive,
      };

      this.employeeService.updateEmployee(employeeId, updatedEmployee).subscribe({
        next: (updatedEmployee) => {
          this.employees.update((currentEmployees) =>
            currentEmployees.map((employee) =>
              employee.userId === updatedEmployee.userId ? updatedEmployee : employee,
            ),
          );

          this.employeeForm.reset();
          this.editingEmployeeId.set(null);
          this.showForm.set(false);
        },

        error: (error) => {
          console.error('Error updating employee:', error);
        },
      });
    } else {
      this.employeeService.createEmployee(newEmployee).subscribe({
        next: (createdEmployee) => {
          console.log('Employee created:', createdEmployee);

          this.employees.update((currentEmployees) => [...currentEmployees, createdEmployee]);

          this.employeeForm.reset();
          this.showForm.set(false);
        },

        error: (error) => {
          console.error('Error creating employee:', error);
        },
      });
    }
  }

  cancelForm(): void {
    this.employeeForm.reset();
    this.editingEmployeeId.set(null);
    this.showForm.set(false);
  }

  editEmployee(employee: Employee): void {
    this.editingEmployeeId.set(employee.userId);

    const passwordControl = this.employeeForm.get('password');

    passwordControl?.clearValidators();
    passwordControl?.updateValueAndValidity();

    this.employeeForm.patchValue({
      firstName: employee.firstName,
      lastName: employee.lastName,
      email: employee.email,
      roleId: employee.roleId,
      departmentId: employee.departmentId,
    });

    this.showForm.set(true);
  }
}
