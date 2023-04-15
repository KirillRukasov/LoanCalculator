using NUnit.Framework;

namespace LoanCalculator.Tests;

[TestFixture]
public class DifferentialLoanCalculatorTests
{
    private DifferentialLoanCalculator _calculator;
    private DateTime _today = DateTime.Now;
    
    [SetUp]
    public void Setup()
    {
        _calculator = new DifferentialLoanCalculator();
    }
    
    [Test]
    public void CalculatePayments_ReturnsCorrectNumberOfPayments()
    {
        // Arrange
        var request = new MonthlyPaymentRequest
        {
            LoanAmount = 100000,
            InterestRate = 12,
            LoanIssueDate = _today,
            LoanClosureDate = _today.AddYears(1),
            PaymentType = PaymentType.Differentiated,
            PaymentDay = _today.Day
        };
        
        // Act
        var result = _calculator.CalculatePayments(request);
        
        // Assert
        Assert.That(result.Count, Is.EqualTo(12));
    }
    
    [Test]
    public void CalculatePayments_ReturnsCorrectPrincipalPaymentt()
    {
        // Arrange
        var request = new MonthlyPaymentRequest
        {
            LoanAmount = 100000,
            InterestRate = 12,
            LoanIssueDate = _today,
            LoanClosureDate = _today.AddYears(1),
            PaymentType = PaymentType.Differentiated,
            PaymentDay = _today.Day
        };
        
        // Act
        var result = _calculator.CalculatePayments(request);
        
        // Assert
        Assert.That(result[0].PrincipalPayment, Is.EqualTo(Math.Round(request.LoanAmount / result.Count, 2)));
    }
    
    [Test]
    public void CalculatePayments_ReturnsCorrectLoanAmountAfterAllPayments()
    {
        // Arrange
        var request = new MonthlyPaymentRequest
        {
            LoanAmount = 100000,
            InterestRate = 12,
            LoanIssueDate = _today,
            LoanClosureDate = _today.AddYears(1),
            PaymentType = PaymentType.Differentiated,
            PaymentDay = 15
        };
        
        // Act
        var result = _calculator.CalculatePayments(request);
        var principalPayment = result.Sum(payment => payment.PrincipalPayment); 
        
        // Assert
        Assert.That(principalPayment, Is.EqualTo(request.LoanAmount));
    }
}