import React from 'react';
import TaskCard from './TaskCard';

const TaskList = ({ tasks, onEditTask }) => {
  if (tasks.length === 0) {
    return (
      <div className="text-center py-5">
        <i className="fas fa-clipboard-list fa-4x text-muted mb-3"></i>
        <h4 className="text-muted">No tasks found</h4>
        <p className="text-muted">Create your first task to get started!</p>
      </div>
    );
  }

  return (
    <div className="row">
      {tasks.map(task => (
        <div key={task._id} className="col-md-6 col-lg-4 mb-3">
          <TaskCard task={task} onEdit={onEditTask} />
        </div>
      ))}
    </div>
  );
};

export default TaskList;