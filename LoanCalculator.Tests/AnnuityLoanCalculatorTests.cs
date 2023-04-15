using NUnit.Framework;

namespace LoanCalculator.Tests;

[TestFixture]
public class AnnuityLoanCalculatorTests
{
    private AnnuityLoanCalculator _calculator;
    private DateTime _today = DateTime.Now;
    
    [SetUp]
    public void Setup()
    {
        _calculator = new AnnuityLoanCalculator();
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
            PaymentType = 0,
            PaymentDay = _today.Day
        };
        
        // Act
        var result = _calculator.CalculatePayments(request);
        
        // Assert
        Assert.That(result.Count, Is.EqualTo(12));
    }
    
    [Test]
    public void CalculatePayments_ReturnsCorrectMonthlyPayment()
    {
        // Arrange
        var request = new MonthlyPaymentRequest
        {
            LoanAmount = 100000,
            InterestRate = 12,
            LoanIssueDate = _today,
            LoanClosureDate = _today.AddYears(1),
            PaymentType = 0,
            PaymentDay = _today.Day
        };
        
        // Act
        var result = _calculator.CalculatePayments(request);
        var monthlyPayment = result[0].InterestPayment + result[0].PrincipalPayment;
        
        // Assert
        Assert.That(monthlyPayment, Is.EqualTo(8884.88m));
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
            PaymentType = 0,
            PaymentDay = 15
        };
        
        // Act
        var result = _calculator.CalculatePayments(request);
        var principalPayment = result.Sum(payment => payment.PrincipalPayment); 
        
        // Assert
        Assert.That(principalPayment, Is.EqualTo(request.LoanAmount));
    }
    
    [Test]
    public void CalculatePayments_CheckFebraryPaymentDate()
    {
        // Arrange
        var request = new MonthlyPaymentRequest
        {
            LoanAmount = 100000,
            InterestRate = 12,
            LoanIssueDate = _today,
            LoanClosureDate = _today.AddYears(1),
            PaymentType = 0,
            PaymentDay = 30
        };
        
        // Act
        var result = _calculator.CalculatePayments(request);
        var februaryPayments = result.Where(p => p.PaymentDate.Month == 2);
       
        // Assert
        Assert.That(februaryPayments, Is.Not.Empty);
        
        foreach (var payment in februaryPayments)
        {
            Assert.That(payment.PaymentDate.Day, Is.EqualTo(28).Or.EqualTo(29));
        }
    }
}