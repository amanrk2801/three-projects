# Library Management System - ASP.NET Core Web API

A comprehensive library management system built with ASP.NET Core 8, demonstrating enterprise-level C# development practices and modern web API design.

## Features

### Core Functionality
- **User Management**: Registration, authentication, and profile management
- **Book Management**: Complete CRUD operations with search and filtering
- **Borrowing System**: Book checkout, return, and tracking
- **Fine Calculation**: Automatic fine calculation for overdue books
- **Role-based Access**: Admin, Librarian, and Member roles with different permissions

### Technical Features
- **JWT Authentication**: Secure token-based authentication
- **Role-based Authorization**: Method-level security with role restrictions
- **Entity Framework Core**: Code-first approach with migrations
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Comprehensive input validation
- **Swagger/OpenAPI**: Interactive API documentation
- **CORS Support**: Cross-origin resource sharing configuration

## Technology Stack

### Framework & Runtime
- **ASP.NET Core 8.0**: Web API framework
- **.NET 8**: Runtime environment
- **C# 12**: Programming language

### Data & Persistence
- **Entity Framework Core**: ORM framework
- **SQL Server**: Production database
- **In-Memory Database**: Development and testing
- **Code-First Migrations**: Database schema management

### Security & Authentication
- **ASP.NET Core Identity**: User management framework
- **JWT Bearer Tokens**: Stateless authentication
- **Role-based Authorization**: Fine-grained access control
- **Password Hashing**: Secure password storage

### Additional Libraries
- **AutoMapper**: Object mapping
- **FluentValidation**: Input validation
- **Swashbuckle**: Swagger/OpenAPI documentation
- **Microsoft.IdentityModel.Tokens**: JWT token handling

## Project Structure

```
asp-dotnet-project/
├── Controllers/
│   ├── AuthController.cs          # Authentication endpoints
│   ├── BooksController.cs         # Book management
│   └── BorrowingsController.cs    # Borrowing operations
├── Data/
│   ├── LibraryContext.cs          # Entity Framework context
│   └── SeedData.cs                # Database seeding
├── DTOs/
│   ├── AuthDTOs.cs                # Authentication data transfer objects
│   ├── BookDTOs.cs                # Book-related DTOs
│   └── BorrowingDTOs.cs           # Borrowing-related DTOs
├── Models/
│   ├── ApplicationUser.cs         # User entity (extends IdentityUser)
│   ├── Book.cs                    # Book entity
│   └── Borrowing.cs               # Borrowing entity
├── Services/
│   ├── IAuthService.cs            # Authentication service interface
│   ├── AuthService.cs             # Authentication service implementation
│   ├── IBookService.cs            # Book service interface
│   ├── BookService.cs             # Book service implementation
│   ├── IBorrowingService.cs       # Borrowing service interface
│   └── BorrowingService.cs        # Borrowing service implementation
├── Mappings/
│   └── MappingProfile.cs          # AutoMapper configuration
├── Program.cs                     # Application entry point
├── appsettings.json              # Configuration settings
└── LibraryManagement.csproj      # Project file
```

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `GET /api/auth/profile` - Get user profile (protected)
- `PUT /api/auth/profile` - Update user profile (protected)
- `GET /api/auth/validate` - Validate JWT token (protected)

### Books
- `GET /api/books` - Get books with pagination and filtering
- `GET /api/books/{id}` - Get book by ID
- `GET /api/books/available` - Get available books
- `GET /api/books/genres` - Get all genres
- `POST /api/books` - Create book (Admin/Librarian only)
- `PUT /api/books/{id}` - Update book (Admin/Librarian only)
- `DELETE /api/books/{id}` - Delete book (Admin/Librarian only)
- `GET /api/books/{id}/availability` - Check book availability

### Borrowings
- `GET /api/borrowings` - Get all borrowings (Admin/Librarian only)
- `GET /api/borrowings/{id}` - Get borrowing by ID
- `GET /api/borrowings/my-borrowings` - Get current user's borrowings
- `GET /api/borrowings/overdue` - Get overdue borrowings (Admin/Librarian only)
- `POST /api/borrowings/borrow` - Borrow a book
- `POST /api/borrowings/return` - Return a book (Admin/Librarian only)
- `GET /api/borrowings/{id}/calculate-fine` - Calculate fine for borrowing

## Installation & Setup

### Prerequisites
- .NET 8 SDK
- SQL Server (for production) or SQL Server LocalDB
- Visual Studio 2022 or VS Code

