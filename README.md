# Restaurant Management System

A **.NET 10 console-based Restaurant Management System** built with a clean **N-Tier Architecture**.  
The application provides an interactive menu-driven experience for managing restaurant menu items and customer orders, with a strong focus on separation of concerns, maintainability, and asynchronous data access.

---

## Architecture & Layer Structure

This solution follows a **4-layer N-Tier architecture**:

### 1. Presentation Layer
- Console application with an interactive menu system
- Handles all user input/output operations
- Uses **Dependency Injection** to resolve services
- Acts as the entry point of the application

### 2. Business Layer
- Contains the application's business logic
- Includes:
  - Services
  - DTOs
  - AutoMapper profiles
- Responsible for validation, orchestration, and data transformation

### 3. DataAccess Layer
- Handles persistence and database access
- Built with **Entity Framework Core**
- Contains:
  - `DbContext`
  - Repository pattern implementations
- Includes an overridden `SaveChangesAsync()` method for automatic timestamping of new orders

### 4. Entity Layer
- Contains the core domain models
- Includes entities such as:
  - `Order`
  - `OrderItem`
  - `MenuItem`

---

## Tech Stack & Libraries

- **.NET 10**
- **C#**
- **Console Application**
- **Entity Framework Core 10**
- **SQL Server**
- **AutoMapper**
- **Microsoft.Extensions.DependencyInjection**
- **Asynchronous programming** with `async/await`

### Main Projects
- `RestorantApp.Presentation`
- `RestorantApp.Businnes`
- `RestorantApp.DataAccess`
- `RestorantApp.Entity`

---

## Key Features & Implementation Details

### MenuItem Management
- Full CRUD support for menu items
- Search menu items by:
  - ID
  - Name
  - Category
  - Price range

### Order Management
- Full CRUD support for orders
- Query orders by:
  - Date
  - Date range
  - Price range

### Dependency Injection
- Implemented using `Microsoft.Extensions.DependencyInjection`
- Services and repositories are registered in the console startup pipeline

### AutoMapper Integration
- Used to map between:
  - Entities
  - Create DTOs
  - Update DTOs
  - Return DTOs

### Automatic Timestamping
- New `Order` records receive a timestamp automatically
- Handled in `RestorantContext.SaveChangesAsync()`
- Uses `DateTime.Now`

### Automatic Total Calculation
- Order total price is calculated automatically
- Based on:
  - Selected menu items
  - Item quantities
- Ensures consistent and centralized order pricing logic

### Async/Await Everywhere
- Repository and service operations are implemented asynchronously
- Improves responsiveness and keeps data access patterns modern and scalable

---

## How to Run / Setup Instructions

### Prerequisites
- **.NET 10 SDK**
- **SQL Server Express** or another compatible SQL Server instance
- Visual Studio 2026 or a compatible .NET IDE

### Database Configuration
The database connection string is configured in:

- `RestorantApp.DataAccess/Context/RestorantContext.cs`

Default database:
- `RestorantDB`

If needed, update the connection string to match your local SQL Server instance.

### Run from the Command Line
From the solution root directory:

### Run in Visual Studio
1. Open `RestorantApp.slnx`
2. Set `RestorantApp.Presentation` as the startup project
3. Press **F5** or **Ctrl+F5**

### Optional: EF Core Tools
If you are working with migrations, you can manage them using `dotnet ef` commands after installing the EF Core tooling.

---

## Solution Overview

---

## Notes

- The application is designed for learning and practical use of layered architecture in .NET.
- Business rules such as order total calculation and automatic order timestamps are centralized in the appropriate layers.
- The structure makes the project easy to extend with additional features such as authentication, reporting, and more advanced filtering.

---