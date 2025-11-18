# Inventory Management System

A full-stack web application for managing product inventory featuring specific category analytics.

## 1. Quick Start

### Prerequisites
* .NET 8 SDK
* Node.js & npm

### Setup & Run

**Backend:**
1.  Navigate to `InventoryApi`.
2.  Run `dotnet run`.
3.  API will launch at `http://localhost:5000` (or similar).
4.  Swagger UI available at `/swagger/index.html`.
5.  *Note:* Database is automatically created and seeded on first run in `inventory.db`.

**Frontend:**
1.  Navigate to `inventory-app`.
2.  Run `npm install`.
3.  Run `ng serve -o`.
4.  Frontend will launch at `http://localhost:4200`.

## 2. Architecture

* **Backend:** C# .NET Web API following **Clean Architecture principles**.
* **Database:** SQLite using **Entity Framework Core**.
* **Frontend:** Angular (Single Page Application).

## 3. Design Decisions

### Clean Architecture (Services vs Repositories)
I utilized a **Service Layer pattern** injecting the `DbContext` directly.
* **Decision:** While the Repository pattern is popular, EF Core's `DbContext` is already a Unit of Work/Repository abstraction. For a project of this scope, adding a generic repository layer often adds unnecessary abstraction.
* **Benefit:** Keeps code concise while still separating Controllers (Presentation) from Business Logic (Services).
* **DI:** Services are injected into Controllers via Constructor Injection.

### EF Core & Optimization
* **Reads:** Used `.AsNoTracking()` for GET requests to improve performance by bypassing the change tracker.
* **N+1 Prevention:** Used `.Include(p => p.Category)` to eager load related data in a single query.
* **Analytics:** The Category Summary is generated using a single SQL aggregation query via Linq `.Select()` projection, rather than fetching all products into memory and calculating in C#.

### Database Indexing
Added indexes in `OnModelCreating`:
1.  `Product.CategoryId`: Essential for Foreign Key lookups and joining tables.
2.  `Product.IsActive`: Since the API heavily relies on soft deletes, almost every query filters by `IsActive`. This index speeds up those filters.

### Assumptions & Trade-offs
* **Soft Delete:** Deleted items remain in the DB but are flagged `IsActive = false`. Unique constraints (like unique names) might conflict with deleted items if not handled carefully (handled here by ignoring uniqueness for simplicity).
* **Security:** No Authentication/Authorization implemented as per requirements.

## If I had more time

- run on hTTPS