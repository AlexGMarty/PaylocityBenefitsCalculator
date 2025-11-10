using System;
using System.Collections.Generic;
using Api.Models;
using Api.Services;
using Xunit;

namespace ApiTests.UnitTests;

public class BenefitsCalculatorServiceTests
{
    private readonly BenefitsCalculatorService _calculator;

    public BenefitsCalculatorServiceTests()
    {
        _calculator = new BenefitsCalculatorService();
    }

    [Fact]
    public void CalculatePaycheckDeduction_EmployeeWithNoDependentsLowSalary_ReturnsBaseCostOnly()
    {
        var employee = new Employee
        {
            Id = 1,
            FirstName = "Collin",
            LastName = "Gillespie",
            Salary = 50000m,
            DateOfBirth = new DateTime(1990, 1, 1),
            Dependents = new List<Dependent>()
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        // Base cost: $1000/month * 12 months / 26 paychecks = $461.54
        Assert.Equal(461.54m, result.TotalDeduction, 2);
        Assert.Equal(461.54m, result.EmployeeBaseCost, 2);
        Assert.Equal(0m, result.DependentsCost);
        Assert.Equal(0m, result.DependentAgeSurcharge);
        Assert.Equal(0m, result.HighSalarySurcharge);
        Assert.Equal(0, result.NumberOfDependents);
        Assert.Equal(0, result.DependentsOver50);
    }

    [Fact]
    public void CalculatePaycheckDeduction_EmployeeWithNoDependentsHighSalary_IncludesSalarySurcharge()
    {
        var employee = new Employee
        {
            Id = 2,
            FirstName = "Jalen",
            LastName = "Green",
            Salary = 100000m,
            DateOfBirth = new DateTime(1985, 5, 15),
            Dependents = new List<Dependent>()
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        // Base cost: $461.54
        // Salary surcharge: $100,000 * 2% / 26 = $76.92
        Assert.Equal(538.46m, result.TotalDeduction, 2);
        Assert.Equal(461.54m, result.EmployeeBaseCost, 2);
        Assert.Equal(0m, result.DependentsCost);
        Assert.Equal(0m, result.DependentAgeSurcharge);
        Assert.Equal(76.92m, result.HighSalarySurcharge, 2);
        Assert.Equal(0, result.NumberOfDependents);
        Assert.Equal(0, result.DependentsOver50);
    }

    [Fact]
    public void CalculatePaycheckDeduction_EmployeeWithYoungDependents_IncludesDependentCosts()
    {
        var employee = new Employee
        {
            Id = 3,
            FirstName = "Dillon",
            LastName = "Brooks",
            Salary = 60000m,
            DateOfBirth = new DateTime(1988, 3, 20),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Child1",
                    LastName = "Brooks",
                    DateOfBirth = new DateTime(2015, 6, 10),
                    Relationship = Relationship.Child
                },
                new()
                {
                    Id = 2,
                    FirstName = "Child2",
                    LastName = "Brooks",
                    DateOfBirth = new DateTime(2018, 8, 5),
                    Relationship = Relationship.Child
                }
            }
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        // Base cost: $461.54
        // Dependents cost: 2 * $600/month * 12 / 26 = $553.85
        // Total: $461.54 + $553.85 = $1015.39 (rounded components)
        Assert.Equal(1015.39m, result.TotalDeduction, 2); 
        Assert.Equal(461.54m, result.EmployeeBaseCost, 2);
        Assert.Equal(553.85m, result.DependentsCost, 2);
        Assert.Equal(0m, result.DependentAgeSurcharge);
        Assert.Equal(0m, result.HighSalarySurcharge);
        Assert.Equal(2, result.NumberOfDependents);
        Assert.Equal(0, result.DependentsOver50);
    }

    [Fact]
    public void CalculatePaycheckDeduction_EmployeeWithDependentOver50_IncludesAgeSurcharge()
    {
        var employee = new Employee
        {
            Id = 4,
            FirstName = "Devin",
            LastName = "Booker",
            Salary = 70000m,
            DateOfBirth = new DateTime(1975, 2, 14),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Luka",
                    LastName = "Doncic",
                    DateOfBirth = new DateTime(1970, 4, 20), 
                    Relationship = Relationship.Spouse
                }
            }
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        // Base cost: $461.54
        // Dependent cost: $600/month * 12 / 26 = $276.92
        // Age surcharge: $200/month * 12 / 26 = $92.31
        Assert.Equal(830.77m, result.TotalDeduction, 2);
        Assert.Equal(461.54m, result.EmployeeBaseCost, 2);
        Assert.Equal(276.92m, result.DependentsCost, 2);
        Assert.Equal(92.31m, result.DependentAgeSurcharge, 2);
        Assert.Equal(0m, result.HighSalarySurcharge);
        Assert.Equal(1, result.NumberOfDependents);
        Assert.Equal(1, result.DependentsOver50);
    }

