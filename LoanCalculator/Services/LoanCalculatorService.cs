using LoanCalculator.Models;
using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;
using LoanCalculator.Services.Calculators;
using LoanCalculator.Services.Interfaces;

namespace LoanCalculator.Services;

public class LoanCalculatorService : ILoanCalculatorService
{
    public List<MonthlyPaymentResponse> CalculateMonthlyPayments(MonthlyPaymentRequest request)
    {
        if (DateTime.Now > request.LoanIssueDate)
        {
            throw new ArgumentException("Кредит не может быть выдан задним числом.");
        }
        if (request.LoanClosureDate.Day != request.PaymentDay)
        {
            throw new ArgumentException("День закрытия кредита должен совпадать с выбранным днем платежа");
        }

        ILoanCalculator loanCalculator = GetLoanCalculator(request.PaymentType);
        return loanCalculator.CalculatePayments(request);
        
    }

    private ILoanCalculator GetLoanCalculator(PaymentType loanType)
    {
        switch (loanType)
        {
            case PaymentType.Annuity:
                return new AnnuityLoanCalculator();
            case PaymentType.Differentiated:
                return new DifferentialLoanCalculator();
            default:
                throw new ArgumentException("Unsupported loan type", nameof(loanType));
        }
    }
}