namespace Api.Models;

public class PaycheckDeductionBreakdown {
    public decimal TotalDeduction { get; set; }

    public decimal EmployeeBaseCost { get; set; }
    public decimal DependentsCost { get; set; }
    public decimal DependentAgeSurcharge { get; set; }
    public decimal HighSalarySurcharge { get; set; }
    public int NumberOfDependents { get; set; }
    public int DependentsOver50 { get; set; }
}
