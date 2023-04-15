using LoanCalculator.Services;
using NUnit.Framework;

namespace LoanCalculator.Tests;

[TestFixture]
public class LoanCalculatorServiceTest
{
    private DateTime _today = DateTime.Now;
    [Test]
    public void CalculateMonthlyPayments_DoesNotThrowException_WhenLoanIssueDateIsInTheFuture()
    {
        // Arrange
        var request = new MonthlyPaymentRequest
        {
            LoanAmount = 10000,
            LoanIssueDate = _today.AddDays(1), // дата выдачи кредита в будущем
            LoanClosureDate = _today.AddYears(1),
            InterestRate = 0.1m,
            PaymentType = PaymentType.Annuity,
            PaymentDay = _today.Day
        };

        var calculator = new LoanCalculatorService();

        // Act & Assert
        Assert.DoesNotThrow(() => calculator.CalculateMonthlyPayments(request));
    }

    [Test]
    public void CalculateMonthlyPayments_ThrowsException_WhenLoanIssueDateIsInThePast()
    {
        // Arrange
        var request = new MonthlyPaymentRequest
        {
            LoanAmount = 10000,
            LoanIssueDate = _today.AddDays(-1), // дата выдачи кредита в прошлом
            LoanClosureDate = _today.AddYears(1),
            InterestRate = 0.1m,
            PaymentType = PaymentType.Annuity,
            PaymentDay = _today.Day
        };

        var calculator = new LoanCalculatorService();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => calculator.CalculateMonthlyPayments(request));
    }
    
    [Test]
    public void CalculateMonthlyPayments_ThrowsException_WhenPaymentDayDoesNotMatchLoanClosureDate()
    {
        // Arrange
        var request = new MonthlyPaymentRequest
        {
            LoanAmount = 10000,
            LoanIssueDate = _today, 
            LoanClosureDate = _today.AddYears(1),
            InterestRate = 0.1m,
            PaymentType = PaymentType.Annuity,
            PaymentDay = _today.Day + 1 //день платежа не совпадает с днем закрытия
        };

        var calculator = new LoanCalculatorService();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => calculator.CalculateMonthlyPayments(request));
    }
}