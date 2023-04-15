using LoanCalculator.Models.Requests;
using LoanCalculator.Models.Responses;
using LoanCalculator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanCalculator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanCalculatorController : ControllerBase
{
    private readonly ILoanCalculatorService _loanCalculatorService;

    public LoanCalculatorController(ILoanCalculatorService loanCalculatorService)
    {
        _loanCalculatorService = loanCalculatorService;
    }
    
    [HttpPost("calculate")]
    public ActionResult<List<MonthlyPaymentResponse>> CalculateMonthlyPayments([FromBody] MonthlyPaymentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            List<MonthlyPaymentResponse> payments = _loanCalculatorService.CalculateMonthlyPayments(request);
            return Ok(payments);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}