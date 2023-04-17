using LoanCalculator.Models;

namespace LoanCalculator.Services.Strategies;

public class DifferentiatedPrincipalPaymentStrategy : IPrincipalPaymentStrategy
{
    private decimal _multiplierPayment = 1.05m;

    public decimal GetPrincipalPayment(CalculationParameters calculationParameters, decimal interestPayment, double paymentCount)
    {
        decimal principalPayment = calculationParameters.MonthlyPayment;

        if (paymentCount < 2)
        {
            principalPayment = Math.Round(calculationParameters.MonthlyPayment * (decimal)paymentCount, 2);
        }

        if (principalPayment * _multiplierPayment > calculationParameters.RemainPrincipal)
        {
            principalPayment = calculationParameters.RemainPrincipal;
        }

        return principalPayment;
    }
}