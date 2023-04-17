using LoanCalculator.Models;
using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;
using LoanCalculator.Services.Calculators.Helpers;
using LoanCalculator.Services.Interfaces;

namespace LoanCalculator.Services.Calculators;

public class AnnuityLoanCalculator : ILoanCalculator
{
    private readonly NumbersOfPaymentsCalculator _paymentsCalculator;

    public AnnuityLoanCalculator()
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
        double numberOfPayments = _paymentsCalculator.GetNumberOfPayments(request);
        decimal annuityFactor = (decimal)Math.Pow(1 + (double)request.InterestRate / 12 / 100, numberOfPayments);
        decimal annuityCoefficient = request.InterestRate / 12 / 100 * annuityFactor / (annuityFactor - 1);
        decimal monthlyPayment = Math.Round(request.LoanAmount * annuityCoefficient, 2);

        var calculationParameters = new CalculationParameters
        {
            DailyInterestRate = request.InterestRate / 365 / 100,
            RemainPrincipal = request.LoanAmount,
            CurrentDate = request.LoanIssueDate,
            NextPaymentDate = request.LoanIssueDate.GetNextPaymentDate(request.PaymentDay),
            NumbersOfPayments = numberOfPayments,
            MonthlyPayment = monthlyPayment,
        };

        return calculationParameters;
    }
}