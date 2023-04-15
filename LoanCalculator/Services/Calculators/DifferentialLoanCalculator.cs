using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;
using LoanCalculator.Services.Interfaces;

namespace LoanCalculator.Services.Calculators;

public class DifferentialLoanCalculator : ILoanCalculator
{
    private decimal _multiplierPayment = 1.05m;
    public List<MonthlyPaymentResponse> CalculatePayments(MonthlyPaymentRequest request)
    {
        decimal dailyInterestRate = request.InterestRate / 365 / 100;
        decimal remainingPrincipal = request.LoanAmount;
        
        List<MonthlyPaymentResponse> paymentSchedule = new List<MonthlyPaymentResponse>();
        
        DateTime currentDate = request.LoanIssueDate;
        DateTime nextPaymentDate = currentDate.GetNextPaymentDate(request.PaymentDay);
        
        double numberOfPayments = Math.Round((request.LoanClosureDate.Year - request.LoanIssueDate.Year) * 12 
                                             + request.LoanClosureDate.Month - request.LoanIssueDate.Month 
                                             + (double)(request.LoanClosureDate.Day - request.LoanIssueDate.Day) 
                                             / DateTime.DaysInMonth(request.LoanIssueDate.Year, request.LoanIssueDate.Month), 2);

        decimal monthlyPayment = Math.Round(request.LoanAmount / (decimal)numberOfPayments, 2);

        double paymentCount = 0;
        while (paymentCount < numberOfPayments)
        {
            double monthBetweenPayments = Math.Round(Math.Ceiling((nextPaymentDate-currentDate).TotalDays) / DateTime.DaysInMonth(currentDate.Year, currentDate.Month), 2);
            paymentCount += monthBetweenPayments;
            decimal interestPayment = remainingPrincipal * dailyInterestRate * (nextPaymentDate - currentDate).Days;
            decimal principalPayment = monthlyPayment;

            if (paymentCount < 2)
            {
                principalPayment = monthlyPayment * (decimal)paymentCount;
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