### Development Setup

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd asp-dotnet-project
   ```

2. **Restore packages**:
   ```bash
   dotnet restore
   ```

3. **Update connection string** (optional):
   Edit `appsettings.json` to configure your database connection.

4. **Run the application**:
   ```bash
   dotnet run
   ```

5. **Access the API**:
   - API Base URL: https://localhost:7000 or http://localhost:5000
   - Swagger UI: https://localhost:7000 (root URL)

### Database Configuration

#### Development (In-Memory Database)
The application uses an in-memory database by default in development mode. No additional setup required.

#### Production (SQL Server)
1. **Update connection string** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=LibraryManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

2. **Create and apply migrations**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

### JWT Configuration

Update JWT settings in `appsettings.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "LibraryManagementAPI",
    "Audience": "LibraryManagementClient",
    "ExpiryInMinutes": 60
  }
}
```

## Sample Data

The application includes comprehensive seed data:

### Default User Accounts
- **Admin**: admin@library.com / Admin123!
- **Librarian**: librarian@library.com / Librarian123!
- **Members**: 
  - john.doe@email.com / Member123!
  - jane.smith@email.com / Member123!
  - bob.johnson@email.com / Member123!

### Sample Books
- Classic Literature (The Great Gatsby, To Kill a Mockingbird, 1984, Pride and Prejudice)
- Fantasy (Harry Potter, The Lord of the Rings)
- Science Fiction (Dune)
- Coming-of-age Fiction (The Catcher in the Rye)

### Sample Borrowings
- Active borrowings with different due dates
- Returned borrowings with history
- Overdue borrowings with fines

## API Usage Examples

### Authentication
```bash
# Register
curl -X POST https://localhost:7000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "password": "Password123!",
    "confirmPassword": "Password123!"
  }'

# Login
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "Password123!"
  }'
```

### Books
```bash
# Get books with filtering
curl "https://localhost:7000/api/books?page=1&pageSize=10&genre=Fantasy&isAvailable=true"

# Get book by ID
curl https://localhost:7000/api/books/1

# Create book (requires Admin/Librarian role)
curl -X POST https://localhost:7000/api/books \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "New Book",
    "author": "Author Name",
    "isbn": "1234567890123",
    "genre": "Fiction",
    "totalCopies": 5,
    "availableCopies": 5,
    "price": 19.99
  }'
```

### Borrowings
```bash
# Borrow a book
curl -X POST https://localhost:7000/api/borrowings/borrow \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "bookId": 1,
    "notes": "Looking forward to reading this!"
  }'

# Get my borrowings
curl -X GET https://localhost:7000/api/borrowings/my-borrowings \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Return a book (Admin/Librarian only)
curl -X POST https://localhost:7000/api/borrowings/return \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "borrowingId": 1,
    "status": "Returned",
    "fineAmount": 0
  }'
```

## Key Features Demonstrated

### ASP.NET Core Skills
- **Web API Development**: RESTful API design and implementation
- **Dependency Injection**: Service registration and consumption
- **Configuration Management**: appsettings.json and environment-specific configs
- **Middleware Pipeline**: Authentication, CORS, and custom middleware
- **Model Binding**: Request/response model binding and validation

### Entity Framework Core
- **Code-First Approach**: Entity modeling and database generation
- **Relationships**: One-to-many and many-to-one relationships
- **Migrations**: Database schema versioning
- **LINQ Queries**: Complex data retrieval and filtering
- **Change Tracking**: Entity state management

### Security & Authentication
- **ASP.NET Core Identity**: User management and role-based security
- **JWT Authentication**: Token generation and validation
- **Authorization Policies**: Role-based and claim-based authorization
- **Password Security**: Hashing and validation

### Software Architecture
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic separation
- **DTO Pattern**: Data transfer object implementation
- **Dependency Inversion**: Interface-based programming
- **Single Responsibility**: Class and method design

### Advanced Features
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Comprehensive input validation
- **Swagger Integration**: API documentation and testing
- **Error Handling**: Global exception handling
- **Logging**: Structured logging with built-in providers

## Testing

### Run Tests
```bash
dotnet test
```

### Test Coverage
- Unit tests for services and business logic
- Integration tests for controllers and API endpoints
- Authentication and authorization tests

## Deployment

### Docker (Optional)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LibraryManagement.csproj", "."]
RUN dotnet restore "./LibraryManagement.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "LibraryManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibraryManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryManagement.dll"]
```

### Cloud Deployment
- **Azure App Service**: Easy deployment with Azure SQL Database
- **AWS Elastic Beanstalk**: Scalable deployment with RDS
- **Google Cloud Run**: Containerized deployment

## Interview Talking Points

### Technical Skills Demonstrated
- "Built a comprehensive library management API using ASP.NET Core 8"
- "Implemented JWT authentication with role-based authorization"
- "Designed complex database relationships using Entity Framework Core"
- "Created RESTful APIs following REST principles and best practices"
- "Used dependency injection and service-oriented architecture"
- "Implemented comprehensive input validation with FluentValidation"
- "Applied repository and service patterns for clean architecture"

### Advanced Features
- "Integrated AutoMapper for object-to-object mapping"
- "Used ASP.NET Core Identity for user management"
- "Implemented pagination, filtering, and sorting for large datasets"
- "Created comprehensive API documentation with Swagger/OpenAPI"
- "Applied SOLID principles and clean code practices"

### Database & Performance
- "Designed normalized database schema with proper relationships"
- "Used Entity Framework Core with code-first migrations"
- "Implemented efficient LINQ queries for data retrieval"
- "Applied indexing strategies for optimal performance"

## License

This project is for educational and portfolio purposes. Feel free to use it as a reference for learning ASP.NET Core development.