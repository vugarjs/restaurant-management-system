# 🍽️ Restaurant Management System

A **.NET 10 console-based Restaurant Management System** built with a clean **4-layer N-Tier Architecture**.

The project is designed as a practical application for managing restaurant menu items and customer orders while demonstrating important .NET concepts such as **Entity Framework Core, Repository Pattern, Dependency Injection, DTOs, AutoMapper, and asynchronous programming**.

---

## 📌 Overview

The application provides an interactive, menu-driven console interface for managing:

* 🍔 Menu Items
* 🧾 Orders
* 📦 Order Items
* 🔎 Advanced searching and filtering
* 💰 Automatic order total calculation
* 🕒 Automatic order timestamps

The main goal of the project is to demonstrate how a maintainable .NET application can be structured using **separation of concerns** and layered architecture.

---

## 🏗️ Architecture

The solution follows a **4-layer N-Tier Architecture**:

```text
┌──────────────────────────────┐
│     Presentation Layer       │
│       Console Application    │
└──────────────┬───────────────┘
               │
┌──────────────▼───────────────┐
│       Business Layer         │
│   Services • DTOs • Mapping  │
└──────────────┬───────────────┘
               │
┌──────────────▼───────────────┐
│      DataAccess Layer        │
│ EF Core • Repositories       │
└──────────────┬───────────────┘
               │
┌──────────────▼───────────────┐
│        Entity Layer          │
│   Domain Models / Entities   │
└──────────────────────────────┘
```

### 1. Presentation Layer

Responsible for interaction with the user.

**Responsibilities:**

* Console menu system
* User input/output
* Application entry point
* Dependency Injection configuration
* Calling business services

**Project:**

```text
RestorantApp.Presentation
```

---

### 2. Business Layer

Contains the application's business logic and application-level operations.

**Responsibilities:**

* Business rules
* Validation
* Service implementations
* DTOs
* AutoMapper profiles
* Coordination between repositories and presentation

**Project:**

```text
RestorantApp.Businnes
```

---

### 3. DataAccess Layer

Responsible for communication with the database.

Built using **Entity Framework Core** and the **Repository Pattern**.

**Contains:**

* `DbContext`
* Repository implementations
* Database configurations
* Entity Framework Core operations
* Automatic order timestamp handling

**Project:**

```text
RestorantApp.DataAccess
```

---

### 4. Entity Layer

Contains the core domain models used throughout the application.

**Main entities:**

* `MenuItem`
* `Order`
* `OrderItem`

**Project:**

```text
RestorantApp.Entity
```

---

# 🚀 Features

## 🍔 Menu Item Management

Complete CRUD functionality for restaurant menu items.

### Supported operations

* Create menu item
* Update menu item
* Delete menu item
* Get menu item by ID
* Get all menu items

### Search & Filtering

Menu items can be searched or filtered by:

* ID
* Name
* Category
* Price range

---

## 🧾 Order Management

Orders can be created, updated, deleted and queried through the application.

### Supported operations

* Create order
* Update order
* Delete order
* Get order by ID
* Get all orders

### Order Filtering

Orders can be queried by:

* Specific date
* Date range
* Price range

---

## 💰 Automatic Order Total Calculation

Order totals are calculated automatically based on the selected menu items and their quantities.

The calculation follows the basic rule:

```text
Item Price × Quantity
        ↓
Order Item Total
        ↓
Sum of all Order Items
        ↓
Final Order Total
```

This keeps pricing logic centralized inside the appropriate application layer.

---

## 🕒 Automatic Order Timestamp

New orders automatically receive their creation timestamp.

This logic is handled inside:

```text
RestorantContext.SaveChangesAsync()
```

The application uses:

```csharp
DateTime.Now
```

to assign the timestamp to newly created orders.

---

# 🔄 Asynchronous Programming

Database operations are implemented using **async/await**.

Repositories and services use asynchronous operations such as:

```text
ToListAsync()
FirstOrDefaultAsync()
FindAsync()
SaveChangesAsync()
```

This keeps database access modern and avoids unnecessary blocking operations.

---

# 💉 Dependency Injection

The application uses:

**Microsoft.Extensions.DependencyInjection**

Services and repositories are registered during application startup.

This allows dependencies to be injected instead of creating them manually.

Example architecture:

```text
Presentation
      ↓
   Service
      ↓
 Repository
      ↓
  DbContext
      ↓
 SQL Server
```

---

# 🔄 AutoMapper

**AutoMapper** is used for converting between entities and DTOs.

The application separates database entities from objects used by the presentation/business layers.

