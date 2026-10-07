import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  selectAllTasks,
  selectTaskLoading,
  selectTaskError,
} from '../../store/task/task.selectors';
import { loadTasks } from '../../store/task/task.action';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-tasks',
  imports: [AsyncPipe],
  templateUrl: './tasks.html',
  styleUrl: './tasks.css',
})
export class Tasks implements OnInit {
  private store = inject(Store);

  tasks$ = this.store.select(selectAllTasks);
  loading$ = this.store.select(selectTaskLoading);
  error$ = this.store.select(selectTaskError);

  ngOnInit(): void {
    this.store.dispatch(loadTasks());
  }
}
