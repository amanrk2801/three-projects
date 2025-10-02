# E-commerce REST API - Spring Boot Application

A comprehensive e-commerce REST API built with Spring Boot, demonstrating enterprise-level Java development practices.

## Features

### Core Functionality
- **User Authentication & Authorization**: JWT-based security with role-based access control
- **Product Management**: Complete CRUD operations with filtering, pagination, and search
- **Shopping Cart**: Add, update, remove items with stock validation
- **Order Management**: Place orders, track status, order history
- **Admin Panel**: Product management, inventory tracking, order management

### Technical Features
- **RESTful API Design**: Following REST principles and best practices
- **Spring Security**: JWT authentication, method-level security, CORS configuration
- **Data Persistence**: JPA/Hibernate with H2 (development) and MySQL (production)
- **Input Validation**: Bean validation with custom error handling
- **Exception Handling**: Global exception handling with proper HTTP status codes
- **Database Relationships**: One-to-Many, Many-to-One relationships with proper mapping

## Technology Stack

### Backend Framework
- **Spring Boot 3.1.5**: Main framework
- **Spring Security**: Authentication and authorization
- **Spring Data JPA**: Data persistence layer
- **Spring Web**: RESTful web services
- **Spring Validation**: Input validation

### Database
- **H2 Database**: In-memory database for development
- **MySQL**: Production database support
- **Hibernate**: ORM framework

### Security & Authentication
- **JWT (JSON Web Tokens)**: Stateless authentication
- **BCrypt**: Password hashing
- **Role-based Access Control**: ADMIN and CUSTOMER roles

### Build & Dependencies
- **Maven**: Dependency management and build tool
- **Java 17**: Programming language

## Project Structure

```
spring-boot-project/
├── src/main/java/com/example/ecommerce/
│   ├── EcommerceApiApplication.java    # Main application class
│   ├── config/
│   │   └── WebSecurityConfig.java     # Security configuration
│   ├── controller/
│   │   ├── AuthController.java        # Authentication endpoints
│   │   ├── ProductController.java     # Product management
│   │   └── CartController.java        # Shopping cart operations
│   ├── dto/
│   │   ├── AuthRequest.java           # Login request DTO
│   │   ├── AuthResponse.java          # Authentication response DTO
│   │   └── RegisterRequest.java       # Registration request DTO
│   ├── entity/
│   │   ├── User.java                  # User entity
│   │   ├── Product.java               # Product entity
│   │   ├── Order.java                 # Order entity
│   │   ├── OrderItem.java             # Order item entity
│   │   └── CartItem.java              # Cart item entity
│   ├── repository/
│   │   ├── UserRepository.java        # User data access
│   │   ├── ProductRepository.java     # Product data access
│   │   ├── OrderRepository.java       # Order data access
│   │   └── CartItemRepository.java    # Cart data access
│   ├── security/
│   │   ├── JwtUtils.java              # JWT utility class
│   │   ├── JwtAuthTokenFilter.java    # JWT authentication filter
│   │   └── JwtAuthenticationEntryPoint.java # JWT entry point
│   └── service/
│       └── UserDetailsServiceImpl.java # User details service
├── src/main/resources/
│   ├── application.yml                # Application configuration
│   └── data.sql                       # Sample data
└── pom.xml                           # Maven dependencies
```

## API Endpoints

### Authentication
- `POST /api/auth/signup` - Register new user
- `POST /api/auth/signin` - User login
- `GET /api/auth/me` - Get current user info (protected)

### Products
- `GET /api/products` - Get all products (with pagination and filters)
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/categories` - Get all categories
- `POST /api/products` - Create product (ADMIN only)
- `PUT /api/products/{id}` - Update product (ADMIN only)
- `DELETE /api/products/{id}` - Delete product (ADMIN only)
- `GET /api/products/low-stock` - Get low stock products (ADMIN only)

### Shopping Cart
- `GET /api/cart` - Get user's cart (protected)
- `POST /api/cart/add` - Add item to cart (protected)
- `PUT /api/cart/update/{itemId}` - Update cart item (protected)
- `DELETE /api/cart/remove/{itemId}` - Remove item from cart (protected)
- `DELETE /api/cart/clear` - Clear entire cart (protected)

### Query Parameters for Products
- `page`: Page number (default: 0)
- `size`: Page size (default: 10)
- `sortBy`: Sort field (default: id)
- `sortDir`: Sort direction (asc/desc, default: asc)
- `name`: Filter by product name
- `category`: Filter by category
- `minPrice`: Minimum price filter
- `maxPrice`: Maximum price filter

## Installation & Setup

### Prerequisites
- Java 17 or higher
- Maven 3.6+
- MySQL (for production) - optional

### Development Setup

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd spring-boot-project
   ```

