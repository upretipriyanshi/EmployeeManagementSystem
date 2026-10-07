import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, switchMap, concatMap, of } from 'rxjs';

import { TaskService } from '../../services/task';
import {
  loadTasks,
  loadTasksSuccess,
  loadTasksFailure,
  createTask,
  createTaskSuccess,
  createTaskFailure,
  updateTask,
  updateTaskSuccess,
  updateTaskFailure,
  deleteTask,
  deleteTaskSuccess,
  deleteTaskFailure,
} from './task.action';

@Injectable()
export class TaskEffects {
  private actions$ = inject(Actions);
  private taskService = inject(TaskService);

  loadTasks$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadTasks),

      switchMap(() =>
        this.taskService.getTasks().pipe(
          map((tasks) => loadTasksSuccess({ tasks })),

          catchError(() =>
            of(
              loadTasksFailure({
                error: 'Unable to load tasks',
              }),
            ),
          ),
        ),
      ),
    ),
  );

  createTask$ = createEffect(() =>
    this.actions$.pipe(
      ofType(createTask),

      concatMap(({ task }) =>
        this.taskService.createTask(task).pipe(
          map((createdTask) => createTaskSuccess({ task: createdTask })),

          catchError(() =>
            of(
              createTaskFailure({
                error: 'Unable to create task',
              }),
            ),
          ),
        ),
      ),
    ),
  );

  updateTask$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateTask),

      concatMap(({ id, task }) =>
        this.taskService.updateTask(id, task).pipe(
          map((updatedTask) => updateTaskSuccess({ task: updatedTask })),

          catchError(() =>
            of(
              updateTaskFailure({
                error: 'Unable to update task',
              }),
            ),
          ),
        ),
      ),
    ),
  );

  deleteTask$ = createEffect(() =>
    this.actions$.pipe(
      ofType(deleteTask),

      concatMap(({ id }) =>
        this.taskService.deleteTask(id).pipe(
          map(() => deleteTaskSuccess({ id })),

          catchError(() =>
            of(
              deleteTaskFailure({
                error: 'Unable to delete task',
              }),
            ),
          ),
        ),
      ),
    ),
  );
}