Mappings include:

```text
Entity
   ↕
Create DTO
   ↕
Update DTO
   ↕
Return DTO
```

This helps keep the application structure clean and reduces manual mapping code.

---

# 🗄️ Database

The application uses:

* **SQL Server**
* **Entity Framework Core 10**
* **Code First approach**

### Default Database

```text
RestorantDB
```

The connection string is configured inside:

```text
RestorantApp.DataAccess
└── Context
    └── RestorantContext.cs
```

Update the connection string if your SQL Server instance uses a different server or instance name.

---

# 🛠️ Tech Stack

| Technology               | Usage                   |
| ------------------------ | ----------------------- |
| C#                       | Programming language    |
| .NET 10                  | Application framework   |
| Entity Framework Core 10 | ORM / database access   |
| SQL Server               | Database                |
| AutoMapper               | Object mapping          |
| Dependency Injection     | Dependency management   |
| async/await              | Asynchronous operations |
| Repository Pattern       | Data access abstraction |

---

# 📁 Solution Structure

```text
RestorantApp
│
├── RestorantApp.Presentation
│   ├── Program.cs
│   └── ...
│
├── RestorantApp.Businnes
│   ├── DTOs
│   ├── Services
│   ├── Profiles
│   └── ...
│
├── RestorantApp.DataAccess
│   ├── Context
│   ├── Repositories
│   ├── Configurations
│   └── ...
│
├── RestorantApp.Entity
│   ├── Entities
│   ├── Enums
│   └── ...
│
└── RestorantApp.slnx
```

---

# ⚙️ Getting Started

## Prerequisites

Make sure you have the following installed:

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* SQL Server Express or another compatible SQL Server instance
* Visual Studio 2026 or another compatible .NET IDE
* EF Core CLI tools *(optional, for migrations)*

---

## 📥 Clone the Repository

```bash
git clone https://github.com/your-username/your-repository.git

cd your-repository
```

Replace the repository URL with the actual GitHub repository URL.

---

## 🗄️ Configure the Database

Open:

```text
RestorantApp.DataAccess/Context/RestorantContext.cs
```

and update the connection string according to your local SQL Server configuration.

Example:

```text
Server=YOUR_SERVER;
Database=RestorantDB;
Trusted_Connection=True;
TrustServerCertificate=True;
```

---

## 🧱 Apply Migrations

If migrations are already included in the repository, update the database using:

```bash
dotnet ef database update
```

If EF Core tools are not installed:

```bash
dotnet tool install --global dotnet-ef
```

---

## ▶️ Run the Application

### Using Visual Studio

1. Open `RestorantApp.slnx`
2. Set `RestorantApp.Presentation` as the startup project
3. Press **F5** or **Ctrl + F5**

### Using CLI

From the solution root:

```bash
dotnet run --project RestorantApp.Presentation
```

---

# 🧪 Example Application Flow

When the application starts, the user can interact with the console menu:

```text
--- RESTAURANT MANAGEMENT SYSTEM ---

1. Menu Item Management
2. Order Management
0. Exit

Select an option:
```

Menu item operations include:

```text
--- MENU ITEM MANAGEMENT ---

1. Add Menu Item
2. Update Menu Item
3. Delete Menu Item
4. Get All Menu Items
5. Search by Category
6. Search by Price Range
7. Search by Name
0. Back
```

---

# 🎯 Project Goals

This project was built to practice and demonstrate:

* N-Tier Architecture
* Separation of Concerns
* SOLID principles
* Dependency Injection
* Repository Pattern
* Entity Framework Core
* DTO pattern
* AutoMapper
* LINQ
* Async/Await
* Database relationships
* CRUD operations
* Business logic organization
* Clean and maintainable project structure

---

# 🔮 Future Improvements

Possible future extensions include:

* 🔐 Authentication & Authorization
* 👤 User management
* 📊 Sales and revenue reporting
* 📈 Dashboard
* 🧾 Receipt generation
* 🔍 More advanced filtering
* 📦 Inventory management
* 💳 Payment integration
* 🌐 ASP.NET Core Web API
* 🖥️ Web-based frontend
* 📝 Logging and centralized exception handling

---

# 📚 Purpose

This project was created primarily for **learning and practical implementation of .NET architecture patterns**.

Rather than putting all application logic into a single console project, responsibilities are separated into dedicated layers. This makes the application easier to understand, maintain, test, and extend.

---

## 👨‍💻 Author

**Vuqar**

Built with **C# & .NET** ❤️

---

⭐ If you find this project useful, feel free to star the repository!
