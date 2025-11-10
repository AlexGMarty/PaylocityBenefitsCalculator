# Hello, friends at Paylocity!

Look, it's me, Alex, messing up your README file!

Please see below for an overview on the work I did, as well as instructions on how to run the API and tests.

## Files Added

### Models

- `Api/Models/PaycheckDeductionBreakdown.cs` - Breakdown model for benefits deductions

### Repositories

- `Api/Repositories/IEmployeeRepository.cs` - Interface for employee data access
- `Api/Repositories/InMemoryEmployeeRepository.cs` - In-memory implementation of employee repository
- `Api/Repositories/IDependentRepository.cs` - Interface for dependent data access
- `Api/Repositories/InMemoryDependentRepository.cs` - In-memory implementation of dependent repository

### Services

- `Api/Services/BenefitsCalculatorService.cs` - Logic for calculating benefits deductions

### DTOs

- `Api/Dtos/Paycheck/GetPaycheckDto.cs` - Data transfer object for paycheck information including employee details

### Tests

- `ApiTests/UnitTests/BenefitsCalculatorServiceTests.cs` - Unit tests covering calculation scenarios
- `ApiTests/IntegrationTests/EmployeeIntegrationTests.cs` - 4 new integration tests for paycheck endpoint

## Files Modified

### Controllers

- `Api/Controllers/EmployeesController.cs` - Implemented GET by ID, GET all, and GET paycheck endpoints
- `Api/Controllers/DependentsController.cs` - Implemented GET by ID and GET all endpoints

### Configuration

- `Api/Program.cs` - Configured dependency injection for repositories and services

### Test Infrastructure

- `ApiTests/IntegrationTest.cs` - Fixed SSL certificate validation for development testing

### Project Configuration

- `Api/Api.csproj` - Upgraded from .NET 6.0 to .NET 9.0
- `ApiTests/ApiTests.csproj` - Upgraded from .NET 6.0 to .NET 9.0

## How to Run the Project

### Prerequisites

- .NET SDK 9.0 or later
- macOS, Linux, or Windows

### Running the API

```bash
cd PaylocityBenefitsCalculator
dotnet run --project Api/Api.csproj
```

Swagger UI: `https://localhost:7124/swagger`

### Running ALL Tests

```bash
cd PaylocityBenefitsCalculator
dotnet test
```

### Running ONLY Unit Tests

```bash
cd PaylocityBenefitsCalculator
dotnet test --filter "FullyQualifiedName~UnitTests"
```

### Running ONLY Integration Tests

```bash
cd PaylocityBenefitsCalculator
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

**NOTE:** Integration tests require the API to be running in a separate terminal.

## API Endpoints

### Employees

- `GET /api/v1/employees` - Get all employees
- `GET /api/v1/employees/{id}` - Get employee by ID
- `GET /api/v1/employees/{id}/paycheck` - Get paycheck calculation for employee

### Dependents

- `GET /api/v1/dependents` - Get all dependents
- `GET /api/v1/dependents/{id}` - Get dependent by ID

---

# What is this?

A project seed for a C# dotnet API ("PaylocityBenefitsCalculator"). It is meant to get you started on the Paylocity BackEnd Coding Challenge by taking some initial setup decisions away.

The goal is to respect your time, avoid live coding, and get a sense for how you work.

# Coding Challenge

**Show us how you work.**

Each of our Paylocity product teams operates like a small startup, empowered to deliver business value in
whatever way they see fit. Because our teams are close knit and fast moving it is imperative that you are able
to work collaboratively with your fellow developers.

This coding challenge is designed to allow you to demonstrate your abilities and discuss your approach to
design and implementation with your potential colleagues. You are free to use whatever technologies you
prefer but please be prepared to discuss the choices you’ve made. We encourage you to focus on creating a
logical and functional solution rather than one that is completely polished and ready for production.

The challenge can be used as a canvas to capture your strengths in addition to reflecting your overall coding
standards and approach. There’s no right or wrong answer. It’s more about how you think through the
problem. We’re looking to see your skills in all three tiers so the solution can be used as a conversation piece
to show our teams your abilities across the board.

Requirements will be given separately.
