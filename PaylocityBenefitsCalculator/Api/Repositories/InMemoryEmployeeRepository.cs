using Api.Models;

namespace Api.Repositories;

/// In-memory implementation of employee repository for purposes of this project
/// In production, this would be replaced with database-backed implementation
public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _employees;

    public InMemoryEmployeeRepository()
    {
        _employees = new List<Employee>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30),
                Dependents = new List<Dependent>()
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<Dependent>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<Dependent>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            },
            new()
            {
                Id = 4,
                FirstName = "Anthony",
                LastName = "Edwards",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<Dependent>
                {
                    new()
                    {
                        Id = 5,
                        FirstName = "Child1",
                        LastName = "Edwards",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 6,
                        FirstName = "Child2",
                        LastName = "Edwards",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    },
                    new()
                    {
                        Id = 7,
                        FirstName = "Child3",
                        LastName = "Edwards",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2022, 4, 15)
                    },
                    new()
                    {
                        Id = 8,
                        FirstName = "Child4",
                        LastName = "Edwards",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2023, 3, 10)
                    },
                    new()
                    {
                        Id = 9,
                        FirstName = "Child5",
                        LastName = "Edwards",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2024, 2, 5)
                    },
                    new()
                    {
                        Id = 10,
                        FirstName = "Child6",
                        LastName = "Edwards",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2025, 1, 1)
                    },
                    new()
                    {
                        Id = 11,
                        FirstName = "Child7",
                        LastName = "Edwards",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2025, 12, 25)
                    }
                }
            }
        };
    }

    public Task<List<Employee>> GetAllAsync()
    {
        return Task.FromResult(_employees);
    }

    public Task<Employee?> GetByIdAsync(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(employee);
    }
}
