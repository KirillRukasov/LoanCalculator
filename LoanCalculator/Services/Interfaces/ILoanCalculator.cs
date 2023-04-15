using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;

namespace LoanCalculator.Services.Interfaces;

public interface ILoanCalculator
{
    List<MonthlyPaymentResponse> CalculatePayments(MonthlyPaymentRequest request);
}