    [Fact]
    public void CalculatePaycheckDeduction_HighSalaryEmployeeWithDependentsOver50_IncludesAllSurcharges()
    {
        var employee = new Employee
        {
            Id = 5,
            FirstName = "Grayson",
            LastName = "Allen",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1963, 2, 17),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Morgan",
                    LastName = "Reid",
                    DateOfBirth = new DateTime(1974, 1, 2),
                    Relationship = Relationship.DomesticPartner
                }
            }
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        // Base cost: $461.54
        // Dependent cost: $276.92
        // Age surcharge: $92.31
        // Salary surcharge: $143,211.12 * 2% / 26 = $110.16
        Assert.Equal(940.93m, result.TotalDeduction, 2);
        Assert.Equal(461.54m, result.EmployeeBaseCost, 2);
        Assert.Equal(276.92m, result.DependentsCost, 2);
        Assert.Equal(92.31m, result.DependentAgeSurcharge, 2);
        Assert.Equal(110.16m, result.HighSalarySurcharge, 2);
        Assert.Equal(1, result.NumberOfDependents);
        Assert.Equal(1, result.DependentsOver50);
    }

    [Fact]
    public void CalculatePaycheckDeduction_MultipleDependentsSomeOver50_CalculatesCorrectly()
    {
        var employee = new Employee
        {
            Id = 10,
            FirstName = "Royce",
            LastName = "O'Neale",
            Salary = 75000m,
            DateOfBirth = new DateTime(1980, 1, 1),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "OldSpouse",
                    LastName = "Case",
                    DateOfBirth = new DateTime(1970, 1, 1), // Over 50
                    Relationship = Relationship.Spouse
                },
                new()
                {
                    Id = 2,
                    FirstName = "YoungChild",
                    LastName = "Case",
                    DateOfBirth = new DateTime(2015, 1, 1), // Under 50
                    Relationship = Relationship.Child
                },
                new()
                {
                    Id = 3,
                    FirstName = "OldChild",
                    LastName = "Case",
                    DateOfBirth = new DateTime(1972, 1, 1), // Over 50
                    Relationship = Relationship.Child
                }
            }
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        // Base cost: $461.54
        // 3 dependents: 3 * $276.92 = $830.77
        // 2 over 50: 2 * $92.31 = $184.62
        // Total: $461.54 + $830.77 + $184.62 = $1476.93 (rounded components)
        Assert.Equal(1476.93m, result.TotalDeduction, 2); 
        Assert.Equal(461.54m, result.EmployeeBaseCost, 2);
        Assert.Equal(830.77m, result.DependentsCost, 2);
        Assert.Equal(184.62m, result.DependentAgeSurcharge, 2);
        Assert.Equal(0m, result.HighSalarySurcharge);
        Assert.Equal(3, result.NumberOfDependents);
        Assert.Equal(2, result.DependentsOver50);
    }

    [Fact]
    public void CalculatePaycheckDeduction_SalaryExactly80000_NoSalarySurcharge()
    {
        // Edge case: salary at threshold should NOT trigger surcharge
        var employee = new Employee
        {
            Id = 6,
            FirstName = "Khaman",
            LastName = "Maluach",
            Salary = 80000m,
            DateOfBirth = new DateTime(1990, 1, 1),
            Dependents = new List<Dependent>()
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        Assert.Equal(461.54m, result.TotalDeduction, 2);
        Assert.Equal(0m, result.HighSalarySurcharge);
    }

    [Fact]
    public void CalculatePaycheckDeduction_SalarySlightlyOver80000_IncludesSalarySurcharge()
    {
        // Count edge case: salary just over threshold should trigger surcharge
        var employee = new Employee
        {
            Id = 7,
            FirstName = "Koby",
            LastName = "Brea",
            Salary = 80000.01m,
            DateOfBirth = new DateTime(1990, 1, 1),
            Dependents = new List<Dependent>()
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        // Should have salary surcharge: $80,000.01 * 2% / 26 = $61.54
        Assert.Equal(61.54m, result.HighSalarySurcharge, 2);
        Assert.True(result.HighSalarySurcharge > 0);
    }

    [Fact]
    public void CalculatePaycheckDeduction_DependentExactly50_NoAgeSurcharge()
    {
        // Edge case: exactly 50 should NOT trigger age surcharge
        var fiftyYearsAgo = DateTime.Today.AddYears(-50);
        var employee = new Employee
        {
            Id = 8,
            FirstName = "Steve",
            LastName = "Nash",
            Salary = 50000m,
            DateOfBirth = new DateTime(1990, 1, 1),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Lilla",
                    LastName = "Frederick",
                    DateOfBirth = fiftyYearsAgo, 
                    Relationship = Relationship.Spouse
                }
            }
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        Assert.Equal(0m, result.DependentAgeSurcharge);
        Assert.Equal(0, result.DependentsOver50);
    }

    [Fact]
    public void CalculatePaycheckDeduction_DependentOver50_IncludesAgeSurcharge()
    {
        // Count edge case: dependent just over 50 should trigger age surcharge
        var fiftyOneYearsAgo = DateTime.Today.AddYears(-51);
        var employee = new Employee
        {
            Id = 9,
            FirstName = "Kevin",
            LastName = "Johnson",
            Salary = 50000m,
            DateOfBirth = new DateTime(1990, 1, 1),
            Dependents = new List<Dependent>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Michelle",
                    LastName = "Rhee",
                    DateOfBirth = fiftyOneYearsAgo,
                    Relationship = Relationship.Spouse
                }
            }
        };

        var result = _calculator.CalculatePaycheckDeduction(employee);

        Assert.Equal(92.31m, result.DependentAgeSurcharge, 2);
        Assert.Equal(1, result.DependentsOver50);
    }
}
