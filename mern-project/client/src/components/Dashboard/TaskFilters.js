import React from 'react';

const TaskFilters = ({ filters, onFilterChange }) => {
  const handleFilterChange = (key, value) => {
    onFilterChange({ [key]: value });
  };

  return (
    <div className="card mb-4">
      <div className="card-body">
        <h6 className="card-title">
          <i className="fas fa-filter me-2"></i>
          Filters & Sorting
        </h6>
        
        <div className="row">
          <div className="col-md-3 mb-2">
            <label htmlFor="statusFilter" className="form-label small">Status</label>
            <select
              id="statusFilter"
              className="form-select form-select-sm"
              value={filters.status}
              onChange={(e) => handleFilterChange('status', e.target.value)}
            >
              <option value="">All Status</option>
              <option value="pending">Pending</option>
              <option value="in-progress">In Progress</option>
              <option value="completed">Completed</option>
            </select>
          </div>

          <div className="col-md-3 mb-2">
            <label htmlFor="priorityFilter" className="form-label small">Priority</label>
            <select
              id="priorityFilter"
              className="form-select form-select-sm"
              value={filters.priority}
              onChange={(e) => handleFilterChange('priority', e.target.value)}
            >
              <option value="">All Priorities</option>
              <option value="high">High</option>
              <option value="medium">Medium</option>
              <option value="low">Low</option>
            </select>
          </div>

          <div className="col-md-3 mb-2">
            <label htmlFor="sortFilter" className="form-label small">Sort By</label>
            <select
              id="sortFilter"
              className="form-select form-select-sm"
              value={filters.sort}
              onChange={(e) => handleFilterChange('sort', e.target.value)}
            >
              <option value="createdAt">Newest First</option>
              <option value="dueDate">Due Date</option>
              <option value="priority">Priority</option>
            </select>
          </div>

          <div className="col-md-3 mb-2 d-flex align-items-end">
            <button
              className="btn btn-outline-secondary btn-sm w-100"
              onClick={() => onFilterChange({ status: '', priority: '', sort: 'createdAt' })}
            >
              <i className="fas fa-times me-1"></i>
              Clear Filters
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TaskFilters;