# LibraCore Backend 📚

This is a hands-on project for learning .NET API development, aimed at creating the backend for a Library Management System. 🚀

---

## 🛠️ Setup and Running the Application

### 1. Set Environment Variables

Before running the application, make sure to set the environment variables for sensitive credentials.

#### Steps:

1. Ensure you're in a **BASH terminal**.

2. Run the shell script to set the environment variables:
   ```bash
   source Deployment/set_env_vars.sh
   ```

### 2. Run the Application
Once the environment variables are set, you can run the application.

- To run the application with HTTPS:
   ```bash
   dotnet run --launch-profile https
   ```
- To run the application with hot reload:
   ```bash
   dotnet watch
   ```

## ⚙️ Useful Commands

### 💻 .NET

#### Run with a Launch Profile

To run the application with the specified launch profile (e.g., HTTPS):

```bash
dotnet run --launch-profile https
```

#### Run with Hot Reload

To start the application with hot reload (automatically reload changes without restarting the app):

```bash
dotnet watch
```

#### List Installed .NET SDKs

To see all the installed .NET SDK versions on your machine:

```bash
dotnet --list-sdks
```

#### View Installed Packages in the Project

To list all the packages currently being used in the project:

```bash
dotnet list package
```

### 🧑‍💻 Git Commands

#### Generate the Default .gitignore File

To create a .gitignore file with default settings suitable for .NET projects:

```bash
dotnet new gitignore
```

## ✅ Key Concepts Covered
- ☑︎ ORM for MySQL
  - <span style="font-size:13px;">Using the `EntityFrameworkCore` to interact with the `MySQL` database.</span>
- ☑︎ Basic CRUD operations
- ☑︎ Centralized exception handling
  - <span style="font-size:13px;">Using the `Middleware` to streamline error handling across the application.</span>
- ☑︎ Sensitive credentials management
  - <span style="font-size:13px;">Storing credentials securely with the `Environment Variables`.</span>
- ☑︎ Authentication and Authorization
  - <span style="font-size:13px;">Authentication is implemented using `Auth0` as the `OAuth2` client. `Scope-based authorization` is enforced via endpoint-specific `decorators`. For more details, refer to the documentation: <a href="https://auth0.com/docs/quickstart/backend/aspnet-core-webapi/interactive?download=true">Auth0 Quickstart for ASP.NET Core Web API</a>.</span>
- ☑︎ Input Validation
  - <span style="font-size:13px;">Validating request bodies with the `FluentValidation` library.</span>
- ☐ Pagination and Filtering
- ☐ Logging and Monitoring
- ☐ Unit Testing
- ☐ API Documentation
- ☐ Database Migration
- ☐ Versioning

Happy coding! 😊