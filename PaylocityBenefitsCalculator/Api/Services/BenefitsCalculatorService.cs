using Api.Models;

namespace Api.Services;

public class BenefitsCalculatorService
{
    private const decimal EmployeeMonthlyBaseCost = 1000m;
    private const decimal DependentMonthlyBaseCost = 600m;
    private const decimal DependentOver50MonthlySurcharge = 200m;
    private const decimal HighSalaryThreshold = 80000m;
    private const decimal HighSalarySurchargeRate = 0.02m;
    private const int PaychecksPerYear = 26;
    /// We're already making everything else its own const, so why not?
    private const int MonthsPerYear = 12;

    /// Calculates the benefits deduction breakdown for an employee's paycheck
    /// <param name="employee">The employee to calculate benefits for</param>
    /// <returns>Detailed breakdown of the paycheck deduction</returns>
    public PaycheckDeductionBreakdown CalculatePaycheckDeduction(Employee employee)
    {
        var breakdown = new PaycheckDeductionBreakdown();

        // Base employee cost per paycheck
        var yearlyEmployeeCost = EmployeeMonthlyBaseCost * MonthsPerYear;
        breakdown.EmployeeBaseCost = Math.Round(yearlyEmployeeCost / PaychecksPerYear, 2);

        // Dependent costs per paycheck
        var dependentsOver50 = 0;
        var totalDependentMonthlyCost = 0m;

        foreach (var dependent in employee.Dependents)
        {
            totalDependentMonthlyCost += DependentMonthlyBaseCost;
            if (IsDependentOver50(dependent))
            {
                totalDependentMonthlyCost += DependentOver50MonthlySurcharge;
                dependentsOver50++;
            }
        }

        var yearlyDependentBaseCost = employee.Dependents.Count * DependentMonthlyBaseCost * MonthsPerYear;
        breakdown.DependentsCost = Math.Round(yearlyDependentBaseCost / PaychecksPerYear, 2);

        var yearlyAgeSurcharge = dependentsOver50 * DependentOver50MonthlySurcharge * MonthsPerYear;
        breakdown.DependentAgeSurcharge = Math.Round(yearlyAgeSurcharge / PaychecksPerYear, 2);

        // High salary surcharge, if applicable, per paycheck
        if (employee.Salary > HighSalaryThreshold)
        {
            var yearlySalarySurcharge = employee.Salary * HighSalarySurchargeRate;
            breakdown.HighSalarySurcharge = Math.Round(yearlySalarySurcharge / PaychecksPerYear, 2);
        }

        // Metadata
        breakdown.NumberOfDependents = employee.Dependents.Count;
        breakdown.DependentsOver50 = dependentsOver50;

        // Calculate total
        breakdown.TotalDeduction = breakdown.EmployeeBaseCost 
            + breakdown.DependentsCost 
            + breakdown.DependentAgeSurcharge 
            + breakdown.HighSalarySurcharge;

        return breakdown;
    }

    /// Determines if a dependent is over 50 years old
    /// <param name="dependent">The dependent to check</param>
    /// <returns>True if the dependent is over 50, false otherwise</returns>
    private bool IsDependentOver50(Dependent dependent)
    {
        var today = DateTime.Today;
        var age = today.Year - dependent.DateOfBirth.Year;
        // Subtract one year if birthday hasn't occurred yet this year
        if (dependent.DateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age > 50;
    }
}
