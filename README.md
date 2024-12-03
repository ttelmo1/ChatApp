# ChatApp API

A real-time chat API developed with .NET, using SignalR for real-time communication, RabbitMQ for asynchronous message processing, and Entity Framework Core for data persistence.

## 🚀 Technologies

- .NET 8.0
- Entity Framework Core
- SignalR
- RabbitMQ
- SQL Server
- Docker

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop)
- [SQL Server](https://www.microsoft.com/sql-server)

## 🛠️ Environment Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd ChatApp
```

### 2. RabbitMQ Setup

Run RabbitMQ using Docker:

```bash
docker-compose up -d
docker-compose up --build
```

This will start RabbitMQ with:
- Port 5672: AMQP Communication
- Port 15672: Management Interface (user/password: guest/guest)

### 3. Database Setup

1. Verify the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ChatApp;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

2. Run database migrations:

```bash
dotnet tool install --global dotnet-ef  # If EF Core CLI is not installed
dotnet ef database update
```

## 🚀 Running the Project

1. Ensure RabbitMQ is running:

```bash
docker ps  # Check if RabbitMQ container is active
```

2. Run the API:

```bash
dotnet restore
dotnet build
dotnet run
```

The API will be available at: `https://localhost:5125` (or configured port)
