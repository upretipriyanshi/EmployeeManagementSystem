import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Employee, CreateEmployee, UpdateEmployee } from '../models/employee.model';
import { API_BASE_URL } from '../config/api-base-url';
import { EmployeeService } from './employee';

describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpTestingController: HttpTestingController;
  const apiUrl = `${API_BASE_URL}/api/Employees`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(EmployeeService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTestingController.verify());

  it('gets employees from the API', () => {
    const employees: Employee[] = [
      {
        userId: 7,
        firstName: 'Sam',
        lastName: 'Lee',
        email: 'sam@example.com',
        roleId: 3,
        roleName: 'Employee',
        departmentId: null,
        departmentName: null,
        isActive: true,
        createdAt: '2026-01-01T00:00:00Z',
      },
    ];

    service.getEmployees().subscribe((result) => expect(result).toEqual(employees));

    const request = httpTestingController.expectOne(apiUrl);
    expect(request.request.method).toBe('GET');
    request.flush(employees);
  });

  it('creates an employee through the API', () => {
    const employee: CreateEmployee = {
      firstName: 'Sam',
      lastName: 'Lee',
      email: 'sam@example.com',
      password: 'strong-password',
      roleId: 3,
      departmentId: null,
    };

    service.createEmployee(employee).subscribe();

    const request = httpTestingController.expectOne(apiUrl);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual(employee);
    request.flush({});
  });

  it('updates an employee through the API', () => {
    const employee: UpdateEmployee = {
      firstName: 'Sam',
      lastName: 'Lee',
      email: 'sam@example.com',
      roleId: 3,
      departmentId: null,
      isActive: true,
    };

    service.updateEmployee(7, employee).subscribe();

    const request = httpTestingController.expectOne(`${apiUrl}/7`);
    expect(request.request.method).toBe('PUT');
    expect(request.request.body).toEqual(employee);
    request.flush({});
  });
});
