import { createAction, props } from '@ngrx/store';
import { Task, CreateTask, UpdateTask } from '../../models/task.model';

export const loadTasks = createAction('[Task] Load Tasks');

export const loadTasksSuccess = createAction(
  '[Task] Load Tasks Success',
  props<{ tasks: Task[] }>(),
);

export const loadTasksFailure = createAction(
  '[Task] Load Tasks Failure',
  props<{ error: string }>(),
);

export const createTask = createAction('[Task] Create Task', props<{ task: CreateTask }>());

export const createTaskSuccess = createAction(
  '[Task] Create Task Success',
  props<{ task: Task }>(),
);

export const createTaskFailure = createAction(
  '[Task] Create Task Failure',
  props<{ error: string }>(),
);

export const updateTask = createAction(
  '[Task] Update Task',
  props<{ id: number; task: UpdateTask }>(),
);

export const updateTaskSuccess = createAction(
  '[Task] Update Task Success',
  props<{ task: Task }>(),
);

export const updateTaskFailure = createAction(
  '[Task] Update Task Failure',
  props<{ error: string }>(),
);

export const deleteTask = createAction('[Task] Delete Task', props<{ id: number }>());

export const deleteTaskSuccess = createAction(
  '[Task] Delete Task Success',
  props<{ id: number }>(),
);

export const deleteTaskFailure = createAction(
  '[Task] Delete Task Failure',
  props<{ error: string }>(),
);
