# Inventory Management System

A full-stack web application for managing product inventory featuring specific category analytics.

## 1. Quick Start

### Prerequisites

- .NET v10 SDK
- Node.js v24.11.1
- npm v11.6.2
- Angular v20

### Setup & Run

**Backend:**

1. Navigate to `InventoryApi`.
1. Run `dotnet run`.
1. API will launch at `http://localhost:5197`.
1. OpenAPI document available at `/openapi/v1.json`.
1. *Note:* Database is automatically created and seeded on first run in `inventory.db`.

**Frontend:**

1. Navigate to `inventory-app`.
1. Run `npm install`.
1. Run `ng serve -o`.
1. Frontend will launch at `http://localhost:4200`.

## 2. Architecture

- Standalone back end web API
  - Requests are handled by the Controller
  - Controller determines which Service to call and passes along input DTOs if appropriate
  - Service layer interacts directly with DbContext using LINQ queries
  - Service layer replies with new DTOs reflecting the most current state of the database
  - Database is built using Entity Framework

- Angular web client communicates with the backend using REST endpoints.

### Database Schema

1. Product table
    - integer Id
    - text Name
    - text Description
    - text Price
    - integer StockQuantity
    - text CreatedDate
    - integer IsActive
    - integer Category Id (Foreign Key)

1. Category table
    - integer Id
    - text Name
    - text Description
    - integer IsActive

### Technologies

- **Backend:** C# .NET Web API
- **Database:** SQLite managed with Entity Framework
- **Frontend:** Angular (Single Page Application)

## 3. Design Decisions

### Single Responsibility

Product and Category actions are separated into their own services.

### Dependency Inversion

Controllers interact with the Service Layer via Interfaces so that the individual Services can change without impacting the Controllers directly.

### EF Core and query optimization

I defined a code-first database and used LINQ to query the database. I did not do any additional query optimization outside of Linq and using .AsNoTracking() when we are getting data and not making any changes to the database.

### Complex endpoint: Category Analytics

I chose to work on Category Analytics because the capability for Complex Search can be handled by other products and does not need to be a core feature for this webAPI. Also, performing math on database values is generally easier to implement than a robust search solution.

### Not using a Repository

I decided not to create an explicit Repository pattern because we did not need any capabilities that DbContext couldn't handle and hoped that it would take less time to use DbContext directly instead of creating a Repository layer (this is also the method I am more familiar with). This has the trade-off of making it harder to change our database technology in the future as our Service layer would also have to be changed.

### Index strategy

I kept the suggested Product.CategoryId index since the Category is always returned and the Product.IsActive index as that is checked for our data retrievals, but I did not explore if there were any further query optimmizations in this case.

## 4. What I Would Do With More Time

- The Create/Update methods need further refinement to handle malformed DTOs.
- More specificity around which fields are actually required would let us handle intermediate cases when a new Product or Category might be in the process of being created but we don't have all of the information yet.
- If this ends up being hosted on Azure, Search can be integrated by adding a service to talk to Azure Search. We would then also have to expand our services to update the Azure Search Index whenever we're updating the database, depending on what data we want to make available.
- Logging for both errors and user experience, tracking the user, what they were doing, how long the request took, and what errors may have appeared.

## 5. Assumptions & Trade-offs

- Products would only have 1 Category. Any Products which would fit in multiple Categories would require refactoring our Entity model.
- Products are only soft deleted, which means our database may grow indefinitely.
- No network latency between the frontend client and backend api. I would have to add support for retries or handshakes when transmitting data if the network turns out to be inconsistent.
- No security for our endpoints means anybody can change the database. This works for a local proof-of-concept, but a full authentication scheme with OAuth and defined roles for each of our Controllers will be necessary for Production.
- Similarly, Production would require running everything over an encrypted HTTPS connection.

---

# Presentation

1. Code structure and key components

1. Design decisions and trade-offs
    - Controllers using interfaces to interact with the Service layer allows us to change the application logic without affecting any of the business logic in the future.

1. Alternative approaches, scaling, extensibility 
    - First thing, design workshop with customers to decide how far across the supply chain this solution is going to extend.
    - Separate Product Counts from the Products themselves so that we can manage Product descriptions, categories, and properties separately.
    - Switch from Int to GUID for product IDs, to prevent confusing Products and Categories. This may result in slower writes, but most of our DB actions will be incrementing and decrementing and not adding/deleting Products and Categories. (or switch to SKUs, but those are not unique across businesses if we ever have multiple clients using the same database)
    - Add endpoints for increment/decrement, since those will be used most often by our stockers. This also keeps more of the business logic on the back end instead of having to account for them in the front end.
    - More endpoints also allows for more granular logging. Labels can be tagged to an entire endpoint, instead of needing more logic within the Service layer to identify what type of call is being made.
    - Mobile app using the stripped down endpoints.
    - Functional testing for each endpoint.
    - Document branching strategy, PR requirements.
    - Different roles for who can Create/Update/Delete Products vs. Product Counts
    - Migrate off of SQLite so that database is separate from the application.
    - Seed data via external files/database bacpac.
    - Which will also allow us to figure out a data backup/archive/restore strategy.
    - Document business/functional requirements - how do our customers want to use this app?
    - Include a Revision history, either for the inventory counts or Products list or both.
    - We'll likely need some ability to export data into reports, probably text and excel, which can be handled in its own controller.
    - A module to handle currency conversions, and decide on a canonical currency internal to the system.
