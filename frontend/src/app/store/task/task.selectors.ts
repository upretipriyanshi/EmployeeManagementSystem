import { createFeatureSelector, createSelector } from '@ngrx/store';
import { TaskState } from './task.state';

export const selectTaskState = createFeatureSelector<TaskState>('tasks');

export const selectAllTasks = createSelector(selectTaskState, (state) => state.tasks);

export const selectSelectedTask = createSelector(selectTaskState, (state) => state.selectedTask);

export const selectTaskLoading = createSelector(selectTaskState, (state) => state.loading);

export const selectTaskError = createSelector(selectTaskState, (state) => state.error);

export const selectTotalCount = createSelector(selectTaskState, (state) => state.totalCount);

export const selectPageNumber = createSelector(selectTaskState, (state) => state.pageNumber);

export const selectPageSize = createSelector(selectTaskState, (state) => state.pageSize);

export const selectTotalPages = createSelector(selectTaskState, (state) => state.totalPages);
