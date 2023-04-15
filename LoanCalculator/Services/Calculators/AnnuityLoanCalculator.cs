using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;
using LoanCalculator.Services.Interfaces;

namespace LoanCalculator.Services.Calculators;

public class AnnuityLoanCalculator : ILoanCalculator
{
    private decimal _averageDaysInMonth = Math.Round(365m / 12, 2);
    private decimal _multiplierPayment = 1.05m;
     public List<MonthlyPaymentResponse> CalculatePayments(MonthlyPaymentRequest request)
     { 
        decimal monthlyInterestRate = request.InterestRate / 12 / 100;
        double numberOfPayments = Math.Round((request.LoanClosureDate.Year - request.LoanIssueDate.Year) * 12 
                                            + request.LoanClosureDate.Month - request.LoanIssueDate.Month 
                                            + (double)(request.LoanClosureDate.Day - request.LoanIssueDate.Day) 
                                            / DateTime.DaysInMonth(request.LoanIssueDate.Year, request.LoanIssueDate.Month), 2);

        decimal annuityFactor = (decimal)Math.Pow(1 + (double)monthlyInterestRate, numberOfPayments);
        decimal annuityCoefficient = monthlyInterestRate * annuityFactor / (annuityFactor - 1);
        
        decimal monthlyPayment = Math.Round(request.LoanAmount * annuityCoefficient, 2);
        decimal remainingPrincipal = request.LoanAmount;
        
        List<MonthlyPaymentResponse> paymentSchedule = new List<MonthlyPaymentResponse>();
        
        DateTime currentDate = request.LoanIssueDate;
        DateTime nextPaymentDate = currentDate.GetNextPaymentDate(request.PaymentDay);

        double paymentCount = 0;
        while (paymentCount < numberOfPayments)
        {
            double monthBetweenPayments = Math.Round(Math.Ceiling((nextPaymentDate-currentDate).TotalDays) 
                                                                  / DateTime.DaysInMonth(currentDate.Year, currentDate.Month), 2);
            paymentCount += monthBetweenPayments;
            
            decimal interestPayment = Math.Round(remainingPrincipal * monthlyInterestRate / _averageDaysInMonth * (nextPaymentDate - currentDate).Days, 2);
            decimal principalPayment = Math.Round(monthlyPayment - interestPayment, 2);

            if (paymentCount < 2)
            {
                principalPayment = Math.Round(monthlyPayment * (decimal)paymentCount - interestPayment, 2);
            }
            
            if (principalPayment * _multiplierPayment > remainingPrincipal)
            {
                principalPayment = remainingPrincipal;
            }
            
            remainingPrincipal -= principalPayment;

            paymentSchedule.Add(new MonthlyPaymentResponse
            {
                PaymentDate = nextPaymentDate,
                PrincipalPayment = Math.Round(principalPayment, 2),
                InterestPayment = Math.Round(interestPayment, 2),
                RemainingPrincipal = Math.Round(remainingPrincipal, 2)
            });
            
            currentDate = nextPaymentDate;
            nextPaymentDate = nextPaymentDate.GetNextPaymentDate(request.PaymentDay);
        }

        return paymentSchedule;
    }
}