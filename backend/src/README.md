# Fundo – Backend (.NET 8) + SQL Server + Testcontainers

This repository contains an **ASP.NET Core (.NET 8)** API for **loan management**, following a layered architecture (Clean Architecture style), using **CQRS/MediatR**, **validation with FluentValidation**, **persistence with EF Core + SQL Server**, **logging with Serilog**, **Swagger**, and both **unit** and **integration tests** (via **Testcontainers**).

## Overview

### Main API endpoints
Base route: `/loans`

- `GET /loans` – list loans
- `GET /loans/{id:guid}` – get loan by id
- `POST /loans` – create a loan
- `POST /loans/{id:guid}/payment` – apply a payment to a loan

> Note: the API adds (or generates) the `X-Correlation-Id` header and also documents this header in Swagger.

---

## Project structure

- **Fundo.Domain**
  - Domain entities and business rules (e.g. `Loan`, `LoanStatus`)
  - Repository contracts (`ILoanRepository`) and `IUnitOfWork`

- **Fundo.Application**
  - Use cases (CQRS) using **MediatR**
  - **Commands**: `CreateLoan`, `ApplyPayment`
  - **Queries**: `GetLoans`, `GetLoanById`
  - **FluentValidation** with `ValidationBehavior` (MediatR pipeline)

- **Fundo.Infrastructure.Persistence**
  - **EF Core 8** + SQL Server (`AppDbContext`)
  - Repositories and `UnitOfWork`
  - Dependency Injection setup (`AddPersistence`)
  - **Seed** (`LoanSeed`) to populate initial data

- **Fundo.WebApi**
  - Controllers (e.g. `LoanManagementController`)
  - Middlewares:
    - `CorrelationIdMiddleware` (`X-Correlation-Id` header)
    - `ErrorHandlingMiddleware` (returns `500` with `ErrorResponse`)
  - **Swagger (Swashbuckle)** + OperationFilter for correlation header
  - **Serilog** (enriched with `Application` and `Environment`)
  - **CORS** enabled (`AllowAnyOrigin/AnyHeader/AnyMethod`) under the `AngularDev` policy

- **Fundo.Services.Tests**
  - **Unit tests**: xUnit + Moq + FluentAssertions
  - **Integration tests**: `WebApplicationFactory<Program>` + **Testcontainers (MsSql)**

---

## Requirements

### To run the API
- .NET SDK 8 (to run locally without Docker) **or**
- Docker + Docker Compose (to run everything containerized)

### To run integration tests (Testcontainers)
- Docker installed and **running**
- On Linux, it is usually required to run with permissions to access the Docker daemon
- On Windows/macOS, make sure Docker Desktop is running

---

## Running with Docker Compose

At the root of the `src` folder, there is a `docker-compose.yaml` with two services:

- `sqlserver` (Microsoft SQL Server 2022)
- `webapi` (API exposed at `http://localhost:8080`)

### 1) Start the containers

```bash
cd src
docker compose up --build
```

The API will be available at:
- `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`

### 2) Healthcheck / database dependency

The compose file already defines a SQL Server `healthcheck` and uses `depends_on` with `condition: service_healthy`,
so the API should only start after SQL Server is accepting connections.

> If you still see errors on the first startup, it is common for SQL Server to take a few extra seconds to become fully “ready”.
> The healthcheck helps, but you can increase `retries/interval` in the compose file if needed.

### 3) Important: `ConnectionStrings__DefaultConnection`

The API uses the `DefaultConnection` connection string. In `docker-compose.yaml`, it should look like:

```text
Server=sqlserver,1433;Database=FundoDb;User Id=sa;Password=Dev@123456;TrustServerCertificate=True;
```

> In the zip you shared, this line appears as `...` (likely an editing or copy/paste artifact).
> If it is like that in your local file, **replace it with the full value above**.

---

## Running locally (without Docker)

```bash
cd src/Fundo.WebApi
dotnet run
```

The API will start using the values from `appsettings` and environment variables.
You will need an accessible SQL Server and a properly configured `DefaultConnection`.

---

## Running tests

### 1) All tests (unit + integration)

From the `src` root:

```bash
dotnet test
```

### 2) Unit tests only

```bash
dotnet test src.sln --filter "FullyQualifiedName~Unit"
```

### 3) Integration tests only

```bash
dotnet test src.sln --filter "FullyQualifiedName~Integration"
```

---

## How integration tests work (Testcontainers)

Integration tests live under:

- `Fundo.Services.Tests/Integration/...`

They use:
- `SqlServerContainerFixture` (**Testcontainers.MsSql**) to spin up an ephemeral SQL Server
- `CustomWebApplicationFactory` to:
  - override the `DbContext` to point to the container database
  - run `db.Database.Migrate()`
  - execute `LoanSeed` to insert initial data
- `WebApplicationFactory`’s `HttpClient` to call real endpoints (`/loans`, `/payment`, etc.)

### Important notes (when tests fail)

- **Docker is not running** → integration tests cannot start the container
- **Ports / firewall** → in some environments, port binding may fail
- **First run is slower** → pulling `mcr.microsoft.com/mssql/server:2022-latest` can take some time

If the container fails to start, the fixture captures logs:
- `SQL Server container failed to start... LOGS: ...`

These logs usually help diagnose password issues, resource constraints, or Docker daemon problems.

---

## Main technologies and libraries

- .NET 8 / ASP.NET Core
- EF Core 8 (SQL Server)
- MediatR (CQRS)
- FluentValidation (+ pipeline behavior)
- Serilog (console + configuration)
- Swagger / Swashbuckle
- xUnit + FluentAssertions + Moq
- Testcontainers (MsSql)

---

## Quick tips

- Quick API test:
  ```bash
  curl http://localhost:8080/loans
  ```
- If consumed by an Angular UI, CORS is enabled via the `AngularDev` policy.
