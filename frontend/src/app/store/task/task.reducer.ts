import { createReducer, on } from '@ngrx/store';
import { initialTaskState } from './task.state';
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

export const taskReducer = createReducer(
  initialTaskState,

  // When loading starts
  on(loadTasks, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  // When API succeeds
  on(loadTasksSuccess, (state, { tasks }) => ({
    ...state,
    tasks: tasks,
    loading: false,
    error: null,
  })),

  // When API fails
  on(loadTasksFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error: error,
  })),

  // When Create Task starts
  on(createTask, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  // When Create Task succeeds
  on(createTaskSuccess, (state, { task }) => ({
    ...state,
    tasks: [...state.tasks, task],
    loading: false,
    error: null,
  })),

  // When Create Task fails
  on(createTaskFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error: error,
  })),

  on(updateTask, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(updateTaskSuccess, (state, { task }) => ({
    ...state,
    tasks: state.tasks.map((existingTask) =>
      existingTask.taskId === task.taskId ? task : existingTask,
    ),
    loading: false,
    error: null,
  })),

  on(updateTaskFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error: error,
  })),

  on(deleteTask, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(deleteTaskSuccess, (state, { id }) => ({
    ...state,
    tasks: state.tasks.filter((task) => task.taskId !== id),
    loading: false,
    error: null,
  })),

  on(deleteTaskFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error: error,
  })),
);
