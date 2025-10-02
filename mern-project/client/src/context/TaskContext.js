import React, { createContext, useContext, useReducer } from 'react';
import axios from 'axios';

const TaskContext = createContext();

const taskReducer = (state, action) => {
  switch (action.type) {
    case 'GET_TASKS':
      return {
        ...state,
        tasks: action.payload,
        loading: false
      };
    case 'ADD_TASK':
      return {
        ...state,
        tasks: [action.payload, ...state.tasks]
      };
    case 'UPDATE_TASK':
      return {
        ...state,
        tasks: state.tasks.map(task =>
          task._id === action.payload._id ? action.payload : task
        )
      };
    case 'DELETE_TASK':
      return {
        ...state,
        tasks: state.tasks.filter(task => task._id !== action.payload)
      };
    case 'SET_LOADING':
      return {
        ...state,
        loading: action.payload
      };
    case 'TASK_ERROR':
      return {
        ...state,
        error: action.payload,
        loading: false
      };
    default:
      return state;
  }
};

export const TaskProvider = ({ children }) => {
  const initialState = {
    tasks: [],
    loading: false,
    error: null
  };

  const [state, dispatch] = useReducer(taskReducer, initialState);

  // Get all tasks
  const getTasks = async (filters = {}) => {
    try {
      dispatch({ type: 'SET_LOADING', payload: true });
      const params = new URLSearchParams(filters).toString();
      const res = await axios.get(`/api/tasks?${params}`);
      dispatch({ type: 'GET_TASKS', payload: res.data });
    } catch (error) {
      dispatch({ 
        type: 'TASK_ERROR', 
        payload: error.response?.data?.message || 'Failed to fetch tasks' 
      });
    }
  };

  // Add task
  const addTask = async (taskData) => {
    try {
      const res = await axios.post('/api/tasks', taskData);
      dispatch({ type: 'ADD_TASK', payload: res.data });
      return { success: true };
    } catch (error) {
      dispatch({ 
        type: 'TASK_ERROR', 
        payload: error.response?.data?.message || 'Failed to create task' 
      });
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to create task' 
      };
    }
  };

  // Update task
  const updateTask = async (id, taskData) => {
    try {
      const res = await axios.put(`/api/tasks/${id}`, taskData);
      dispatch({ type: 'UPDATE_TASK', payload: res.data });
      return { success: true };
    } catch (error) {
      dispatch({ 
        type: 'TASK_ERROR', 
        payload: error.response?.data?.message || 'Failed to update task' 
      });
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to update task' 
      };
    }
  };

  // Delete task
  const deleteTask = async (id) => {
    try {
      await axios.delete(`/api/tasks/${id}`);
      dispatch({ type: 'DELETE_TASK', payload: id });
      return { success: true };
    } catch (error) {
      dispatch({ 
        type: 'TASK_ERROR', 
        payload: error.response?.data?.message || 'Failed to delete task' 
      });
      return { 
        success: false, 
        message: error.response?.data?.message || 'Failed to delete task' 
      };
    }
  };

  return (
    <TaskContext.Provider value={{
      ...state,
      getTasks,
      addTask,
      updateTask,
      deleteTask
    }}>
      {children}
    </TaskContext.Provider>
  );
};

export const useTask = () => {
  const context = useContext(TaskContext);
  if (!context) {
    throw new Error('useTask must be used within a TaskProvider');
  }
  return context;
};