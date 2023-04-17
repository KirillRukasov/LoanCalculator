using LoanCalculator.Models;

namespace LoanCalculator.Services.Strategies;

public interface IPrincipalPaymentStrategy
{
    decimal GetPrincipalPayment(CalculationParameters calculationParameters, decimal interestPayment, double paymentCount);
}