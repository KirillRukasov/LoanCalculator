using LoanCalculator.Models.Requests;

namespace LoanCalculator.Services.Calculators.Helpers;

public class NumbersOfPaymentsCalculator
{
    public double GetNumberOfPayments(MonthlyPaymentRequest request)
    {
        return Math.Round((request.LoanClosureDate.Year - request.LoanIssueDate.Year) * 12 
                          + request.LoanClosureDate.Month - request.LoanIssueDate.Month 
                          + (double)(request.LoanClosureDate.Day - request.LoanIssueDate.Day) 
                          / DateTime.DaysInMonth(request.LoanIssueDate.Year, request.LoanIssueDate.Month), 2);
    }
}