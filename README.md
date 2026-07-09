# .NET Post API Clean Architecture

A simple .NET Web API project built using Clean Architecture principles.

This project includes JWT authentication, PostgreSQL, Entity Framework Core, Repository Pattern, Unit of Work Pattern, and Swagger support.

## Project Overview

This application allows users to:

- Register a new account
- Login using email and password
- Receive a JWT token after successful login
- Create posts using the JWT token
- View all posts
- View a post by id
- Update their own posts
- Delete their own posts

The main architecture flow is:

```text
Controller → Service → Repository → UnitOfWork → DbContext → PostgreSQL
```

## Technologies Used

- .NET 10 Web API
- ASP.NET Core Controllers
- Entity Framework Core
- PostgreSQL
- JWT Bearer Authentication
- BCrypt password hashing
- Swagger
- Repository Pattern
- Unit of Work Pattern
- Clean Architecture

## Project Structure

```text
PostApi
│
├── PostApi
│   ├── Controllers
│   │   ├── AuthController.cs
│   │   └── PostsController.cs
│   │
│   ├── Extensions
│   │   ├── AuthenticationExtensions.cs
│   │   └── OpenApiExtensions.cs
│   │
│   ├── Program.cs
│   ├── appsettings.json
│   └── PostApi.http
│
├── PostApi.Application
│   ├── DTOs
│   │   ├── RegisterRequest.cs
│   │   ├── LoginRequest.cs
│   │   ├── LoginResponse.cs
│   │   ├── CreatePostRequest.cs
│   │   ├── UpdatePostRequest.cs
│   │   └── PostResponse.cs
│   │
│   ├── Interfaces
│   │   ├── IGenericRepository.cs
│   │   ├── IUserRepository.cs
│   │   ├── IPostRepository.cs
│   │   ├── IUnitOfWork.cs
│   │   ├── IJwtTokenService.cs
│   │   ├── IAuthService.cs
│   │   └── IPostService.cs
│   │
│   └── Services
│       ├── JwtTokenService.cs
│       ├── AuthService.cs
│       └── PostService.cs
│
├── PostApi.Domain
│   └── Entities
│       ├── User.cs
│       └── Post.cs
│
└── PostApi.Infrastructure
    ├── Data
    │   └── AppDbContext.cs
    │
    ├── Repositories
    │   ├── GenericRepository.cs
    │   ├── UserRepository.cs
    │   └── PostRepository.cs
    │
    └── UnitOfWork
        └── UnitOfWork.cs
```

## Architecture Explanation

### API Layer

The API layer receives HTTP requests and returns HTTP responses.

This layer contains:

- Controllers
- Program.cs
- appsettings.json
- Extension classes

Example controllers:

- AuthController
- PostsController

Controllers should not contain business logic. They call services from the Application layer.

Example flow:

```text
HTTP Request → Controller → Service
```

### Application Layer

The Application layer contains the business logic of the system.

This layer contains:

- DTOs
- Interfaces
- Services

Examples:

- AuthService
- PostService
- IAuthService
- IPostService
- IUnitOfWork

This layer does not directly depend on the API layer.

### Domain Layer

The Domain layer contains the main business entities.

This layer contains:

- User
- Post

The Domain layer is the core layer of the application.

It should not depend on Application, Infrastructure, or API projects.

### Infrastructure Layer

The Infrastructure layer contains database-related implementation.

This layer contains:

- AppDbContext
- GenericRepository
- UserRepository
- PostRepository
- UnitOfWork

This layer communicates with PostgreSQL using Entity Framework Core.

## Main Flow

```text
Controller
    ↓
Service
    ↓
UnitOfWork
    ↓
Repository
    ↓
AppDbContext
    ↓
PostgreSQL
```

## Database Design

### Users Table

```text
Id
UserName
Email
PasswordHash
CreatedAt
```

### Posts Table

```text
Id
Title
Content
UserId
CreatedAt
UpdatedAt
```

