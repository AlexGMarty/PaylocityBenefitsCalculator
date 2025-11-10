using Api.Models;

namespace Api.Dtos.Paycheck;

public class GetPaycheckDto
{
    public int EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal GrossPay { get; set; }
    public decimal BenefitsDeduction { get; set; }
    public decimal NetPay { get; set; }
    public PaycheckDeductionBreakdown? DeductionBreakdown { get; set; }
}
