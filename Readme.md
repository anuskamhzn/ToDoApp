# ToDo App - Personal Task Manager with Authentication

A simple full-stack **ToDo** application where each user can manage their own private tasks.

- **Backend**: ASP.NET Core MVC + Entity Framework Core + SQL Server/LocalDB
- **Frontend**: Razor Views + Bootstrap + basic JavaScript
- Authentication: ASP.NET Core Identity (register/login/logout)

## Features

- User registration and login/logout
- Create, read, update, delete (CRUD) personal tasks
- Mark tasks as completed
- Filter tasks: All / Pending / Completed
- Only see your own tasks (user isolation via UserId)
- Responsive UI with Bootstrap
- Protected routes (tasks only visible after login)

## Tech Stack

| Layer       | Technology                          |
|-------------|-------------------------------------|
| Framework   | ASP.NET Core MVC (.NET 8 / .NET 7)  |
| ORM         | Entity Framework Core               |
| Database    | SQL Server / LocalDB                |
| Auth        | ASP.NET Core Identity               |
| Frontend    | Razor Views, Bootstrap 5, CSS/JS    |
| UI Library  | Bootstrap (navbar, buttons, tables) |

## Project Structure
```text
ToDoApp/
├── Areas/
│   └── Identity/
│       └── Pages/               ← Scaffolded Identity pages (Login, Register, etc.)
├── Controllers/
│   └── TasksController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   └── TaskItem.cs
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml
│   ├── Tasks/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   └── Delete.cshtml
│   └── Home/
│       └── Index.cshtml
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
├── appsettings.json
├── Program.cs
└── ...
```

## API Endpoints

| Method | Route                     | Description                     | Authentication |
|--------|---------------------------|---------------------------------|----------------|
| GET    | `/`                       | Home page                       | Public         |
| GET    | `/Tasks`                  | List all your tasks             | Required       |
| GET    | `/Tasks/Create`           | Show create task form           | Required       |
| POST   | `/Tasks/Create`           | Create new task                 | Required       |
| GET    | `/Tasks/Edit/{id}`        | Show edit form                  | Required       |
| POST   | `/Tasks/Edit/{id}`        | Update task                     | Required       |
| GET    | `/Tasks/Details/{id}`     | View task details               | Required       |
| GET    | `/Tasks/Delete/{id}`      | Show delete confirmation        | Required       |
| POST   | `/Tasks/Delete/{id}`      | Delete task                     | Required       |
| POST   | `/Tasks/ToggleComplete/{id}` | Toggle completed status      | Required       |
| GET    | `/Identity/Account/Login` | Login page                      | Public         |
| GET    | `/Identity/Account/Register` | Register page                | Public         |

## How to Run the Project

### Prerequisites

- .NET 7 SDK
- SQL Server (local or remote)

### Steps

1. **Clone the repository**

```bash
git clone https://github.com/anuskamhzn/ToDoApp.git
cd ToDoApp
```

2. **Update connection string (if needed)**

Open appsettings.json or appsettings.Development.json and adjust:
```bash
JSON"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=StudentDb;Trusted_Connection=True;TrustServerCertificate=True;" 
  }
```

3. **Create & migrate database**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. **Run the application**

5. **Open browser at:**

```bash
https://localhost:7184/             → Home page
https://localhost:7184/Tasks        → Your tasks (after login)
https://localhost:7184/Identity/Account/Register   → Create account
https://localhost:7184/Identity/Account/Login      → Login
```
Note: Port 7184 is just an example — your port may be different (check console output).
