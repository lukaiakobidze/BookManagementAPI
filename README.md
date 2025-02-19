# Book Management API

This is a RESTful API for managing books, built using **ASP.NET Core** and **Entity Framework Core**. It supports authentication via **JWT tokens** and provides endpoints for managing books and users.

## Features
- **User Authentication** (JWT-based login system)
- **CRUD Operations** for Books
- **Bulk Add/Delete Books**
- **Calculate Book Popularity on the Fly**
- **Pagination for Popular Books**
- **Swagger API Documentation**

## Technologies Used
- **ASP.NET Core 6**
- **Entity Framework Core** (EF Core)
- **Microsoft SQL Server**
- **JWT Authentication**
- **Swagger UI**

## Getting Started

### Prerequisites
- .NET SDK (6.0 or later)
- Microsoft SQL Server
- Postman (optional, for testing API endpoints)

### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/your-username/book-management-api.git
   cd book-management-api
   ```
2. Update the **appsettings.json** file with your database connection string:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=BookDB;User Id=your_user;Password=your_password;"
   },
   "Jwt": {
       "Key": "your_secret_key",
       "Issuer": "yourIssuer",
       "Audience": "yourAudience"
   }
   ```
3. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
4. Run the application:
   ```sh
   dotnet run
   ```

## API Endpoints

### Authentication
| Method | Endpoint       | Description          |
|--------|--------------|----------------------|
| POST   | `/api/auth/login` | User login and JWT token generation |

### Books
| Method | Endpoint       | Description          |
|--------|--------------|----------------------|
| GET    | `/api/books` | Get all books |
| GET    | `/api/books/{id}` | Get book by ID (includes popularity calculation) |
| POST   | `/api/books` | Add a new book |
| PUT    | `/api/books/{id}` | Update book details |
| DELETE | `/api/books/{id}` | Delete a book |
| DELETE | `/api/books/bulk` | Bulk delete books |
| POST   | `/api/books/bulk` | Bulk add books |
| GET    | `/api/books/popular?pageNumber=1&pageSize=10` | Get popular books with pagination |

## Security & Authentication
- Uses **JWT authentication** for secure access.
- All book-related endpoints require a **valid JWT token**.
- Add the token in the **Authorization** header as: `Bearer YOUR_TOKEN_HERE`.

## Swagger Documentation
Once the API is running, you can access the **Swagger UI** for testing endpoints:
```
https://localhost:5001/swagger/index.html
```





