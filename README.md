
# Take-Home Test – Full Stack Application

  

This repository contains a **full-stack application** composed of:

  

-  **Backend**: ASP.NET Core (.NET 8) API

-  **Frontend**: Angular

-  **Database**: SQL Server

-  **Infrastructure**: Docker & Docker Compose

  

The project was designed to demonstrate **software engineering best practices**, including **clean architecture**, **separation of concerns**, **testability**, and **containerized execution**.

  

---

  

## Project overview

  

- Backend follows **Clean Architecture principles**

- Application layer uses **CQRS with MediatR**

- Validation is handled via **FluentValidation**

- Persistence with **EF Core 8 + SQL Server**

- Structured logging with **Serilog**

- API documentation via **Swagger**

- Automated **unit tests** and **integration tests** using **Testcontainers**

- Frontend built with **Angular**, consuming the backend API

  

---

  

## How to run the project

  

### Backend (Docker Compose)

  

#### Requirements

- Docker

- Docker Compose

  

#### Steps

  

1. Navigate to the backend folder:

```bash

cd backend/src

```

  

2. Build and start the services (API + SQL Server):

```bash

docker compose up --build

```

  

3. After startup:

- API: `http://localhost:8080`

- Swagger: `http://localhost:8080/swagger`

  

> The `docker-compose.yaml` file already configures the SQL Server connection string and ensures the API only starts after the database becomes healthy.

  

---

  

### Frontend (Angular)

  

#### Requirements

- Node.js 20+

- NPM

  

#### Steps

  

1. Install dependencies:

```bash

cd frontend

npm install

```

  

2. Start the development server:

```bash

npm start

```

  

3. Access the application at:

```

http://localhost:4200/

```

  

---

  

## Backend documentation (architecture & tests)

  

The backend contains **detailed documentation** covering:

  

- Architectural decisions

- Project structure and responsibilities

- API endpoints

- Docker setup

- Integration tests using **Testcontainers**

  

📘 Full backend documentation:

https://github.com/jorgelucasac/take-home-test/tree/main/backend/src

  

---

  

## Testing strategy

  

-  **Unit tests**

- Validate business rules and application logic

- Use xUnit, Moq, and FluentAssertions

  

-  **Integration tests**

- Spin up a real SQL Server using **Testcontainers**

- Execute real HTTP requests against the API

- Apply migrations and seed data automatically

  

> Integration tests require Docker to be installed and running.

  

---

  

## What is being evaluated

  

This project aims to demonstrate:

  

- Code organization and readability

- Proper use of architectural patterns

- Separation of concerns

- Testability and automated testing

- Containerized development setup

- Clear and maintainable documentation

  

---

  

## Notes for reviewers

  

- The project is fully executable via Docker with minimal setup.

- Backend and frontend can also be run independently for local development.

- The backend README contains deeper technical details for architectural review.