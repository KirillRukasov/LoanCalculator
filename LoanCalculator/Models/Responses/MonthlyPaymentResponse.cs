namespace LoanCalculator.Models.Responses;

public class MonthlyPaymentResponse
{
    public DateTime PaymentDate { get; set; }
    public decimal PrincipalPayment { get; set; }
    public decimal InterestPayment { get; set; }
    public decimal RemainingPrincipal { get; set; }
}