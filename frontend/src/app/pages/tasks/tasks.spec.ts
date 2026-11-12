import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Tasks } from './tasks';
import { appTestProviders } from '../../testing/app-test-providers';

describe('Tasks', () => {
  let component: Tasks;
  let fixture: ComponentFixture<Tasks>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Tasks],
      providers: appTestProviders,
    }).compileComponents();

    fixture = TestBed.createComponent(Tasks);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
