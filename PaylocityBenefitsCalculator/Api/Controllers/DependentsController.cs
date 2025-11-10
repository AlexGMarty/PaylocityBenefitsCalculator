using Api.Dtos.Dependent;
using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentRepository _dependentRepository;

    public DependentsController(IDependentRepository dependentRepository)
    {
        _dependentRepository = dependentRepository;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _dependentRepository.GetByIdAsync(id);

        if (dependent == null)
        {
            return NotFound(new ApiResponse<GetDependentDto>
            {
                Data = null,
                Success = false,
                Message = $"Dependent with ID {id} not found"
            });
        }

        var dependentDto = new GetDependentDto
        {
            Id = dependent.Id,
            FirstName = dependent.FirstName,
            LastName = dependent.LastName,
            DateOfBirth = dependent.DateOfBirth,
            Relationship = dependent.Relationship
        };

        var result = new ApiResponse<GetDependentDto>
        {
            Data = dependentDto,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentRepository.GetAllAsync();

        var dependentDtos = dependents.Select(d => new GetDependentDto
        {
            Id = d.Id,
            FirstName = d.FirstName,
            LastName = d.LastName,
            DateOfBirth = d.DateOfBirth,
            Relationship = d.Relationship
        }).ToList();

        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependentDtos,
            Success = true
        };

        return result;
    }
}
