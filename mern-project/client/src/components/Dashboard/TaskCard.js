import React from 'react';
import { useTask } from '../../context/TaskContext';

const TaskCard = ({ task, onEdit }) => {
  const { deleteTask, updateTask } = useTask();

  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete this task?')) {
      await deleteTask(task._id);
    }
  };

  const handleStatusChange = async (newStatus) => {
    await updateTask(task._id, { status: newStatus });
  };

  const getStatusBadgeClass = (status) => {
    switch (status) {
      case 'completed':
        return 'bg-success';
      case 'in-progress':
        return 'bg-warning';
      default:
        return 'bg-secondary';
    }
  };

  const getPriorityIcon = (priority) => {
    switch (priority) {
      case 'high':
        return 'fas fa-exclamation-triangle text-danger';
      case 'medium':
        return 'fas fa-minus-circle text-warning';
      default:
        return 'fas fa-circle text-success';
    }
  };

  const formatDate = (date) => {
    return new Date(date).toLocaleDateString();
  };

  return (
    <div className={`card task-card h-100 priority-${task.priority}`}>
      <div className="card-body">
        <div className="d-flex justify-content-between align-items-start mb-2">
          <h6 className="card-title mb-0">{task.title}</h6>
          <div className="dropdown">
            <button 
              className="btn btn-sm btn-outline-secondary dropdown-toggle" 
              type="button" 
              data-bs-toggle="dropdown"
            >
              <i className="fas fa-ellipsis-v"></i>
            </button>
            <ul className="dropdown-menu">
              <li>
                <button 
                  className="dropdown-item" 
                  onClick={() => onEdit(task)}
                >
                  <i className="fas fa-edit me-2"></i>Edit
                </button>
              </li>
              <li>
                <button 
                  className="dropdown-item text-danger" 
                  onClick={handleDelete}
                >
                  <i className="fas fa-trash me-2"></i>Delete
                </button>
              </li>
            </ul>
          </div>
        </div>

        {task.description && (
          <p className="card-text text-muted small mb-2">
            {task.description.length > 100 
              ? `${task.description.substring(0, 100)}...` 
              : task.description
            }
          </p>
        )}

        <div className="d-flex align-items-center mb-2">
          <i className={`${getPriorityIcon(task.priority)} me-2`}></i>
          <span className="small text-muted me-3">
            {task.priority.charAt(0).toUpperCase() + task.priority.slice(1)}
          </span>
          <span className={`badge status-badge ${getStatusBadgeClass(task.status)}`}>
            {task.status.replace('-', ' ').toUpperCase()}
          </span>
        </div>

        {task.dueDate && (
          <div className="small text-muted mb-2">
            <i className="fas fa-calendar me-1"></i>
            Due: {formatDate(task.dueDate)}
          </div>
        )}

        <div className="mt-3">
          <div className="btn-group w-100" role="group">
            <button 
              className={`btn btn-sm ${task.status === 'pending' ? 'btn-secondary' : 'btn-outline-secondary'}`}
              onClick={() => handleStatusChange('pending')}
              disabled={task.status === 'pending'}
            >
              Pending
            </button>
            <button 
              className={`btn btn-sm ${task.status === 'in-progress' ? 'btn-warning' : 'btn-outline-warning'}`}
              onClick={() => handleStatusChange('in-progress')}
              disabled={task.status === 'in-progress'}
            >
              In Progress
            </button>
            <button 
              className={`btn btn-sm ${task.status === 'completed' ? 'btn-success' : 'btn-outline-success'}`}
              onClick={() => handleStatusChange('completed')}
              disabled={task.status === 'completed'}
            >
              Completed
            </button>
          </div>
        </div>
      </div>

      <div className="card-footer bg-transparent">
        <small className="text-muted">
          Created: {formatDate(task.createdAt)}
        </small>
      </div>
    </div>
  );
};

export default TaskCard;