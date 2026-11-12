import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Employees } from './employees';
import { appTestProviders } from '../../testing/app-test-providers';

describe('Employees', () => {
  let component: Employees;
  let fixture: ComponentFixture<Employees>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Employees],
      providers: appTestProviders,
    }).compileComponents();

    fixture = TestBed.createComponent(Employees);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