### Relationship

```text
One User can have many Posts.
One Post belongs to one User.
```

Simple relationship:

```text
User
 └── Many Posts

Post
 └── One User
```

## API Endpoints

### Authentication Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login user and return JWT token |

### Post Endpoints

| Method | Endpoint | Description | Auth Required |
|---|---|---|---|
| GET | `/api/posts` | Get all posts | Yes |
| GET | `/api/posts/{id}` | Get post by id | Yes |
| POST | `/api/posts` | Create new post | Yes |
| PUT | `/api/posts/{id}` | Update own post | Yes |
| DELETE | `/api/posts/{id}` | Delete own post | Yes |

## Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/dotnet-postapi-clean-architecture.git
```

```bash
cd dotnet-postapi-clean-architecture
```

### 2. Create PostgreSQL database

Create a PostgreSQL database:

```sql
CREATE DATABASE post_api_db;
```

You only need to create the database manually.

Entity Framework Core migrations will create the tables.

Expected tables after migration:

```text
Users
Posts
__EFMigrationsHistory
```

### 3. Update connection string

Open:

```text
PostApi/appsettings.json
```

Update your PostgreSQL password:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=post_api_db;Username=postgres;Password=your_password"
  }
}
```

### 4. JWT settings

Example JWT configuration:

```json
{
  "Jwt": {
    "Issuer": "PostApi",
    "Audience": "PostApiClient",
    "SecretKey": "your-secret-key",
    "ExpiryMinutes": 60
  }
}
```

For local development, this can be inside `appsettings.json`.

For production, store secrets in environment variables or a secret manager.

### 5. Restore packages

Open the solution in Visual Studio and restore NuGet packages.

Or run:

```bash
dotnet restore
```

### 6. Apply database migration

Open Package Manager Console in Visual Studio:

```text
Tools → NuGet Package Manager → Package Manager Console
```

Run:

```powershell
Update-Database -Project PostApi.Infrastructure -StartupProject PostApi
```

### 7. Run the project

In Visual Studio, select the `https` profile and run the project.

The API will run on a URL similar to:

```text
https://localhost:7057
```

```

The root URL may show 404:

```text
https://localhost:7057/
```

That is normal because this project does not have a default home endpoint.

Use the API endpoints directly.

## Testing with Postman

### Register User

Method:

```text
POST
```

URL:

```text
https://localhost:7057/api/auth/register
```

Headers:

```text
Content-Type: application/json
```

Body:

```json
{
  "userName": "Mithun",
  "email": "mithun@gmail.com",
  "password": "123456"
}
```

Expected response:

```json
{
  "message": "User registered successfully."
}
```

### Login User

Method:

```text
POST
```

URL:

```text
https://localhost:7057/api/auth/login
```

Headers:

```text
Content-Type: application/json
```

Body:

```json
{
  "email": "mithun@gmail.com",
  "password": "123456"
}
```

Expected response:

```json
{
  "token": "jwt-token-here",
  "userName": "Mithun",
  "email": "mithun@gmail.com"
}
```

Copy the token from the response.

### Create Post

Method:

```text
POST
```

URL:

```text
https://localhost:7057/api/posts
```

Authorization:

```text
Bearer Token
```

Token:

```text
your_token_here
```

Headers:

```text
Content-Type: application/json
```

Body:

```json
{
  "title": "My First Post",
  "content": "This is my first post content"
}
```

### Get All Posts

Method:

```text
GET
```

URL:

```text
https://localhost:7057/api/posts
```

Authorization:

```text
Bearer Token
```

Token:

```text
your_token_here
```

### Get Post By Id

Method:

```text
GET
```

URL:

```text
https://localhost:7057/api/posts/{id}
```

Authorization:

```text
Bearer Token
```

Token:

```text
your_token_here
```

### Update Post

Method:

```text
PUT
```

URL:

```text
https://localhost:7057/api/posts/{id}
```

Authorization:

```text
Bearer Token
```

Token:

```text
your_token_here
```

Headers:

```text
Content-Type: application/json
```

Body:

```json
{
  "title": "Updated Post Title",
  "content": "This is updated post content"
}
```

### Delete Post

Method:

```text
DELETE
```

URL:

```text
https://localhost:7057/api/posts/{id}
```

Authorization:

```text
Bearer Token
```

Token:

```text
your_token_here
```

## Saving JWT Token in Postman Variable

After calling the login API, go to the Login request's Scripts tab and add this in the Post-response script:

```javascript
const response = pm.response.json();

