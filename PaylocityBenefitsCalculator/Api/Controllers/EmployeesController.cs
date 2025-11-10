using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Repositories;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly BenefitsCalculatorService _benefitsCalculator;

    public EmployeesController(
        IEmployeeRepository employeeRepository,
        BenefitsCalculatorService benefitsCalculator)
    {
        _employeeRepository = employeeRepository;
        _benefitsCalculator = benefitsCalculator;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound(new ApiResponse<GetEmployeeDto>
            {
                Data = null,
                Success = false,
                Message = $"Employee with ID {id} not found"
            });
        }

        var employeeDto = new GetEmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary,
            DateOfBirth = employee.DateOfBirth,
            Dependents = employee.Dependents.Select(d => new GetDependentDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                DateOfBirth = d.DateOfBirth,
                Relationship = d.Relationship
            }).ToList()
        };

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = employeeDto,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get paycheck for employee by id")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> GetPaycheck(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound(new ApiResponse<GetPaycheckDto>
            {
                Data = null,
                Success = false,
                Message = $"Employee with ID {id} not found"
            });
        }

        // Calculate benefits deduction
        var deductionBreakdown = _benefitsCalculator.CalculatePaycheckDeduction(employee);

        // Calculate paycheck details
        var grossPay = Math.Round(employee.Salary / 26, 2); // 26 paychecks per year
        var netPay = Math.Round(grossPay - deductionBreakdown.TotalDeduction, 2);

        var paycheck = new GetPaycheckDto
        {
            EmployeeId = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            GrossPay = grossPay,
            BenefitsDeduction = deductionBreakdown.TotalDeduction,
            NetPay = netPay,
            DeductionBreakdown = deductionBreakdown
        };

        var result = new ApiResponse<GetPaycheckDto>
        {
            Data = paycheck,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _employeeRepository.GetAllAsync();

        var employeeDtos = employees.Select(e => new GetEmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Salary = e.Salary,
            DateOfBirth = e.DateOfBirth,
            Dependents = e.Dependents.Select(d => new GetDependentDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                DateOfBirth = d.DateOfBirth,
                Relationship = d.Relationship
            }).ToList()
        }).ToList();

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employeeDtos,
            Success = true
        };

        return result;
    }
}
