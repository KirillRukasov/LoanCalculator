using LoanCalculator.Models;
using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;
using LoanCalculator.Services.Calculators.Helpers;
using LoanCalculator.Services.Interfaces;

namespace LoanCalculator.Services.Calculators;

public class DifferentialLoanCalculator : ILoanCalculator
{
    private readonly NumbersOfPaymentsCalculator _paymentsCalculator;

    public DifferentialLoanCalculator()
    {
        _paymentsCalculator = new NumbersOfPaymentsCalculator();
    }
    public List<MonthlyPaymentResponse> CalculatePayments(MonthlyPaymentRequest request)
    {
        var calculationParameters = GetCalculationParameters(request);
        
        List<MonthlyPaymentResponse> paymentSchedule = new List<MonthlyPaymentResponse>();

        PaymentScheduleCalculator calculator = new PaymentScheduleCalculator(request);
        calculator.AddingPaymentToSchedule(request, calculationParameters, paymentSchedule);

        return paymentSchedule;
    }

    private CalculationParameters GetCalculationParameters(MonthlyPaymentRequest request)
    {
        double numbersOfPayments = _paymentsCalculator.GetNumberOfPayments(request);
        decimal monthlyPayment =
            Math.Round(request.LoanAmount / (decimal)numbersOfPayments, 2);
        var calculationParameters = new CalculationParameters
        {
            DailyInterestRate = request.InterestRate / 365 / 100,
            RemainPrincipal = request.LoanAmount,
            CurrentDate = request.LoanIssueDate,
            NextPaymentDate = request.LoanIssueDate.GetNextPaymentDate(request.PaymentDay),
            NumbersOfPayments = numbersOfPayments,
            MonthlyPayment = monthlyPayment
        };

        return calculationParameters;
    }
}   