2. **Run with Maven**:
   ```bash
   mvn spring-boot:run
   ```

3. **Access the application**:
   - API Base URL: http://localhost:8080
   - H2 Console: http://localhost:8080/h2-console
     - JDBC URL: `jdbc:h2:mem:ecommerce`
     - Username: `sa`
     - Password: `password`

### Production Setup

1. **Configure MySQL**:
   ```yaml
   # application-prod.yml
   spring:
     profiles:
       active: prod
     datasource:
       url: jdbc:mysql://localhost:3306/ecommerce_db
       username: your_username
       password: your_password
   ```

2. **Run with production profile**:
   ```bash
   mvn spring-boot:run -Dspring-boot.run.profiles=prod
   ```

### Build for Production

```bash
mvn clean package
java -jar target/ecommerce-api-0.0.1-SNAPSHOT.jar
```

## Sample Data

The application includes sample data with:
- **Users**: Admin and customer accounts
- **Products**: Electronics, clothing, books, footwear
- **Categories**: Electronics, Clothing, Books, Footwear
- **Sample Orders**: Order history examples

### Default Accounts
- **Admin**: admin@example.com / password123
- **Customer**: john@example.com / password123
- **Customer**: jane@example.com / password123

## API Usage Examples

### Authentication
```bash
# Register
curl -X POST http://localhost:8080/api/auth/signup \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com","password":"password123"}'

# Login
curl -X POST http://localhost:8080/api/auth/signin \
  -H "Content-Type: application/json" \
  -d '{"email":"john@example.com","password":"password123"}'
```

### Products
```bash
# Get all products with pagination
curl "http://localhost:8080/api/products?page=0&size=5&sortBy=name&sortDir=asc"

# Get products by category
curl "http://localhost:8080/api/products?category=Electronics"

# Get product by ID
curl http://localhost:8080/api/products/1
```

### Shopping Cart (requires authentication)
```bash
# Add to cart
curl -X POST http://localhost:8080/api/cart/add \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"productId":1,"quantity":2}'

# Get cart
curl -X GET http://localhost:8080/api/cart \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Key Features Demonstrated

### Spring Boot Skills
- **Auto-configuration**: Leveraging Spring Boot's auto-configuration
- **Profiles**: Development and production configurations
- **Actuator**: Application monitoring (can be added)
- **Testing**: Unit and integration testing setup

### Spring Security
- **JWT Authentication**: Stateless authentication mechanism
- **Method Security**: `@PreAuthorize` annotations
- **CORS Configuration**: Cross-origin resource sharing
- **Password Encoding**: BCrypt password hashing

### Data Management
- **JPA Relationships**: Complex entity relationships
- **Custom Queries**: JPQL and native queries
- **Pagination**: Spring Data pagination support
- **Validation**: Bean validation with custom messages

### REST API Design
- **HTTP Methods**: Proper use of GET, POST, PUT, DELETE
- **Status Codes**: Appropriate HTTP status codes
- **Error Handling**: Consistent error response format
- **Documentation**: Clear API documentation

## Testing

### Run Tests
```bash
mvn test
```

### Test Coverage
- Unit tests for services and repositories
- Integration tests for controllers
- Security tests for authentication

## Deployment

### Docker (Optional)
```dockerfile
FROM openjdk:17-jdk-slim
COPY target/ecommerce-api-0.0.1-SNAPSHOT.jar app.jar
EXPOSE 8080
ENTRYPOINT ["java","-jar","/app.jar"]
```

### Cloud Deployment
- **Heroku**: Easy deployment with Heroku Postgres
- **AWS**: EC2 with RDS MySQL
- **Google Cloud**: App Engine with Cloud SQL

## Interview Talking Points

### Technical Skills Demonstrated
- "Built a complete e-commerce REST API using Spring Boot"
- "Implemented JWT-based authentication with role-based access control"
- "Designed complex database relationships with JPA/Hibernate"
- "Created RESTful endpoints with proper HTTP methods and status codes"
- "Used Spring Security for authentication and authorization"
- "Implemented pagination, filtering, and sorting for large datasets"
- "Applied validation and error handling best practices"

### Architecture & Design
- "Followed layered architecture with separation of concerns"
- "Used DTOs for data transfer and entity mapping"
- "Implemented repository pattern for data access"
- "Applied dependency injection and inversion of control"

### Database & Performance
- "Designed normalized database schema with proper relationships"
- "Used custom JPQL queries for complex data retrieval"
- "Implemented soft delete and audit fields"
- "Optimized queries with proper indexing strategies"

## License

This project is for educational and portfolio purposes. Feel free to use it as a reference for learning Spring Boot development.