namespace LoanCalculator.Models;

public struct CalculationParameters
{
    public decimal DailyInterestRate { get; set; }
    public decimal RemainPrincipal { get; set; }
    public DateTime CurrentDate { get; set; }
    public DateTime NextPaymentDate { get; set; }
    public double NumbersOfPayments { get; set; }
    public decimal MonthlyPayment { get; set; }
}