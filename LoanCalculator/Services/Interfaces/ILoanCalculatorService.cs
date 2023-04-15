using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;

namespace LoanCalculator.Services.Interfaces;

public interface ILoanCalculatorService
{
    List<MonthlyPaymentResponse> CalculateMonthlyPayments(MonthlyPaymentRequest request);
}