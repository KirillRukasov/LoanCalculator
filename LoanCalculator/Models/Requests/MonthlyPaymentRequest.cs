using System.ComponentModel.DataAnnotations;

namespace LoanCalculator.Models.Requests;

public class MonthlyPaymentRequest
{
    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Сумма кредита должна быть больше 0.")]
    public decimal LoanAmount { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime LoanIssueDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime LoanClosureDate { get; set; }

    [Required]
    [Range(0, 100, ErrorMessage = "Процентная ставка должна быть в диапазоне от 0 до 100.")]
    public decimal InterestRate { get; set; }

    [Required]
    public PaymentType PaymentType { get; set; }

    [Required]
    [Range(1, 31, ErrorMessage = "День платежа должен быть в диапазоне от 1 до 31.")]
    public int PaymentDay { get; set; }
}