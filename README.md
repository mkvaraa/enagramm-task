# Money Transfer

A full-stack web application that simulates money transfers between bank accounts. Transfers are not real banking operations — they only update account balances stored in a database.

## Tech Stack

| Layer | Technologies |
|---|---|
| Backend | .NET 9 Web API, Clean Architecture, EF Core, MS SQL Server |
| Frontend | React 19, TypeScript, Vite, Tailwind CSS |
| Data | MS SQL Server 2022 |
| DevOps | Docker, Docker Compose |
| Testing | xUnit, FluentAssertions, EF Core InMemory |

## Features

- View accounts and their current balances
- Transfer money between accounts with validation
- View transaction history
- View individual account details and history (`/account/:id`)
- Clear API error responses for failed operations

### Transfer Rules

- Sender balance is decreased, receiver balance is increased
- Transfer fails if sender has insufficient funds
- Account balances never become negative
- Same-account transfers are rejected
- Cross-currency transfers are rejected

## Project Structure

```
enagramm-task/
├── backend/                          # .NET solution
│   ├── src/
│   │   ├── MoneyTransfer.Api/        # Controllers, middleware, Program.cs
│   │   ├── MoneyTransfer.Application/ # Use cases, DTOs, validators
│   │   ├── MoneyTransfer.Domain/     # Entities, business rules
│   │   └── MoneyTransfer.Infrastructure/ # EF Core, migrations, repositories
│   ├── tests/
│   │   └── MoneyTransfer.UnitTests/
│   ├── Dockerfile
│   └── MoneyTransfer.sln
│
├── frontend/                         # React SPA
│   ├── src/
│   │   ├── api/                      # Axios HTTP client
│   │   ├── features/                 # Reusable UI blocks + hooks
│   │   ├── pages/                    # Route entry points (lazy-loaded)
│   │   ├── routes/                   # Centralized route config
│   │   ├── layouts/                  # App shell with navbar
│   │   └── types/                    # TypeScript interfaces
│   ├── Dockerfile
│   └── nginx.conf                    # SPA routing + API proxy
│
└── docker-compose.yml                # Orchestrates all services
```

## Quick Start (Docker)

**Requirements:** Docker Desktop only. No manual configuration needed.

```bash
git clone <repository-url>
cd enagramm-task
docker compose up --build
```

Wait until all services are healthy (~1–2 minutes on first run).

| Service | URL |
|---|---|
| Web App | http://localhost:3000 |
| API (Swagger) | http://localhost:5000/swagger |
| SQL Server | `localhost:1433` (user: `sa`, password: `Your_password123`) |

On first startup the API automatically:
1. Applies EF Core migrations (creates tables)
2. Seeds 3 test accounts: Alice ($1000), Bob ($500), Charlie ($200)

### Stop

```bash
# Stop containers (data preserved in Docker volume)
docker compose down

# Stop and remove database volume (fresh start)
docker compose down -v
```

## Local Development

For development without Docker Compose (run each service separately).

### Prerequisites

- .NET 9 SDK
- Node.js 20+
- MS SQL Server (via Docker or local instance)

### 1. Start SQL Server

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_password123" \
  -p 1433:1433 --name mt-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Start Backend

```bash
cd backend
dotnet run --project src/MoneyTransfer.Api
```

API runs at http://localhost:5181 (see `launchSettings.json`).

Swagger: http://localhost:5181/swagger

### 3. Start Frontend

```bash
cd frontend
npm install
npm run dev
```

Frontend runs at http://localhost:5173. Vite proxies `/api` requests to `http://localhost:5181`.

### Run Tests

```bash
cd backend
dotnet test
```

12 unit tests covering domain logic and transfer scenarios.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Accounts` | List all accounts |
| `GET` | `/api/Accounts/{id}` | Get account by ID |
| `GET` | `/api/Transactions` | List all transactions |
| `POST` | `/api/Transfers` | Execute a transfer |

### Transfer Request

```json
{
  "fromAccountId": "guid",
  "toAccountId": "guid",
  "amount": 100.00
}
```

### Success Response (200)

```json
{
  "success": true,
  "transaction": {
    "id": "guid",
    "fromAccountId": "guid",
    "toAccountId": "guid",
    "amount": 100.00,
    "currency": "USD",
    "createdAt": "2026-07-07T12:00:00Z"
  }
}
```

### Error Response (422)

```json
{
  "success": false,
  "error": "INSUFFICIENT_FUNDS",
  "message": "Account GE001 has insufficient funds."
}
```

### Error Codes

| Code | Meaning |
|---|---|
| `INSUFFICIENT_FUNDS` | Sender balance too low |
| `SAME_ACCOUNT` | Cannot transfer to the same account |
| `INVALID_AMOUNT` | Amount must be positive |
| `FROM_ACCOUNT_NOT_FOUND` | Sender account does not exist |
| `TO_ACCOUNT_NOT_FOUND` | Receiver account does not exist |
| `CURRENCY_MISMATCH` | Accounts have different currencies |
| `VALIDATION_FAILED` | Request body failed validation (400) |

## Frontend Routes

| Route | Page |
|---|---|
| `/` | Accounts list + transfer form |
| `/transactions` | All transactions |
| `/account/:id` | Account details and history |
| `*` | 404 page |

## Architecture Highlights

### Backend (Clean Architecture)

- **Domain** — rich entities with invariants (`Account.Debit()`, `Account.Credit()`)
- **Application** — `TransferService` with Result pattern, FluentValidation
- **Infrastructure** — EF Core with `Serializable` transactions, optimistic concurrency (`RowVersion`)
- **Api** — thin controllers, global exception handler, Swagger, CORS

### Frontend

- Feature-based folder structure with separated concerns
- React Query for server state (caching, auto-refetch after mutations)
- React Hook Form + Zod for form validation
- Lazy-loaded routes with Suspense
- Nginx reverse proxy in production (Docker)

## Troubleshooting

| Problem | Solution |
|---|---|
| Port 1433 already in use | Stop other SQL Server instances or change port in `docker-compose.yml` |
| API cannot connect to DB | Wait for SQL healthcheck to pass (~30s after `docker compose up`) |
| Frontend shows empty data | Ensure API is running and accessible |
| `docker compose` not found | Use Docker Desktop (includes Compose v2) |
