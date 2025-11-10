using Api.Models;

namespace Api.Repositories;

/// Repository interface for employee data access
public interface IEmployeeRepository
{
    /// Gets all employees
    Task<List<Employee>> GetAllAsync();

    /// Gets an employee by ID
    Task<Employee?> GetByIdAsync(int id);
}