pm.environment.set("token", response.token);
```

Then in other protected APIs, use:

```text
{{token}}
```

as the Bearer Token.

## JWT Authentication Flow

```text
User logs in with email and password
        ↓
API verifies user credentials
        ↓
API generates JWT token
        ↓
Client sends token with protected requests
        ↓
API validates token
        ↓
API allows access to protected endpoints
```

The token contains:

```text
userId
userName
email
```

The backend reads the `userId` from the token when creating, updating, or deleting posts.

## Security Notes

- Passwords are hashed using BCrypt.
- Plain passwords are not stored in the database.
- JWT token is required for post APIs.
- Users can only update or delete their own posts.
- UserId is read from the JWT token, not from the request body.
- Do not commit real database passwords.
- Do not commit production JWT secret keys.

## Patterns Used

### Repository Pattern

Repositories handle database query logic.

Examples:

```text
GenericRepository
UserRepository
PostRepository
```

Repositories use `AppDbContext` to communicate with the database.

### Unit of Work Pattern

Unit of Work groups repositories and saves database changes in one place.

Example:

```csharp
await _unitOfWork.Posts.AddAsync(post);
await _unitOfWork.SaveChangesAsync();
```

### DTO Pattern

DTOs are used to send and receive API data.

Examples:

```text
RegisterRequest
LoginRequest
LoginResponse
CreatePostRequest
UpdatePostRequest
PostResponse
```

DTOs prevent exposing database entities directly.

### Clean Architecture

This project separates responsibilities into different layers.

```text
API → Application → Domain
API → Infrastructure → Application → Domain
```

Inner layers do not depend on outer layers.

## SOLID Principles Used

### Single Responsibility Principle

Each file has one main responsibility.

Examples:

```text
User.cs                  → represents user entity
Post.cs                  → represents post entity
AuthService.cs           → handles authentication logic
PostService.cs           → handles post business logic
AppDbContext.cs          → manages database mapping
AuthController.cs        → handles auth HTTP requests
PostsController.cs       → handles post HTTP requests
```

### Dependency Inversion Principle

Controllers and services depend on interfaces instead of concrete classes.

Examples:

```text
AuthController → IAuthService
PostsController → IPostService
AuthService → IUnitOfWork
PostService → IUnitOfWork
```

## Example Request Flow

### Register Flow

```text
POST /api/auth/register
        ↓
AuthController
        ↓
AuthService
        ↓
IUnitOfWork
        ↓
UserRepository
        ↓
AppDbContext
        ↓
PostgreSQL
```

### Login Flow

```text
POST /api/auth/login
        ↓
AuthController
        ↓
AuthService
        ↓
UserRepository
        ↓
BCrypt verifies password
        ↓
JwtTokenService creates token
        ↓
LoginResponse returned
```

### Create Post Flow

```text
POST /api/posts
        ↓
JWT token validated
        ↓
PostsController reads userId from token
        ↓
PostService
        ↓
PostRepository
        ↓
UnitOfWork saves changes
        ↓
PostgreSQL
```

## Useful Git Commands

```bash
git add .
git commit -m "Initial commit - .NET clean architecture post API"
git push
```

## Recommended `.gitignore`

```gitignore
.vs/
bin/
obj/
*.user
*.suo
*.log
.env
.vscode/
.idea/
.DS_Store
Thumbs.db
```

## Author

Created by Mithun Salinda.
