using LoanCalculator.Models;

namespace LoanCalculator.Services.Strategies;

public class AnnuityPrincipalPaymentStrategy : IPrincipalPaymentStrategy
{
    private decimal _multiplierPayment = 1.05m;

    public decimal GetPrincipalPayment(CalculationParameters calculationParameters, decimal interestPayment, double paymentCount)
    {
        decimal principalPayment = Math.Round(calculationParameters.MonthlyPayment - interestPayment, 2);

        if (paymentCount < 2)
        {
            principalPayment = Math.Round(calculationParameters.MonthlyPayment * (decimal)paymentCount - interestPayment, 2);
        }

        if (principalPayment * _multiplierPayment > calculationParameters.RemainPrincipal)
        {
            principalPayment = calculationParameters.RemainPrincipal;
        }

        return principalPayment;
    }
}