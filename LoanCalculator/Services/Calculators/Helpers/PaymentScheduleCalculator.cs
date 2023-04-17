using LoanCalculator.Models;
using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;
using LoanCalculator.Services.Strategies;

namespace LoanCalculator.Services.Calculators.Helpers;

public class PaymentScheduleCalculator
{
    private readonly IPrincipalPaymentStrategy _principalPaymentStrategy;

    public PaymentScheduleCalculator(MonthlyPaymentRequest request)
    {
        switch (request.PaymentType)
        {
            case PaymentType.Annuity:
                _principalPaymentStrategy = new AnnuityPrincipalPaymentStrategy();
                break;
            case PaymentType.Differentiated:
                _principalPaymentStrategy = new DifferentiatedPrincipalPaymentStrategy();
                break;
        }

    }
    public void AddingPaymentToSchedule(MonthlyPaymentRequest request, CalculationParameters calculationParameters,
        List<MonthlyPaymentResponse> paymentSchedule)
    {
        double paymentCount = 0;
        while (paymentCount < calculationParameters.NumbersOfPayments)
        {
            double monthBetweenPayments = Math.Round(
                Math.Ceiling((calculationParameters.NextPaymentDate - calculationParameters.CurrentDate).TotalDays)
                / DateTime.DaysInMonth(calculationParameters.CurrentDate.Year, calculationParameters.CurrentDate.Month), 2);
            double daysBetweenDate =
                Math.Ceiling((calculationParameters.NextPaymentDate - calculationParameters.CurrentDate).TotalDays);
            paymentCount += monthBetweenPayments;

            decimal interestPayment = Math.Round(calculationParameters.RemainPrincipal *
                                                 calculationParameters.DailyInterestRate
                                                 * (decimal)daysBetweenDate, 2);

            decimal principalPayment = _principalPaymentStrategy.GetPrincipalPayment(calculationParameters, interestPayment, paymentCount);

            calculationParameters.RemainPrincipal -= principalPayment;

            paymentSchedule.Add(new MonthlyPaymentResponse
            {
                PaymentDate = calculationParameters.NextPaymentDate,
                PrincipalPayment = principalPayment,
                InterestPayment = interestPayment,
                RemainingPrincipal = calculationParameters.RemainPrincipal
            });

            calculationParameters.CurrentDate = calculationParameters.NextPaymentDate;
            calculationParameters.NextPaymentDate = 
                calculationParameters.NextPaymentDate.GetNextPaymentDate(request.PaymentDay);
        }
    }
}