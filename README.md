# Product Management Backend

## Overview

This is the backend service for the Product Management system, built using .NET 8 with Clean Architecture + DDD.
It provides RESTful APIs for managing products, including CRUD operations, stock and price updates, and file uploads.

## Tech Stack

* .NET 8, C#, ASP.NET Core Web API
* SQL Server (EF Core)
* FluentValidation for request validation
* Mapster for DTO <-> Entity mapping
* MemoryCache for caching
* xUnit + Moq for unit testing

## Prerequisites

* .NET 8 SDK
* SQL Server instance
* Postman (optional, for API testing)

## Setup

1. Clone repository:

```bash
git clone https://github.com/pumpig/ECommerce.ProductManagementBE
cd backend
```

2. Configure database connection in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "<Your SQL Server connection string>"
}
```

3. Run migrations (if using EF Core migrations):

```bash
dotnet ef database update
```

4. Restore dependencies:

```bash
dotnet restore
```

5. Run the project:

```bash
dotnet run
```

6. API will be available at:

```
https://localhost:7228/api/products
```

## Testing

* Run unit tests:

```bash
dotnet test
```

## Postman Collection

* Include `ProductManagement.postman_collection.json` in the repo.
* Set environment variable `BaseUrl` to your backend URL.

## Future Improvements

* Add multi-warehouse support
* Move image storage to cloud (Azure Blob / S3)
* Implement distributed caching (Redis)
* Split into microservices when scaling
