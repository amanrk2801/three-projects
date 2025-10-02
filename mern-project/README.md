# Task Manager - MERN Stack Application

A full-stack task management application built with MongoDB, Express.js, React, and Node.js.

## Features

### Backend (Node.js + Express + MongoDB)
- **User Authentication**: JWT-based registration and login
- **RESTful API**: Complete CRUD operations for tasks
- **Data Validation**: Input validation using express-validator
- **Security**: Password hashing with bcryptjs
- **Database**: MongoDB with Mongoose ODM

### Frontend (React)
- **Modern React**: Hooks, Context API, and functional components
- **Responsive Design**: Bootstrap 5 with custom styling
- **Real-time Updates**: Dynamic task status changes
- **Filtering & Sorting**: Filter by status, priority, and sort options
- **User Experience**: Loading states, error handling, and confirmations

### Task Management Features
- Create, read, update, and delete tasks
- Task priorities (Low, Medium, High)
- Task status tracking (Pending, In Progress, Completed)
- Due date management
- Task statistics dashboard
- User-specific task isolation

## Technology Stack

### Backend
- **Node.js**: Runtime environment
- **Express.js**: Web framework
- **MongoDB**: NoSQL database
- **Mongoose**: MongoDB object modeling
- **JWT**: Authentication tokens
- **bcryptjs**: Password hashing
- **express-validator**: Input validation
- **cors**: Cross-origin resource sharing
- **dotenv**: Environment variables

### Frontend
- **React 18**: UI library with hooks
- **React Router**: Client-side routing
- **Context API**: State management
- **Axios**: HTTP client
- **Bootstrap 5**: CSS framework
- **Font Awesome**: Icons

## Installation & Setup

### Prerequisites
- Node.js (v14 or higher)
- MongoDB (local installation or MongoDB Atlas)
- npm or yarn

### Backend Setup

1. **Install dependencies**:
   ```bash
   npm install
   ```

2. **Environment Configuration**:
   Create a `.env` file in the root directory:
   ```env
   MONGODB_URI=mongodb://localhost:27017/taskmanager
   JWT_SECRET=your_super_secret_jwt_key_here
   NODE_ENV=development
   ```

3. **Start MongoDB**:
   - Local: `mongod`
   - Or use MongoDB Atlas cloud database

4. **Run the server**:
   ```bash
   npm run dev
   ```
   Server runs on http://localhost:5000

### Frontend Setup

1. **Navigate to client directory**:
   ```bash
   cd client
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Start the React app**:
   ```bash
   npm start
   ```
   Client runs on http://localhost:3000

### Full Stack Development

Run both server and client concurrently:
```bash
# Terminal 1 - Backend
npm run dev

# Terminal 2 - Frontend
cd client && npm start
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `GET /api/auth/me` - Get current user (protected)

### Tasks
- `GET /api/tasks` - Get all user tasks (with filters)
- `GET /api/tasks/:id` - Get single task
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/:id` - Update task
- `DELETE /api/tasks/:id` - Delete task

### Query Parameters for Tasks
- `status`: Filter by status (pending, in-progress, completed)
- `priority`: Filter by priority (low, medium, high)
- `sort`: Sort by field (createdAt, dueDate, priority)

## Project Structure

```
mern-project/
├── server.js              # Express server setup
├── package.json           # Backend dependencies
├── .env                   # Environment variables
├── models/
│   ├── User.js           # User model
│   └── Task.js           # Task model
├── routes/
│   ├── auth.js           # Authentication routes
│   └── tasks.js          # Task routes
├── middleware/
│   └── auth.js           # JWT authentication middleware
└── client/
    ├── package.json      # Frontend dependencies
    ├── public/
    │   └── index.html    # HTML template
    └── src/
        ├── App.js        # Main App component
        ├── index.js      # React entry point
        ├── index.css     # Global styles
        ├── context/
        │   ├── AuthContext.js    # Authentication state
        │   └── TaskContext.js    # Task state management
        └── components/
            ├── Navbar.js         # Navigation component
            ├── PrivateRoute.js   # Route protection
            ├── Auth/
            │   ├── Login.js      # Login form
            │   └── Register.js   # Registration form
            └── Dashboard/
                ├── Dashboard.js   # Main dashboard
                ├── TaskList.js    # Task list display
                ├── TaskCard.js    # Individual task card
                ├── TaskForm.js    # Task creation/editing
                └── TaskFilters.js # Filtering controls
```

## Key Features Demonstrated

### Backend Skills
- RESTful API design
- MongoDB database modeling
- JWT authentication implementation
- Input validation and error handling
- Middleware usage
- Environment configuration
- Security best practices

### Frontend Skills
- Modern React with hooks
- Context API for state management
- Component composition
- Responsive design
- Form handling and validation
- HTTP client integration
- Route protection
- User experience optimization

### Full Stack Integration
- API consumption
- Authentication flow
- Real-time data updates
- Error handling across layers
- Development workflow

## Usage

1. **Register/Login**: Create an account or login
2. **Dashboard**: View task statistics and overview
3. **Create Tasks**: Add new tasks with title, description, priority, and due date
4. **Manage Tasks**: Update status, edit details, or delete tasks
5. **Filter & Sort**: Use filters to find specific tasks
6. **Track Progress**: Monitor task completion with visual indicators

## Deployment

### Backend Deployment (Heroku)
1. Create Heroku app
2. Set environment variables
3. Deploy with Git

### Frontend Deployment (Netlify/Vercel)
1. Build the React app: `npm run build`
2. Deploy the build folder

### Database (MongoDB Atlas)
1. Create MongoDB Atlas cluster
2. Update MONGODB_URI in environment variables

## Contributing

This is a portfolio project demonstrating MERN stack capabilities. Feel free to fork and enhance!

## License

MIT License - feel free to use this project for learning and portfolio purposes.