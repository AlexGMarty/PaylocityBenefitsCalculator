using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        var employees = new List<GetEmployeeDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
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
                Dependents = new List<GetDependentDto>
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
                Dependents = new List<GetDependentDto>
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
        await response.ShouldReturn(HttpStatusCode.OK, employees);
    }

    [Fact]
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1");
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }
    
    [Fact]
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedForPaycheck_ShouldReturnCorrectCalculationsForEmployeeWithNoDependents()
    {
        // LeBron James - no dependents, salary below $80k threshold
        var response = await HttpClient.GetAsync("/api/v1/employees/1/paycheck");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResponse<Api.Dtos.Paycheck.GetPaycheckDto>>(
            await response.Content.ReadAsStringAsync());
        
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        
        var paycheck = apiResponse.Data;
        Assert.Equal(1, paycheck.EmployeeId);
        Assert.Equal("LeBron", paycheck.FirstName);
        Assert.Equal("James", paycheck.LastName);
        Assert.Equal(2900.81m, paycheck.GrossPay);
        Assert.Equal(461.54m, paycheck.BenefitsDeduction);
        Assert.Equal(2439.27m, paycheck.NetPay);
        Assert.NotNull(paycheck.DeductionBreakdown);
        Assert.Equal(0, paycheck.DeductionBreakdown.NumberOfDependents);
        Assert.Equal(0m, paycheck.DeductionBreakdown.HighSalarySurcharge);
    }

    [Fact]
    public async Task WhenAskedForPaycheck_ShouldReturnCorrectCalculationsForHighSalaryEmployee()
    {
        // Ja Morant - 3 young dependents, salary above $80k threshold
        var response = await HttpClient.GetAsync("/api/v1/employees/2/paycheck");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResponse<Api.Dtos.Paycheck.GetPaycheckDto>>(
            await response.Content.ReadAsStringAsync());
        
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        
        var paycheck = apiResponse.Data;
        Assert.Equal(2, paycheck.EmployeeId);
        Assert.Equal("Ja", paycheck.FirstName);
        Assert.Equal("Morant", paycheck.LastName);
        Assert.Equal(3552.51m, paycheck.GrossPay);
        Assert.NotNull(paycheck.DeductionBreakdown);
        Assert.Equal(3, paycheck.DeductionBreakdown.NumberOfDependents);
        Assert.Equal(0, paycheck.DeductionBreakdown.DependentsOver50);
        Assert.Equal(71.05m, paycheck.DeductionBreakdown.HighSalarySurcharge);
    }

    [Fact]
    public async Task WhenAskedForPaycheck_ShouldReturnCorrectCalculationsForEmployeeWithDependentOver50()
    {
        // Michael Jordan - 1 dependent over 50, high salary
        var response = await HttpClient.GetAsync("/api/v1/employees/3/paycheck");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResponse<Api.Dtos.Paycheck.GetPaycheckDto>>(
            await response.Content.ReadAsStringAsync());
        
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        
        var paycheck = apiResponse.Data;
        Assert.Equal(3, paycheck.EmployeeId);
        Assert.Equal("Michael", paycheck.FirstName);
        Assert.Equal("Jordan", paycheck.LastName);
        Assert.NotNull(paycheck.DeductionBreakdown);
        Assert.Equal(1, paycheck.DeductionBreakdown.NumberOfDependents);
        Assert.Equal(1, paycheck.DeductionBreakdown.DependentsOver50);
        Assert.Equal(92.31m, paycheck.DeductionBreakdown.DependentAgeSurcharge);
        Assert.Equal(110.16m, paycheck.DeductionBreakdown.HighSalarySurcharge);
    }

    [Fact]
    public async Task WhenAskedForPaycheckForNonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}/paycheck");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}

