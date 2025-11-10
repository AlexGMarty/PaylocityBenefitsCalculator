using Api.Models;

namespace Api.Repositories;

/// In-memory implementation of dependent repository for purposes of this project
/// In production, this would be replaced with database-backed implementation
public class InMemoryDependentRepository : IDependentRepository
{
    private readonly IEmployeeRepository _employeeRepository;

    public InMemoryDependentRepository(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<Dependent>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        var allDependents = employees.SelectMany(e => e.Dependents).ToList();
        return allDependents;
    }

    public async Task<Dependent?> GetByIdAsync(int id)
    {
        var employees = await _employeeRepository.GetAllAsync();
        var dependent = employees
            .SelectMany(e => e.Dependents)
            .FirstOrDefault(d => d.Id == id);
        return dependent;
    }
}
