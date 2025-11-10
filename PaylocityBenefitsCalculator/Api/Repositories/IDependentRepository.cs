using Api.Models;

namespace Api.Repositories;

/// Repository interface for dependent data access
public interface IDependentRepository
{
    /// Gets all dependents across all employees
    Task<List<Dependent>> GetAllAsync();

    /// Gets a dependent by ID
    Task<Dependent?> GetByIdAsync(int id);
}
